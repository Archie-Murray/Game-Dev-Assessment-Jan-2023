using System;

using Enemy;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {
    public bool BossDead = false;
    public bool PlayerAlive = true;
    public bool BossSpawned = false;

    TickSystem _tickSystem;

    [SerializeField] private EnemyManager[] _enemyManagers = null;

    private void Start() {
        _enemyManagers = FindObjectsOfType<EnemyManager>();
        _tickSystem = GetComponent<TickSystem>();
        _tickSystem.TickLoop += HandleEndGame;
    }

    public bool GameEnd => BossDead && SpawnersFinished;
    private bool SpawnersFinished { 
        get { 
            foreach (EnemyManager enemyManager in _enemyManagers) {
                if (!enemyManager.FinishedSpawning) {
                    return false;
                }
            }
            return true;
        } 
    }
    private void HandleEndGame(float dt) {
        if (GameEnd) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        } else if (!PlayerAlive) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
    }
}