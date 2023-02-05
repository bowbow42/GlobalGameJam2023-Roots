using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Obstacle : MonoBehaviour {

    SpriteRenderer _renderer;

    // Start is called before the first frame update
    void Start () {

        _renderer = GetComponent<SpriteRenderer> ();
        //_renderer.flipX = Random.Range ( 0, 101 ) > 50;
        //_renderer.flipY = Random.Range ( 0, 101 ) > 50;

        WaterSource.AddPolygonCollider2D ( gameObject, _renderer.sprite );
    }

}
