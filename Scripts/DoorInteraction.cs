using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DoorInteraction: MonoBehaviour
{
    public Transform _doorSprite;
    Transform _startTransform;
    public float _speed = 1;
    public float _timeToOpen = 1f;
    bool _triggered = false;
    bool _opening = false;

    public ShadowCaster2D _testShadow;
    ShadowCaster2D _selfShadow;

    private void Awake()
    {
        _startTransform = _doorSprite;
        _selfShadow = gameObject.GetComponent<ShadowCaster2D>();
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
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameState._isCloseToDoor = false;
            _triggered = false;
        }
    }

    void Opening()
    {
        _doorSprite.rotation = Quaternion.Lerp(_startTransform.rotation, Quaternion.identity, Time.deltaTime * _speed);
        if(_testShadow!=null)
            _testShadow.enabled = false;
        if(_selfShadow)
            _selfShadow.enabled = false;
    }

    IEnumerator RemoveCollider()
    {
        yield return new WaitForSeconds(_timeToOpen);
        _opening = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
        Destroy(this);
    }
}
