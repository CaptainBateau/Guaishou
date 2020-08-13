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
    }

    private void FixedUpdate()
    {
        //if(GameState._isCharacterFlipped == false)
            _rb.AddForce(transform.right * _horizontalInput * Time.fixedDeltaTime * _force);
        //else
          //  _rb.AddForce(transform.right * _horizontalInput * Time.fixedDeltaTime * -_force);
    }


    void GetInput()
    {
        _horizontalInput = Input.GetAxis("Horizontal");

    }

    
}
