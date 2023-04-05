using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScripts : MonoBehaviour
{

    public void StartGame()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }

    public void QuitGame()
    {

        Application.Quit();

    }

    public void Credits()
    {

        SceneManager.LoadScene("Credits");

    }
    public void MainMenu()
    {

        SceneManager.LoadScene("Menu");

    }

}
