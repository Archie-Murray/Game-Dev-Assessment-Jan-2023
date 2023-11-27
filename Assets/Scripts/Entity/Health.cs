using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour {
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _currentHealth;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private float _deathTimer = 0.25f;

    public event UnityAction<float> OnDamage;
    public event UnityAction<float> OnHeal;
    public event UnityAction OnDeath;

    public float PercentHealth => Mathf.Clamp01(_currentHealth / _maxHealth);
    public float CurrentHealth => _currentHealth;
    public float MaxHealth => _maxHealth;

    private void Awake() {
        _currentHealth = _maxHealth;
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void Damage(float amount) {
        if (_currentHealth < 0f) {
            return;
        }
        _currentHealth = Mathf.Max(_currentHealth - amount, 0f);
        _renderer.FlashColour(Color.red, 0.25f, this);
        OnDamage?.Invoke(amount);
        if (_currentHealth == 0f) {
            OnDeath?.Invoke();
            Destroy(gameObject, _deathTimer);
        }
    }

    public void Heal(float amount) { 
        _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
        OnHeal?.Invoke(amount);
    }
}