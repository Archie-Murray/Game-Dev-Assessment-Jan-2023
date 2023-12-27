using TMPro;

using UnityEngine;
using UnityEngine.Audio;

public class Globals : Singleton<Globals> {
    public int Money = 0;
    public bool TutorialMode = true;
    public Camera MainCamera;
    private MoneyReadout _moneyReadout;
    public LayerMask PlayerLayer;
    public LayerMask EnemyLayer;
    public SoundManager SoundManager;
    [SerializeField] private SoundEffect[] _effects;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioMixerGroup _sfx, _bgm;

    public void Start() {
        SoundManager = new SoundManager(_effects, _audioMixer, _sfx, _bgm);
        _moneyReadout = FindFirstObjectByType<MoneyReadout>();
    }

    public void AddMoney(int amount) {
        Money += amount;
        _moneyReadout.OrNull()?.UpdateMoney(Money);
    }
}