using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class shoot : MonoBehaviour {

    [Space]
    [Header("Ball Stats")]
    public int ballQuantity = 1;
    public float ballDelay = 0.05f;
    [Space]
    [Header("Ball Types")]
    [Range(0,5)]
    public int ballIndex = 0;
    public GameObject[] ballType;

    public Transform shootPoint;
    public TextMeshProUGUI text;
 
	void Update () {

        
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(ShootBall(ballQuantity));
        }
	}
    IEnumerator ShootBall(int balls)
    {
        while (balls>0)
        {
            text.text = (balls).ToString();
            AudioEventManager.instance.PlaySound("pop");
            Instantiate(ballType[ballIndex], shootPoint.position, shootPoint.rotation);
            yield return new WaitForSeconds(ballDelay);
            balls--;
        }
       
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 20);
    }
}
