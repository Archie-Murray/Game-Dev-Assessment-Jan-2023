using System;
using System.Collections;

using TMPro;

using UnityEngine;

using Utilities;

public class MoneyDisplay : MonoBehaviour {
    [SerializeField] private float _updateInterval = 0.1f;
    [SerializeField] private int _displayMoney;
    [SerializeField] private TMP_Text _readout;
    [SerializeField] private string _prefix = "Money: ";

    private Coroutine _moneyTick;

    private void Start() {
        _displayMoney = Globals.Instance.Money;
        _readout = GetComponent<TMP_Text>();
        _readout.text = DisplayText();
    }

    public void UpdateMoney(int newValue) {
        if (_moneyTick == null) {
            _moneyTick = StartCoroutine(TickMoney(newValue));
        } else {
            StopCoroutine(_moneyTick);
            _moneyTick = StartCoroutine(TickMoney(newValue));
        }
    }

    private IEnumerator TickMoney(int newValue) {
        int increment = (int) Mathf.Sign(newValue - _displayMoney);
        while (_displayMoney != newValue) {
            _displayMoney += increment;
            _readout.text = DisplayText();
            yield return Yielders.WaitForSeconds(_updateInterval);
        }
    }

    private string DisplayText() {
        return $"{_prefix}{_displayMoney}";
    }
}