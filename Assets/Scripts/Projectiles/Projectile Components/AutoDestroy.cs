using UnityEngine;

namespace ProjectileComponents {

    public class AutoDestroy : MonoBehaviour {
        public float Duration;

        public void Start() {
            Destroy(gameObject, Duration);
        }
    }
}