using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Utilities;

public class ProjectileUIManager : MonoBehaviour {
    [SerializeField] private List<ProjectileSpawnStrategy> _strategies;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private bool _isOpen = false;
    [SerializeField] private bool _movingUI = false;

    private void Move() {
        StartCoroutine(MoveUI());
    }

    private void Awake() {
        _playerController = FindFirstObjectByType<PlayerController>(); 
        InitStrategyButtons<LightUI>(ProjectileSpawnStrategyType.LIGHT);
        InitStrategyButtons<HeavyUI>(ProjectileSpawnStrategyType.HEAVY);
        InitStrategyButtons<EliteUI>(ProjectileSpawnStrategyType.ELITE);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab) && !_movingUI) {
            Move();
        }
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

    private IEnumerator MoveUI(float speed = 5f) {
        _movingUI = true;
        if (_isOpen) {
            for (int i = 0; i < gameObject.transform.childCount; i++) {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector3 targetPos = Globals.Instance.MainCamera.ScreenToWorldPoint(rectTransform.position + 
        (_isOpen ? Vector3.up : Vector3.down) * rectTransform.rect.height);
        while (Vector3.Distance(targetPos, transform.position) > Mathf.Epsilon) {
            transform.position = Vector3.MoveTowards(targetPos, transform.position, speed * Time.fixedDeltaTime);
            yield return Yielders.WaitForFixedUpdate;
        }
        _isOpen = !_isOpen;
        if (_isOpen) {
            for (int i = 0; i < transform.childCount; i++) {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        _movingUI = false;
    }
}
