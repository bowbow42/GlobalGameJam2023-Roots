using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSource : MonoBehaviour {

    public float _waterReserve = 30;

    private SpriteRenderer _renderer;

    private void Start () {
        float randomAngel = Random.Range(0f, 90f);
        transform.rotation = Quaternion.Euler ( new Vector3 ( 0f, 0f, randomAngel ) );

        _renderer = GetComponent<SpriteRenderer> ();
        //_renderer.flipX = Random.Range ( 0, 101 ) > 50;
        //_renderer.flipY = Random.Range ( 0, 101 ) > 50;

        AddPolygonCollider2D ( gameObject, _renderer.sprite );
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
}
