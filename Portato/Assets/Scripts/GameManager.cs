using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
public class Upgrade 
{
    public int index, upgradeCount, upgradesSelected, price, playerCredits;
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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
    [SerializeField] private TextMeshProUGUI _actualRunPointsText;
    [SerializeField] private TextMeshProUGUI _maxPointsText;
    [SerializeField] private PlayerUpgrades[] _playerUpgrades;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private TextMeshProUGUI _creditsText;
    [SerializeField] private TextMeshProUGUI _dashCooldownInfo;
    [SerializeField] private TextMeshProUGUI _energyEconInfo;
    [SerializeField] private TextMeshProUGUI _slowFallInfo;
    [SerializeField] private AudioSource _startmusic;
    [SerializeField] private AudioSource _playingmusic;
    
    //private PlayerUpgrades _slowFallUpgrade;
    //private PlayerUpgrades _dashCooldownUpgrade;
    //private PlayerUpgrades _energyEconUpgrade;
    public bool isgamestarted;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 0f;
        isgamestarted = false;
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainScene")) 
        {
            OptionsMenu.SetActive(false);
            OptionsMenuPause.SetActive(false);
            PauseMenu.SetActive(false);
            GameOverMenu.SetActive(false);
              
        }
        // Reset de todos os upgrades ao iniciar
        foreach (var pu in _playerUpgrades)
        {
            pu.UpgradesSelected = 0;
            pu.playerCredits = pu.startingCredits; // valor base definido no ScriptableObject
        }

        if (_creditsText != null)
            _creditsText.text = _playerUpgrades[0].playerCredits.ToString();

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainScene"))
        {
            OptionsMenu.SetActive(false);
            OptionsMenuPause.SetActive(false);
            PauseMenu.SetActive(false);
            GameOverMenu.SetActive(false);
        }

