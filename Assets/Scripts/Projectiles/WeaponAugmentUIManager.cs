using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Utilities;

public class WeaponAugmentUIManager : MonoBehaviour {
    [SerializeField] private List<ProjectileSpawnStrategy> _strategies;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private bool _isOpen = true;
    [SerializeField] private bool _augmentUIEnabled = true;
    [SerializeField] private RectTransform _weaponAugmentUIRoot;
    [SerializeField] private Vector3 _targetAugmentUIPos;
    [SerializeField] private GameObject _weaponAugmentAnchor;

    private Vector3 OpenPos => _weaponAugmentAnchor.transform.position;
    private Vector3 ClosedPos { 
        get {
            return Globals.Instance.MainCamera.ScreenToWorldPoint(
                Globals.Instance.MainCamera.WorldToScreenPoint(_weaponAugmentAnchor.transform.position) + 
                Vector3.up * _weaponAugmentUIRoot.rect.height        
            );
        } 
    }

    private void MoveUI(float speed = 5f) {
        if (_weaponAugmentUIRoot.position != _targetAugmentUIPos && _augmentUIEnabled) {
            for (int i = 0; i < _weaponAugmentUIRoot.childCount; i++) {
                foreach (Button button in _weaponAugmentUIRoot.GetChild(i).gameObject.GetComponentsInChildren<Button>()) {
                    button.interactable = false;
                }
            }
            _augmentUIEnabled = false;
        } else if (!_augmentUIEnabled) {
            for (int i = 0; i < _weaponAugmentUIRoot.childCount; i++) {
                foreach (Button button in _weaponAugmentUIRoot.GetChild(i).gameObject.GetComponentsInChildren<Button>()) {
                    button.interactable = true;
                }
            }
        }
        if (_weaponAugmentUIRoot.position != _targetAugmentUIPos) {
            _weaponAugmentUIRoot.position = Vector3.MoveTowards(_weaponAugmentUIRoot.position, _targetAugmentUIPos, Time.fixedDeltaTime * speed);
        }
    }

    private void Awake() {
        _playerController = FindFirstObjectByType<PlayerController>();
        _weaponAugmentUIRoot = FindFirstObjectByType<WeaponAugment>().GetComponent<RectTransform>();
        InitStrategyButtons<LightUI>(ProjectileSpawnStrategyType.LIGHT);
        InitStrategyButtons<HeavyUI>(ProjectileSpawnStrategyType.HEAVY);
        InitStrategyButtons<EliteUI>(ProjectileSpawnStrategyType.ELITE);
        _weaponAugmentAnchor = FindFirstObjectByType<WeaponAugmentAnchor>().gameObject;
        _targetAugmentUIPos = _isOpen ? OpenPos : ClosedPos;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            _targetAugmentUIPos = _targetAugmentUIPos == OpenPos ? ClosedPos : OpenPos;
        }
    }

    private void FixedUpdate() {
        MoveUI();
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
