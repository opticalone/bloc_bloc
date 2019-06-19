using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour {

    Rigidbody rb;
    public float speed =10f;
    Vector3 velocity;
    public Color color;
	void Start () {
        rb = GetComponent<Rigidbody>();
        velocity = this.transform.forward * speed;
        rb.velocity = velocity;
        //rb.AddForce(velocity);
    }
	
	// Update is called once per frame
	void Update () {
        
	}
    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 5);
    }
}
