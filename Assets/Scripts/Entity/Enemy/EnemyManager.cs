using System;

using UnityEngine;

using Utilities;
using Spawning;

namespace Enemy {

    public class EnemyManager : MonoBehaviour {
        [SerializeField] private Transform[] _wanderPoints;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _enemyProjectile;
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private CountDownTimer _spawnTimer = new CountDownTimer(5f);
        [SerializeField] private GameObject _enemyPrefab;

        public Transform[] WanderPoints { get { return _wanderPoints; } }
        public Transform Target { get { return _target; } }
        public GameObject EnemyProjectile { get { return _enemyProjectile; } }

        private void Awake() {
            _target = FindFirstObjectByType<PlayerController>().transform;
            _wanderPoints = Array.ConvertAll(FindObjectsOfType<WanderPoint>(), (WanderPoint point) => point.transform);
            _spawnPoints = Array.ConvertAll(GetComponentsInChildren<EnemySpawnPoint>(), (EnemySpawnPoint point) => point.transform);
            _enemySpawner = new EnemySpawner(new RandomSpawnStrategy(_spawnPoints), _enemyPrefab);
        }

        private void FixedUpdate() {
            _spawnTimer.Update(Time.fixedDeltaTime);
            if (_spawnTimer.IsFinished) {
                Spawn();
                _spawnTimer.Reset();
                _spawnTimer.Start();
            }
        }

        public void Spawn() {
            _enemySpawner.Spawn(transform).GetComponent<Health>().OnDeath += EnemyKillReward;
        }

        private void EnemyKillReward() {
            Globals.Instance.AddMoney(10);
        }
    }
}