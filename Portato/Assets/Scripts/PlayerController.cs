using Unity.Cinemachine;
using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _cinemachineCamera;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Transform _playerTransform;
    [SerializeField][Range(0f, 25f)] private float _maxDistanceToClick = 25f;
    [SerializeField] private float _maxForce = 10f;
    [SerializeField] private ParticleSystem _particleSystem;

    [SerializeField][Tooltip("Cap the player's velocity at the chosen value")][Range(1f, 100f)] 
    private float _maxVelocity = 50f;

    // Limite para impedir o jogador de voltar muito para trás
    private float _leftBoundX = 0f;
    private bool stopped = false;
    float _dashTimer = 0f;

    // Anchor usado para manter Y fixo e mover apenas em X
    private Transform _cameraAnchor;

    void Start()
    {
        _particleSystem = GameObject.Find("Shock_VFX").GetComponent<ParticleSystem>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.interpolation = RigidbodyInterpolation2D.Interpolate; // reduz tremor quando física move o player
        _leftBoundX = transform.position.x; // ponto de início

        if (_cinemachineCamera != null && _playerTransform != null)
        {
            // cria um anchor que terá Y fixo (a mesma Y da câmera inicial)
            _cameraAnchor = new GameObject("CameraAnchor").transform;
            _cameraAnchor.position = new Vector3(_playerTransform.position.x, 0, _cinemachineCamera.transform.position.z);
            _cinemachineCamera.Follow = _cameraAnchor;
        }
    }

    void Update()
    {
        if (stopped) return;
        if (Input.GetMouseButtonDown(0)) 
        { 
            ClickParticlesAt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            ApplyForce();
        }

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.LeftShift))
            TryDash();

        if (_dashTimer > 0) _dashTimer -= Time.deltaTime;
        // Impede o jogador de voltar para trás do limite
        EnforceLeftBound();
        if (_cameraAnchor != null && _playerTransform != null && !stopped)
        {
            Vector3 pos = _cameraAnchor.position;
            pos.x = _playerTransform.position.x;
            _cameraAnchor.position = pos;
        }
        if (transform.position.y < -50f)
        {
            CoreManager.Instance.Restart();
        }
    }
    void TryDash()
    {
        if (_dashTimer > 0) return; // ainda em cooldown
        if (CoreManager.Instance.dashCooldown <= 0) return; // upgrade não desbloqueado

        _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y); // reset velocidade horizontal
        _rb.AddForce(Vector2.left * CoreManager.Instance.dashForce, ForceMode2D.Impulse);

        _dashTimer = CoreManager.Instance.dashCooldown;

        // Roda o sprite para a esquerda durante o dash
        _playerTransform.rotation = Quaternion.Euler(0, 0, 90f);
    }
    void ApplyForce()
    {
        if (Camera.main == null) return;


        Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(clickPos, _playerTransform.position);

        if (clickPos.x >= _playerTransform.position.x +1) return;
        if (distance > _maxDistanceToClick) return;

        float t = Mathf.Clamp01(1f - (distance / _maxDistanceToClick));
        float force = Mathf.Lerp(0f, _maxForce, t);

        Vector2 direction = ((Vector2)_playerTransform.position - clickPos).normalized;

        // Roda APENAS o sprite — o Rigidbody2D nunca roda
        Vector2 rotationDir = direction;
        float angle = Mathf.Atan2(rotationDir.y, rotationDir.x) * Mathf.Rad2Deg - 90f;
        _playerTransform.rotation = Quaternion.Euler(0, 0, angle); // sprite
        _rb.rotation = 0f; // física sempre direita

        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x,0); // reset velocidade vertical
        _rb.AddForce(direction * force, ForceMode2D.Impulse);

        if (_rb.linearVelocity.magnitude > _maxVelocity)
            _rb.linearVelocity = _rb.linearVelocity.normalized * _maxVelocity;

        if (_playerTransform.position.x > _leftBoundX)
            _leftBoundX = _playerTransform.position.x - 15f;

        GameEvents.DrainEnergy(10f * CoreManager.Instance.consumoMult);
    }

    void EnforceLeftBound()
    {
        // Se o jogador tentar voltar demasiado para trás, para-o
        if (_rb.position.x < _leftBoundX)
        {
            _rb.position = new Vector2(_leftBoundX, _rb.position.y);
            // Cancela velocidade horizontal para a esquerda
            if (_rb.linearVelocity.x < 0)
                _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
        }
    }
    void ClickParticlesAt(Vector2 position) 
    {
        _particleSystem.transform.position = position;
        _particleSystem.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);
        _particleSystem.Play();
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