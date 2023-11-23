using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
namespace Enemy {

    public class EnemyManager : MonoBehaviour {
        [SerializeField] private Transform[] _wanderPoints;
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _enemyProjectile;

        public Transform[] WanderPoints { get { return _wanderPoints; } }
        public Transform Target { get { return _target; } }
        public GameObject EnemyProjectile { get { return _enemyProjectile; } }

        private void Awake() {
            _target = FindFirstObjectByType<PlayerController>().transform;
            _wanderPoints = Array.ConvertAll(FindObjectsOfType<WanderPoint>(), (WanderPoint point) => point.transform);
        }
    }
}