using UnityEngine;
using System;
public static class GameEvents
{
    public static event Action<float> OnEnergyChanged;
    public static event Action OnPlayerDied;
    public static event Action OnAllDevicesCharged;

    // Energia
    static float energy = 100f;
    public static float Energy => energy;

    public static void DrainEnergy(float amount)
    {
        energy = Mathf.Max(0, energy - amount);
        OnEnergyChanged?.Invoke(energy);
        if (energy <= 0) OnPlayerDied?.Invoke();
    }

    public static void RechargeEnergy(float amount)
    {
        energy = Mathf.Min(100f, energy + amount);
        OnEnergyChanged?.Invoke(energy);
    }
    public static void DevicesCharged() => OnAllDevicesCharged?.Invoke();

    public static void Reset() => energy = 100f;
}
