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

    private Vector2 _move = Vector2.zero;
    private bool _isMoving = false;

    private GameObject _roots;
    private MeshFilter _meshFilter;
    private MeshRenderer _renderer;

    private List<Vector3> _meshPoints = new List<Vector3> ();
    private List<int> _triangles = new List<int> ();

    // Start is called before the first frame update
    void Start () {

        _roots = transform.GetChild ( 0 ).gameObject;
        _meshFilter = _roots.AddComponent<MeshFilter> ();
        _meshFilter.mesh = new Mesh ();

        _renderer = _roots.AddComponent<MeshRenderer> ();
        _renderer.material = _rootMaterial;

        _meshPoints.Add ( transform.position );
        _meshPoints.Add ( new Vector3 ( transform.position.x - Random.Range ( _widthVariationMin, _widthVariationMax ), transform.position.y, transform.position.z ) );
        _meshPoints.Add ( new Vector3 ( transform.position.x + Random.Range ( _widthVariationMin, _widthVariationMax ), transform.position.y, transform.position.z ) );
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
    }

    // Update is called once per frame
    void FixedUpdate () {
        Move ();
    }

    private void Move () {
        if ( _isMoving ) {

            Vector3 dir = new( _move.x, _move.y, 0f );
            Vector3 newPoint;
            if ( _meshPoints.Count > 3 ) {
                //calculate normal
                Vector3 normal = Vector3.Cross ( Vector3.forward, _meshPoints[ ^1 ] - _meshPoints[ ^4 ] ).normalized;
                //add points
                newPoint = _meshPoints[ ^1 ] + _moveSpeed * Time.fixedDeltaTime * dir;
                _meshPoints.Add ( _meshPoints[ ^1 ] - normal * Random.Range ( _widthVariationMin, _widthVariationMax ) );
                _meshPoints.Add ( _meshPoints[ ^2 ] + normal * Random.Range ( _widthVariationMin, _widthVariationMax ) );
                _meshPoints.Add ( newPoint );
            } else {
                newPoint = _meshPoints[ 0 ] + _moveSpeed * Time.fixedDeltaTime * dir;
                _meshPoints.Add ( newPoint );
            }

            _triangles.AddRange ( BaseTraingulation ( _meshPoints.Count - 4 ) );

            _meshFilter.mesh.vertices = _meshPoints.ToArray ();
            _meshFilter.mesh.triangles = _triangles.ToArray ();
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
