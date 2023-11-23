using UnityEngine;

namespace Enemy {
    public class EnemyChaseState : EnemyState { 
        public EnemyChaseState(EnemyController controller, SpriteRenderer spriteRenderer, EnemyManager enemyManager) : base(controller, spriteRenderer, enemyManager) { }

        public override void Start() {
            _spriteRenderer.material.color = Color.red;
            _controller.Agent.destination = _enemyManager.Target.position;
        }
        public override void FixedUpdate() {
            if (Vector2.Distance(_enemyManager.Target.position, _controller.Agent.destination) > 1f) {
                _controller.Agent.destination = _enemyManager.Target.position;
            }
        }
        public override void Update() { }
        public override void Exit() {
            _spriteRenderer.material.color = Color.white;
        }
    }
}
