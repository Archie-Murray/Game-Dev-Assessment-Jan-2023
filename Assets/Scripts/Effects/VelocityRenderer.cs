using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(SpriteRenderer))]
public class VelocityRenderer : MonoBehaviour {
    [SerializeField] private Gradient _colourGradientSources;
    [SerializeField] private ParticleSystem _emitter;
    [SerializeField] private ParticleSystem.MainModule _mainModule;
    [SerializeField] private Rigidbody2D _rb2D;
    [SerializeField] private Color _targetColour = Color.white;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _turnSpeed = 5f;
    [SerializeField] private float _offset = 0.6f;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _speedPercent;

    public void Awake() {
        _emitter = GetComponent<ParticleSystem>();
        _mainModule = _emitter.main;
        _rb2D = transform.parent.GetComponent<Rigidbody2D>();        
        _spriteRenderer = GetComponent<SpriteRenderer>();
        transform.parent = null;
    }
    private void FixedUpdate() {
        _speedPercent = GetSpeedPercent();
        _targetColour = _colourGradientSources.Evaluate(_speedPercent);
        _mainModule = _emitter.main;
        _mainModule.startColor = _targetColour;
        _spriteRenderer.color = _targetColour;
        Quaternion rotation = Quaternion.AngleAxis(_rb2D.transform.eulerAngles.z + 180f, Vector3.forward);
        transform.SetPositionAndRotation(_rb2D.transform.position - _rb2D.transform.up * _offset, Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * _turnSpeed));
    }

    private float GetSpeedPercent() {
        return Mathf.Clamp01(_rb2D.velocity.sqrMagnitude / (_maxSpeed * _maxSpeed));
    }
}