using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

using Utilities;

public class SettingsManager : MonoBehaviour {
    [SerializeField] private Settings _currentSettings;
    public Settings CurrentSettings => _currentSettings;
    [SerializeField] private Toggle _tutorialToggle;
    [SerializeField] private Slider _globalVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;
    [SerializeField] private Slider _bgmVolumeSlider;

    private Coroutine _saveCoroutine = null;

    private string filePath;

    private void Awake() {
        FindFirstObjectByType<WeaponAugmentUIManager>();
        filePath = Path.Combine(Application.dataPath, "Settings.data");
        if (File.Exists(filePath)) {
            _currentSettings = JsonUtility.FromJson<Settings>(File.ReadAllText(filePath));
            _currentSettings ??= Settings.Defaults; // Can't compress this as unity is initialising Settings
        } else {
            _currentSettings = Settings.Defaults;
        }
        GetComponent<UIMover>().OnCloseFinish += Save;
    }

    private void Start() {
        _tutorialToggle.onValueChanged.AddListener((bool value) => {
            _currentSettings.TutorialMode = value;
            Settings.ApplySettings(_currentSettings);
        });
        _globalVolumeSlider.onValueChanged.AddListener((float value) => { 
            _currentSettings.GlobalVolume = VolumeRemap(value); 
            Settings.ApplySettings(_currentSettings); 
        });
        _sfxVolumeSlider.onValueChanged.AddListener((float value) => { 
            _currentSettings.SFXVolume = VolumeRemap(value); 
            Settings.ApplySettings(_currentSettings); 
        });
        _bgmVolumeSlider.onValueChanged.AddListener((float value) => { 
            _currentSettings.BGMVolume = VolumeRemap(value); 
            Settings.ApplySettings(_currentSettings); 
        });
        LoadSettings();
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

    private IEnumerator SaveData() {
        Task saveTask = File.WriteAllTextAsync(filePath, JsonUtility.ToJson(_currentSettings, true));
        saveTask.Start();
        while (!saveTask.IsCompleted) { 
            yield return Yielders.WaitForEndOfFrame;
        }
        _saveCoroutine = null;
    }

    public void LoadSettings() {
        _globalVolumeSlider.value = ReverseVolumeRemap(_currentSettings.GlobalVolume);
        _sfxVolumeSlider.value = ReverseVolumeRemap(_currentSettings.SFXVolume);
        _bgmVolumeSlider.value = ReverseVolumeRemap(_currentSettings.BGMVolume);
        _tutorialToggle.isOn = _currentSettings.TutorialMode;
    }
}