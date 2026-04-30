using UnityEngine;
using System;

public static class GameEvents
{
    public static event Action<float> OnEnergyChanged;
    public static event Action OnPlayerDied;
    public static event Action OnAllDevicesCharged;
    public static event Action<int> OnPointsChanged;
    
    
    static int points = PlayerPrefs.GetInt("Points", 0);

    static float energy = 100f;
    public static float Energy => energy;
    public static int Points => points;


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
    public static void AddPoints(int amount)
    {
        points += amount;
        PlayerPrefs.SetInt("Points", points);
        PlayerPrefs.Save();

        int record = PlayerPrefs.GetInt("MaxPoints", 0);
        if (points > record)
        {
            PlayerPrefs.SetInt("MaxPoints", points);
            PlayerPrefs.Save();
        }

        OnPointsChanged?.Invoke(points);
    }

    public static void SpendPoints(int amount)
    {
        points = Mathf.Max(0, points - amount);
        PlayerPrefs.SetInt("Points", points);
        PlayerPrefs.Save();
        OnPointsChanged?.Invoke(points);
    }
    public static void DevicesCharged() => OnAllDevicesCharged?.Invoke();

    public static void Reset()
    {
        energy = 100f;
        points = 0; 
        PlayerPrefs.SetInt("Points", 0); 
        PlayerPrefs.Save(); 
        OnEnergyChanged?.Invoke(energy);
        OnPointsChanged?.Invoke(points); 
    }
}