using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private Rigidbody2D _rb;
    private Transform _playerTransform;
    private float _forceByClick = 10f, _distance, _magnitude;
    void Start()
    {
        _playerTransform = GetComponent<Transform>();
    }
    void Update()
    {

    }

    public void HandleMovement()
    {
        //
        _playerTransform.position = Vector2.MoveTowards(_playerTransform.position, Input.mousePosition, _forceByClick * Time.deltaTime);
        _distance = Vector2.Distance(_playerTransform.position, Input.mousePosition);
        _magnitude = _distance * _forceByClick;
        Vector2 direction = (Input.mousePosition - _playerTransform.position).normalized;
    }
}
