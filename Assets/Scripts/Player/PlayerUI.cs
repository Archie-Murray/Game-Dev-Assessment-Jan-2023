using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlayerUI {
    [SerializeField] private Image[] _cooldownTimers;

    public PlayerUI(Image[] cooldownTimers) => _cooldownTimers = cooldownTimers;

    public void UpdateFireCooldowns(float normalProgress, float heavyProgress, float specialProgress) {
        _cooldownTimers[0].fillAmount = normalProgress;
        _cooldownTimers[1].fillAmount = heavyProgress;
        _cooldownTimers[2].fillAmount = specialProgress;
    }
}