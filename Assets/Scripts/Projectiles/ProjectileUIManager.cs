using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileUIManager : MonoBehaviour {
    [SerializeField] private List<ProjectileSpawnStrategy> _strategies;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GameObject _buttonPrefab;

    private void Awake() {
        _playerController = FindFirstObjectByType<PlayerController>(); 
        InitStrategyButtons<LightUI>(ProjectileSpawnStrategyType.LIGHT);
        InitStrategyButtons<HeavyUI>(ProjectileSpawnStrategyType.HEAVY);
        InitStrategyButtons<EliteUI>(ProjectileSpawnStrategyType.ELITE);
    }

    public void InitStrategyButtons<T>(ProjectileSpawnStrategyType type) where T : Component {
        Transform buttonLayout = FindFirstObjectByType<T>().transform;
        foreach (ProjectileSpawnStrategy strategy in _strategies.Where((ProjectileSpawnStrategy strategy) => strategy.Type == type)) {
            Button button = Instantiate(_buttonPrefab, buttonLayout).GetComponent<Button>();
            button.onClick.AddListener(() => TryUnlockStrategy(strategy));
            button.GetComponentInChildren<TMP_Text>().text = ProjectileSpawnStrategy.Display(strategy.GetType());
        }
    }

    public void TryUnlockStrategy(ProjectileSpawnStrategy strategy) {
        if (Globals.Instance.Money > strategy.Cost) {
            _playerController.AddSpawnStrategy(strategy);
        }
    }
}
