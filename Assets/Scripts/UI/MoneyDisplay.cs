using System;
using System.Collections;

using TMPro;

using UnityEngine;

using Utilities;

public class MoneyDisplay : MonoBehaviour {
    [SerializeField] private TMP_Text _readout;
    [SerializeField] private string _prefix = "Money: ";
    [SerializeField] private GameObject _moneyChangePrefab;

    private Coroutine _moneyTick;

    private void Start() {
        _readout = GetComponent<TMP_Text>();
        _readout.text = $"{_prefix}{Globals.Instance.Money}";
    }

    public void UpdateMoney(int amount, int current) {
        Instantiate(_moneyChangePrefab, _readout.transform).GetComponent<MoneySlider>().Init(amount);
        _readout.text = $"{_prefix}{current}";
    }
}
