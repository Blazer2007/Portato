using UnityEngine;

public class ParentStation : MonoBehaviour
{
    [SerializeField] SpriteRenderer _stationSprite;
    [SerializeField] Station _station;

    private void Start() {
        _stationSprite = GetComponent<SpriteRenderer>();
        _station = GetComponentInChildren<Station>();
        _station.Init(_stationSprite); // passa a referência correta
    }
}
