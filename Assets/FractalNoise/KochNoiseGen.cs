using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KochNoiseGen : MonoBehaviour {

    public Color gizmoColor;

    protected enum _axis {xAxis, yAxis, zAxis};
    protected enum _init {Triangle, Square, Pentagon, Hexagon, Heptagon, Octaagon};

    [Header("Axis")]
    [SerializeField]
    protected _axis axis = new _axis();

    private struct LineSegment
    {
        public Vector3 StartPos { get; set; }
        public Vector3 EndPos { get; set; }
        public Vector3 Dir { get; set; }
        public float Length { get; set; }
    }

    [Space]
    [Header("Shape")]
    [SerializeField]
    protected _init init = new _init(); //InitiATOR

    [SerializeField]
    protected AnimationCurve _genCurve;
    protected Keyframe[] _keys;

    protected int _genCount;

    protected int _initPointAmount;
    Vector3[] _initPoint;
    private Vector3 _rotVec;
    private Vector3 _rotAxis;
    private float _initalRot;
    [SerializeField]
    protected float _initSize;

    protected Vector3[] _pos;
    protected Vector3[] _targetPos;
    private List<LineSegment> _lineSegment;


    void Awake()
    {
        GetInitPoints();
        //assign

        _pos = new Vector3[_initPointAmount+1];
        _targetPos = new Vector3[_initPointAmount + 1];
        _lineSegment = new List<LineSegment>();
        _keys = _genCurve.keys;


        _rotVec = Quaternion.AngleAxis(_initalRot, _rotAxis) * _rotVec;
        for (int i = 0; i < _initPointAmount; i++)
        {
            _pos[i] = _rotVec * _initSize;
            _rotVec = Quaternion.AngleAxis(360 / _initPointAmount, _rotAxis) * _rotVec;
        }
        _pos[_initPointAmount] = _pos[0];
        _targetPos = _pos;
    }
    protected void KochGenerator(Vector3[]positions, bool isOut, float genMult)
    {

        //Create lines
        _lineSegment.Clear();
        for (int i = 0; i < positions.Length-1; i++)
        {
            LineSegment line = new LineSegment();
            line.StartPos = positions[i];
            if (i == positions.Length-1)
            {
                line.EndPos = positions[0];
            }
            else
            {
                line.EndPos = positions[i + 1];
            }
            line.Dir = (line.EndPos - line.StartPos).normalized;
            line.Length = Vector3.Distance(line.EndPos, line.StartPos);
            _lineSegment.Add(line);
        }

        //add line segments to point array
        List<Vector3> newPos = new List<Vector3>();
        List<Vector3> targetPos = new List<Vector3>();
        for (int i = 0; i < _lineSegment.Count; i++)
        {
            newPos.Add(_lineSegment[i].StartPos);
            targetPos.Add(_lineSegment[i].StartPos);
            for (int j = 0; j < _keys.Length; j++)
            {
                float moveAmount = _lineSegment[i].Length* _keys[j].time;
                float heightAmount = _lineSegment[i].Length * _keys[j].value * genMult;
                Vector3 movePos = _lineSegment[i].StartPos + (_lineSegment[i].Dir * moveAmount);
                Vector3 dir;
                if (isOut)
                {
                    dir = Quaternion.AngleAxis(-90, _rotAxis)*_lineSegment[i].Dir;
                }
                else
                {
                    dir = Quaternion.AngleAxis(90, _rotAxis) * _lineSegment[i].Dir;
                }
                newPos.Add(movePos);
                targetPos.Add(movePos + (dir * heightAmount));
            }
        }

        newPos.Add(_lineSegment[0].StartPos);
        targetPos.Add(_lineSegment[0].StartPos);

        _pos = new Vector3[newPos.Count];
        _targetPos = new Vector3[targetPos.Count];

        _pos = newPos.ToArray();
        _targetPos = targetPos.ToArray();

        _genCount++;
    }

    void OnDrawGizmos()
    {
        GetInitPoints();
        _initPoint = new Vector3[_initPointAmount];

        _rotVec = Quaternion.AngleAxis(_initalRot, _rotAxis) * _rotVec;
        for (int i = 0; i < _initPointAmount; i++)
        {
            _initPoint[i] = _rotVec * _initSize;
            _rotVec = Quaternion.AngleAxis(360 / _initPointAmount, _rotAxis) * _rotVec;
        }
        for (int i = 0; i < _initPointAmount; i++)
        {
            Gizmos.color = gizmoColor;

            Matrix4x4 _rotMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.matrix = _rotMatrix;

            if (i< _initPointAmount-1)
            {
                Gizmos.DrawLine(_initPoint[i], _initPoint[i + 1]);
            }
            else
            {
                Gizmos.DrawLine(_initPoint[i], _initPoint[0]);
            }
        }

    }



    void GetInitPoints()
    {
        switch (init)
        {
            case _init.Triangle:
                _initPointAmount = 3;
                _initalRot = 0;
                break;
            case _init.Square:
                _initPointAmount = 4;
                _initalRot = 45;
                break;
            case _init.Pentagon:
                _initPointAmount = 5;
                _initalRot = 36;
                break;
            case _init.Hexagon:
                _initPointAmount = 6;
                _initalRot = 30;
                break;
            case _init.Heptagon:
                _initPointAmount = 7;
                _initalRot = 25.71428f;
                break;
            case _init.Octaagon:
                _initPointAmount = 8;
                _initalRot = 22.5f;
                break;
            default:
                _initPointAmount = 3;
                _initalRot = 0;
                break;
        }
        switch (axis)
        {
            case _axis.xAxis:
                _rotVec = new Vector3(1, 0, 0);
                _rotAxis = new Vector3(0, 0, 1);
                break;
            case _axis.yAxis:
                _rotVec = new Vector3(0, 1, 0);
                _rotAxis = new Vector3(1, 0, 0);
                break;
            case _axis.zAxis:
                _rotVec = new Vector3(0, 0, 1);
                _rotAxis = new Vector3(0, 1, 0);
                break;
            default:
                _rotVec = new Vector3(0, 1, 0);
                _rotAxis = new Vector3(1, 0, 0);
                break;

        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
