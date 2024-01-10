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
            _attackTimer = new CountDownTimer(_bossAttacks[_attackIndex].Cooldown);
            _agent = GetComponent<NavMeshAgent>();
            _health = GetComponent<Health>();
            _agent.updateUpAxis = false;
            _attackTimer.Start();
        }

        private void Start() {
            _health.OnDeath += () => GameManager.Instance.BossDead = true;
            _health.OnDeath += () => Globals.Instance.AddMoney(100);
            _health.OnDamage += (float damage) => _emitter.Play(SoundEffectType.HIT);
            _health.OnDeath += () => _emitter.Play(SoundEffectType.DESTROY);
        }

        public void SetAttacks(BossAttack[] attacks) {
            _bossAttacks = attacks;
        }

        private void FixedUpdate() {
            _attackTimer.Update(Time.fixedDeltaTime);
            Attack();
        }

        public void Attack() {
            if (!_attackTimer.IsFinished) {
                return;
            }
            _bossAttacks[_attackIndex].Attack(transform);
            _attackTimer.Reset(_bossAttacks[_attackIndex].Cooldown);
            _emitter.Play(_bossAttacks[_attackIndex].AbilitySoundEffect);
            _attackTimer.Start();
            _attackIndex = ++_attackIndex % _bossAttacks.Length;
        }
    }
}