using System;

using UnityEngine;
using UnityEngine.AI;

namespace Boss {
    public class HomingProjectileMover : MonoBehaviour {

        private Transform _target;
        private float _rotationSpeed = 5f;
        private NavMeshAgent _agent;

        private Vector3 UnitVectorToTarget => _target ? (_target.position - transform.position).normalized : transform.up;

        private void Awake() {
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateUpAxis = false;
            _agent.updateRotation = false;
        }

        public void Init(Transform target, float speed) {
            _agent.speed = speed;
            _target = target;
        }

        public void FixedUpdate() {
            if (!_target) {
                Destroy(gameObject);
                return;
            }

            if (Vector3.Distance(_target.position, transform.position) < 0.1f) { 
                _agent.destination = _target.position;
            }

            Rotate();
        }

        private void Rotate() {
            Quaternion rotation = Quaternion.AngleAxis(Vector2.SignedAngle(transform.up, UnitVectorToTarget), Vector3.back);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotationSpeed * Time.fixedDeltaTime);
        }
    }
}