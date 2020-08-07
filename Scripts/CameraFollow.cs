using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform _targetToFollow;
    public Vector3 _offset;

    private void Update()
    {
        transform.position = _targetToFollow.position + _offset;
    }
}
