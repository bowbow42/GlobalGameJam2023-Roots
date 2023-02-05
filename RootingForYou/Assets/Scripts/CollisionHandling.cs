using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandling : MonoBehaviour {

    private RootMover _rootMover;
    
    // Start is called before the first frame update
    void Start () {
        _rootMover = transform.parent.gameObject.GetComponent<RootMover> ();
    }

    private void OnCollisionEnter2D ( Collision2D collision ) {
        if ( collision.collider.CompareTag ( "WaterSource" ) ) {
            WaterSource source = collision.collider.gameObject.GetComponent<WaterSource> ();
            if ( !source._isConnectedToRoot ) {
                _rootMover.listOfWaterSources.Add ( source );
                source._isConnectedToRoot = true;
                source.contactPoint = new Vector3(collision.GetContact ( 0 ).point.x, collision.GetContact ( 0 ).point.y, 0f);
            }
        }
    }
}
