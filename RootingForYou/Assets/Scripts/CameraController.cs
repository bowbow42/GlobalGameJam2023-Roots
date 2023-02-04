using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField]
    private Transform _referenceTransform;
    [SerializeField]
    private float _initialYoffset;

    private Camera _cam;

    // Start is called before the first frame update
    void Start () {
        _cam = Camera.main;
    }

    // Update is called once per frame
    void Update () {
        float moveY = _referenceTransform.position.y + _initialYoffset;
        if (moveY < 0 && _cam.transform.position.y > moveY) {
            _cam.transform.position = new Vector3 (0f, moveY, -10f);
        }
    }

    public bool IsInFrustum ( Vector3 position ) {
        position = Camera.main.WorldToViewportPoint ( position, Camera.MonoOrStereoscopicEye.Mono );
        return ( position.x > 0f && position.x < 1f && position.y < 1f );
    }

}
