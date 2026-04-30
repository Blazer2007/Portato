using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUpgrades", menuName = "Scriptable Objects/PlayerUpgrades")]
public class PlayerUpgrades : ScriptableObject
{
    [Range(1, 3)] public int UpgradeCount;
    public int UpgradesSelected;
    [Range(1, 25)] public int UpgradeCost;
    public Sprite[] UpgradeSprites;
    [HideInInspector]public int playerCredits;    
    public int UpgradeIndex;
    public string UpgradeDescription;
}
