using System;

using UnityEngine;

namespace Boss {

    public class BossManager : MonoBehaviour {
        private bool _canSpawn = false;
        private BossSpawner _spawner;
        [SerializeField] private float _spawnDelay = 2.5f;
        [SerializeField] private GameObject _bossPrefab;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private BossAttack[] _attacks;

        public Action OnSpawnerFinish;

        private void Start() {
            _spawner = new BossSpawner(_bossPrefab, _attacks);
            _spawnPoint = GetComponentInChildren<BossSpawnPoint>().transform;
        }

        public void EnableSpawn() {
            Invoke(nameof(SpawnBoss), _spawnDelay);
        }

        private void SpawnBoss() {
            _spawner.Spawn(_spawnPoint);
            OnSpawnerFinish?.Invoke();
        }
    }
}