Shader "Custom/RaymarchShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			
			#include "UnityCG.cginc"
			#include "DistanceFunctions.cginc"

			sampler2D _MainTex;
			uniform float4x4 _CamFrustrum, _CamToWorld;
			uniform float maxDist, _box1round, _boxSphereSmooth, _sphereIntersectSmooth;
			uniform float4 _sphere1,_box1,_sphere2;
			uniform float3 _lightDir, _modInterval;
			uniform fixed4 _MainColor;
			uniform sampler2D _CameraDepthTexture;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 ray : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				half idx = v.vertex.z;
				v.vertex.z = 0;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

				o.ray = _CamFrustrum[(int)idx].xyz;
				o.ray /= abs(o.ray.z);

				o.ray = mul(_CamToWorld, o.ray);
				return o;
			}
			
			float BoxSphere(float3 p)
			{
				/*float modX = pMod1(p.x,_modInterval.x);
				float modY = pMod1(p.y, _modInterval.y);
				float modZ = pMod1(p.z, _modInterval.z);*/
				//used to make endless
				float Sphere1 = sdSphere(p - _sphere1.xyz, _sphere1.w);
				float Box1 = sdRoundBox(p - _box1.xyz, _box1.www, _box1round);
				float combo1 = opSS(_sphere1, _box1, _boxSphereSmooth);
				float Sphere2 = sdSphere(p - _sphere2.xyz, _sphere2.w);
				float combo2 = opIS(_sphere2, combo1, _sphereIntersectSmooth);
				return combo2;
			}

			float distField(float3 p)
			{
				float ground = sdPlane(p, float4(0, 1, 0, 0));
				float bp1 = BoxSphere(p);
				return opU(ground, bp1);
			}
			float3 getNormal(float3 p)
			{
				const float2 offset = float2(0.001, 0.0);
				float3 n = float3
				(

				distField(p + offset.xyy) - distField(p - offset.xyy),
				distField(p + offset.yxy) - distField(p - offset.yxy),
				distField(p + offset.yyx) - distField(p - offset.yyx)
					
				);
				return normalize(n);
					
			}

			fixed4 raymarch(float3 ro, float3 rd, float depth)
			{
				fixed4 result = fixed4(1, 1, 1, 1);

				const int max_iteration = 1024;

				float t = 0;

				for (int i = 0; i < max_iteration; i++)
				{
					if (t>maxDist || t>= depth)
					{
						//env
						result = fixed4(rd, 0);
						break;
					}
					float3 p = ro + rd * t;

					//check hit within dist field
					float d = distField(p);
					if (d < 0.01)
					{
						//shading!
						float3 n = getNormal(p);
						float light = dot(-_lightDir, n);

						result = fixed4(_MainColor.rgb * light,1);
						break;
					}
					t += d;
				}

				return result;

			}

			fixed4 frag (v2f i) : SV_Target
			{
				float depth = LinearEyeDepth(tex2D(_CameraDepthTexture,i.uv).r);
				depth *= length(i.ray);
				fixed3 col = tex2D(_MainTex, i.uv);
				float3 rayDir = normalize(i.ray.xyz);
				float3 rayOrigin = _WorldSpaceCameraPos;
				fixed4 result = raymarch(rayOrigin, rayDir, depth);
				return fixed4(col * (1.0-result.w) + result.xyz * result.w ,1);
			}
			ENDCG
		}
	}
}
