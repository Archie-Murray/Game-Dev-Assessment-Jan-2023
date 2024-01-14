using System;
using System.Collections;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.InputSystem;

using Utilities;

public class TutorialManager : MonoBehaviour {
    [SerializeField] private Tutorial[] _tutorials;
    [SerializeField] private GameObject _tutorialPrefab;
    [SerializeField] private Transform _canvas;
    CanvasGroup[] _tutorialCanvases;
    private int _tutorialIndex = 0;

    private WeaponAugmentUIManager _augmentUIManager;
    private InputHandler _inputHandler;
    private UIMover _augmentMover;

    private void Start() {
        _canvas = FindFirstObjectByType<TutorialLayout>().OrNull()?.transform ?? null;
        if (_tutorials == null || _canvas == null) {
            Debug.Log($"Could not find {(_tutorials == null ? "_tutorials" : "_canvas")}");
            Destroy(gameObject);
            return;
        }
        _tutorialCanvases = new CanvasGroup[_tutorials.Length];
        for (int i = 0; i < _tutorialCanvases.Length; i++) {
            _tutorialCanvases[i] = Instantiate(_tutorialPrefab, _canvas).GetComponent<CanvasGroup>();
            _tutorialCanvases[i].alpha = 0f;
            _tutorialCanvases[i].GetComponentInChildren<TMP_Text>().text = _tutorials[i].Text;
        }
        _augmentUIManager = FindFirstObjectByType<WeaponAugmentUIManager>();
        _inputHandler = FindFirstObjectByType<InputHandler>();
        _augmentMover = _augmentUIManager.GetComponentInChildren<UIMover>();

        foreach (Tutorial tutorial in _tutorials) {
            switch (tutorial.Action) {
                case ActionType.MOVEMENT_PRESS:
                    _inputHandler.InputActions.PlayerControls.Move.started += (InputAction.CallbackContext context) => TryComplete(tutorial);
                    break;

                case ActionType.DASH_PRESSED:
                    _inputHandler.InputActions.PlayerControls.Dash.started += (InputAction.CallbackContext context) => TryComplete(tutorial);
                    break;

                case ActionType.SPRINT_PRESSED:
                    _inputHandler.InputActions.PlayerControls.Sprint.started += (InputAction.CallbackContext context) => TryComplete(tutorial);
                    break;

                case ActionType.BRAKE_PRESSED:
                    _inputHandler.InputActions.PlayerControls.Brake.started += (InputAction.CallbackContext context) => TryComplete(tutorial);
                    break;

                case ActionType.SHOOT_PRESSED:
                    _inputHandler.InputActions.PlayerControls.Fire.started += (InputAction.CallbackContext context) => TryComplete(tutorial);
                    _inputHandler.InputActions.PlayerControls.HeavyFire.started += (InputAction.CallbackContext context) => TryComplete(tutorial);
                    _inputHandler.InputActions.PlayerControls.SpecialFire.started += (InputAction.CallbackContext context) => TryComplete(tutorial);
                    break;

                case ActionType.TAB_PRESSED:
                    _augmentMover.OnOpenStart += () => TryComplete(tutorial);
                    break;

                case ActionType.AUGMENT_BUY:
                    _augmentUIManager.AugmentPurchase += () => TryComplete(tutorial);
                    break;
            }
        }  
        StartCoroutine(FadePrompt(_tutorialCanvases[0], 1f, true));
    }

    private void TryComplete(Tutorial tutorial) {
        if (_tutorials[_tutorialIndex] == tutorial) {
            UnsubscribeListener(tutorial);
            NextPrompt();
        }
    }

    private void UnsubscribeListener(Tutorial tutorial) {
        switch (tutorial.Action) {
            case ActionType.MOVEMENT_PRESS:
                _inputHandler.InputActions.PlayerControls.Move.started -= (InputAction.CallbackContext context) => TryComplete(tutorial);
                break;

            case ActionType.DASH_PRESSED:
                _inputHandler.InputActions.PlayerControls.Dash.started -= (InputAction.CallbackContext context) => TryComplete(tutorial);
                break;

            case ActionType.SPRINT_PRESSED:
                _inputHandler.InputActions.PlayerControls.Sprint.started -= (InputAction.CallbackContext context) => TryComplete(tutorial);
                break;

            case ActionType.BRAKE_PRESSED:
                _inputHandler.InputActions.PlayerControls.Brake.started -= (InputAction.CallbackContext context) => TryComplete(tutorial);
                break;

            case ActionType.SHOOT_PRESSED:
                _inputHandler.InputActions.PlayerControls.Fire.started -= (InputAction.CallbackContext context) => TryComplete(tutorial);
                _inputHandler.InputActions.PlayerControls.HeavyFire.started -= (InputAction.CallbackContext context) => TryComplete(tutorial);
                _inputHandler.InputActions.PlayerControls.SpecialFire.started -= (InputAction.CallbackContext context) => TryComplete(tutorial);
                break;

            case ActionType.TAB_PRESSED:
                _augmentMover.OnOpenStart -= () => TryComplete(tutorial);
                break;

            case ActionType.AUGMENT_BUY:
                _augmentUIManager.AugmentPurchase -= () => TryComplete(tutorial);
                break;
        }
    }

    public void NextPrompt() {
        if (_tutorialIndex >= _tutorials.Length) {
            return;
        }
        StartCoroutine(FadePrompt(_tutorialCanvases[_tutorialIndex], 1f, false));
        _tutorialIndex++;
        if (_tutorialIndex < _tutorials.Length) {
            StartCoroutine(FadePrompt(_tutorialCanvases[_tutorialIndex], 1f, true));
        } else if (_tutorialIndex >= _tutorials.Length) { //Player finished tutorial
            StartCoroutine(FadePrompt(_tutorialCanvases[^1], 1f, false));
            Globals.Instance.SettingsManager.CurrentSettings.TutorialMode = false;
            Settings.ApplySettings(Globals.Instance.SettingsManager.CurrentSettings);
            Globals.Instance.SettingsManager.Save();
            Debug.Log("Tutorial Finished");
            Destroy(gameObject, 1.5f);
        }
    }

    private IEnumerator FadePrompt(CanvasGroup text, float time, bool isFadingIn = true) {
        float targetAlpha = isFadingIn ? 1.0f : 0.0f;
        float initialAlpha = isFadingIn ? 0.0f : 1.0f;
        Debug.Log($"Interpolating prompt {_tutorials[Array.IndexOf(_tutorialCanvases, text)].Action} alpha from {initialAlpha}, to {targetAlpha}");
        CountDownTimer timer = new CountDownTimer(time);
        timer.Start();
        while (timer.IsRunning) {
            text.alpha = Mathf.Lerp(initialAlpha, targetAlpha, 1f - timer.Progress);
            timer.Update(Time.fixedDeltaTime);
            yield return Yielders.WaitForFixedUpdate;
        }
    }
}