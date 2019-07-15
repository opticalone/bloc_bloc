using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class KochLineGen : KochNoiseGen {

    LineRenderer _lineRenderer;

    [Range(0,1)]
    public float _lerpAmount;
    Vector3[] _lerpPos;
    public float _generationMult;

    // Use this for initialization
    void Start () {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = true;
        _lineRenderer.useWorldSpace = false;
        _lineRenderer.loop = true;
        _lineRenderer.positionCount = _pos.Length;
        _lineRenderer.SetPositions(_pos);
	}
	
	// Update is called once per frame
	void Update () {
        if (_genCount!=0)
        {
            for (int i = 0; i < _pos.Length; i++)
            {
                _lerpPos[i] = Vector3.Lerp(_pos[i], _targetPos[i], _lerpAmount);
            }
            _lineRenderer.SetPositions(_lerpPos);
        }
        if (Input.GetKeyUp(KeyCode.O))
        {
            KochGenerator(_targetPos,true, _generationMult);
            _lerpPos = new Vector3[_pos.Length];
            _lineRenderer.positionCount = _pos.Length;
            _lineRenderer.SetPositions(_pos);
        }
        if (Input.GetKeyUp(KeyCode.I))
        {
            KochGenerator(_targetPos, false, _generationMult);
            _lerpPos = new Vector3[_pos.Length];
            _lineRenderer.positionCount = _pos.Length;
            _lineRenderer.SetPositions(_pos);
        }
  
    }
}
