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
            transform.localScale = new Vector3(((AudioVis._bandBuffer[band] - 1) * scaleMult) + startScale, ((AudioVis._bandBuffer[band]-1) * scaleMult) + startScale, ((AudioVis._bandBuffer[band] - 1) * scaleMult) + startScale);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, (AudioVis._freqBand[band] * scaleMult) + startScale, transform.localScale.z);
        }
        
	}
}
