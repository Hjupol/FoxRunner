using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public static bool pause;
    public GameObject pausePanel;

    void Awake()
    {
        pause = false;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    void Update()
    {
        PauseCheck();
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(0);
        pause = false;
    }

    void PauseCheck()
    {
        if (pause)
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
            if(Input.touches.Length>1 || Input.anyKey)
            {
                RestartLevel();
            }
        }
    }
}
