using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aim : MonoBehaviour {

    Vector3 mouseOffset;
    private float mouseZ;

    // Use this for initialization
    void Start () {
        mouseZ = Camera.main.WorldToViewportPoint(gameObject.transform.position).z;
        mouseOffset = gameObject.transform.position - GetWorldPosition();
	}

    private Vector3 GetWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;

        mousePos.z = mouseZ;

        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
