using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialMenuScript : MonoBehaviour
{
    public void PlayButton()
    {
        SoundManager.Instance.SELECT.Play();

        SceneManager.LoadScene(2);
    }
}
