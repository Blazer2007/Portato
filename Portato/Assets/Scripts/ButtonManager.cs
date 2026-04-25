using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonManager : MonoBehaviour
{
    
    public void OnClickPlayButton()
    {
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
