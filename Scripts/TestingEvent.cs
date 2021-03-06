﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestingEvent : MonoBehaviour
{
    public UnityEvent _event;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            _event.Invoke();
        }
    }


    public void IsGoingInside(bool isInside)
    {
        GameState._isInside = isInside;
        GameState._isOnMetal = false;
    }

    public void IsGoingOnMetal()
    {
        GameState._isOnMetal = true;
        GameState._isInside = false;
    }
}
