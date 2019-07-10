using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BarParams : MonoBehaviour {

    public bool doScale;
    public bool doLight;
    public bool useBuffer;
    [Space]
    public int band;
    public float startScale, scaleMult;
    
    Material mat;
    

	// Use this for initialization
	void Start () {
        mat = GetComponent<MeshRenderer>().material;

        
	}
	
	// Update is called once per frame
	void Update () {
        if (useBuffer)
        {
            if (doScale)
            {
                transform.localScale = new Vector3(this.transform.localScale.x, ((AudioVis._audioBandBuffer[band]) * scaleMult) + startScale, this.transform.localScale.z);
            }



            if (doLight) {
                Color col = new Color((AudioVis._audioBandBuffer[band]) + .25F, (AudioVis._audioBandBuffer[band]) + .25F, (AudioVis._audioBandBuffer[band]) + .25F);
                mat.SetColor("_EmissionColor", col);
            }
            
        }
        else
        {   
            if(doScale)
            transform.localScale = new Vector3(transform.localScale.x, (AudioVis._audioBand[band] * scaleMult) + startScale, transform.localScale.z);
        }
        
	}
    
}
