using UnityEngine;

namespace ProjectileComponents {

    public class ProjectileMover : MonoBehaviour {

        public float Speed = 0f;

        private void FixedUpdate() {
            transform.position += Speed * Time.fixedDeltaTime * transform.up;
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.gameObject.HasComponent<Wall>()) {
                Instantiate(Assets.Instance.HitParticles, transform.position, Quaternion.LookRotation(-transform.up));
                Destroy(gameObject);
            }
        }
    }
}