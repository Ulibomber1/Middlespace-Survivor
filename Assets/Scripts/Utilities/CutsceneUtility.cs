using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneUtility : MonoBehaviour
{
    //Public

    //Private
    float timer;

    void Start()
    {
        timer = 0;
    }
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 56 || Input.anyKeyDown)
            SceneManager.LoadScene("Title Screen");
    }
}
