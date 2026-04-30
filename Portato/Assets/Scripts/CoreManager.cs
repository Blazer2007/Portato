using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CoreManager : MonoBehaviour
{
    public static CoreManager Instance;

    [Header("Refer�ncias")]
    [SerializeField] Rigidbody2D _playerRB;
    [SerializeField] ChunkSpawner spawner;
    [SerializeField] Slider energyBar;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] PlayerController _playerController;
    [SerializeField] PlayerUpgrades[] _upgrades;
    [SerializeField] Button _energyButton;
    [SerializeField] Button _dashButton;
    [SerializeField] Button _slowFallButton;
    [SerializeField] Image[] _energyImage;
    [SerializeField] Image[] _dashImage;
    [SerializeField] Image[] _slowFallImage;

    [Header("Upgrades")]
    public float consumoMult = 1f;
    [Range(1f, 5f)] public float dashCooldown = 5f;   // segundos entre dashes (diminui com upgrades)
    public float dashForce = 15f;  // for�a do dash para tr�s
    public float floatDuration = 0f;

    float floatTimer = 0f;

    void Awake() => Instance = this; 

    void OnEnable()
    {
        GameEvents.OnEnergyChanged += UpdateEnergyUI;
        GameEvents.OnPlayerDied += TriggerDeath;
    }
    void OnDisable()
    {
        GameEvents.OnEnergyChanged -= UpdateEnergyUI;
        GameEvents.OnPlayerDied -= TriggerDeath;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) GameEvents.RechargeEnergy(100f);

        if (floatDuration > 0 && Input.GetKeyDown(KeyCode.Space) && floatTimer <= 0)
        {
            floatTimer = floatDuration;
            _playerRB.gravityScale = 0f;
        }
        if (floatTimer > 0)
        {
            floatTimer -= Time.deltaTime;
            if (floatTimer <= 0) _playerRB.gravityScale = 1f;
        }
    }

    void UpdateEnergyUI(float val) => energyBar.value = val / 100f;

    public void TriggerDeath()
    {
        _playerController.enabled = false;
        gameOverPanel.SetActive(true);
    
    }

    public void ApplyUpgrade(int upgradeIndex, int upgradeCount)
    {
        foreach (PlayerUpgrades upgrade in _upgrades) 
        {
            upgradeIndex = upgrade.UpgradeIndex; 
        }
        switch (upgradeIndex)
        {
            case 0: consumoMult = Mathf.Max(0.5f, consumoMult - 0.25f * upgradeCount); 
                _energyButton.targetGraphic = _energyImage[upgradeCount];
                break; // Reduz o consumo de energia
                
            case 1: dashCooldown = dashCooldown - 1.5f; 
                _dashButton.targetGraphic = _dashImage[upgradeCount];
                break; // Reduz cooldown do dash
            case 2: floatDuration += 2f;
                _slowFallButton.targetGraphic = _slowFallImage[upgradeCount];
                break; // Aplica um timer para diminuir a gravidade do jogador
        }
    }

    public void Restart()
    {
        GameEvents.Reset();
        DeviceTracker.Reset();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}