using System;
using System.Collections;

using TMPro;

using UnityEngine;

using Utilities;

public class MoneySlider : MonoBehaviour {
    
    [SerializeField] private int _amount;
    [SerializeField] private Vector2 _offset;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _slideSpeed = 5f;
    [SerializeField] private float _slideTime = 0.75f;

    private void Start() {
        _text = GetComponent<TMP_Text>();
    }

    public void Init(int amount) {
        _text.color = amount > 0 ? Color.green : Color.red;
        GetComponent<RectTransform>().anchoredPosition += _offset;
        _text.text = $"{(amount > 0 ? "+" : "-")}{amount}";
        _amount = amount;
        StartCoroutine(Slide());
    }

    private IEnumerator Slide() {
        float timer = 0f;
        while (timer < _slideTime) {
            timer += Time.fixedDeltaTime;
            transform.position += Vector3.up * Mathf.Sign(_amount) * _slideSpeed * Time.fixedDeltaTime;
            _text.alpha = 1f - Mathf.Clamp01(timer / _slideTime);
            yield return Yielders.WaitForFixedUpdate;
        }
        Destroy(gameObject);
    }
}