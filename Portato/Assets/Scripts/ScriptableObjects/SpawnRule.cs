using UnityEngine;

[CreateAssetMenu(menuName = "Game/SpawnRule", fileName = "NewSpawnRule")]
public class SpawnRule : ScriptableObject
{
    [Header("O que pode spawnar aqui")]
    public WeightedPrefab[] possiblePrefabs;

    [Header("Regras de exclusão")]
    public InteractableType[] cannotFollowTypes; // ex: Faca não pode seguir Faca
    public InteractableType[] mustBeNearTypes;   // ex: Dispositivo precisa de Cabo perto

    [Header("Limites por chunk")]
    public int maxPerChunk = 2;
    public int minPerChunk = 0;
    [Range(0, 1)] public float spawnChance = 0.7f;
}

[System.Serializable]
public class WeightedPrefab
{
    public GameObject prefab;
    [Range(0, 1)] public float weight = 0.5f;
}
