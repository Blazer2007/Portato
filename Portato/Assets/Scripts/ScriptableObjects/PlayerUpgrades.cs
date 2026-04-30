using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUpgrades", menuName = "Scriptable Objects/PlayerUpgrades")]
public class PlayerUpgrades : ScriptableObject
{
    public int UpgradeIndex;
    [Range(1, 3)] public int UpgradeCount;
    public int UpgradesSelected;
    [Range(1, 25)] public int UpgradeCost;
    public string UpgradeDescription;
}
