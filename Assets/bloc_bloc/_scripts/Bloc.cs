using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bloc : MonoBehaviour {
    
    [Header("Mesh Details")]    
    [Space]    
    [SerializeField]private Mesh triMesh;
    public Material triangleMaterial;
    [Space]
    [SerializeField]private Mesh boxMesh;
    public Material boxMaterial;
    [Space]
   /* [HideInInspector]*/public MeshFilter filter;
   /* [HideInInspector]*/public MeshRenderer blocRenderer;
    [Space]

    [Header("Mesh Collider")]
    [Space]
    public MeshCollider meshCollider;
    public PhysicMaterial physMat;

    [Header("Game Data")]
    [Space]
    public int value = 1;
    public int blockHealth;
    public TextMeshProUGUI text;
    
    public bool isDestroyed = false;

    

    public enum BlocType { box, tri, ball };


    private void Start()
    {
        
        filter = GetComponent<MeshFilter>();
        blocRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();

        blockHealth = value;
        int rand = Random.Range(0, 100);
        //Debug.Log(rand);
        if (rand%2 == 0)
        {
            GetComponent<MeshFilter>().mesh = boxMesh;
            meshCollider.sharedMesh = boxMesh;
            meshCollider.material = physMat;
            blocRenderer.material = boxMaterial;
        }
        else if (rand%3 == 0 || rand % 7 == 0 || rand % 5 == 0)
        {
            meshCollider.isTrigger = true;
            blocRenderer.gameObject.SetActive(false);
        }
        else
        {  
            var vec = transform.eulerAngles;
            vec.y = Random.Range(0, 360);
            vec.y = Mathf.Round(vec.y / 90) * 90;
            transform.eulerAngles = vec;

            GetComponent<MeshFilter>().mesh = triMesh;
            meshCollider.sharedMesh = triMesh;
            meshCollider.material = physMat;
            blocRenderer.material = triangleMaterial;           
            
        }       
    }
    void Update()
    {
        text.text = blockHealth.ToString();
        if (blockHealth <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("ball"))
        {
            blocRenderer.material.color = Color.Lerp(blocRenderer.material.color, Color.yellow, .1f);
            blockHealth--;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ball"))
        {
            blockHealth--;
        }
    }


    
}
