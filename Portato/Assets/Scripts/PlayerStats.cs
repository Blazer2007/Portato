using UnityEngine;
using System;

[Serializable]
public class PlayerStats : MonoBehaviour
{
    [SerializeField] public ScriptableFloat Energy;
    [SerializeField] public ScriptableFloat MaxEnergy;
    [SerializeField] public ScriptableFloat Gravity;
    [SerializeField] public ScriptableFloat Range;
    [SerializeField] public ScriptableFloat SlowFall;
}
