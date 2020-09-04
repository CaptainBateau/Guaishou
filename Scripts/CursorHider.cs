using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHider : MonoBehaviour
{
    private void Awake()
    {
        Cursor.visible = false;
    }
}
