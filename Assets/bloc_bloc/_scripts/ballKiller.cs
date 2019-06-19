using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballKiller : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ball"))
        {
            Destroy(other.gameObject);
        }
    }
}
