using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlayerUI : MonoBehaviour {
    [SerializeField] private Image _lightFire;
    [SerializeField] private Image _heavyFire;
    [SerializeField] private Image _eliteFire;
    [SerializeField] private Image _dashCooldown;
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Image _healthBarFillImage;
    [SerializeField] private Health _health;
    [SerializeField] private float _healthBarLerp = 1f;
    [SerializeField] private float _healthBarSpeed = 1f;
    [SerializeField] private Gradient _healthBarGradient;
    [SerializeField] private Color _enabledColour;

    public void Awake() {
        _health = GetComponent<Health>();
        Image[] cooldownTimers = FindFirstObjectByType<FireCooldown>().GetComponentsInChildren<Image>();
        _lightFire = cooldownTimers[1];  // First Will be panel
        _heavyFire = cooldownTimers[2];
        _eliteFire = cooldownTimers[3];
        _dashCooldown = cooldownTimers[4];
        _lightFire.color = Color.gray;
        _heavyFire.color = Color.gray;
        _eliteFire.color = Color.gray;
        _healthBar = FindFirstObjectByType<PlayerHealthBar>().GetComponent<Slider>();
        _healthBarFillImage = _healthBar.GetComponentInChildren<HealthBarFill>().GetComponent<Image>();
    }

    public void EnableTimer(ProjectileSpawnStrategyType type) {
        switch (type) {
            case ProjectileSpawnStrategyType.LIGHT:
                _lightFire.color = _enabledColour;
                break;
            case ProjectileSpawnStrategyType.HEAVY:
                _heavyFire.color = _enabledColour;
                break;
            case ProjectileSpawnStrategyType.ELITE:
                _eliteFire.color = _enabledColour;
                break;
            default:
                break;
        }
    }

    public void UpdateFireCooldowns(float lightProgress, float heavyProgress, float eliteProgress) {
        _lightFire.fillAmount = lightProgress;
        _heavyFire.fillAmount = heavyProgress;
        _eliteFire.fillAmount = eliteProgress;
    }

    public void UpdateDashCooldown(float progress) {
        _dashCooldown.fillAmount = progress;
    }

    public void UpdateHealthBar() {
        _healthBarLerp = Mathf.MoveTowards(_healthBarLerp, _health.PercentHealth, Time.fixedDeltaTime * _healthBarSpeed);
        _healthBar.value = _healthBarLerp;
        _healthBarFillImage.color = _healthBarGradient.Evaluate(_healthBarLerp);
    }
}