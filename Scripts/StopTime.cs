using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopTime : MonoBehaviour
{
    WeaponController _weaponController;
    bool _isPaused;

    private void Awake()
    {
        _weaponController = FindObjectOfType<WeaponController>();
        StopTimeScale();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
                RestoreTimeScale();
            else
                StopTimeScale();
        }
    }

    public void StopTimeScale()
    {
        Time.timeScale = 0;
        _weaponController.enabled = false;
        _isPaused = true;
    }

    public void RestoreTimeScale()
    {
        Time.timeScale = 1;
        _weaponController.enabled = true;
        _isPaused = false; 
    }
}
