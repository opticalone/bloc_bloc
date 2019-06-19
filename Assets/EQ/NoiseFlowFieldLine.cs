using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFlowFieldLine : MonoBehaviour {

    FastNoise _FN;
    public Color color;

    public Vector3Int _gridSize;
    public float _increment;
    public Vector3 _offset, _offsetSpeed;

    private void OnDrawGizmos()
    {
        _FN = new FastNoise();
        float xOff = 0;
        for (int x = 0; x < _gridSize.x; x++)
        {
            float yOff = 0;
            for (int y = 0; y < _gridSize.y; y++)
            {
                float zOff = 0;
                for (int z = 0; z < _gridSize.z; z++)
                {
                    float noise = _FN.GetSimplex(xOff + _offset.x, yOff + _offset.y, zOff + _offset.z) + 1;
                    Vector3 noiseDir = new Vector3(Mathf.Cos(noise * Mathf.PI), Mathf.Sin(noise * Mathf.PI), Mathf.Cos(noise * Mathf.PI));


                    Gizmos.color = color;
                    //color = new Color(color.r, color.g, color.b, noise * .5f);
                    color = new Color(-noiseDir.normalized.x, -noiseDir.normalized.y, -noiseDir.normalized.z, 1);

                    

                    Vector3 pos = new Vector3(x, y, z) + transform.position;
                    Vector3 scale = new Vector3(1, 1, 1);
                    Vector3 endPos = pos + Vector3.Normalize(noiseDir);

                    Gizmos.DrawLine(pos, endPos);


                    zOff += _increment;
                }
                yOff += _increment;
            }
            xOff += _increment;
        }
    }



}
