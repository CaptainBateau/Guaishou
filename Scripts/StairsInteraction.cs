using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsInteraction : MonoBehaviour
{
    public Transform _exitStairs;
    bool _triggered = false;

    private void Update()
    {
        if (Input.GetKeyDown(GameState._interactButton))
        {
            if (_triggered)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.transform.position = _exitStairs.position;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            _triggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _triggered = false;
        }
    }
}
