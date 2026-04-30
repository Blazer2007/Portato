using UnityEngine;
using System.Collections.Generic;

public static class DeviceTracker
{
    static HashSet<int> charged = new();
    static int total = 0;

    public static void Register(int id) => total++;
    public static void Charge(int id)
    {
        if (charged.Contains(id)) return; 
        charged.Add(id);
        GameEvents.AddPoints(10);
        int record = PlayerPrefs.GetInt("MaxDevices", 0);
        if (charged.Count > record)
        {
            PlayerPrefs.SetInt("MaxDevices", charged.Count);
            PlayerPrefs.Save();
        }
        if (charged.Count >= total)
            GameEvents.DevicesCharged();
        GameEvents.DrainEnergy(20f);
    }
    public static void Reset() { charged.Clear(); total = 0; }
}
