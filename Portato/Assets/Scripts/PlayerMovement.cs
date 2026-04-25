using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField][Range(0f, 1f)] private float _maxDistanceToClick = 25f;
    [SerializeField] private float _maxForce = 10f;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField][Range(4,64)] private int _circleSegments = 64;
    [SerializeField] private float _lineWidth = 0.05f;

    private Transform _playerTransform;
    private float _forceByClick;
    private float _distance;
    private Vector2 _clickPosition;

    void Start()
    {
        _playerTransform = GetComponent<Transform>();

        // Garantir LineRenderer para visualizar o círculo em runtime
        if (_lineRenderer == null)
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }
        if (_lineRenderer == null)
        {
            _lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        _lineRenderer.useWorldSpace = true;
        _lineRenderer.loop = true;
        _lineRenderer.positionCount = _circleSegments;
        _lineRenderer.startWidth = _lineWidth;
        _lineRenderer.endWidth = _lineWidth;
        UpdateLineRenderer();
    }

    void Update()
    {
        HandleMovement();
        UpdateLineRenderer();
    }

    public void HandleMovement()
    {
        if (Camera.main == null) return;

        _clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _distance = Vector2.Distance(_clickPosition, _playerTransform.position);

        if (Input.GetMouseButtonDown(0) && _distance <= _maxDistanceToClick)
        {
            // t é 1 quando o clique está no jogador (força máxima), 0 quando no limite (sem força)
            float t = Mathf.Clamp01(1f - (_distance / _maxDistanceToClick));
            _forceByClick = Mathf.Lerp(0f, _maxForce, t);

            // Direção contrária ao clique: empurra o jogador para longe do ponto clicado
            Vector2 direction = ((Vector2)_playerTransform.position - _clickPosition).normalized;

            _rb.AddForce(direction * _forceByClick, ForceMode2D.Impulse);
        }
    }

    private void UpdateLineRenderer()
    {
        if (_lineRenderer == null || _circleSegments <= 0) return;

        float angleStep = 360f / _circleSegments;
        Vector3 center = _playerTransform.position;

        for (int i = 0; i < _circleSegments; i++)
        {
            float angle = Mathf.Deg2Rad * (i * angleStep);
            float x = Mathf.Cos(angle) * _maxDistanceToClick;
            float y = Mathf.Sin(angle) * _maxDistanceToClick;
            _lineRenderer.SetPosition(i, new Vector3(center.x + x, center.y + y, center.z));
        }
    }

    // Mostra o círculo no Editor (Gizmos)
    void OnDrawGizmos()
    {
        if (_playerTransform == null) _playerTransform = GetComponent<Transform>();
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_playerTransform != null ? _playerTransform.position : transform.position, _maxDistanceToClick);
    }
}
