using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkController : MonoBehaviour {

    [SerializeField]
    private Transform _referenceTransform;
    [SerializeField]
    private GameObject _bgChunk;
    [SerializeField]
    private GameObject _waterSource;
    [SerializeField]
    private int _maxNumberOfWaterSourcePerChunk;

    private int _spawnedChunks = 0;
    private float _lastSpawnedY = 0;
    private const float ChunkVerticalSize = 9.92f;
    private const float ChunkVerticalHalfSize = 4.96f;

    // Start is called before the first frame update
    void Start () {
        SpawnBGChunk ( 1.1f );
        
    }

    // Update is called once per frame
    void Update () {
        if (_referenceTransform.position.y < 1.1f - ChunkVerticalSize * _spawnedChunks + ChunkVerticalHalfSize ) {
            SpawnBGChunk ( _lastSpawnedY - ChunkVerticalSize );
            for ( int i = 0; i < Random.Range(1,_maxNumberOfWaterSourcePerChunk+1); i++ ) {
                SpawnWaterSource ( _lastSpawnedY - ChunkVerticalSize );
            }
        }
    }

    private void SpawnBGChunk (float yPos) {
        //GameObject go = Instantiate ( _bgChunk, transform );
        //go.transform.position = new Vector3 ( 0f, yPos, 0f );
        _spawnedChunks++;
        _lastSpawnedY = yPos;
    }

    private void SpawnWaterSource (float yPosOfChunk) {
        GameObject go = Instantiate ( _waterSource, transform );
        go.transform.position = new Vector3 ( Random.Range(-8f, 8f), Random.Range(yPosOfChunk, yPosOfChunk+ChunkVerticalSize), 0f );
    }
}
