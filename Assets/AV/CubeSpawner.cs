using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour {

    public GameObject barPrefab;
    private GameObject[] bars = new GameObject[512];

    public float _maxScale;

    private void Start()
    {
        for (int i = 0; i < bars.Length; i++)
        {
            GameObject bar = Instantiate(barPrefab);
            bar.transform.position = this.transform.position;
            bar.transform.parent = this.transform;
            bar.name = "VisBar " + i;
            this.transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
            bar.transform.position = Vector3.forward * 100;
            bars[i] = bar; 
        }
    }

    private void Update()
    {
        for (int i = 0; i < bars.Length; i++)
        {
            if (bars !=null)
            {
                bars[i].transform.localScale = new Vector3(1, (AudioVis.samples[i] * _maxScale)+2, 1);
            }
        }   
    }
}
