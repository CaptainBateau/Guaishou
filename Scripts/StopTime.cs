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
        _pauseEvent.AddListener(StopTimeFct);
        _unpauseEvent.AddListener(RestoreTime);
        Pause();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
                Unpause();
            else
                Pause();
        }
    }

    public void Pause()
    {
        _pauseEvent.Invoke();
    }

    public void Unpause()
    {
        _unpauseEvent.Invoke();
    }

    public void StopTimeFct()
    {
        Time.timeScale = 0;
        _weaponController.enabled = false;
        _isPaused = true;
    }

    public void EndGame()
    {
        CharacterMovement characMove = FindObjectOfType<CharacterMovement>();
        characMove._animator.SetBool("walking", false);
        characMove.enabled = false;
        _weaponController.enabled = false;
        _isPaused = true;
    }

    public void RestoreTime()
    {
        Time.timeScale = 1;
        _weaponController.enabled = true;
        _isPaused = false;
    }
}
