using System;

using UnityEngine;

using Utilities;
namespace Boss {

    public class BossManager : MonoBehaviour {
        private int _spawnCount = 0;
        private int _maxSpawnCount = 1;
        private bool _canSpawn = false;
        private CountDownTimer _spawnTimer = new CountDownTimer(1f);
        private BossSpawner _spawner;
        [SerializeField] private GameObject _bossPrefab;
        [SerializeField] private Transform _spawnPoint;

        public bool IsFinished => _spawnCount == _maxSpawnCount;

        public Action OnSpawnerFinish;

        private void Start() {
            _spawnTimer.Start();
            _spawner = new BossSpawner(_bossPrefab);
            _spawnPoint = GetComponentInChildren<BossSpawnPoint>().transform;
        }

        private void FixedUpdate() {
            if (_canSpawn) {
                _spawnTimer.Update(Time.fixedDeltaTime);
                if (_spawnTimer.IsFinished && _spawnCount < _maxSpawnCount) {
                    _spawner.Spawn(_spawnPoint);
                    _spawnCount++;
                    if (_spawnCount == _maxSpawnCount) {
                        OnSpawnerFinish?.Invoke();
                    }
                }
            }
        }

        public void EnableSpawn() {
            _canSpawn = true;
        }
    }
}