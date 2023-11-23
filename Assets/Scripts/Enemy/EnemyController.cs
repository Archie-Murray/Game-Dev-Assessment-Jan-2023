using UnityEngine;
using UnityEngine.AI;
namespace Enemy {
    public class EnemyController : MonoBehaviour {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private float _maxSpeed = 200f;
        [SerializeField] private float _maxAcceleration = 60f;
        [SerializeField] private float _maxTurnSpeed = 5f;
        private EnemyState _state;

        public EnemyState State { get { return _state; } set { _state = value; } }
        public NavMeshAgent Agent { get { return _agent; } }

        public void Awake() {
            _agent = GetComponent<NavMeshAgent>();
            _agent.acceleration = _maxAcceleration;
            _agent.speed = _maxSpeed;
            _agent.angularSpeed = _maxTurnSpeed;
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
        }

        public void Start() {
            //_state = new EnemyPatrolState(this, GetComponent<SpriteRenderer>(), FindFirstObjectByType<EnemyManager>());
            _state = new EnemyChaseState(this, GetComponent<SpriteRenderer>(), FindFirstObjectByType<EnemyManager>());
            _state?.Start();
        }

        public void Update() {
            _state?.Update();
        }

        public void FixedUpdate() {
            _state?.FixedUpdate();
        }
    }
}