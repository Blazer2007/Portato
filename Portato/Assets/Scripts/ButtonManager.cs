using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public class ButtonManager : MonoBehaviour
{
    [SerializeField] private Canvas mainMenu;
    public static bool Play;

    public void OnClickPlayButton()
    {
        Play = true;
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
