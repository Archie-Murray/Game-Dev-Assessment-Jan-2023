using UnityEngine;
using UnityEngine.AI;
namespace Enemy {
    public class EnemyController : MonoBehaviour {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private float _maxSpeed = 200f;
        [SerializeField] private float _maxAcceleration = 60f;
        [SerializeField] private float _maxTurnSpeed = 5f;

        public EnemyState State { get; internal set; }

        public void Awake() {
            _agent = GetComponent<NavMeshAgent>();
            _agent.acceleration = _maxAcceleration;
            _agent.speed = _maxSpeed;
            _agent.angularSpeed = _maxTurnSpeed;
        }
    }
}