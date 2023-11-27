using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.AI;

using Utilities;

namespace Enemy {
    public class EnemyController : MonoBehaviour {

        [Header("Component References")]
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private EnemyManager _enemyManager;

        [Header("Enemy Parameters")]
        [SerializeField] private float _maxSpeed = 200f;
        [SerializeField] private float _maxAcceleration = 60f;
        [SerializeField] private float _maxTurnSpeed = 5f;
        [SerializeField] private float _chaseRange = 10f;
        [SerializeField] private float _attackRange = 5f;
        [SerializeField] private float _fireRate = 1f;
        [SerializeField] private float _damage = 1f;

        [Header("Gameplay Variables")]
        [SerializeField] private CountDownTimer _attackTimer;
        private EnemyState _state;
        private EnemyState.EnemyStateFactory _stateFactory;

        public EnemyState State { get { return _state; } set { _state = value; } }
        public NavMeshAgent Agent { get { return _agent; } }
        public CountDownTimer AttackTimer { get { return _attackTimer; } }
        public float Damage => _damage;
        public bool InChaseRange => Vector3.Distance(_enemyManager.Target.position, transform.position) <= _chaseRange;
        public bool InAttackRange => Vector3.Distance(_enemyManager.Target.position, transform.position) <= _attackRange;
        public bool HasTarget => _enemyManager.Target != null;

        public void Awake() {
            _agent = GetComponent<NavMeshAgent>();
            _agent.acceleration = _maxAcceleration;
            _agent.speed = _maxSpeed;
            _agent.updateUpAxis = false;
            _agent.updateRotation = false;
            _agent.stoppingDistance = _attackRange - 0.5f;
            _enemyManager = transform.parent.GetComponent<EnemyManager>();
            _attackTimer = new CountDownTimer(_fireRate);
        }

        public void Start() {
            _stateFactory = new EnemyState.EnemyStateFactory(this, GetComponent<SpriteRenderer>(), _enemyManager);
            _state = _stateFactory.State<EnemyPatrolState>();
            if (_state == null) {
                Debug.Log("State was not initialised, help, aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            }
            _state?.Start();
        }

        public void Update() {
            _state?.Update();
            _state?.CheckTransitions();
            UpdateRotation();
        }

        public void FixedUpdate() {
            _state?.FixedUpdate();
            _attackTimer.Update(Time.fixedDeltaTime);
        }

        public void UpdateRotation() {
            if (!HasTarget) {
                return;
            }
            float angle = Vector2.SignedAngle((Vector2)(_enemyManager.Target.position - transform.position), Vector2.up);
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.back);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * _maxTurnSpeed);
        }
    }
}