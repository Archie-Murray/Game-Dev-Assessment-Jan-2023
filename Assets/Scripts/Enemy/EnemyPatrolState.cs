using UnityEngine;

namespace Enemy {
    public class EnemyPatrolState : EnemyState {
        private int _wanderPointIndex;
        public EnemyPatrolState(EnemyController controller, SpriteRenderer spriteRenderer, EnemyManager enemyManager) : base(controller, spriteRenderer, enemyManager) { }

        public override void Start() {
            _spriteRenderer.material.color = Color.red;
            _wanderPointIndex = Random.Range(0, _enemyManager.WanderPoints.Length);
            Debug.Log($"Index: {_wanderPointIndex}, max: {_enemyManager.WanderPoints.Length}");
            _controller.Agent.destination = _enemyManager.WanderPoints[_wanderPointIndex].position;
        }

        public override void FixedUpdate() {
            if (Vector3.Distance(_controller.Agent.destination, _controller.transform.position) <= _controller.Agent.stoppingDistance) {
                _wanderPointIndex = ++_wanderPointIndex % _enemyManager.WanderPoints.Length;
                _controller.Agent.destination = _enemyManager.WanderPoints[_wanderPointIndex].position;
            }
        }

        public override void Update() { }
        public override void Exit() { 
            _spriteRenderer.material.color = Color.white; 
        }
    }
}