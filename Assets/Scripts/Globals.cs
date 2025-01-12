using TMPro;

using UnityEngine;
using UnityEngine.Audio;

public class Globals : Singleton<Globals> {
    public int Money = 100;
    public bool TutorialMode = true;
    public Camera MainCamera;
    private MoneyDisplay _moneyReadout;
    public LayerMask PlayerLayer;
    public LayerMask EnemyLayer;
    public SoundManager SoundManager;
    public SettingsManager SettingsManager;
    [SerializeField] private SoundEffect[] _effects;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioMixerGroup _sfx, _bgm;

    public void Start() {
        SoundManager = new SoundManager(_effects, _audioMixer, _sfx, _bgm);
        _moneyReadout = FindFirstObjectByType<MoneyDisplay>();
        SettingsManager = FindFirstObjectByType<SettingsManager>();
    }

    public void AddMoney(int amount) {
        Money += amount;
        _moneyReadout.OrNull()?.UpdateMoney(amount, Money);
    }
}