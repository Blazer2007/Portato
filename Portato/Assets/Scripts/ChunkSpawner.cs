using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] _chunkPrefabs;
    [SerializeField] Transform _player;
    [SerializeField] float _chunkWidth = 20f;
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
            }
        }
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