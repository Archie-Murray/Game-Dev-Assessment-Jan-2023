using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlayerUI {
    [SerializeField] private Image[] _cooldownTimers;

    public PlayerUI(Image[] cooldownTimers) => _cooldownTimers = cooldownTimers;

    public void EnableTimer(ProjectileSpawnStrategyType type) {
        if (_cooldownTimers[(int)type].color.a == 0f) {
            _cooldownTimers[(int)type].color = Color.white;
        }
    }

    public void UpdateFireCooldowns(float normalProgress, float heavyProgress, float magicProgress) {
        _cooldownTimers[0].fillAmount = normalProgress;
        _cooldownTimers[1].fillAmount = heavyProgress;
        _cooldownTimers[2].fillAmount = magicProgress;
    }
}