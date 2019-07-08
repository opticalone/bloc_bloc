using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GlobalVarDemo
{
    public class neededNumber
    {
        public static float WEE = 0.0f;
    }
}


public class CubeSpawner : MonoBehaviour {

    public GameObject barPrefab;
    private GameObject[] bars = new GameObject[512];
    public float circleRadius = 100f;
    public float _maxScale;
    int fBand;
    private void Start()
    {
        for (int i = 0; i < bars.Length; i++)
        {
            GameObject bar = Instantiate(barPrefab);
            bar.transform.position = this.transform.position;
            bar.transform.parent = this.transform;
            bar.name = "VisBar " + i;
            this.transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
            bar.transform.position = Vector3.forward * circleRadius;
            #region dumbbandchecker

            fBand = (i / 64) + (int)GlobalVarDemo.neededNumber.WEE++;
            
            #endregion
            bar.GetComponent<BarParams>().band = fBand;
            //fBand = Mathf.Clamp(fBand, 0, 7);
            
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
