using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour {

    public float speed = 2;
    public float rotationRate = 10f;


	void Update () {
   
        transform.Rotate(Vector3.up * rotationRate * Input.GetAxis("Mouse X"));
        transform.Translate(Vector3.left * Input.GetAxis("Horizontal") * (-speed/10),Space.World);      
	}
}
