using System;
using System.Collections;
using System.IO;

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Utilities;

public class SettingsManager : MonoBehaviour {
    [SerializeField] private Settings _currentSettings;
    public Settings CurrentSettings => _currentSettings;
    [SerializeField] private Toggle _tutorialToggle;
    [SerializeField] private Slider _globalVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;
    [SerializeField] private Slider _bgmVolumeSlider;
    [SerializeField] private bool _inMainMenu = false;
    [SerializeField] private AudioMixer _audioMixer;

    private Coroutine _saveCoroutine = null;

    private string _filePath;

    private void Awake() {
        FindFirstObjectByType<WeaponAugmentUIManager>();
        _filePath = Path.Combine(Application.dataPath, "Settings.data");
        if (File.Exists(_filePath)) {
            _currentSettings = JsonUtility.FromJson<Settings>(File.ReadAllText(_filePath));
            _currentSettings ??= Settings.Defaults; // Can't compress this as unity is initialising Settings
        } else {
            _currentSettings = Settings.Defaults;
        }
        _inMainMenu = SceneManager.GetActiveScene().buildIndex == 0;
        if (!_inMainMenu) {
            GetComponent<UIMover>().OnCloseFinish += Save;
            if (_currentSettings.TutorialMode) {
                GameManager.Instance.StartTutorial();
                Debug.Log("Started tutorial mode");
            }
        }
    }

    private void Start() {
        _tutorialToggle.onValueChanged.AddListener((bool value) => {
            _currentSettings.TutorialMode = value;
            ApplySettings();
        });
        _globalVolumeSlider.onValueChanged.AddListener((float value) => { 
            _currentSettings.GlobalVolume = VolumeRemap(value); 
            ApplySettings();
        });
        _sfxVolumeSlider.onValueChanged.AddListener((float value) => { 
            _currentSettings.SFXVolume = VolumeRemap(value); 
            ApplySettings();
        });
        _bgmVolumeSlider.onValueChanged.AddListener((float value) => { 
            _currentSettings.BGMVolume = VolumeRemap(value); 
            ApplySettings();
        });
        LoadSettings();
    }

    private void ApplySettings() {
        if (_inMainMenu) {
            Settings.ApplySettings(_currentSettings, true, _audioMixer);
        } else {
            Settings.ApplySettings(_currentSettings);
        }
    }

    public void SaveMainMenu() {
        if (_saveCoroutine != null) {
            StopCoroutine(_saveCoroutine);
            _saveCoroutine = null;
        }
        _saveCoroutine = StartCoroutine(SaveToMainMenu());
    }

    private float VolumeRemap(float value) {
        return 20f * Mathf.Log10(value);
    }

    private float ReverseVolumeRemap(float value) {
        return Mathf.Pow(10, value / 20f);
    }

    public void Save() {
        if (_saveCoroutine != null) {
            StopCoroutine(_saveCoroutine); 
            _saveCoroutine = null;
        }
        _saveCoroutine = StartCoroutine(SaveData());
    }

    private IEnumerator SaveToMainMenu() {
        string json = JsonUtility.ToJson(_currentSettings, true);
        yield return Yielders.WaitForFixedUpdate;
        File.WriteAllText(_filePath, json);
        _saveCoroutine = null;
        Debug.Log("Finished saving");
        yield return Yielders.WaitForEndOfFrame;
        GameManager.Instance.MainMenu();
    }

    private IEnumerator SaveData() {
        string json = JsonUtility.ToJson(_currentSettings, true);
        yield return Yielders.WaitForFixedUpdate;
        File.WriteAllText(_filePath, json);
        _saveCoroutine = null;
        Debug.Log("Finished saving");
    }

    public void LoadSettings() {
        _globalVolumeSlider.value = ReverseVolumeRemap(_currentSettings.GlobalVolume);
        _sfxVolumeSlider.value = ReverseVolumeRemap(_currentSettings.SFXVolume);
        _bgmVolumeSlider.value = ReverseVolumeRemap(_currentSettings.BGMVolume);
        _tutorialToggle.isOn = _currentSettings.TutorialMode;
    }
}