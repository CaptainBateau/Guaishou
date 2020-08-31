using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform _targetToFollow;
    public Vector3 _offset;

    Vector3 _oldOffset;
    Vector3 _newOffset;
    float _endTime;
    int _currentIndex = -1;

    [System.Serializable]
    public struct OffsetStruct
    {
        public float _transitionTime;
        public Vector3 _offset;
    }
    public OffsetStruct[] _offsets;

    private void Start()
    {
        _endTime = -2;
        _oldOffset = _offset;
    }


    private void Update()
    {
        if (Time.time - _endTime <= 0.02f)
            MovingOffset();
        transform.position = _targetToFollow.position + _offset;
    }



    void MovingOffset()
    {
        _offset = new Vector3(Mathf.Lerp(_newOffset.x, _oldOffset.x, (_endTime - Time.time) / _offsets[_currentIndex]._transitionTime), Mathf.Lerp(_newOffset.y, _oldOffset.y, (_endTime - Time.time) / _offsets[_currentIndex]._transitionTime), Mathf.Lerp(_newOffset.z, _oldOffset.z, (_endTime - Time.time) / _offsets[_currentIndex]._transitionTime));
    }

    public void ChangeOffset(int index)
    {
        if (_currentIndex !=-1)
            _oldOffset = _offsets[_currentIndex]._offset;
        _newOffset = _offsets[index]._offset;
        _endTime = Time.time + _offsets[index]._transitionTime;
        _currentIndex = index;
    }

    public void NextOffset()
    {
        ChangeOffset((_currentIndex + 1) % _offsets.Length);
    }
}
