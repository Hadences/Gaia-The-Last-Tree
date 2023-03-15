using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    public void HomeButton()
    {
        SoundManager.Instance.SELECT.Play();

        SceneManager.LoadScene(0);
    }
}
