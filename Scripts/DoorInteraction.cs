using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction: MonoBehaviour
{
    public Transform _doorSprite;
    Transform _startTransform;
    public float _speed = 1;
    public float _timeToOpen = 1f;
    bool _triggered = false;
    bool _opening = false;
    private void Awake()
    {
        _startTransform = _doorSprite;
    }

    private void Update()
    {
        if (Input.GetKeyDown(GameState._interactButton))
        {
            if (_triggered)
            {
                _opening = true;
                StartCoroutine(RemoveCollider());
            }
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
        _doorSprite.rotation = Quaternion.Lerp(_startTransform.rotation, Quaternion.identity, Time.deltaTime * _speed);
    }

    IEnumerator RemoveCollider()
    {
        yield return new WaitForSeconds(_timeToOpen);
        _opening = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
        Destroy(this);
    }
}
