using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowParticles : MonoBehaviour {
    public float moveSpeed;

	void Update () {
        this.transform.position += transform.forward * moveSpeed * Time.deltaTime;
	}

    public void ApplyRotation(Vector3 rot, float rotSpeed)
    {
        Quaternion targetRot = Quaternion.LookRotation(rot.normalized);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotSpeed * Time.deltaTime);
            
    }
}
