using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreater : MonoBehaviour
{
    private LineRenderer _renderer;

    private float _startTime = 0.0f;
    private List<Vector3> renderPoints;
    [SerializeField]
    private float _animationDuration = 9.0f;
    private float _timeChunks;

    private float _maxX = 0.0f;
    private float _minX = 999999999.0f;
    private float _maxY = 0.0f;
    private float _minY = 999999999.0f;


    // Start is called before the first frame update
    void Start()
    {
        renderer = gameObject.transform.GetComponent<LineRenderer>();
        _startTime = Time.deltaTime;

        _timeChunks = _animationDuration / RootMover._rootPoints.Count;

        // get minimal and Maximal values
        for (int i = 0; i < RootMover._rootPoints.Count; i++)
        {
            if(RootMover._rootPoints[i].x < _maxX ) {
                _maxX = RootMover._rootPoints[i].x;
            }
            if(RootMover._rootPoints[i].x > _minX ) {
                _minX = RootMover._rootPoints[i].x;
            }
            if(RootMover._rootPoints[i].y < _maxY) {
                _maxY = RootMover._rootPoints[i].y;
            }
            if(RootMover._rootPoints[i].y > _minY ) {
                _minY = RootMover._rootPoints[i].y;
            }
        }
    }

    void Update() 
    {
        // time --- starttime --- starttime + animationDuration
        int chunksShown = (int)(((_startTime + _animationDuration) - Time.deltaTime) / _timeChunks) ;
        if(chunksShown > RootMover._rootPoints.Count)
        {
            //chunksShown = RootMover._rootPoints.Count;
            return;
        }

        // transform Ranges
        // x => (_minX, _maxX) => (-7,-1)
        // y => (_minY, _maxY) => (-4, 4)
        List<Vector3> linePoints = RootMover._rootPoints.GetRange(0, chunksShown);
        for (int i = 0; i < chunksShown; i++)
        {
            Vector3 curVec = RootMover._rootPoints[i];
            float x = Remap(curVec.x, _minX, _maxX, -7, -1);
            float y = Remap(curVec.y, _minY, _maxY, -4, 4);
            RootMover._rootPoints[i] = curVec;
        }

        // render new Line
        GetComponent<Renderer>().positionCount = chunksShown;
        GetComponent<Renderer>().SetPositions(linePoints.ToArray());
    }

    public float Remap (float value, float min, float max, float newMin, float newMax){
        if(Mathf.Approximately (max,min)){
            return value;
        }
        return newMax + (value-min) * (newMax - newMin) / (max - min);
    }

}
