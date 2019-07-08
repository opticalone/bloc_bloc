using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarParams : MonoBehaviour {

    public int band;
    public float startScale, scaleMult;
    public bool useBuffer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (useBuffer)
        {
            transform.localScale = new Vector3( this.transform.localScale.x, ((AudioVis._audioBandBuffer[band-1]) * scaleMult) + startScale, this.transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, (AudioVis._audioBand[band] * scaleMult) + startScale, transform.localScale.z);
        }
        
	}
}
