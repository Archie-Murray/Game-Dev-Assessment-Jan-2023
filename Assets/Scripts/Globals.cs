using TMPro;

using UnityEngine;

public class Globals : Singleton<Globals> {
    public int Money = 0;
    public Camera MainCamera;
    private TMP_Text _moneyReadout;
    public LayerMask PlayerLayer;
    public LayerMask EnemyLayer;
    public SoundManager SoundManager;
    [SerializeField] private SoundEffect[] _effects;
    public void UpdatePlayerMoney() {
        if (!_moneyReadout) {
            _moneyReadout = FindFirstObjectByType<MoneyReadout>().GetComponentInChildren<TMP_Text>();
        }
        _moneyReadout.text = $"Money: {Money}";
    }

    public void Start() {
        SoundManager = new SoundManager(_effects);
    }

    public void AddMoney(int amount) {
        Money += amount;
        UpdatePlayerMoney();
    }
}