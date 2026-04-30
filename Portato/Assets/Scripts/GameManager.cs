using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
public class Upgrade 
{
    public int index, upgradeCount, upgradesSelected, price, playerCredits;
}
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject OptionsMenu;
    [SerializeField] private GameObject OptionsMenuPause;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject GameOverMenu;
    [SerializeField] private ScriptableFloat PlayerEnergy;
    [SerializeField] private CoreManager _coreManager;
    [SerializeField] private GameObject _dashCoolDownPanel;
    [SerializeField] private GameObject _EnergyEconPanel;
    [SerializeField] private GameObject _SlowFallPanel;
    [SerializeField] private PlayerUpgrades[] _playerUpgrades;
    [SerializeField] private PlayerController _playerController;

    //private PlayerUpgrades _slowFallUpgrade;
    //private PlayerUpgrades _dashCooldownUpgrade;
    //private PlayerUpgrades _energyEconUpgrade;
    public bool isgamestarted;

    void Awake()
    {
        Time.timeScale = 0f;
        isgamestarted = false;
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainScene")) 
        {
            OptionsMenu.SetActive(false);
            OptionsMenuPause.SetActive(false);
            PauseMenu.SetActive(false);
            GameOverMenu.SetActive(false); 
        }
        
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

    //public void Start()
    //{
    //    _energyEconUpgrade = _playerUpgrades[0];
    //    _dashCooldownUpgrade = _playerUpgrades[1];
    //    _slowFallUpgrade = _playerUpgrades[2];
    //}
    #region Main methods
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
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
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
    #endregion

    public void BuyUpgrade(Upgrade upgrade)
    {
        if (upgrade.upgradesSelected == upgrade.upgradeCount) return;

        if (upgrade.upgradesSelected < upgrade.upgradeCount)
        {
            if (upgrade.index == 2 && upgrade.playerCredits >= upgrade.price)
            {
                upgrade.playerCredits -= upgrade.price;
            }
            else
            {
                switch (upgrade.upgradesSelected)
                {
                    case 0:
                        upgrade.playerCredits -= upgrade.price;
                        break;
                    case 1:
                        upgrade.playerCredits -= upgrade.price * 2;
                        break;
                    case 2:
                        upgrade.playerCredits -= upgrade.price * 4;
                        break;
                }
            }
        }
    }
    public void backtoshop(GameObject caller) 
    {
        caller.SetActive(false);
    }
    public void Shop()
    {
        SceneManager.LoadScene("Shop");
    }
    public void BackFromShop()
    {
        SceneManager.LoadScene("MainScene");
    }

    #region Panels
    public void OpenEnergyEconPanel()
    {
        _EnergyEconPanel.SetActive(true);
    }
    public void OpenDashCooldownPanel()
    {
        _dashCoolDownPanel.SetActive(true);
    }
    public void OpenSlowFallPanel()
    {
        _SlowFallPanel.SetActive(true);
    }
    #endregion
}
