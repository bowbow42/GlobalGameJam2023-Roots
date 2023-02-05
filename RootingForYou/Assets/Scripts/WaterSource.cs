using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSource : MonoBehaviour {

    private float _waterReserveMax = 0f;
    public float _waterReserve = 30f;
    public float _waterFlow = 1f;
    public Vector3 contactPoint;
    private Vector3 _initialPosition;
    private Vector3 _initialScale;

    public bool _isConnectedToRoot = false;

    private SpriteRenderer _renderer;

    private void Start () {
        float randomAngel = Random.Range(0f, 90f);
        //transform.rotation = Quaternion.Euler ( new Vector3 ( 0f, 0f, randomAngel ) );

        _renderer = GetComponent<SpriteRenderer> ();
        //_renderer.flipX = Random.Range ( 0, 101 ) > 50;
        //_renderer.flipY = Random.Range ( 0, 101 ) > 50;

        AddPolygonCollider2D ( gameObject, _renderer.sprite );
        _waterReserveMax = _waterReserve;
        _initialPosition = transform.position;
        _initialScale = transform.localScale;
    }

    private void Update () {
        if ( _isConnectedToRoot ) {
            float percentage = _waterReserve / _waterReserveMax;
            transform.localScale = Vector3.Lerp ( Vector3.zero, _initialScale, percentage );
            transform.position = Vector3.Lerp (  contactPoint, _initialPosition, percentage );
        }
    }

    public static void AddPolygonCollider2D ( GameObject go, Sprite sprite ) {
        PolygonCollider2D polygon = go.AddComponent<PolygonCollider2D> ();

        int shapeCount = sprite.GetPhysicsShapeCount ();
        polygon.pathCount = shapeCount;
        var points = new List<Vector2> ( 64 );
        for ( int i = 0; i < shapeCount; i++ ) {
            sprite.GetPhysicsShape ( i, points );
            polygon.SetPath ( i, points );
        }
    }

    public void PlaySounds () {
        GetComponent<AudioSource> ().Play ();
    }
}
