using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenuButtonController : MonoBehaviour
{
    private bool InfoActive = false;

    public GameObject ControllerInfoTextObject;
    public GameObject PlayButtonObject;
    public GameObject InfoButtonObject;
    public GameObject MenuButtonObject;
    public GameObject QuitButtonObject;
    public GameObject InfoTextObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Xbox Y-button
        if (Input.GetKeyDown(KeyCode.Joystick2Button3) && InfoActive == false)
        {
            SceneManager.LoadScene(1);
        }

        // Xbox X-button
        if (Input.GetKeyDown(KeyCode.Joystick2Button2) && InfoActive == false)
        {
            InfoActive = true;
            Info();
        }

        // Xbox A-button
        if (Input.GetKeyDown(KeyCode.Joystick2Button0) && InfoActive == true)
        {
            InfoActive = false;
            Menu();
        }


        // Xbox A-button QUIT
        if (Input.GetKeyDown(KeyCode.Joystick2Button0) && InfoActive == false)
        {
            QuitGame();
        }
    }

    public void Info()
    {
        ControllerInfoTextObject.SetActive(false);

        InfoTextObject.SetActive(true);

        PlayButtonObject.SetActive(false);
        InfoButtonObject.SetActive(false);

        MenuButtonObject.SetActive(true);

        QuitButtonObject.SetActive(false);
    }

    public void Menu()
    {
        ControllerInfoTextObject.SetActive(true);

        InfoTextObject.SetActive(false);

        PlayButtonObject.SetActive(true);
        InfoButtonObject.SetActive(true);

        MenuButtonObject.SetActive(false);

        QuitButtonObject.SetActive(true);
    }

    public void QuitGame()
    {
        // Quit game
        Application.Quit();
    }
}
