using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AV_SphereParams : MonoBehaviour {
    public int band;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.localScale = new Vector3(AudioVis._audioBandBuffer[band], AudioVis._audioBandBuffer[band], AudioVis._audioBandBuffer[band])*50;

    }
}
