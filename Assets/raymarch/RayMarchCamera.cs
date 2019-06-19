using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class RayMarchCamera : SceneViewFilter
{

    [SerializeField]Shader _shader;
	
    public Material _raymarchMaterial
    {
        get
        {
            if (!rayMat && _shader)
            {
                rayMat = new Material(_shader);
                rayMat.hideFlags = HideFlags.HideAndDontSave;
            }
            return rayMat; 
        }
    }
    public Transform directionalLight;
    private Material rayMat;
    public Camera _camera
    {
        get
        {
            if (!cam)
            {
                cam = GetComponent<Camera>();
            }
            return cam;
        }
    }

    private Camera cam;
    public float _maxDist;
    public Color _mainColor;
    [Header("dist field things")]
    public Vector4 _sphere1;
    public Vector4 _box1;
    public float _box1round;
    public float _boxSphereSmooth;
    public Vector4 _sphere2;
    public float _sphereIntersectSmooth;

    //public Vector3 _modInterval; 

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!_raymarchMaterial)
        {
            Graphics.Blit(source, destination);
            return;
        }

        _raymarchMaterial.SetVector ("_lightDir",               directionalLight ? directionalLight.forward : Vector3.down);
        _raymarchMaterial.SetMatrix ("_CamFrustrum",            CamFrustrum(_camera));
        _raymarchMaterial.SetMatrix ("_CamToWorld",             _camera.cameraToWorldMatrix);
        _raymarchMaterial.SetFloat  ("maxDist",                 _maxDist);
        _raymarchMaterial.SetFloat  ("_box1round",              _box1round);
        _raymarchMaterial.SetFloat  ("_boxSphereSmooth",        _boxSphereSmooth);
        _raymarchMaterial.SetFloat  ("_sphereIntersectSmooth", _sphereIntersectSmooth);
        _raymarchMaterial.SetVector ("_box1",                   _box1);
        _raymarchMaterial.SetVector ("_sphere1",                _sphere1);
        _raymarchMaterial.SetVector ("_sphere2",                _sphere2);
        _raymarchMaterial.SetColor  ("_MainColor",              _mainColor);
       // _raymarchMaterial.SetVector("_modInterval", _modInterval);

        RenderTexture.active = destination;
        _raymarchMaterial.SetTexture("_MainTex", source);

        GL.PushMatrix();
        GL.LoadOrtho();
        _raymarchMaterial.SetPass(0);
        GL.Begin(GL.QUADS);

        //bl
        GL.MultiTexCoord2(0, 0.0f, 0.0f);
        GL.Vertex3(0.0f, 0.0f, 3.0f);
        //BR
        GL.MultiTexCoord2(0, 1.0f, 0.0f);
        GL.Vertex3(1.0f, 0.0f, 2.0f);
        
        //TR
        GL.MultiTexCoord2(0, 1.0f, 1.0f);
        GL.Vertex3(1.0f, 1.0f, 1.0f);
        //TL
        GL.MultiTexCoord2(0, 0.0f, 1.0f);
        GL.Vertex3(0.0f, 1.0f, 0.0f);


        GL.End();
        GL.PopMatrix();

    }
    private Matrix4x4 CamFrustrum(Camera cam)
    {
        Matrix4x4 frustrum = Matrix4x4.identity;
        float fov = Mathf.Tan((cam.fieldOfView * .5f) * Mathf.Deg2Rad);

        Vector3 goUp = Vector3.up* fov;
        Vector3 goRight = Vector3.right * fov * cam.aspect;

        Vector3 TL = (-Vector3.forward - goRight + goUp);
        Vector3 TR = (-Vector3.forward + goRight + goUp);
        Vector3 BL = (-Vector3.forward - goRight - goUp);
        Vector3 BR = (-Vector3.forward + goRight - goUp);
        frustrum.SetRow(0, TL);
        frustrum.SetRow(1, TR);
        frustrum.SetRow(2, BR);
        frustrum.SetRow(3, BL);


        return frustrum;

    }
}
