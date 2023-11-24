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
        #region Old Implementation
        //_buttons = new Dictionary<ProjectileSpawnStrategyType, Button[]> {
        //    { ProjectileSpawnStrategyType.LIGHT, FindFirstObjectByType<LightUI>().GetComponentsInChildren<Button>() },
        //    { ProjectileSpawnStrategyType.HEAVY, FindFirstObjectByType<HeavyUI>().GetComponentsInChildren<Button>() },
        //    { ProjectileSpawnStrategyType.SPECIAL, FindFirstObjectByType<SpecialUI>().GetComponentsInChildren<Button>() }
        //};
        //if (_buttons.Values.Count != _strategies.Count) {
        //    Debug.LogWarning(
        //        $"ERR: Mismatch of button and spawn strategy collection lengths, " +
        //        $"omitting any {(_buttons.Values.Count > _strategies.Count ? "buttons" : "spawn strategies")} " +
        //        $"after index: {Mathf.Min(_buttons.Values.Count, _strategies.Count) - 1}");
        //}
        //foreach (ProjectileSpawnStrategy strategy in _strategies) {
        //    Button button = _buttons[strategy.Type].Where((Button button) => button.onClick.GetPersistentEventCount() == 0).First();
        //    if (button) { 
        //        button.onClick.AddListener(() => { TryUnlockStrategy(strategy); }); 
        //        button.GetComponentInChildren<TMP_Text>().text = ProjectileSpawnStrategy.Display(strategy.GetType());
        //    }
        //}
        #endregion
        InitStrategyButtons<LightUI>(ProjectileSpawnStrategyType.LIGHT);
        InitStrategyButtons<HeavyUI>(ProjectileSpawnStrategyType.HEAVY);
        InitStrategyButtons<MagicUI>(ProjectileSpawnStrategyType.MAGIC);
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
