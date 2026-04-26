using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public class ButtonManager : MonoBehaviour
{
    [SerializeField] private Canvas mainMenu;

    public void OnClickPlayButton()
    {
        mainMenu.enabled = false;
        SceneManager.LoadScene("SampleScene");
    }
    public void OnClickQuitButton()
    {
        Application.Quit();
    }
    public void OnClickOptionsButton()
    {
        SceneManager.LoadScene("MainOptions");
    }
    public void OnClickBackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
}
