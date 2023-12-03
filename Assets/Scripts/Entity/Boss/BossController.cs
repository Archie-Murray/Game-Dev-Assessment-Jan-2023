using System.Collections.Generic;


using UnityEngine;
using UnityEngine.AI;

using Utilities;

namespace Boss {
    public class BossController : MonoBehaviour {
        [Header("Component References")]
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Health _health;

        [Header("Configuration Variables")]
        [SerializeField] private List<BossAttack> _bossAttacks;
        [SerializeField] private int _attackIndex = 0;
        [SerializeField] private CountDownTimer _attackTimer;

        private void Awake() {
            _attackTimer = new CountDownTimer(_bossAttacks[_attackIndex].Duration);
            _agent = GetComponent<NavMeshAgent>();
            _health = GetComponent<Health>();
            _agent.updateUpAxis = false;
            _attackTimer.Start();
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
            _attackTimer.Reset(_bossAttacks[_attackIndex].Duration);
            _attackTimer.Start();
            _attackIndex = ++_attackIndex % _bossAttacks.Count;
        }
    }
}