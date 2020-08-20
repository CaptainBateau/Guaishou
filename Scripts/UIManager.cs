﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    EventSystem ES;

    private void Awake()
    {
        ES = FindObjectOfType<EventSystem>();
    }

    public void LoadSceneNamed(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void LoadSceneIndex(int index)
    {
        SceneManager.LoadScene(index);
    }


    public void ShowObject(GameObject _object)
    {
        _object.SetActive(true);
    }

    public void UnShowObject(GameObject _object)
    {
        _object.SetActive(false);
    }

    public void SetSelection(GameObject _object)
    {
        ES.SetSelectedGameObject(null);
        ES.SetSelectedGameObject(_object);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
