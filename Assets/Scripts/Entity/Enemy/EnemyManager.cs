using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.SocialPlatforms;

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

        private void Start() {
            Globals.Instance.UpdatePlayerMoney();
            foreach (Health enemyHealth in Array.ConvertAll(GetComponentsInChildren<EnemyController>(), (EnemyController controller) => controller.GetComponent<Health>()).ToArray()) {
                enemyHealth.OnDeath += EnemyKillReward;
            }
        }

        private void EnemyKillReward() {
            Globals.Instance.AddMoney(10);
        }
    }
}