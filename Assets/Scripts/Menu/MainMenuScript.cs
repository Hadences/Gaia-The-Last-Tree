using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void StartButton()
    {
        //go to game
        SoundManager.Instance.SELECT.Play();

        SceneManager.LoadScene(1); //load game
    }

    public void QuitButton()
    {
        SoundManager.Instance.SELECT.Play();

        Application.Quit();
    }
}
