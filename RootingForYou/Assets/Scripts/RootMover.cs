using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class RootMover : MonoBehaviour {

    [SerializeField]
    private float _moveSpeed = 2f;
    [SerializeField]
    private float _widthVariationMin = 0.2f;
    [SerializeField]
    private float _widthVariationMax = 0.4f;
    [SerializeField]
    private Material _rootMaterial;
    [SerializeField]
    private int _interpolationSteps = 10;

    [SerializeField]
    SproutSpawner _sprouts;

    private Vector2 _move = Vector2.zero;
    private bool _isMoving = false;

    private GameObject _roots;
    private MeshFilter _meshFilter;
    private MeshRenderer _renderer;

    private List<Vector3> _meshPoints = new List<Vector3> ();
    private List<int> _triangles = new List<int> ();
    private List<Vector2> _uvs = new List<Vector2> ();

    private float _curUVPos = 0.0f; 

    private float _curInterpolationStep = 0.0f;
    private float _startingInterpolationWidth = 0.0f;
    private float _endingInterpolationWidth = 0.0f;

    // Start is called before the first frame update
    void Start () {
        _roots = transform.GetChild ( 1 ).gameObject;
        _meshFilter = _roots.AddComponent<MeshFilter> ();
        _meshFilter.mesh = new Mesh ();

        _renderer = _roots.AddComponent<MeshRenderer> ();
        _renderer.material = _rootMaterial;

        _endingInterpolationWidth = _widthVariationMin;

        _meshPoints.Add ( new Vector3 ( transform.position.x - Random.Range ( _widthVariationMin, _widthVariationMax ), transform.position.y, transform.position.z ) );
        _uvs.Add ( new Vector2(0.0f, 0.0f) );
        _meshPoints.Add ( new Vector3 ( transform.position.x + Random.Range ( _widthVariationMin, _widthVariationMax ), transform.position.y, transform.position.z ) );
        _uvs.Add ( new Vector2(1.0f, 0.0f) );
        _meshPoints.Add ( transform.position );
        _uvs.Add ( new Vector2(0.5f, 0.0f) );
    }

    public void InputMove ( InputAction.CallbackContext context ) {
        if ( context.canceled ) {
            _move = Vector2.zero;
            _isMoving = false;
        }
        if ( context.performed ) {
            _move = context.ReadValue<Vector2> ();
            _isMoving = true;
        }

        _sprouts.UpdateInput(_isMoving);
    }

    // Update is called once per frame
    void FixedUpdate () {
        Move ();
    }

    private void Move () {
        if ( _isMoving ) {
            Vector3 dir = new( _move.x, _move.y, 0f );

            // smoothen root look so it gets small and bigger
            float _range = Random.Range ( _widthVariationMin, _widthVariationMax );
            if(_curInterpolationStep == 0){
                _startingInterpolationWidth = _endingInterpolationWidth;
                _endingInterpolationWidth = Random.Range ( _widthVariationMin, _widthVariationMax );

                _range = _startingInterpolationWidth;
            } 
            _range = Mathf.Lerp(_startingInterpolationWidth, _endingInterpolationWidth, _curInterpolationStep / _interpolationSteps);
            _curInterpolationStep++;
            if(_curInterpolationStep == _interpolationSteps)
                _curInterpolationStep = 0;
            
            // create root
            Vector3 newPoint;
            if ( _meshPoints.Count > 3 ) {
                // make rotation smoother
                Vector3 prevDir = (_meshPoints[ ^1 ] - _meshPoints[ ^4 ]);
                if(Vector3.Angle(dir, prevDir) > 5){
                    dir = Vector3.RotateTowards(prevDir, dir, 0.09f, 1.0f);
                }

                //calculate normal
                Vector3 normal = Vector3.Cross ( Vector3.forward, _meshPoints[ ^1 ] - _meshPoints[ ^4 ] ).normalized;
                //add points
                newPoint = _meshPoints[ ^1 ] + _moveSpeed * Time.fixedDeltaTime * dir;
                float curMagnitude = (newPoint - _meshPoints[ ^1 ]).magnitude;

                _sprouts.SpawnNewSprout(newPoint, newPoint - _meshPoints[ ^1 ]);

                _curUVPos = (_curUVPos + curMagnitude) > 1 ? _curUVPos+curMagnitude-1 : _curUVPos+curMagnitude;
                _meshPoints.Add ( _meshPoints[ ^1 ] - normal * _range );
                _uvs.Add ( new Vector2(0.0f, _curUVPos) );
                _meshPoints.Add ( _meshPoints[ ^2 ] + normal * _range );
                _uvs.Add ( new Vector2(1.0f, _curUVPos) );
                _meshPoints.Add ( newPoint );
                _uvs.Add ( new Vector2(0.5f, _curUVPos) );
            } else {
                newPoint = _meshPoints[ ^1 ] + _moveSpeed * Time.fixedDeltaTime * dir;
                float curMagnitude = (newPoint - _meshPoints[ ^1 ]).magnitude;
                _curUVPos = (_curUVPos + curMagnitude) > 1 ? _curUVPos+curMagnitude-1 : _curUVPos+curMagnitude;
                //generate fake normal
                Vector3 normal = Vector3.right;
                // add points
                _meshPoints.Add(_meshPoints[^1] - normal * _range);
                _uvs.Add(new Vector2(0.0f, _curUVPos));
                _meshPoints.Add(_meshPoints[^2] + normal * _range);
                _uvs.Add(new Vector2(1.0f, _curUVPos));
                _meshPoints.Add ( newPoint );
                _uvs.Add(new Vector2(0.5f, _curUVPos));
            }

            _triangles.AddRange ( BaseTraingulation ( _meshPoints.Count - 4 ) );

            _meshFilter.mesh.vertices = _meshPoints.ToArray ();
            _meshFilter.mesh.triangles = _triangles.ToArray ();
            _meshFilter.mesh.uv = _uvs.ToArray ();
            _meshFilter.mesh.RecalculateNormals ();
            _meshFilter.mesh.RecalculateBounds ();
            
            //_isMoving = false;
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
