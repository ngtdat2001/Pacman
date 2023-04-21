using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartAtMenu : MonoBehaviour
{
    private void Update()
    {
        PressToStart();
    }
    private void PressToStart()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
