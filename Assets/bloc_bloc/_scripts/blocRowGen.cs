using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blocRowGen : MonoBehaviour {

    public Bloc[] blocks = new Bloc[9];

	// Use this for initialization
	void Start () {
        blocks = GetComponentsInChildren<Bloc>();       
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
