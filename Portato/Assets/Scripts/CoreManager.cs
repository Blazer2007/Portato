using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CoreManager : MonoBehaviour
{
    public static CoreManager Instance;

    [Header("Referencias")]
    [SerializeField] Rigidbody2D _playerRB;
    [SerializeField] ChunkSpawner spawner;
    [SerializeField] Slider energyBar;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] PlayerController _playerController;
    [SerializeField] PlayerUpgrades[] _upgrades;
    [SerializeField] Image _energyImage;
    [SerializeField] Image _dashImage;
    [SerializeField] Image _slowFallImage;
    [SerializeField] Sprite[] _energySprites;
    [SerializeField] Sprite[] _dashSprites;
    [SerializeField] Sprite[] _slowFallSprites;
    [SerializeField] private AudioSource _deathsound;

    [Header("Upgrades")]
    public float consumoMult = 1f;
    [Range(1f, 5f)] public float dashCooldown = 5f;   // segundos entre dashes (diminui com upgrades)
    public float dashForce = 15f;  // for�a do dash para tr�s
    public float floatDuration = 0f;

    float floatTimer = 0f;

    void Awake() 
    {
        Instance = this;

        // Reset dos sprites para o nível 0
        if (_energySprites.Length > 0) _energyImage.sprite = _energySprites[0];
        if (_dashSprites.Length > 0) _dashImage.sprite = _dashSprites[0];
        if (_slowFallSprites.Length > 0) _slowFallImage.sprite = _slowFallSprites[0];

        // Reset dos multiplicadores
        consumoMult = 1f;
        dashCooldown = 5f;
        floatDuration = 0f;
    } 

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
        _deathsound.Play();
    }
    public void ApplyUpgrade(int upgradeIndex)
    {
        // Garante que o index é válido
        if (upgradeIndex < 0 || upgradeIndex >= _upgrades.Length) return;

        var upgrade = _upgrades[upgradeIndex];

        // Verifica se já atingiu o máximo
        if (upgrade.UpgradesSelected >= upgrade.UpgradeCount) return;

        // Aplica o efeito do upgrade
        switch (upgradeIndex)
        {
            case 0:
                consumoMult = Mathf.Max(0.5f, consumoMult - 0.25f);
                break;
            case 1:
                dashCooldown = Mathf.Max(1f, dashCooldown - 1.5f);
                break;
            case 2:
                floatDuration += 2f;
                break;
        }

        // Incrementa o nível do upgrade
        upgrade.UpgradesSelected++;

        // Atualiza o sprite do botão correspondente
        UpdateUpgradeSprite(upgradeIndex, upgrade.UpgradesSelected);
    }

    void UpdateUpgradeSprite(int upgradeIndex, int level)
    {
        (Image image, Sprite[] sprites) = upgradeIndex switch
        {
            0 => (_energyImage, _energySprites),
            1 => (_dashImage, _dashSprites),
            2 => (_slowFallImage, _slowFallSprites),
            _ => (null, null)
        };

        Debug.Log($"Upgrade {upgradeIndex} | Nível {level} | Image: {image} | Sprites: {sprites?.Length}");

        if (image == null || sprites == null) return;
        if (level < 0 || level >= sprites.Length) return;

        image.sprite = sprites[level];

        Debug.Log($"Sprite alterado para: {sprites[level]?.name}");
    }
    public void Restart()
    {
        GameEvents.Reset();
        DeviceTracker.Reset();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}