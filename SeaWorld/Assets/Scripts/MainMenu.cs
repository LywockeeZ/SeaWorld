using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void PlayGame()
    {
        SceneChanger.Instance.FadeToNextScene();
        //GameObject.Find("SceneChanger").GetComponent<SceneChanger>().FadeToNextScene();
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
