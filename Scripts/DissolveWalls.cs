using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveWalls : MonoBehaviour
{

    public SpriteRenderer _renderer;
    public float _duration;



    public void LaunchDissolveLeftToRight()
    {
        StartCoroutine(DissolveLeftToRight(_renderer));
    }


    public void LaunchDissolveRightToLeft()
    {
        StartCoroutine(DissolveRightToLeft(_renderer));
    }


    public void LaunchRessolveLeftToRight()
    {
        StartCoroutine(RessolveLeftToRight(_renderer));
    }


    public void LaunchRessolveRightToLeft()
    {
        StartCoroutine(RessolveRightToLeft(_renderer));
    }


    IEnumerator DissolveLeftToRight(SpriteRenderer rend)
    {
        float timer = 0;
        while(timer < _duration)
        {
            timer += Time.deltaTime;
            rend.material.SetFloat("_Fade", Mathf.Lerp(1,-1,timer/_duration));
            yield return null;
        }
    }


    IEnumerator RessolveLeftToRight(SpriteRenderer rend)
    {
        float timer = 0;
        while (timer < _duration)
        {
            timer += Time.deltaTime;
            rend.material.SetFloat("_Fade", Mathf.Lerp(-1, 1, timer / _duration));
            yield return null;
        }
    }
    IEnumerator DissolveRightToLeft(SpriteRenderer rend)
    {
        float timer = 0;
        while (timer < _duration)
        {
            timer += Time.deltaTime;
            rend.material.SetFloat("_Fade", Mathf.Lerp(-1, 1, timer / _duration));
            yield return null;
        }
    }

    IEnumerator RessolveRightToLeft(SpriteRenderer rend)
    {
        float timer = 0;
        while (timer < _duration)
        {
            timer += Time.deltaTime;
            rend.material.SetFloat("_Fade", Mathf.Lerp(1, -1, timer / _duration));
            yield return null;
        }
    }
}
