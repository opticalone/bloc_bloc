using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAudioAffector : MonoBehaviour {

    Light _light;

    public int band;

    void Start () {
        _light = GetComponent<Light>();
	}
	

	void Update () {
        _light.intensity = AudioVis._audioBandBuffer[band] * 10;
	}
}
