using System;
using System.Collections;

using Boss;

using Enemy;

using UnityEngine;
using UnityEngine.SceneManagement;

using Utilities;

public class GameManager : Singleton<GameManager> {
    public bool BossDead = false;
    public bool PlayerAlive = true;
    public bool InMenu = false;
    [SerializeField] private BossSpawner _bossSpawner;
    [SerializeField] private CanvasGroup _winScreen;
    [SerializeField] private CanvasGroup _loseScreen;
    [SerializeField] private TickSystem _tickSystem;
    [SerializeField] private EnemyManager[] _enemyManagers = null;
    [SerializeField] private BossManager _bossManager = null;
    [SerializeField] private CountDownTimer _combatTimer = new CountDownTimer(0f);
    [SerializeField] private BGMEmitter _bgmEmitter;
    private Coroutine _endGameState = null;

    private void Start() {
        _enemyManagers = FindObjectsOfType<EnemyManager>();
        _bossManager = FindFirstObjectByType<BossManager>();
        _tickSystem = GetComponent<TickSystem>();
        _tickSystem.TickLoop += HandleEndGame;
        _tickSystem.TickLoop += (float deltaTime) => _combatTimer.Update(deltaTime);
        _bgmEmitter = GetComponent<BGMEmitter>();
        foreach (EnemyManager enemyManager in _enemyManagers) {
            enemyManager.OnSpawnFinish += CheckBossCanSpawn;
        }
        _combatTimer.OnTimerStart += () => _bgmEmitter.PlayBGM(BGMType.COMBAT);
        _combatTimer.OnTimerStop += () => _bgmEmitter.PlayBGM(BGMType.PASSIVE);
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
        if (_endGameState != null) {
            return;
        }
        if (GameEnd) {
            _endGameState = StartCoroutine(WinState());
        } else if (!PlayerAlive) {
            _endGameState = StartCoroutine(LoseState());
        }
    }

    private void CheckBossCanSpawn() {
        if (SpawnersFinished) {
            _bossManager.EnableSpawn();
        }
    }

    private IEnumerator WinState() {
        ToggleWinScreen(true);
        yield return Yielders.WaitForSeconds(2f);
        SceneManager.LoadScene(0);
    }

    private void ToggleWinScreen(bool toggle) {
        _winScreen.interactable = toggle;
        _winScreen.alpha = toggle ? 1f : 0f;
    }

    private IEnumerator LoseState() {
        ToggleLoseScreen(true);
        yield return Yielders.WaitForSeconds(2f);
        SceneManager.LoadScene(0);
    }

    private void ToggleLoseScreen(bool toggle) {
        _loseScreen.interactable = toggle;
        _loseScreen.alpha = toggle ? 1f : 0f;
    }
    
    public void MainMenu() {
        SceneManager.LoadScene(0);
    }

    public void ResetCombatTimer() {
        _combatTimer.Reset(5f);
        _combatTimer.Start();
    }
}