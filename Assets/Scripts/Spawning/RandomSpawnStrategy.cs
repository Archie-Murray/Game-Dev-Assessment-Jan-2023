using System.Collections.Generic;
using UnityEngine;

namespace Spawning {
    public class RandomSpawnStrategy : ISpawnStrategy {
        private Transform[] points;
        private List<Transform> unused;

        public RandomSpawnStrategy(Transform[] points) {
            this.points = points;
            unused = new List<Transform>(points);
        }

        public Transform GetPosition() {
            Transform point = unused[Random.Range(0, unused.Count)];
            unused.Remove(point);
            if (unused.Count == 0) {
                unused = new List<Transform>(points);
            }
            return point;
        }
    }
}