using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SproutSpawner : MonoBehaviour
{
    // possibilities if sprout is generated and growing (0-100)
    [SerializeField]
    private float spawnPossibility = 10.0f; 
    [SerializeField]
    private float growPossibility = 70.0f;
    [SerializeField]
    private Material _rootMaterial;

    private bool _isGrowing = false;

    private GameObject _sprouts;
    private MeshFilter _meshFilter;
    private MeshRenderer _renderer;

    private List<Vector3> _meshPoints = new List<Vector3> ();
    private List<int> _triangles = new List<int> ();
    private List<Vector2> _uvs = new List<Vector2> ();

    private List<List<Vector3>> _growingSprouts = new List<List<Vector3>> ();

    void Start()
    {
        _sprouts = transform.GetChild ( 0 ).gameObject;
    }

    void Update()
    {
        
    }

    public void SpawnNewSprout(Vector3 curPos, Vector3 direction)
    {
        float baseRandomValue = spawnPossibility - Random.Range(0,1000);

        if(baseRandomValue > 0) {
            GameObject child = new GameObject();
            child.AddComponent<SingleSprout>();
            child.GetComponent<SingleSprout>()._growPossibility = growPossibility;
            child.GetComponent<SingleSprout>()._rootMaterial = _rootMaterial;
            child.GetComponent<SingleSprout>()._go = child;
            child.GetComponent<SingleSprout>()._startDirection = direction;
            child.GetComponent<SingleSprout>()._startPoint = curPos;
            child.GetComponent<SingleSprout>()._isGrowing = _isGrowing;
            child.transform.parent = _sprouts.transform;
        }
    }

    private int[] BaseTraingulation () {
        int[] returnValue = new int[ 6 ];
        returnValue[ 0 ] = ( _meshPoints.Count - 4 );
        returnValue[ 1 ] = ( _meshPoints.Count - 1 );
        returnValue[ 2 ] = ( _meshPoints.Count - 3 );

        returnValue[ 3 ] = ( _meshPoints.Count - 4 );
        returnValue[ 4 ] = ( _meshPoints.Count - 2 );
        returnValue[ 5 ] = ( _meshPoints.Count - 1 );
        
        return returnValue;
    }

    private int[] FullTraingulation () {
        int[] returnValue = new int[ 12 ];
        returnValue[ 0 ] = ( _meshPoints.Count - 4 );
        returnValue[ 1 ] = ( _meshPoints.Count - 1 );
        returnValue[ 2 ] = ( _meshPoints.Count - 3 );

        returnValue[ 3 ] = ( _meshPoints.Count - 4 );
        returnValue[ 4 ] = ( _meshPoints.Count - 2 );
        returnValue[ 5 ] = ( _meshPoints.Count - 1 );

        returnValue[ 6 ] = ( _meshPoints.Count - 2 );
        returnValue[ 7 ] = ( _meshPoints.Count - 4 );
        returnValue[ 8 ] = ( _meshPoints.Count - 5 ); // should be wrong

        returnValue[ 9 ] = ( _meshPoints.Count - 3 );
        returnValue[ 10 ] = ( _meshPoints.Count - 6 ); // should be wrong
        returnValue[ 11 ] = ( _meshPoints.Count - 4 );
        
        return returnValue;
    }

    public void UpdateInput(bool moving){
        _isGrowing = moving;
        for(int i = 0; i < _sprouts.transform.childCount; i++)
        {
            GameObject go = _sprouts.transform.GetChild(i).gameObject;
            go.GetComponent<SingleSprout>().UpdateInput(moving);
        }
    }
}
