using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    Rigidbody2D _rb;
    float _horizontalInput;
    public float _force;
    public Animator _animator;
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
        if (GameState._isReloading == false)
        {
            _rb.AddForce(transform.right * _horizontalInput * Time.fixedDeltaTime * _force);
        }
        //else
          //  _rb.AddForce(transform.right * _horizontalInput * Time.fixedDeltaTime * -_force);
    }


    void GetInput()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        if (Mathf.Abs(_horizontalInput) > 0.1f)
        {
            _animator.SetBool("walking", true);
        }
        else
            _animator.SetBool("walking", false);
    }

}
