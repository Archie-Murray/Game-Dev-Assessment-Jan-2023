using System;


using UnityEngine;
namespace Boss {
    [Serializable]
    public class BossSpawner {
        [SerializeField] private GameObject _bossPrefab;

        public BossSpawner(GameObject bossPrefab, BossAttack[] attacks) {
            _bossPrefab = bossPrefab;
            bossPrefab.GetComponent<BossController>().SetAttacks(attacks);
        }

        public void Spawn(Transform point) {
            UnityEngine.Object.Instantiate(_bossPrefab, point.position, Quaternion.identity);
        }
    } 
}