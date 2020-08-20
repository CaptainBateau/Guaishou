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
    bool _closing = false;
    bool _opened = false;
    bool _isPlayerLeftSide;
    public Vector3 _openRotation;
    public Vector3 _closeRotation;

    public ShadowCaster2D _leftShadow;
    public ShadowCaster2D _rightShadow;
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
                PlayerSideOfDoor();
                Debug.Log(_isPlayerLeftSide);
                if (!_opened)
                {
                    _opening = true;
                    StartCoroutine(RemoveCollider());
                }
                else
                {
                    _closing = true;
                    StartCoroutine(ReactiveCollider());
                }
            }
        }
        if (_opening)
            Opening();
        if (_closing)
            Closing();
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
        _doorSprite.eulerAngles = _openRotation;
        //_doorSprite.rotation = Quaternion.Lerp(_startTransform.rotation, Quaternion.identity, Time.deltaTime * _speed);
        if (_isPlayerLeftSide)
            _rightShadow.enabled = false;
        else
            _leftShadow.enabled = false;
        if(_selfShadow)
            _selfShadow.enabled = false;
    }

    void Closing()
    {
        _doorSprite.eulerAngles = _closeRotation;
        //_doorSprite.rotation = Quaternion.Lerp(Quaternion.identity, _startTransform.rotation, Time.deltaTime * _speed);
        if (_isPlayerLeftSide)
            _rightShadow.enabled = true;
        else
            _leftShadow.enabled = true;
        if (_selfShadow)
            _selfShadow.enabled = true;
    }

    void PlayerSideOfDoor()
    {
        GameObject player = GameObject.FindWithTag("Player");
        float test = _doorSprite.position.x - player.transform.position.x;
        _isPlayerLeftSide = test > 0 ? true : false;
    }


    IEnumerator RemoveCollider()
    {
        yield return new WaitForSeconds(_timeToOpen);
        _opening = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
        _opened = true;
        //Destroy(this);
    }

    IEnumerator ReactiveCollider()
    {
        yield return new WaitForSeconds(_timeToOpen);
        _closing = false;
        gameObject.GetComponent<Collider2D>().enabled = true;
        _opened = false;
    }
}
