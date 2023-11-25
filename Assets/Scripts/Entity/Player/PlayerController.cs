using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(InputHandler))]
public class PlayerController : MonoBehaviour {

    [Header("Component References")]
    [SerializeField] private Rigidbody2D _rb2D;
    [SerializeField] private InputHandler _inputHandler;  
    [SerializeField] private ProjectileSpawnerManager _projectileSpawnerManager;
    
    [Header("Player Variables")]
    [SerializeField] private float _maxSpeed = 200f;
    [SerializeField] private float _sprintSpeed = 300f;
    [SerializeField] private float _acceleration = 600f;
    [SerializeField] private float _dashImpulse = 400f;
    [SerializeField] private float _maxRotationSpeed = 5f;
    [SerializeField] private float _rotationAcceleration = 5f;

    [Header("Gameplay Variables")]
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Vector2 _velocity;

    [Header("Projectiles")]
    [SerializeField] private float _fireRate = 0.25f;
    [SerializeField] private float _heavyFireRate = 0.5f;
    [SerializeField] private float _specialFireRate = 1.0f;
    [SerializeField] private CountDownTimer _fireTimer;
    [SerializeField] private CountDownTimer _heavyFireTimer;
    [SerializeField] private CountDownTimer _eliteFireTimer;

    [Header("UI")]
    [SerializeField] private PlayerUI _playerUI;

    private void Awake() {
        _rb2D = GetComponent<Rigidbody2D>();
        _inputHandler = GetComponent<InputHandler>();
        _projectileSpawnerManager = GetComponent<ProjectileSpawnerManager>();
        _fireTimer = new CountDownTimer(_fireRate);
        _heavyFireTimer = new CountDownTimer(_heavyFireRate);
        _eliteFireTimer = new CountDownTimer(_specialFireRate);
        _playerUI = new PlayerUI(FindFirstObjectByType<FireCooldown>().GetComponentsInChildren<Image>());
        _playerUI.UpdateFireCooldowns(1f, 1f, 1f);
    }

    public void AddSpawnStrategy(ProjectileSpawnStrategy spawnStrategy) {
        _playerUI.EnableTimer(spawnStrategy.Type);
        _projectileSpawnerManager.TryAddSpawner(spawnStrategy);
    }

    private void Update() {
        if (_inputHandler.IsDashPressed) {
            _rb2D.AddForce(transform.up * _dashImpulse, ForceMode2D.Impulse);
        }

        if (_fireTimer.IsFinished && _inputHandler.FireInput) {
            _projectileSpawnerManager.Fire(ProjectileSpawnStrategyType.LIGHT);   
            _fireTimer.Start();
        }
        if (_heavyFireTimer.IsFinished && _inputHandler.HeavyFireInput) {
            _projectileSpawnerManager.Fire(ProjectileSpawnStrategyType.HEAVY);
            _heavyFireTimer.Start();
        }
        if (_eliteFireTimer.IsFinished && _inputHandler.EliteFireInput) {
            _projectileSpawnerManager.Fire(ProjectileSpawnStrategyType.ELITE);
            _eliteFireTimer.Start();
        }
    }

    private void FixedUpdate() {
        Move();
        RotateToMouse();
        _fireTimer.Update(Time.fixedDeltaTime);
        _heavyFireTimer.Update(Time.fixedDeltaTime);
        _eliteFireTimer.Update(Time.fixedDeltaTime);
        _playerUI.UpdateFireCooldowns(1f - _fireTimer.Progress, 1f - _heavyFireTimer.Progress, 1f - _eliteFireTimer.Progress);
    }

    private void RotateToMouse() {
        Vector2 lookAt = (Globals.Instance.MainCamera.ScreenToWorldPoint(_inputHandler.MousePosition) - transform.position).normalized;
        _rotationSpeed = Mathf.MoveTowards(_rotationSpeed, _maxRotationSpeed, Time.fixedDeltaTime * _rotationAcceleration * (1.5f - ((Vector2.Dot(lookAt, transform.up) + 1f) * 0.5f)));
        Quaternion rotation = Quaternion.AngleAxis(Vector2.SignedAngle(lookAt, Vector2.up), Vector3.back);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * _rotationSpeed);
    }

    private void Move() {
        if (_inputHandler.IsMovePressed) {
            _speed = Mathf.MoveTowards(_speed, Mathf.Lerp(_maxSpeed, _sprintSpeed, _inputHandler.SprintInput), Time.fixedDeltaTime * _acceleration);
            _velocity = Time.fixedDeltaTime * _speed * _inputHandler.MoveInput;
        } else {
            _speed = Mathf.MoveTowards(_speed, 0f, Time.fixedDeltaTime * _acceleration);
            _velocity = Time.fixedDeltaTime * _speed * _velocity.normalized;
        }
        _rb2D.velocity = _velocity;
    }
}
