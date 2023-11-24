using System;
using System.Collections.Generic;

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

    public EnemyState(EnemyState previousState) {
        _controller = previousState._controller;
        _spriteRenderer = previousState._spriteRenderer;
        _enemyManager = previousState._enemyManager;
    }

    public EnemyState() { }

    public abstract void Start();
    public abstract void FixedUpdate();
    public abstract void Update();
    public abstract void CheckTransitions();
    public abstract void Exit();

    public void SwitchState(EnemyState state) {
        Debug.Log($"Switching from State: {_controller.State.GetType()} to {state.GetType()}");
        _controller.State?.Exit();
        _controller.State = state;
        _controller.State.Start();
    }

    public class EnemyStateFactory {

        private static Dictionary<Type, EnemyState> _states;

        public static void Init(EnemyController controller, SpriteRenderer renderer, EnemyManager manager) {
            _states = new Dictionary<Type, EnemyState>() {
                { typeof(EnemyIdleState), new EnemyIdleState(controller, renderer, manager) },
                { typeof(EnemyPatrolState), new EnemyPatrolState(controller, renderer, manager) },
                { typeof(EnemyChaseState), new EnemyChaseState(controller, renderer, manager) },
                { typeof(EnemyAttackState), new EnemyAttackState(controller, renderer, manager) },
            };
        }

        public static T State<T>() where T : EnemyState {
            if (!_states.ContainsKey(typeof(T))) {
                Debug.Log($"Could not find state: {typeof(T)}");
                return _states[typeof(EnemyIdleState)] as T;
            }
            return _states[typeof(T)] as T;
        }
    }
}