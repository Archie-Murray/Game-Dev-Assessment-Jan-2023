using UnityEngine;

namespace Enemy {
    public class EnemyChaseState : EnemyState {
        public EnemyChaseState(EnemyController controller, SpriteRenderer spriteRenderer, EnemyManager enemyManager, EnemyState.EnemyStateFactory enemyStateFactory) : base(controller, spriteRenderer, enemyManager, enemyStateFactory) { }


        public override void Start() {
            _spriteRenderer.color = Color.yellow;
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
            _spriteRenderer.color = Color.white;
        }

    }
}
