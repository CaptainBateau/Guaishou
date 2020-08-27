using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StopTime : MonoBehaviour
{
    WeaponController _weaponController;
    bool _isPaused;

    public UnityEvent _pauseEvent;
    public UnityEvent _unpauseEvent;

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
        _pauseEvent.Invoke();
    }

    public void RestoreTimeScale()
    {
        Time.timeScale = 1;
        _weaponController.enabled = true;
        _isPaused = false;
        _unpauseEvent.Invoke();
    }
}
