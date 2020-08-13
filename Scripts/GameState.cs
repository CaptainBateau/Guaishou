using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState
{
    [HideInInspector]
    public static bool _isCharacterFlipped = false;
    [HideInInspector]
    public static bool _isCloseToDoor = false;
    public static KeyCode _interactButton = KeyCode.E;
}
