using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BarParams : MonoBehaviour {


    public Color color;
    public bool doScale, doLight, doRandomBand,doRandomColor;
    public bool useBuffer;
    [Space]
    public int band;
    public float startScale, scaleMult;
    [Space]
    [Header("Scale Axis?")]
    Material mat;

    
    public enum _ScaleAxis { ScaleX, ScaleY, ScaleZ, ScaleAll };
    public _ScaleAxis scaleAxis = new _ScaleAxis();

    // Use this for initialization
    void Start () {
        mat = GetComponent<MeshRenderer>().material;
        if (doRandomBand)
        {
            band = (int)Random.Range(0, 7);
        }
        
        
	}
	
	// Update is called once per frame
	void Update () {
        if (useBuffer)
        {
            if (doScale)
            {
                switch (scaleAxis)
                {
                    case _ScaleAxis.ScaleX:
                        transform.localScale = new Vector3(((AudioVis._audioBandBuffer[band]) * scaleMult) + startScale, this.transform.localScale.y, this.transform.localScale.z);
                        break;
                    case _ScaleAxis.ScaleY:
                        transform.localScale = new Vector3(this.transform.localScale.x, ((AudioVis._audioBandBuffer[band]) * scaleMult) + startScale, this.transform.localScale.z);
                        break;
                    case _ScaleAxis.ScaleZ:
                        transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, ((AudioVis._audioBandBuffer[band]) * scaleMult) + startScale);
                        break;
                    case _ScaleAxis.ScaleAll:
                        transform.localScale = new Vector3(((AudioVis._audioBandBuffer[band]) * scaleMult) + startScale, ((AudioVis._audioBandBuffer[band]) * scaleMult) + startScale, ((AudioVis._audioBandBuffer[band]) * scaleMult) + startScale);
                        break;
                    default:
                        break;
                }
            }

           


            if (doLight) {Color col = new Color((AudioVis._audioBandBuffer[band]) + .33f, (AudioVis._audioBandBuffer[band]) + .33f, (AudioVis._audioBandBuffer[band]) + .33f) * color;
                mat.SetColor("_EmissionColor", col);}
            if (doRandomColor)
            {Color col = new Color(AudioVis._audioBandBuffer[band] + Random.Range(0.0f,0.5f), AudioVis._audioBandBuffer[band] +Random.Range(0.0f, 0.5f), AudioVis._audioBandBuffer[band] + Random.Range(0.0f, 0.5f));
                mat.SetColor("_EmissionColor", col);
            }
            
        }
        else
        {   
            if(doScale)
            transform.localScale = new Vector3(transform.localScale.x, (AudioVis._audioBand[band] * scaleMult) + startScale, transform.localScale.z);
        }
        //Debug.Log((AudioVis._audioBandBuffer[band]));
        
	}
    
}
