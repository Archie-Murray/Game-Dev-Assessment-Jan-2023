using System;

using UnityEngine;

using Utilities;
[RequireComponent(typeof(TickSystem))]
public class SawBladeController : MonoBehaviour {
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _damage;
    [SerializeField] private float _radius;

    [SerializeField] private TickSystem _tickSystem;

    private void Start() {
        _radius = GetComponent<SpriteRenderer>().bounds.extents.x;
    }

    public void Init(float damage, float tickRate, float rotationSpeed) {
        _damage = damage;
        _rotationSpeed = rotationSpeed;
        _tickSystem = GetComponent<TickSystem>();
        _tickSystem.SetTickRate(tickRate);
        _tickSystem.TickLoop += Damage;
    }

    private void Damage(float _) {
        foreach ( Collider2D hit in Physics2D.OverlapCircleAll(transform.position, _radius, Globals.Instance.EnemyLayer)) {
            hit.GetComponent<Health>().Damage(_damage);
            Instantiate(Assets.Instance.HitParticles, hit.transform.position, transform.rotation);
        }
    }

    public void FixedUpdate() {
        transform.rotation = Quaternion.AngleAxis(transform.rotation.eulerAngles.z + _rotationSpeed * Mathf.PI * 2f * Time.fixedDeltaTime, Vector3.forward);
    }
}