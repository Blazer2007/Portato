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
    [SerializeField] private ScriptableFloat _playerEnergy;
    [SerializeField] private ScriptableFloat _playerCredits;
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

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainScene") && mainMenu.activeSelf == true)
        {
            _startmusic.Play();
            _playingmusic.Stop();
        }
        else if (GameManager.Instance.isgamestarted) 
        {
            _playingmusic.Play();
            _startmusic.Stop();
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
    void ActivateCheat()
    {
        foreach (var pu in _playerUpgrades)
            pu.playerCredits += 10000;

        if (_creditsText != null)
            _creditsText.text = "Points: " + _playerUpgrades[0].playerCredits.ToString();

        Debug.Log("Cheatcode ativado � +10000 creditos!");
    }

    public void Start()
    {

        Debug.Log("GameManager Start chamado, pontos: " + GameEvents.Points);
        _creditsText = GameObject.Find("PlayerCredits").GetComponent<TextMeshProUGUI>();
        Debug.Log("_creditsText encontrado: " + (_creditsText != null));
        PlayerPrefs.DeleteAll();
        UpdatePointsUI(GameEvents.Points);

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainScene"))
        { 
            _actualRunPointsText = GameObject.Find("ActualRunPoints").GetComponent<TextMeshProUGUI>();
            _maxPointsText = GameObject.Find("MaxPoints").GetComponent<TextMeshProUGUI>();
        }
        
        _actualRunPointsText = GameObject.Find("ActualRunPoints").GetComponent<TextMeshProUGUI>();
        _maxPointsText = GameObject.Find("MaxPoints").GetComponent<TextMeshProUGUI>();
        _creditsText = GameObject.Find("PlayerCredits").GetComponent<TextMeshProUGUI>();
        _dashCooldownInfo = GameObject.Find("DashUpgradeInfo").GetComponent<TextMeshProUGUI>();
        _energyEconInfo = GameObject.Find("EnergyUpgradeInfo").GetComponent<TextMeshProUGUI>();
        _slowFallInfo = GameObject.Find("SlowFallUpgradeInfo").GetComponent<TextMeshProUGUI>();
        _actualRunPointsText = GameObject.Find("ActualRunPoints").GetComponent<TextMeshProUGUI>();
        _maxPointsText = GameObject.Find("MaxPoints").GetComponent<TextMeshProUGUI>();

        UpdatePointsUI(GameEvents.Points);
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

    #region Upgrades
    public void BuyEnergyEcon()
    {
        BuyUpgrade(new Upgrade
        {
            index = 0,
            upgradeCount = 3,
            upgradesSelected = _playerUpgrades[0].UpgradesSelected,
            price = _playerUpgrades[0].UpgradeCost,
            playerCredits = _playerUpgrades[0].playerCredits
        });
    }

    public void BuyDashCooldown()
    {
        BuyUpgrade(new Upgrade
        {
            index = 1,
            upgradeCount = 3,
            upgradesSelected = _playerUpgrades[1].UpgradesSelected,
            price = _playerUpgrades[1].UpgradeCost,
            playerCredits = _playerUpgrades[1].playerCredits
        });
    }

    public void BuySlowFall()
    {
        BuyUpgrade(new Upgrade
        {
            index = 2,
            upgradeCount = 1,
            upgradesSelected = _playerUpgrades[2].UpgradesSelected,
            price = _playerUpgrades[2].UpgradeCost,
            playerCredits = _playerUpgrades[2].playerCredits
        });
    }
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
    public void BuyUpgrade(Upgrade upgrade)
    {
        if (upgrade.upgradesSelected >= upgrade.upgradeCount) return;

        int[] multipliers = { 1, 2, 4 };
        int cost = upgrade.price * multipliers[upgrade.upgradesSelected];
        if (index < 0 || index >= _playerUpgrades.Length)
        {
            Debug.Log($"FALHA: index inv�lido. _playerUpgrades.Length = {_playerUpgrades.Length}");
            return;
    public void BuyUpgradeByIndex(int index)
    {
        if (index < 0 || index >= _playerUpgrades.Length) return;

        var pu = _playerUpgrades[index];

        // Verifica se j� comprou o m�ximo
        if (pu.UpgradesSelected >= pu.UpgradeCount) return;

        // Calcula o custo baseado no n�vel atual
        int cost = index == 2
            ? pu.UpgradeCost                          // SlowFall: pre�o fixo
            : pu.UpgradeCost * (int)Mathf.Pow(2, pu.UpgradesSelected); // 1x, 2x, 4x

        // Verifica se tem cr�ditos suficientes
        if (pu.playerCredits < cost) return;

        // Desconta cr�ditos diretamente no ScriptableObject
        pu.playerCredits -= cost;

        // Atualiza texto de cr�ditos
        if (_creditsText != null)
            _creditsText.text = pu.playerCredits.ToString();

        // Aplica o upgrade no CoreManager (efeito + sprite)
        CoreManager.Instance.ApplyUpgrade(index);

        // Atualiza os textos de info
        UpdateUpgradeInfoText(index, pu);
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

        if (GameEvents.Points < cost) return;

        GameEvents.SpendPoints(cost);
        CoreManager.Instance.ApplyUpgrade(upgrade.index);
    }
    #endregion
    public void backtoshop(GameObject caller)
    {
        caller.SetActive(false);
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