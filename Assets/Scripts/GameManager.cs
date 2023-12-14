using System;
using System.Collections;

using Enemy;

using UnityEngine;
using UnityEngine.SceneManagement;

using Utilities;

public class GameManager : Singleton<GameManager> {
    public bool BossDead = false;
    public bool PlayerAlive = true;
    [SerializeField] private CanvasGroup _winScreen;
    [SerializeField] private CanvasGroup _loseScreen;
    [SerializeField] private TickSystem _tickSystem;
    [SerializeField] private EnemyManager[] _enemyManagers = null;
    private Coroutine _endGameState = null;

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
        if (_endGameState != null) {
            return;
        }
        if (GameEnd) {
            _endGameState = StartCoroutine(WinState());
        } else if (!PlayerAlive) {
            _endGameState = StartCoroutine(LoseState());
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
}