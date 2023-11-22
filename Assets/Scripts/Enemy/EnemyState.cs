using Enemy;

using UnityEngine;

public abstract class EnemyState : IEnemyState {
    protected readonly EnemyController _controller;
    protected readonly Animator _animator;

    public EnemyState(EnemyController controller, Animator animator) {
        _controller = controller;
        _animator = animator;
    }

    public abstract void Start();
    public abstract void FixedUpdate();
    public abstract void Update();
    public abstract void Exit();

    public void SwitchState(EnemyState state) {
        _controller.State = state;
    }
}