using UnityEngine;

using Utilities;

namespace Enemy {
    public class EnemyAttackState : EnemyState {
        public EnemyAttackState(EnemyController controller, SpriteRenderer spriteRenderer, EnemyManager enemyManager) : base(controller, spriteRenderer, enemyManager) { }
        readonly CountDownTimer _timer = new CountDownTimer(1f);
        public override void Start() {
            GameObject projectile = GameObject.Instantiate(_enemyManager.EnemyProjectile, _controller.transform.position + (2f * _controller.transform.forward), _controller.transform.rotation);
            _spriteRenderer.FlashColour(Color.yellow, 0.5f, _controller);
            projectile.GetOrAddComponent<ProjectileMover>().Speed = 10f;
            projectile.GetOrAddComponent<AutoDestroy>().Duration = 10f;
            _timer.Start();
        }
        public override void FixedUpdate() {
            _timer.Update(Time.fixedDeltaTime);
            //TODO: Maybe refactor to transition table or a check transition function
            if (_timer.IsFinished) {
                SwitchState(EnemyStateFactory.Idle(this));
            }
        }
        public override void Update() { }
        public override void Exit() {
            _spriteRenderer.material.color = Color.white;
        }
    }
}