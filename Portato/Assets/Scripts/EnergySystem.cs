using UnityEngine;

public class EnergySystem : MonoBehaviour
{
    public static EnergySystem Instance;
    [SerializeField] float maxEnergy = 100f;
    public float Current { get; private set; }

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
}
