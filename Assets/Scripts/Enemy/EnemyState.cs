using Enemy;

using UnityEngine;

public abstract class EnemyState : IEnemyState {
    protected readonly EnemyController _controller;
    protected readonly SpriteRenderer _spriteRenderer;
    protected readonly EnemyManager _enemyManager;

    public EnemyState(EnemyController controller, SpriteRenderer spriteRenderer, EnemyManager enemyManager) {
        _controller = controller;
        _spriteRenderer = spriteRenderer;
        _enemyManager = enemyManager;
    }

    public abstract void Start();
    public abstract void FixedUpdate();
    public abstract void Update();
    public abstract void Exit();

    public void SwitchState(EnemyState state) {
        Debug.Log($"Switching from State: {_controller.State.GetType()} to {state.GetType()}");
        _controller.State?.Exit();
        _controller.State = state;
        _controller.State.Start();
    }

    public class EnemyStateFactory {
        public static Animator Animator;
        public static EnemyController Controller;

        public static EnemyPatrolState Idle(EnemyState self) {
            return new EnemyPatrolState(self._controller, self._spriteRenderer, self._enemyManager);
        }

    }
}