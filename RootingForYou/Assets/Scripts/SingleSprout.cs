using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSprout : MonoBehaviour
{
    // possibilities if sprout is generated and growing (0-100)
    public float _growPossibility;
    public Material _rootMaterial;

    public bool _isGrowing = false;
    private bool _isFinished = false;

    public GameObject _go;
    private MeshFilter _meshFilter;
    private MeshRenderer _renderer;

    public Vector3 _startPoint;
    public Vector3 _startDirection;

    private float _curUVPos = 0.0f; 

    private List<Vector3> _meshPoints = new List<Vector3> ();
    private List<int> _triangles = new List<int> ();
    private List<Vector2> _uvs = new List<Vector2> ();

    public void UpdateInput ( bool moving ) {
        if ( !moving ) {
            _isGrowing = false;
        }
        if ( moving ) {
            _isGrowing = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _meshFilter = _go.AddComponent<MeshFilter> ();
        _meshFilter.mesh = new Mesh ();

        _renderer = _go.AddComponent<MeshRenderer> ();
        _renderer.material = _rootMaterial;
    }

    // Update is called once per frame
    void FixedUpdate () {
        Grow ();
    }

    private void Grow() {
        if ( _isGrowing && !_isFinished ) {
            bool dirCheck = Random.Range(0.0f,1.0f)>0.5;
            if(_meshPoints.Count == 0){
                Vector3 prevDirection = _startDirection;
                Vector3 prevNormal = Vector3.Cross( Vector3.forward, prevDirection).normalized;
                Vector3 newDirection = Vector3.RotateTowards(_startDirection, dirCheck ? prevNormal : prevNormal*-1, Random.Range(0.3f,1.57f), 1.0f);
                Vector3 normal = Vector3.Cross( Vector3.forward, newDirection).normalized;

                _meshPoints.Add(_startPoint);
                _uvs.Add(new Vector2(0.5f, 0.0f));
                _meshPoints.Add(_startPoint - normal * 0.2f);
                _uvs.Add(new Vector2(0.0f, 0.0f));
                _meshPoints.Add(_startPoint + normal * 0.2f);
                _uvs.Add(new Vector2(1.0f, 0.0f));

                _curUVPos += 0.03f;

                _meshPoints.Add(_startPoint + newDirection.normalized * 0.03f);
                _uvs.Add(new Vector2(0.5f, _curUVPos));                
            } else {
                Vector3 newStartPoint = _meshPoints[^1];
                Vector3 prevDirection = _meshPoints[^1] - _meshPoints[^4];
                Vector3 prevNormal = Vector3.Cross( Vector3.forward, prevDirection).normalized;
                Vector3 newDirection = Vector3.RotateTowards(prevDirection, dirCheck ? prevNormal : prevNormal*-1, Random.Range(0.1f,0.57f), 1.0f);
                Vector3 normal = Vector3.Cross( Vector3.forward, newDirection).normalized;

                _meshPoints.Add(newStartPoint - normal * 0.2f);
                _uvs.Add(new Vector2(0.0f, _curUVPos));
                _meshPoints.Add(newStartPoint + normal * 0.2f);
                _uvs.Add(new Vector2(1.0f, _curUVPos));

                _curUVPos += 0.03f;

                _meshPoints.Add(newStartPoint + newDirection.normalized * 0.03f);
                _uvs.Add(new Vector2(0.5f, _curUVPos));
            }
            
            _triangles.AddRange ( BaseTraingulation ( _meshPoints.Count - 4 ) );

            _meshFilter.mesh.vertices = _meshPoints.ToArray ();
            _meshFilter.mesh.triangles = _triangles.ToArray ();
            _meshFilter.mesh.uv = _uvs.ToArray ();
            _meshFilter.mesh.RecalculateNormals ();
            _meshFilter.mesh.RecalculateBounds ();

            if(Random.Range(0.0f,1000.0f) > _growPossibility ){
                _isFinished = true;
            }
        }
    }

    private int[] BaseTraingulation ( int baseInt ) {
        int[] returnValue = new int[ baseInt - 3 >= 0 ? 12 : 6 ];
        returnValue[ 0 ] = ( baseInt );
        returnValue[ 1 ] = ( baseInt + 3 );
        returnValue[ 2 ] = ( baseInt + 1 );

        returnValue[ 3 ] = ( baseInt );
        returnValue[ 4 ] = ( baseInt + 2 );
        returnValue[ 5 ] = ( baseInt + 3 );

        if ( baseInt - 3 > 0 ) {
            returnValue[ 6 ] = ( baseInt );
            returnValue[ 7 ] = ( baseInt + 1 );
            returnValue[ 8 ] = ( baseInt - 2 );
                           
            returnValue[ 9 ] = ( baseInt );
            returnValue[ 10 ] = ( baseInt - 1 );
            returnValue[ 11 ] = ( baseInt + 2 );
        }
        return returnValue;
    }
}
