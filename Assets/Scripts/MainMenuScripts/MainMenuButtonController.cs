using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuButtonController : MonoBehaviour
{
    private bool InfoActive = false;

    public GameObject ControllerInfoTextObject;
    public GameObject PlayButtonObject;
    public GameObject InfoButtonObject;
    public GameObject MenuButtonObject;
    public GameObject QuitButtonObject;
    public GameObject InfoTextObject;

    public GameObject BackgroundCover1;
    public GameObject BackgroundCover2;

    public AudioSource MainMenuScriptAudioSource;
    public AudioClip[] MainMenuScriptAudioClipArray;

    public TextMeshProUGUI SwitchingCreditsText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
        StartCoroutine(SwitchingCreditsTextScript());
    }

    // Update is called once per frame
    void Update()
    {
        // PS4 Triangle-button
        if (Input.GetKeyDown(KeyCode.Joystick1Button3) && InfoActive == false)
        {
            Debug.Log("Play painettu active");
            SceneManager.LoadScene(1);
        }

        // PS4 Square-button
        if (Input.GetKeyDown(KeyCode.Joystick1Button0) && InfoActive == false)
        {
            Debug.Log("Info active");
            MainMenuScriptAudioSource.PlayOneShot(OpenInfo());
            InfoActive = true;
            Info();
        }

        // PS4 O-button
        if (Input.GetKeyDown(KeyCode.Joystick1Button2) && InfoActive == true)
        {
            Debug.Log("Main back");
            MainMenuScriptAudioSource.PlayOneShot(OpenInfo());
            InfoActive = false;
            Menu();
        }


        // PS4 X-button QUIT
        if (Input.GetKeyDown(KeyCode.Joystick1Button1) && InfoActive == false)
        {
            Debug.Log("QUIT funktio");
            QuitGame();
        }
    }

    public void Info()
    {
        BackgroundCover1.SetActive(false);
        BackgroundCover2.SetActive(true);
        ControllerInfoTextObject.SetActive(false);

        InfoTextObject.SetActive(true);

        PlayButtonObject.SetActive(false);
        InfoButtonObject.SetActive(false);

        MenuButtonObject.SetActive(true);

        QuitButtonObject.SetActive(false);
    }

    public void Menu()
    {
        BackgroundCover1.SetActive(true);
        BackgroundCover2.SetActive(false);
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

    AudioClip OpenInfo()
    {
        return MainMenuScriptAudioClipArray[Random.Range(0, 1)];
    }


    IEnumerator SwitchingCreditsTextScript()
    {
        yield return new WaitForSeconds(5f);
        SwitchingCreditsText.text = "Indy Blow Darts Cue 3 by pscsound (Attribution 3.0)";
        yield return new WaitForSeconds(5f);
        SwitchingCreditsText.text = "Bubbles4 by kwahmah_02 (Attribution 3.0)";
        yield return new WaitForSeconds(5f);
        SwitchingCreditsText.text = "jump sound by tramppa34 (CC0)";
        yield return new WaitForSeconds(5f);
        SwitchingCreditsText.text = "03-Dolor by ZeritFreeman (CC0)";
        yield return new WaitForSeconds(5f);
        SwitchingCreditsText.text = "multiple bubbles bursting by florianreichelt (CC0)";
        yield return new WaitForSeconds(5f);
        SwitchingCreditsText.text = "Game Pickup by IENBA (CC0)";
        yield return new WaitForSeconds(5f);
        SwitchingCreditsText.text = "Input Prompts by Kenney (CC0)";
        yield return new WaitForSeconds(5f);
        SwitchingCreditsText.text = "Door Handle Wood Clack.aif by RutgerMuller (CC0)";
        yield return new WaitForSeconds(5f);
        SwitchingCreditsText.text = "The Big Heist by Geoff Harvey – Pixabay (Pixabay license) ";
        StartCoroutine(SwitchingCreditsTextScript());
    }
}