using UnityEngine;

public class SpawnableObject : MonoBehaviour
{
    [SerializeField] public GameObject[] possiblePrefabs; // variantes possíveis
    [HideInInspector] public Vector3 localPosition;       // posição relativa ao chunk

    void Awake() => localPosition = transform.localPosition;
}
