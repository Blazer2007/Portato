using UnityEngine;
using System;

[Serializable]
public class PlayerStats : MonoBehaviour
{
    [SerializeField] private ScriptableFloat Energy;
    [SerializeField] private ScriptableFloat MaxEnergy;
    [SerializeField] private ScriptableFloat Gravity;
    [SerializeField] private ScriptableFloat Range;
    [SerializeField] private ScriptableFloat SlowFall;
}
