using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState
{
    [HideInInspector]
    public static bool _isCharacterFlipped = false;
    [HideInInspector]
    public static bool _isCloseToDoor = false;
    [HideInInspector]
    public static bool _isReloading = false;
    public static KeyCode _interactButton = KeyCode.E;
    public static bool _isInside = false;
    public static bool _isOnMetal = false;


    
}
