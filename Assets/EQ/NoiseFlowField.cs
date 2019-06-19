using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFlowField : MonoBehaviour {

    FastNoise _FN;
    public Color color;
    [Space]
    public Vector3[,,] _flowFieldDir;
    public float _cellSize;
    public float _increment;
    public Vector3Int _gridSize;
    
    public Vector3 _offset, _offsetSpeed;
    [Space]
    [Header("Particles")]
    [Space]
    public GameObject particlePrefab;
    public int particleQuantity;
    public List<FlowParticles> flowParticles;
    
    public float spawnRadius;
    public float particleScale,particleMoveSpeed,particleRotateSpeed;


    bool particleSpawnValidate(Vector3 pos)
    {
        bool valid = true;
        foreach (FlowParticles parts in flowParticles)
        {
            if (Vector3.Distance(pos, parts.transform.position)< spawnRadius)
            {
                valid = false;
                break;
            }
        }
        if (valid)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Start()
    {
        _flowFieldDir = new Vector3[_gridSize.x, _gridSize.y, _gridSize.z];
        _FN = new FastNoise();
        flowParticles = new List<FlowParticles>();
        for (int i = 0; i < particleQuantity; i++)
        {

            int attempt = 0;
            while (attempt< 100)
            {
                Vector3 randPos = new Vector3(
                Random.Range(this.transform.position.x, this.transform.position.x + _gridSize.x * _cellSize),
                Random.Range(this.transform.position.y, this.transform.position.y + _gridSize.y * _cellSize),
                Random.Range(this.transform.position.z, this.transform.position.z + _gridSize.z * _cellSize)

                );
                bool isValid = particleSpawnValidate(randPos);
                if (isValid)
                {
                    GameObject particleInstance = Instantiate(particlePrefab);
                    particleInstance.transform.position = randPos;
                    particleInstance.transform.parent = this.transform;
                    particleInstance.transform.localScale = new Vector3(particleScale, particleScale, particleScale);
                    flowParticles.Add(particleInstance.GetComponent<FlowParticles>());
                    break;
                }
                if (!isValid)
                {
                    attempt++;
                }
            }

          //  Debug.Log(flowParticles.Count);
        }

    }
    private void Update()
    {
        CalculateFlowField();
        ParticleBehavior();
    }
    void CalculateFlowField()
    {
        _offset = new Vector3(_offset.x + (_offsetSpeed.x * Time.deltaTime), _offset.y + (_offsetSpeed.y * Time.deltaTime), _offset.z + (_offsetSpeed.z * Time.deltaTime));

        float xOff = 0;
        for (int x = 0; x < _gridSize.x; x++)
        {
            float yOff = 0;
            for (int y = 0; y < _gridSize.y; y++)
            {
                float zOff = 0;
                for (int z = 0; z < _gridSize.z; z++)
                {
                    float noise = _FN.GetSimplex(xOff + _offset.x, yOff + _offset.y, zOff + _offset.z) + 1;
                    Vector3 noiseDir = new Vector3(Mathf.Cos(noise * Mathf.PI), Mathf.Sin(noise * Mathf.PI), Mathf.Cos(noise * Mathf.PI));

                    _flowFieldDir[x, y, z] = Vector3.Normalize(noiseDir);

                    zOff += _increment;
                }
                yOff += _increment;
            }
            xOff += _increment;
        }
    }

    void ParticleBehavior()
    {
        foreach (FlowParticles p in flowParticles)
        {
            //checking box "collision" via gridsize

            //x edges
            if (p.transform.position.x > this.transform.position.x +(_gridSize.x * _cellSize))
            {
                p.transform.position = new Vector3(this.transform.position.x, p.transform.position.y, p.transform.position.z);

            }
            if (p.transform.position.x < this.transform.position.y)
            {
                p.transform.position = new Vector3(this.transform.position.x + (_gridSize.x * _cellSize), p.transform.position.y, p.transform.position.z);
            }



            //y edges
            if (p.transform.position.y > this.transform.position.y + (_gridSize.y * _cellSize))
            {
                p.transform.position = new Vector3(p.transform.position.x, this.transform.position.y, p.transform.position.z);

            }
            if (p.transform.position.y < this.transform.position.x)
            {
                p.transform.position = new Vector3(p.transform.position.x, this.transform.position.y + (_gridSize.y * _cellSize), p.transform.position.z);
            }



            //z edges
            if (p.transform.position.z > this.transform.position.z + (_gridSize.z * _cellSize))
            {
                p.transform.position = new Vector3(p.transform.position.x, p.transform.position.y, this.transform.position.z);

            }
            if (p.transform.position.z < this.transform.position.x)
            {
                p.transform.position = new Vector3(p.transform.position.x, p.transform.position.y , this.transform.position.z+ (_gridSize.y * _cellSize));
            }


            Vector3Int patriclepos = new Vector3Int(
                Mathf.FloorToInt(Mathf.Clamp((p.transform.position.x - this.transform.position.x) / _cellSize, 0, _gridSize.x - 1)),
                Mathf.FloorToInt(Mathf.Clamp((p.transform.position.y - this.transform.position.y) / _cellSize, 0, _gridSize.y - 1)),
                Mathf.FloorToInt(Mathf.Clamp((p.transform.position.z - this.transform.position.z) / _cellSize, 0, _gridSize.z - 1))
                );

            p.ApplyRotation(_flowFieldDir[patriclepos.x, patriclepos.y, patriclepos.z], particleRotateSpeed);
            p.moveSpeed = particleMoveSpeed;
            p.transform.localScale = new Vector3(particleScale, particleScale, particleScale);

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawWireCube(this.transform.position + new Vector3((_gridSize.x * _cellSize)*.5f, (_gridSize.y * _cellSize) * .5f, (_gridSize.z * _cellSize) * .5f),
        new Vector3(_gridSize.x * _cellSize, _gridSize.y * _cellSize, _gridSize.z *_cellSize));

    }

}

