using System;

using UnityEngine;
using UnityEngine.UI;

public class UIMover : MonoBehaviour {
    [SerializeField] private Transform _rootAnchor;
    [SerializeField] private RectTransform _root;
    [SerializeField] private KeyCode _key = KeyCode.Tab;
    [SerializeField] private Vector3 _targetPos;
    [SerializeField] private float _speed = 5;
    [SerializeField] private Vector2 _moveDir = Vector2.up;

    public Action OnCloseStart;
    public Action OnCloseFinish;
    public Action OnOpenStart;
    public Action OnOpenFinish;

    Button[] _buttons = null;
    Slider[] _sliders = null;

    private void Awake() {
        _root = GetComponent<RectTransform>();
        _rootAnchor = GetComponentInChildren<UIAnchor>().transform;
        _buttons = GetComponentsInChildren<Button>();
        _sliders = GetComponentsInChildren<Slider>();

        OnCloseStart += () => ToggleUIChildrenInteractable(false);
        OnOpenFinish += () => ToggleUIChildrenInteractable(true);
    }

    private Vector3 OpenPos => _rootAnchor.position;
    private Vector3 ClosedPos => _rootAnchor.position.Add(x: _root.rect.width * _moveDir.x, y: _root.rect.height * _moveDir.y);

    private void FixedUpdate() {
        if (Vector3.Distance(_rootAnchor.position, _targetPos) <= 0.01f) {
            if (_targetPos == ClosedPos) {
                OnCloseFinish?.Invoke();
            } else {
                OnOpenFinish?.Invoke();
            }
            return;
        }

        _root.position = Vector3.MoveTowards(_root.position, _targetPos, _speed * Time.fixedDeltaTime);
    }

    private void Update() {
        if (Input.GetKeyDown(_key)) {
            _targetPos = _targetPos == OpenPos ? ClosedPos : OpenPos;
            if (_targetPos == OpenPos) {
                OnCloseStart?.Invoke();
            } else {
                OnOpenStart?.Invoke();
            }
        }
    }

    private void ToggleUIChildrenInteractable(bool value) {
        if (_buttons != null) {
            foreach (Button button in _buttons) {
                button.interactable = value;
            }
        }
        if (_sliders != null) {
            foreach (Slider slider in _sliders) {
                slider.interactable = value;
            }
        }
    }

}