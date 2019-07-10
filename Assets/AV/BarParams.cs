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
    
    Material mat;
    

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
            {transform.localScale = new Vector3(this.transform.localScale.x, ((AudioVis._audioBandBuffer[band]) * scaleMult) + startScale, this.transform.localScale.z);}
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
