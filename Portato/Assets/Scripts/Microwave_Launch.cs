using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.UIElements;

public class Microwave_Launch : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _microwaveSound;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Rigidbody2D _playerRigidbody;


    [SerializeField] private float launchForce = 30f;
    [SerializeField] private float launchAngle = 45f;
    [SerializeField] private Animation _microwaveAnimation;
    void Start()
    {
        if(_audioSource == null)
            _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

public void LaunchPlayer()
{
    // Play animation once
    if (_microwaveAnimation != null)
        _microwaveAnimation.Play();

    _playerController.enabled = true;
    _playerRigidbody.bodyType = RigidbodyType2D.Dynamic;
    _playerRigidbody.AddForce(Quaternion.Euler(0, 0, launchAngle) * Vector2.right * launchForce, ForceMode2D.Impulse);
    _audioSource.PlayOneShot(_microwaveSound);

    if (_microwaveAnimation != null)
        _microwaveAnimation.Stop();
}
}
