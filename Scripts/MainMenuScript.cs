using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuScript : MonoBehaviour {

    [SerializeField]
    private AudioMaster AudioM;

    public void Start()
    {
        AudioM.PlaySound("BackgroundMusic");
    }

    public void StartGame()
    {
        AudioM.StopSound("BackgroundMusic");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    
}
