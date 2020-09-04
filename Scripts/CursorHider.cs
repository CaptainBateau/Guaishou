using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHider : MonoBehaviour
{


    public void ActivateCursor()
    {
        Cursor.visible = true;
        
    }

    public void DeactivateCursor()
    {
        Cursor.visible = false;

    }
}
