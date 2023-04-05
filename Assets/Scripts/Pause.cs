using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject PausePanel;
    bool paused;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PausePanel.SetActive(true); 
            paused = true;
        }

        if(paused == true)
        {
            Time.timeScale = 0;
        }
        else if(paused == false) 
        { 
            Time.timeScale = 1; 
        }
    }

    public void ResumeButton()
    { 
        PausePanel.SetActive(false);
        paused = false;
    }
}
