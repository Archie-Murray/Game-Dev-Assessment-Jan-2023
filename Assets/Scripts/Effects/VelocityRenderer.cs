using System;
using System.Linq;

using UnityEngine;
using UnityEngine.UIElements;

public class VelocityRenderer : MonoBehaviour {
    [SerializeField] private Gradient _colourGradientSources;
    [SerializeField] private ParticleSystem _emitter;
    [SerializeField] private ParticleSystem.MainModule _mainModule;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Color _targetColour = Color.white;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _turnSpeed = 5f;
    [SerializeField] private float _offset = 0.6f;

    public void Awake() {
        _emitter = GetComponent<ParticleSystem>();
        _mainModule = _emitter.main;
        _playerController = FindFirstObjectByType<PlayerController>();        
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void FixedUpdate() {
        _targetColour = _colourGradientSources.Evaluate(_playerController.SpeedPercent);
        _mainModule = _emitter.main;
        _mainModule.startColor = _targetColour;
        _spriteRenderer.color = _targetColour;
        Quaternion rotation = Quaternion.AngleAxis(_playerController.transform.eulerAngles.z + 180f, Vector3.forward);
        transform.SetPositionAndRotation(_playerController.transform.position - _playerController.transform.up * _offset, Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * _turnSpeed));
    }
}