using UnityEngine;

public class ShieldController : MonoBehaviour {

    [SerializeField] private float _angleDelta;
    [SerializeField] private float _rotationSpeed;
    public void Init(float angleDelta, float rotationSpeed) {
        _angleDelta = angleDelta;
        _rotationSpeed = rotationSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.HasComponent<EnemyProjectile>()) {
            Instantiate(Assets.Instance.HitParticles, collision.ClosestPoint(transform.position), collision.transform.rotation);
            Destroy(collision.gameObject);
        }
    }

    private void FixedUpdate() {
        transform.rotation = Quaternion.AngleAxis(transform.rotation.eulerAngles.z + (Mathf.Sin(Time.time * _rotationSpeed * Mathf.PI * 2f)) * _angleDelta * Time.fixedDeltaTime, Vector3.forward);
    }
}