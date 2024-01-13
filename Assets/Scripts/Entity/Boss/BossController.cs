using System.Collections.Generic;


using UnityEngine;
using UnityEngine.AI;

using Utilities;

namespace Boss {
    public class BossController : MonoBehaviour {
        [Header("Component References")]
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Health _health;
        [SerializeField] private SFXEmitter _emitter;

        [Header("Configuration Variables")]
        [SerializeField] private BossAttack[] _bossAttacks;
        [SerializeField] private int _attackIndex = 0;
        [SerializeField] private CountDownTimer _attackTimer;

        private void Awake() {
            _agent = GetComponent<NavMeshAgent>();
            _health = GetComponent<Health>();
            _emitter = GetComponent<SFXEmitter>();
            _agent.updateUpAxis = false;
        }

        private void Start() {
            _health.OnDeath += () => GameManager.Instance.BossDead = true;
            _health.OnDeath += () => Globals.Instance.AddMoney(100);
            _health.OnDamage += (float damage) => _emitter.Play(SoundEffectType.HIT);
            _health.OnDamage += (float damage) => GameManager.Instance.ResetCombatTimer();
            _health.OnDamage += (float damage) => GameManager.Instance.CameraShake(intensity: damage);
            _health.OnDeath += () => _emitter.Play(SoundEffectType.DESTROY);
            _attackTimer = new CountDownTimer(_bossAttacks[0].OrNull()?.Cooldown ?? 2f);
            _attackTimer.Start();
        }

        public void SetAttacks(BossAttack[] attacks) {
            _bossAttacks = attacks;
        }

        private void FixedUpdate() {
            _attackTimer.Update(Time.fixedDeltaTime);
            if (_attackTimer.IsFinished) {
                Attack();
            }
        }

        public void Attack() {            _bossAttacks[_attackIndex].Attack(transform);
            _attackTimer.Reset(_bossAttacks[_attackIndex].Cooldown);
            _emitter.Play(_bossAttacks[_attackIndex].AbilitySoundEffect);
            _attackTimer.Start();
            _attackIndex = ++_attackIndex % _bossAttacks.Length;
        }
    }
}