using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandling : MonoBehaviour {

    private RootMover _rootMover;
    
    // Start is called before the first frame update
    void Start () {
        _rootMover = transform.parent.gameObject.GetComponent<RootMover> ();
    }

    // Update is called once per frame
    void Update () {

    }


    private void OnCollisionEnter2D ( Collision2D collision ) {
        if ( collision.collider.CompareTag ( "WaterSource" ) ) {
            
        }
    }
}
