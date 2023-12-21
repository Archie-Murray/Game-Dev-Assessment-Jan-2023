using System;

using UnityEngine;

[Serializable]
public class BossSpawner {
    [SerializeField] private GameObject _bossPrefab;

    public BossSpawner(GameObject bossPrefab) {
        _bossPrefab = bossPrefab;
    }

    public void Spawn(Transform point) {
        UnityEngine.Object.Instantiate(_bossPrefab, point.position, Quaternion.identity);
    }
}