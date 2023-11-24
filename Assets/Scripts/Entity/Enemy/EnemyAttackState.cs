using ProjectileComponents;

using UnityEngine;

using Utilities;

namespace Enemy {
    public class EnemyAttackState : EnemyState {

        public EnemyAttackState(EnemyController controller, SpriteRenderer spriteRenderer, EnemyManager enemyManager) : base(controller, spriteRenderer, enemyManager) { }
        public EnemyAttackState(EnemyState previousState) : base(previousState) { }
        public EnemyAttackState() : base() { }

        public override void Start() {
            _spriteRenderer.color = Color.red;
        }
        public override void FixedUpdate() {
            if (_controller.AttackTimer.IsFinished) {
                CreateMissile();
            }
        }

        private void CreateMissile() {
            GameObject projectile = GameObject.Instantiate(_enemyManager.EnemyProjectile, _controller.transform.position + (2f * _controller.transform.up), _controller.transform.rotation);
            if (_controller) {
                _spriteRenderer.FlashColour(Color.magenta, 0.5f, _controller);
            }
            projectile.GetOrAddComponent<ProjectileMover>().Speed = 10f;
            projectile.GetOrAddComponent<AutoDestroy>().Duration = 10f;
            projectile.GetOrAddComponent<EnemyProjectile>();
            projectile.GetOrAddComponent<EntityDamager>().Init(DamageFilter.Player, _controller.Damage);
            _controller.AttackTimer.Reset();
            _controller.AttackTimer.Start();
        }

        public override void CheckTransitions() {
            if (!_enemyManager.Target) {
                SwitchState(EnemyStateFactory.State<EnemyIdleState>());
                return;
            }
            if (!_controller.InAttackRange && _controller.InChaseRange) {
                SwitchState(EnemyStateFactory.State<EnemyChaseState>());
            } else if (!_controller.InChaseRange) {
                SwitchState(EnemyStateFactory.State<EnemyPatrolState>());
            }
        }

        public override void Update() { }
        public override void Exit() {
            _spriteRenderer.material.color = Color.white;
        }
    }
}