using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointSystem : MonoBehaviour
{
    private static CheckPointSystem instance;
    public Vector2 lastCheckPointPos;
    [HideInInspector] public bool checkPointDefined = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    private void Start()
    {
        lastCheckPointPos = FindObjectOfType<PlayerEvent>().transform.position;
    }
}
