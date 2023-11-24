using UnityEngine;

namespace Enemy {
    public class EnemyIdleState : EnemyState {
        public EnemyIdleState(EnemyController controller, SpriteRenderer spriteRenderer, EnemyManager enemyManager) : base(controller, spriteRenderer, enemyManager) { }
        public EnemyIdleState(EnemyState previousState) : base(previousState) { }
        public EnemyIdleState() : base() { }

        public override void Start() { 
            _spriteRenderer.material.color = Color.gray; 
        }

        public override void FixedUpdate() { }
        public override void Update() { }
        public override void CheckTransitions() { }
        public override void Exit() { }
    }
}