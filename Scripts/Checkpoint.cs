using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    private CheckPointSystem checkPointSystem;

    private void Start()
    {
        checkPointSystem = FindObjectOfType<CheckPointSystem>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")){
            checkPointSystem.lastCheckPointPos = collision.transform.position;
            checkPointSystem.checkPointDefined = true;
        }
    }
}
