using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction: MonoBehaviour
{
    public Transform _doorSprite;
    Transform _startTransform;
    public float _speed = 1;
    bool _triggered = false;
    bool _opening = false;
    private void Awake()
    {
        _startTransform = _doorSprite;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_triggered)
                _opening = true;
        }
        if (_opening)
            Opening();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameState._isCloseToDoor = true;
            _triggered = true;
            Debug.Log("open");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameState._isCloseToDoor = false;
            _triggered = false;
            Debug.Log("close");
        }
    }

    void Opening()
    {
        _doorSprite.rotation = Quaternion.Lerp(_startTransform.rotation, Quaternion.identity, Time.time * _speed);
        if(_doorSprite.rotation == Quaternion.identity)
        {
            _opening = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }
}
