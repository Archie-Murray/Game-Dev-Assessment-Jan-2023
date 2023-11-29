using UnityEngine;
namespace Spawning {
    public class EnemySpawner {
        private ISpawnStrategy _spawnStrategy;
        [SerializeField] GameObject _enemyPrefab;
        public GameObject Spawn() {
            Transform spawnPoint = _spawnStrategy.GetPosition();
            GameObject instance = GameObject.Instantiate(_enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            return instance;
        }
    }
}