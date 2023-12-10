using TMPro;

using UnityEngine;
using UnityEngine.Audio;

public class Globals : Singleton<Globals> {
    public int Money = 0;
    public bool TutorialMode = true;
    public Camera MainCamera;
    private TMP_Text _moneyReadout;
    public LayerMask PlayerLayer;
    public LayerMask EnemyLayer;
    public SoundManager SoundManager;
    [SerializeField] private SoundEffect[] _effects;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioMixerGroup _sfx, _bgm;
    public void UpdatePlayerMoney() {
        if (!_moneyReadout) {
            _moneyReadout = FindFirstObjectByType<MoneyReadout>().GetComponentInChildren<TMP_Text>();
        }
        _moneyReadout.text = $"Money: {Money}";
    }

    public void Start() {
        SoundManager = new SoundManager(_effects, _audioMixer, _sfx, _bgm);
    }

    public void AddMoney(int amount) {
        Money += amount;
        UpdatePlayerMoney();
    }
}