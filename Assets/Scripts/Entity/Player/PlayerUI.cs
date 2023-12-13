using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlayerUI {
    [SerializeField] private Image[] _cooldownTimers;
    [SerializeField] private Image _healthBar;
    [SerializeField] private float _healthBarLerp = 1f;
    [SerializeField] private float _healthBarSpeed = 0.05f;

    public PlayerUI(Image[] cooldownTimers, Image healthBar) {
        _cooldownTimers = cooldownTimers;
        _healthBar = healthBar;
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

    public void UpdateHealthBar(float amount) {
        _healthBarLerp = Mathf.MoveTowards(_healthBarLerp, amount, Time.fixedDeltaTime * _healthBarSpeed);
        _healthBar.fillAmount = _healthBarLerp;
    }
}