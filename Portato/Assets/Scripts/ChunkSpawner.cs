using Unity.VisualScripting;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] _chunkPrefabs;
    [SerializeField] Transform _player;
    [SerializeField] float _chunkWidth = 21.6f;
    float _lastChunkTpBuffer = 30f;
    [SerializeField] int _totalChunks = 5; // quantos chunks existem em cena

    GameObject[] _chunks;
    float[] _chunkPositions;

    void Start()
    {
        _chunks = new GameObject[_totalChunks];
        _chunkPositions = new float[_totalChunks];

        // Cria os chunks iniciais em sequência
        for (int i = 0; i < _totalChunks; i++)
        {
            float x = i * _chunkWidth;
            _chunks[i] = Instantiate(_chunkPrefabs[Random.Range(0, _chunkPrefabs.Length)],
                                    new Vector3(x, 0, 0), Quaternion.identity);
            _chunkPositions[i] = x;
            PopulateChunk(_chunks[i]);
        }
    }

    void Update()
    {
        foreach (var chunk in _chunks)
        {
            // Se o chunk ficou mais de 1 chunkWidth atrás do jogador
            if (chunk.transform.position.x < _player.position.x - _chunkWidth - _lastChunkTpBuffer)
            {
                // Move para a frente do chunk mais à frente
                float furthestX = GetFurthestX();
                chunk.transform.position = new Vector3(furthestX + _chunkWidth, 0, 0);
                PopulateChunk(chunk);
            }
        }
    }
    void PopulateChunk(GameObject chunk)
    {
        // 1. Apaga tudo o que foi spawnado anteriormente
        foreach (Transform child in chunk.transform)
        {
            if (child.CompareTag("Spawned"))
                Destroy(child.gameObject);
        }

        // 2. Recria os objetos fixos com aleatoriedade
        var spawnables = chunk.GetComponentsInChildren<SpawnableObject>();
        foreach (var spawnable in spawnables)
        {
            if (spawnable.possiblePrefabs.Length == 0) continue;

            // Escolhe um prefab aleatório das variantes
            var prefab = spawnable.possiblePrefabs[
                Random.Range(0, spawnable.possiblePrefabs.Length)];

            var obj = Instantiate(prefab,
                                  spawnable.transform.position,
                                  Quaternion.identity,
                                  chunk.transform);
            obj.tag = "Spawned";
        }

        // 3. Spawna itens nos SpawnPoints (lógica que já tinhas)
        var spawnPoints = chunk.GetComponentsInChildren<SpawnPoint>();
        bool forceRecharge = GameEvents.Energy < 30f;

        foreach (var point in spawnPoints)
        {
            if (point.rule == null) continue;
            if (Random.value > point.rule.spawnChance) continue;

            GameObject prefabToSpawn = null;

            if (forceRecharge)
                foreach (var wp in point.rule.possiblePrefabs)
                    if (wp.prefab.GetComponent<Interactable>().type == InteractableType.WaterPot)
                    { prefabToSpawn = wp.prefab; break; }

            if (prefabToSpawn == null)
                prefabToSpawn = PickByWeight(point.rule);

            if (prefabToSpawn == null) continue;

            var obj = Instantiate(prefabToSpawn, point.transform.position,
                                  Quaternion.identity, chunk.transform);
            obj.tag = "Spawned";
        }
    }

    GameObject PickByWeight(SpawnRule rule)
    {
        if (rule.possiblePrefabs.Length == 0) return null;
        float total = 0f;
        foreach (var wp in rule.possiblePrefabs) total += wp.weight;
        float roll = Random.value * total;
        float cumulative = 0f;
        foreach (var wp in rule.possiblePrefabs)
        {
            cumulative += wp.weight;
            if (roll <= cumulative) return wp.prefab;
        }
        return rule.possiblePrefabs[rule.possiblePrefabs.Length - 1].prefab;
    }
    float GetFurthestX()
    {
        float max = float.MinValue;
        foreach (var chunk in _chunks)
            if (chunk.transform.position.x > max)
                max = chunk.transform.position.x;
        return max;
    }
}