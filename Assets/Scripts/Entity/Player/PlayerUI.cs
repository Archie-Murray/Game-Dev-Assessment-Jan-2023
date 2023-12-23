using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlayerUI : MonoBehaviour {
    [SerializeField] private Image[] _cooldownTimers;
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Image _healthBarFillImage;
    [SerializeField] private Health _health;
    [SerializeField] private float _healthBarLerp = 1f;
    [SerializeField] private float _healthBarSpeed = 1f;
    [SerializeField] private Gradient _healthBarGradient;

    public void Awake() {
        _health = GetComponent<Health>();
        _cooldownTimers = FindFirstObjectByType<FireCooldown>().GetComponentsInChildren<Image>();
        _healthBar = FindFirstObjectByType<PlayerHealthBar>().GetComponent<Slider>();
        _healthBarFillImage = _healthBar.GetComponentInChildren<HealthBarFill>().GetComponent<Image>();
    }

    public void EnableTimer(ProjectileSpawnStrategyType type) {
        if (_cooldownTimers[(int)type].color.a == 0f) {
            _cooldownTimers[(int)type].color = Color.white;
        }
    }

    public void UpdateFireCooldowns(float normalProgress, float heavyProgress, float eliteProgress) {
        _cooldownTimers[0].fillAmount = normalProgress;
        _cooldownTimers[1].fillAmount = heavyProgress;
        _cooldownTimers[2].fillAmount = eliteProgress;
    }

    public void UpdateHealthBar() {
        _healthBarLerp = Mathf.MoveTowards(_healthBarLerp, _health.PercentHealth, Time.fixedDeltaTime * _healthBarSpeed);
        _healthBar.value = _healthBarLerp;
        _healthBarFillImage.color = _healthBarGradient.Evaluate(_healthBarLerp);
    }
}