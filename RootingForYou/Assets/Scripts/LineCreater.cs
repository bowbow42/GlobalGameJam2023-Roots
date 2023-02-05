using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LineCreater : MonoBehaviour {
    private LineRenderer _renderer;

    private float _startTime = 0.0f;
    private List<Vector3> _renderPoints;
    [SerializeField]
    private int _animationDuration = 9;
    private float _animationDurationCurrent = 0f;
    private float _timeChunks;

    private float _maxX = 0.0f;
    private float _minX = 999999999.0f;
    private float _maxY = 0.0f;
    private float _minY = 999999999.0f;

    [SerializeField]
    private TextMeshProUGUI _scoretext;


    // Start is called before the first frame update
    void Start () {
        _renderer = gameObject.transform.GetComponent<LineRenderer> ();
        _startTime = 0;

        _timeChunks = _animationDuration / RootMover._rootPoints.Count;

        // get minimal and Maximal values
        for ( int i = 0; i < RootMover._rootPoints.Count; i++ ) {
            if ( RootMover._rootPoints[ i ].x > _maxX ) {
                _maxX = RootMover._rootPoints[ i ].x;
            }
            if ( RootMover._rootPoints[ i ].x < _minX ) {
                _minX = RootMover._rootPoints[ i ].x;
            }
            if ( RootMover._rootPoints[ i ].y > _maxY ) {
                _maxY = RootMover._rootPoints[ i ].y;
            }
            if ( RootMover._rootPoints[ i ].y < _minY ) {
                _minY = RootMover._rootPoints[ i ].y;
            }
        }
    }

    public List<Vector3> _linePoints = new List<Vector3> ();
    private int _chunksShown = 0;
    private float _time = 0f;

    void Update () {

        if (_chunksShown >= RootMover._rootPoints.Count ) {
            return;
        }
        if (_time >= 0 ) { 
            _chunksShown += RootMover._rootPoints.Count / _animationDuration;
            _time -= 1f;
        }
        _time += Time.deltaTime;

        // transform Ranges
        // x => (_minX, _maxX) => (-7,-1)
        // y => (_minY, _maxY) => (-4, 4)
        if (_chunksShown > RootMover._rootPoints.Count ) {
            _chunksShown = RootMover._rootPoints.Count;
        }
        for ( int i = _linePoints.Count; i < _chunksShown; i++ ) {
            Vector3 curVec = RootMover._rootPoints[ i ];
            curVec.x = Remap2 ( curVec.x, _minX, _maxX, -7f, -1f );
            curVec.y = Remap2 ( curVec.y, _minY, _maxY, -4f, 4f );
            curVec.z = 0f;
            _linePoints.Add( curVec );
        }

        //score
        if (_animationDurationCurrent <= _animationDuration * 1f ) { 
            _animationDurationCurrent += Time.deltaTime;
            _scoretext.text = ( RootMover.score * Mathf.FloorToInt ( _animationDurationCurrent / ( _animationDuration * 1f ) * 100f ) / 100 ).ToString ();
        }
        // render new Line
        _renderer.positionCount = _chunksShown;
        _renderer.SetPositions ( _linePoints.ToArray () );
    }


    public static float Remap ( float value, float min, float max, float newMin, float newMax ) {
        if ( Mathf.Approximately ( max, min ) ) {
            return value;
        }
        return newMax + ( value - min ) * ( newMax - newMin ) / ( max - min );
    }

    public float Remap2 ( float value, float oldMin, float oldMax, float newMin, float newMax ) {
        if ( Mathf.Approximately ( oldMax, oldMin ) ) {
            return value;
        }
        float oldRange = ( oldMax - oldMin );
        float newRange = ( newMax - newMin );
        return ( ( ( value - oldMin ) * newRange ) / oldRange ) + newMin;
    }
}
