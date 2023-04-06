using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScripts : MonoBehaviour
{
    public GameObject StartButton;
    public GameObject CreditsButton;
    public GameObject QuitButton;
    public GameObject BackButton;
    public GameObject GameName;
    public GameObject CreditsText;
    public GameObject TextPeople;
    public GameObject Background;
    public GameObject Flower1;
    public GameObject Flower2;



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

        StartButton.SetActive(false);
        QuitButton.SetActive(false);
        CreditsButton.SetActive(false);
        GameName.SetActive(false);
        Background.SetActive(false);
        Flower1.SetActive(false);
        Flower2.SetActive(false);   
        CreditsText.SetActive(true);
        TextPeople.SetActive(true);
        BackButton.SetActive(true);
        TextPeople.SetActive(true);

    }
    public void BackToMenu()
    {

        StartButton.SetActive(true);
        QuitButton.SetActive(true);
        CreditsButton.SetActive(true);
        GameName.SetActive(true);
        Background.SetActive(true);
        Flower1.SetActive(true);
        Flower2.SetActive(true);
        CreditsText.SetActive(false);
        TextPeople.SetActive(false);
        BackButton.SetActive(false);
        TextPeople.SetActive(false);

    }

    public void MainMenu()
    {

        SceneManager.LoadScene("Menu");

    }

}
