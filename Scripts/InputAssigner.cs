using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputAssigner : MonoBehaviour
{
    public KeyCode _interaction;


    private void Awake()
    {
        if (_interaction != GameState._interactButton && _interaction != KeyCode.None)
            GameState._interactButton = _interaction;
    }

    private void OnValidate()
    {
        if (_interaction != GameState._interactButton)
            GameState._interactButton = _interaction;
    }
}
