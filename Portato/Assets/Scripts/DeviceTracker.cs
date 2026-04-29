using UnityEngine;
using System.Collections.Generic;

public static class DeviceTracker
{
    static HashSet<int> charged = new();
    static int total = 0;

    public static void Register(int id) => total++;
    public static void Charge(int id)
    {
        charged.Add(id);
        if (charged.Count >= total)
            GameEvents.DevicesCharged();
        GameEvents.DrainEnergy(20f);
    }
    public static void Reset() { charged.Clear(); total = 0; }
}
