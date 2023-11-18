using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileUIManager : MonoBehaviour {
    [SerializeField] private List<ProjectileSpawnStrategy> _strategies;
    [SerializeField] private ProjectileSpawnerManager _projectileSpawnerManager;
    [SerializeField] private Dictionary<ProjectileSpawnStrategyType, Button[]> _buttons;

    private void Awake() {
        _projectileSpawnerManager = FindFirstObjectByType<ProjectileSpawnerManager>();
        _buttons = new Dictionary<ProjectileSpawnStrategyType, Button[]> {
            { ProjectileSpawnStrategyType.LIGHT, FindFirstObjectByType<LightUI>().GetComponentsInChildren<Button>() },
            { ProjectileSpawnStrategyType.HEAVY, FindFirstObjectByType<HeavyUI>().GetComponentsInChildren<Button>() },
            { ProjectileSpawnStrategyType.SPECIAL, FindFirstObjectByType<SpecialUI>().GetComponentsInChildren<Button>() }
        };
        if (_buttons.Values.Count != _strategies.Count) {
            Debug.LogWarning($"ERR: Mismatch of button and spawn strategy collection lengths, omitting any {(_buttons.Values.Count > _strategies.Count ? "buttons" : "spawn strategies")} after index: {Mathf.Min(_buttons.Values.Count, _strategies.Count) - 1}");
        }
        foreach (ProjectileSpawnStrategy strategy in _strategies) {
            Button button = _buttons[strategy.Type].Where((Button button) => button.onClick.GetPersistentEventCount() == 0).First();
            if (button) { 
                button.onClick.AddListener(() => { TryUnlockStrategy(strategy); }); 
            }
        } 
    }

    public void TryUnlockStrategy(ProjectileSpawnStrategy strategy) {
        if (Globals.Instance.Money > strategy.Cost) {
            _projectileSpawnerManager.TryAddSpawner(strategy);
            Globals.Instance.Money -= strategy.Cost;
        }
    }
}