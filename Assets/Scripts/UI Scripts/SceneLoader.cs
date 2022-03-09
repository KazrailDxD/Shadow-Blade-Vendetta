using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    public void LoadScene(int load)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(load);

    }
}
