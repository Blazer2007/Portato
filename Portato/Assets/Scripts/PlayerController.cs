using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _cinemachineCamera;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _playerHead;
    [SerializeField][Range(0f, 25f)] private float _maxDistanceToClick = 25f;
    [SerializeField] private float _maxForce = 10f;
    [SerializeField]
    [Tooltip("Reference to the player's input particles")]
    private ParticleSystem _InputParticles;
    [Header("Steam Effect")]
    [SerializeField]
    [Tooltip("Reference to the player's steam particles")]
    private ParticleSystem _steamParticles;
    [SerializeField]
    [Tooltip("Cap the player's velocity at the chosen value")]
    [Range(1f, 100f)]
    private float _maxVelocity = 50f;
    private float _leftBoundX = 0f;
    private bool stopped = false;
    float _dashTimer = 0f;
    private Transform _cameraAnchor;

    void Start()
    {
        if (_InputParticles != null)
            _InputParticles = GameObject.Find("Shock_VFX").GetComponent<ParticleSystem>();
        if (_steamParticles != null)
            _steamParticles = GameObject.Find("Steam_VFX").GetComponent<ParticleSystem>();
        _playerHead = GameObject.Find("Head").transform;
        _rb = GetComponent<Rigidbody2D>();
        _rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        _leftBoundX = transform.position.x;
        if (_cinemachineCamera != null && _playerTransform != null)
        {
            _cameraAnchor = new GameObject("CameraAnchor").transform;
            _cameraAnchor.position = new Vector3(_playerTransform.position.x, 0, _cinemachineCamera.transform.position.z);
            _cinemachineCamera.Follow = _cameraAnchor;
        }
    }

    void Update()
    {
        if (_steamParticles != null && _playerHead != null)
            _steamParticles.transform.position = _playerHead.position;

        if (stopped) return;
        if (!GameManager.Instance.isgamestarted) return; 

        if (Input.GetMouseButtonDown(0))
        {
            ClickParticlesAt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            ApplyForce();
        }
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.LeftShift))
            TryDash();
        if (_dashTimer > 0) _dashTimer -= Time.deltaTime;
        EnforceLeftBound();
        if (_cameraAnchor != null && _playerTransform != null && !stopped)
        {
            Vector3 pos = _cameraAnchor.position;
            pos.x = _playerTransform.position.x;
            _cameraAnchor.position = pos;
        }
        if (transform.position.y < -50f)
            GameManager.Instance.backtomainmenu();
        
    }

    void TryDash()
    {
        if (_dashTimer > 0) return;
        if (CoreManager.Instance.dashCooldown <= 0) return;
        _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
        _rb.AddForce(Vector2.left * CoreManager.Instance.dashForce, ForceMode2D.Impulse);
        _dashTimer = CoreManager.Instance.dashCooldown;
        _playerTransform.rotation = Quaternion.Euler(0, 0, 90f);
    }

    void ApplyForce()
    {
        if (Camera.main == null) return;
        Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(clickPos, _playerTransform.position);
        if (clickPos.x >= _playerTransform.position.x + 1) return;
        if (distance > _maxDistanceToClick) return;
        float t = Mathf.Clamp01(1f - (distance / _maxDistanceToClick));
        float force = Mathf.Lerp(0f, _maxForce, t);
        Vector2 direction = ((Vector2)_playerTransform.position - clickPos).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        _playerTransform.rotation = Quaternion.Euler(0, 0, angle);
        _rb.rotation = 0f;
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0);
        _rb.AddForce(direction * force, ForceMode2D.Impulse);
        if (_rb.linearVelocity.magnitude > _maxVelocity)
            _rb.linearVelocity = _rb.linearVelocity.normalized * _maxVelocity;
        if (_playerTransform.position.x > _leftBoundX)
            _leftBoundX = _playerTransform.position.x - 15f;

        GameEvents.DrainEnergy(10f * t * CoreManager.Instance.consumoMult);
    }

    void EnforceLeftBound()
    {
        if (_rb.position.x < _leftBoundX)
        {
            _rb.position = new Vector2(_leftBoundX, _rb.position.y);
            if (_rb.linearVelocity.x < 0)
                _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
        }
    }

    void ClickParticlesAt(Vector2 position)
    {
        _InputParticles.transform.position = position;
        _InputParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _InputParticles.Play();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Station"))
        {
            stopped = true;
            _rb.linearVelocity = Vector2.zero;
            col.GetComponent<Station>().StartInteraction(this);
        }
    }

    public void Resume() => stopped = false;
}