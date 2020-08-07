using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    Rigidbody2D _rb;
    float _horizontalInput;
    bool _upPressed = false;
    public float _force;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetInput();
        if (_upPressed)
        {
            Debug.Log("Je monte");
        }
    }

    private void FixedUpdate()
    {
        if(GameState._isCharacterFlipped == false)
            _rb.AddForce(transform.right * _horizontalInput * Time.fixedDeltaTime * _force);
        else
            _rb.AddForce(transform.right * _horizontalInput * Time.fixedDeltaTime * -_force);
    }


    void GetInput()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(GameState._upButton))
        {
            _upPressed = true;
        }
        if(Input.GetKeyUp(GameState._upButton))
        {
            _upPressed = false;
        }

    }

    public void ChangeUpInput()
    {
        GameState._upButton = KeyCode.Z;
    }
}
