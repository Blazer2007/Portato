using UnityEngine;
using System.Collections.Generic;

public class SpawnPoint : MonoBehaviour
{
    public SpawnRule rule;
    public SpawnZone zone; // Ground, Air, Platform

    // Só para visualização no Editor
    void OnDrawGizmos()
    {
        Gizmos.color = zone switch
        {
            SpawnZone.Ground => Color.green,
            SpawnZone.Air => Color.cyan,
            SpawnZone.Platform => Color.yellow,
            _ => Color.white
        };
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
}

public enum SpawnZone { Ground, Air, Platform }
