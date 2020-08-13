﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputAssigner : MonoBehaviour
{
    public KeyCode _interaction;

    private void OnValidate()
    {
        if (_interaction != GameState._interactButton)
            GameState._interactButton = _interaction;
    }
}
