using UnityEngine;

namespace Enemy {
    public class EnemyChaseState : EnemyState {
        public EnemyChaseState(EnemyController controller, SpriteRenderer spriteRenderer, EnemyManager enemyManager) : base(controller, spriteRenderer, enemyManager) { }
        public EnemyChaseState(EnemyState previousState) : base(previousState) { }
        public EnemyChaseState() : base() { }


        public override void Start() {
            _spriteRenderer.material.color = Color.yellow;
            _controller.Agent.destination = _enemyManager.Target.position;
        }
        public override void FixedUpdate() {
            if (Vector2.Distance(_enemyManager.Target.position, _controller.Agent.destination) > 1f) {
                _controller.Agent.destination = _enemyManager.Target.position;
            }
        }
        public override void CheckTransitions() {
            if (!_controller.HasTarget) {
                SwitchState(_enemyStateFactory.State<EnemyIdleState>());
                return;
            }
            if (_controller.InAttackRange) {
                SwitchState(_enemyStateFactory.State<EnemyAttackState>());
            }
        }

        public override void Update() { }
        public override void Exit() {
            _spriteRenderer.material.color = Color.white;
        }

    }
}
