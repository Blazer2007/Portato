using UnityEngine;
using UnityEngine.Events;

public class EnergySystem : MonoBehaviour
{ 
    public static EnergySystem Instance;
    [SerializeField] private PlayerStats _playerStats;

    [Header("Custos de energia")]
    [SerializeField] private float _clickCost = 5f;
    [SerializeField] private float _transferCost = 20f;
    [SerializeField] float maxEnergy = 100f;
    public float Current { get; private set; }

    [Header("Eventos")]
    public UnityEvent onEnergyDepleted;
    public UnityEvent<float> onEnergyChanged;

    public float EnergyPercent => _playerStats.Energy.Value / _playerStats.MaxEnergy.Value;
    public bool IsEnergyEmpty => _playerStats.Energy.Value <= 0f;

    void Awake()
    {
        Instance = this;
        Current = maxEnergy;
    }

    public void Drain(float amount)
    {
        Current = Mathf.Max(0, Current - amount);
        if (Current <= 0) CoreManager.Instance.TriggerDeath();
    }

    public void Recharge(float amount)
    {
        Current = Mathf.Min(maxEnergy, Current + amount);
    }
    public void DrainEnergy(float amount)
    {
        _playerStats.Energy.Value = Mathf.Max(0f, _playerStats.Energy.Value - amount);
        NotifyEnergyChanged();
    }

    public void RechargeEnergy(float amount)
    {
        _playerStats.Energy.Value = Mathf.Min(_playerStats.MaxEnergy.Value, _playerStats.Energy.Value + amount);
        NotifyEnergyChanged();
    }

    public void OnClick()
    {
        if (IsEnergyEmpty) return;
        DrainEnergy(_clickCost);
    }

    public void OnTransfer()
    {
        if (IsEnergyEmpty) return;
        DrainEnergy(_transferCost);
    }

    private void NotifyEnergyChanged()
    {
        onEnergyChanged?.Invoke(EnergyPercent);
        if (IsEnergyEmpty)
            onEnergyDepleted?.Invoke();
    }
}