        if (mainMenu.activeSelf == true)
        {
            _startmusic.Play();
        }

        
         
    }
    private KeyCode[] _cheatCode = { KeyCode.B, KeyCode.A, KeyCode.T, KeyCode.A, KeyCode.T, KeyCode.A };
    private int _cheatIndex = 0;
    void Update()
    {
        if (GameOverMenu.activeSelf == true)
        {
            _playingmusic.Stop();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isgamestarted) Pause();
            else if (!isgamestarted && PauseMenu.activeSelf) backtogame();
        }

        // Cheatcode
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(_cheatCode[_cheatIndex]))
            {
                _cheatIndex++;
                if (_cheatIndex >= _cheatCode.Length)
                {
                    ActivateCheat();
                    _cheatIndex = 0;
                }
            }
            else
            {
                _cheatIndex = 0; // reset se errar uma tecla
            }
        }
    }
    void ActivateCheat()
    {
        foreach (var pu in _playerUpgrades)
            pu.playerCredits += 10000;

        if (_creditsText != null)
            _creditsText.text = _playerUpgrades[0].playerCredits.ToString();

        Debug.Log("Cheatcode ativado � +10000 cr�ditos!");
    }

    public void Start()
    {
       
        Debug.Log("GameManager Start chamado, pontos: " + GameEvents.Points);
        _creditsText = GameObject.Find("PlayerCredits").GetComponent<TextMeshProUGUI>();
        Debug.Log("_creditsText encontrado: " + (_creditsText != null));
        PlayerPrefs.DeleteAll();
        UpdatePointsUI(GameEvents.Points);
         

        _creditsText = GameObject.Find("PlayerCredits").GetComponent<TextMeshProUGUI>();
        _dashCooldownInfo = GameObject.Find("DashUpgradeInfo").GetComponent<TextMeshProUGUI>();
        _energyEconInfo = GameObject.Find("EnergyUpgradeInfo").GetComponent<TextMeshProUGUI>();
        _slowFallInfo = GameObject.Find("SlowFallUpgradeInfo").GetComponent<TextMeshProUGUI>();
        _actualRunPointsText = GameObject.Find("ActualRunPoints").GetComponent<TextMeshProUGUI>();
        _maxPointsText = GameObject.Find("MaxPoints").GetComponent<TextMeshProUGUI>();

        
    }
    #region Main methods
    public void PLay()
    {
        Time.timeScale = 1f;
        isgamestarted = true;
        mainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        PauseMenu.SetActive(false);
        OptionsMenuPause.SetActive(false);
        _startmusic.Stop();
        _playingmusic.Play();
    }
    public void Pause()
    {
        Time.timeScale = 0f;
        isgamestarted = false;
        PauseMenu.SetActive(true);
        mainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        OptionsMenuPause.SetActive(false);
         _playingmusic.Stop();
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
        GameEvents.Reset();
        DeviceTracker.Reset();
        Time.timeScale = 0f;
        isgamestarted = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
         _playingmusic.Stop();
    }
    public void backtogame()
    {
        Time.timeScale = 1f;
        isgamestarted = true;
        mainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        PauseMenu.SetActive(false);
        OptionsMenuPause.SetActive(false);
        _playingmusic.Play();
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
    private void OnEnable()
    {
        GameEvents.OnPlayerDied += HandlePlayerDied;
        GameEvents.OnPointsChanged += UpdatePointsUI;
    }
    private void OnDisable()
    {
        GameEvents.OnPlayerDied -= HandlePlayerDied;
        GameEvents.OnPointsChanged -= UpdatePointsUI;
    }
    private void HandlePlayerDied() => StartCoroutine(GameOverDelay());
    void UpdatePointsUI(int points)
    {
        if (_actualRunPointsText != null)
            _actualRunPointsText.text = $"Actual Run: {points}";
        if (_maxPointsText != null)
            _maxPointsText.text = $"Record: {PlayerPrefs.GetInt("MaxPoints", 0)}";
    }
    #region Upgrades

    public void BuyUpgradeByIndex(int index)
    {
        Debug.Log($"BuyUpgradeByIndex chamado com index: {index}");

        if (index < 0 || index >= _playerUpgrades.Length)
        {
            Debug.Log($"FALHA: index inv�lido. _playerUpgrades.Length = {_playerUpgrades.Length}");
            return;
        }

        var pu = _playerUpgrades[index];
        Debug.Log($"Upgrade: {pu.name} | Selecionados: {pu.UpgradesSelected} | M�ximo: {pu.UpgradeCount}");

        if (pu.UpgradesSelected >= pu.UpgradeCount)
        {
            Debug.Log("FALHA: upgrade j� no m�ximo");
            return;
        }

        int cost = index == 2
            ? pu.UpgradeCost
            : pu.UpgradeCost * (int)Mathf.Pow(2, pu.UpgradesSelected);

        Debug.Log($"Custo: {cost} | Cr�ditos: {pu.playerCredits}");

        if (pu.playerCredits < cost)
        {
            Debug.Log("FALHA: cr�ditos insuficientes");
            return;
        }

        pu.playerCredits -= cost;
        if (_creditsText != null) _creditsText.text = pu.playerCredits.ToString();

        Debug.Log("A chamar CoreManager.Instance.ApplyUpgrade...");
        CoreManager.Instance.ApplyUpgrade(index);
    }

    void UpdateUpgradeInfoText(int index, PlayerUpgrades pu)
    {
        TextMeshProUGUI infoText = index switch
        {
            0 => _energyEconInfo,
            1 => _dashCooldownInfo,
            2 => _slowFallInfo,
            _ => null
        };

        if (infoText == null) return;

        if (pu.UpgradesSelected >= pu.UpgradeCount)
            infoText.text = "MAX";
        else
        {
            int nextCost = index == 2
                ? pu.UpgradeCost
                : pu.UpgradeCost * (int)Mathf.Pow(2, pu.UpgradesSelected);
            infoText.text = $"N�vel {pu.UpgradesSelected}/{pu.UpgradeCount} � Pr�ximo: {nextCost}";
        }
    }
    #endregion
    public void backtoshop(GameObject caller)
    {
        caller.SetActive(false);
    }
    private IEnumerator GameOverDelay()
    {
       
        _playingmusic.Stop();
        yield return new WaitForSecondsRealtime(3f);
        GameEvents.Reset();
        DeviceTracker.Reset();
        Time.timeScale = 0f;
        isgamestarted = false;
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
