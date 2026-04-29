using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject OptionsMenu;
    [SerializeField] private GameObject OptionsMenuPause;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject GameOverMenu;
    [SerializeField] private ScriptableFloat PlayerEnergy;
    public bool isgamestarted;

    void Awake()
    {
        Time.timeScale = 0f;
        isgamestarted = false;

        OptionsMenu.SetActive(false);
        OptionsMenuPause.SetActive(false);
        PauseMenu.SetActive(false);
        GameOverMenu.SetActive(false);
    }

    public void PLay()
    {
        Time.timeScale = 1f;
        isgamestarted = true;

        mainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        PauseMenu.SetActive(false);
        OptionsMenuPause.SetActive(false);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        isgamestarted = false;

        PauseMenu.SetActive(true);
        mainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        OptionsMenuPause.SetActive(false);
    }

    public void Options()
    {
        Time.timeScale = 0f;
        isgamestarted = false;

        OptionsMenu.SetActive(true);
        mainMenu.SetActive(false);
        PauseMenu.SetActive(false);
        OptionsMenuPause.SetActive(false);
    }

    public void OptionsP()
    {
        Time.timeScale = 0f;
        isgamestarted = false;

        OptionsMenuPause.SetActive(true);
        mainMenu.SetActive(false);
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void backtomainmenu()
    {
        Time.timeScale = 0f;
        isgamestarted = false;

        mainMenu.SetActive(true);
        OptionsMenu.SetActive(false);
        PauseMenu.SetActive(false);
        OptionsMenuPause.SetActive(false);
    }

    public void backtogame()
    {
        Time.timeScale = 1f;
        isgamestarted = true;

        mainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        PauseMenu.SetActive(false);
        OptionsMenuPause.SetActive(false);
    }
    public void backtopause()
    {
        Time.timeScale = 0f;
        isgamestarted = false;

        PauseMenu.SetActive(true);
        mainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        OptionsMenuPause.SetActive(false);
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    public void GameOver()
    {
            Time.timeScale = 0f;
            isgamestarted = false;

            GameOverMenu.SetActive(true);
            mainMenu.SetActive(false);
            OptionsMenu.SetActive(false);
            PauseMenu.SetActive(false);
            OptionsMenuPause.SetActive(false);
    }

    public void Shop()
    {
        SceneManager.LoadScene("Shop");
    }

    public void BackFromShop()
    {
        SceneManager.LoadScene("MainScene");
    }


    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isgamestarted == true)
            {
                Pause();
            }
            else if (isgamestarted == false && PauseMenu.activeSelf == true)
            {
                backtogame();
            }
            
        }
    }

    
}
