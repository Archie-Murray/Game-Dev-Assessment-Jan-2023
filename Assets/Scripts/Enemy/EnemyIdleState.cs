using UnityEngine;

namespace Enemy {
    public class EnemyIdleState : EnemyState {
        public EnemyIdleState(EnemyController controller, SpriteRenderer spriteRenderer, EnemyManager enemyManager) : base(controller, spriteRenderer, enemyManager) { }

        public override void Start() {
            _spriteRenderer.material.color = Color.blue;
        }

        public override void FixedUpdate() { }
        public override void Update() { }
        public override void Exit() { 
            _spriteRenderer.material.color = Color.white; 
        }
    }
}