using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SproutSpawner : MonoBehaviour {
    // possibilities if sprout is generated and growing (0-100)
    [SerializeField]
    private float spawnPossibility = 10.0f;
    [SerializeField]
    private float growPossibility = 70.0f;
    [SerializeField]
    private Material _rootMaterial;
    [SerializeField]
    private SingleSprout _sproutPrefab;

    private bool _isGrowing = false;

    private GameObject _sprouts;
    private MeshFilter _meshFilter;
    private MeshRenderer _renderer;

    private List<Vector3> _meshPoints = new List<Vector3> ();
    private List<int> _triangles = new List<int> ();
    private List<Vector2> _uvs = new List<Vector2> ();

    private List<List<Vector3>> _growingSprouts = new List<List<Vector3>> ();

    void Start () {
        _sprouts = transform.GetChild ( 2 ).gameObject;
    }

    public void SpawnNewSprout ( Vector3 curPos, Vector3 direction ) {
        if ( spawnPossibility - Random.Range ( 0f, 100f ) > 0f ) {
            SingleSprout sprout = Instantiate ( _sproutPrefab.gameObject, _sprouts.transform ).GetComponent<SingleSprout>();
            sprout.initialize(growPossibility, direction, curPos, _isGrowing, _rootMaterial);
        }
    }

    public void UpdateInput ( bool moving ) {
        _isGrowing = moving;
        for ( int i = 0; i < _sprouts.transform.childCount; i++ ) {
            GameObject go = _sprouts.transform.GetChild ( i ).gameObject;
            go.GetComponent<SingleSprout> ().UpdateInput ( moving );
        }
    }
}
