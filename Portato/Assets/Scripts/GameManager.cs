using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject OptionsMenu;
    [SerializeField] private GameObject OptionsMenuPause;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject GameOverMenu;
    [SerializeField] private ScriptableFloat PlayerEnergy;
    public bool isgamestarted;

    void Awake()
    {
        Instance = this;
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
        GameEvents.Reset();
        DeviceTracker.Reset();
        Time.timeScale = 0f;
        isgamestarted = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        GameEvents.OnPlayerDied += () => GameOver(0f);
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerDied -= () => GameOver(0f);
    }
    public void GameOver(float energy)
    {
        if (energy <= 0f)
        {
            StartCoroutine(GameOverDelay());
        }
    }

    private IEnumerator GameOverDelay()
    {
        yield return new WaitForSecondsRealtime(3f);
        GameEvents.Reset();
        DeviceTracker.Reset();
        Time.timeScale = 0f;
        isgamestarted = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
                Pause();
            else if (isgamestarted == false && PauseMenu.activeSelf == true)
                backtogame();
        }
    }
}