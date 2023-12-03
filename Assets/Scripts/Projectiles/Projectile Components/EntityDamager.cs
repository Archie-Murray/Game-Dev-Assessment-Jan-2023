using System;

using Enemy;

using UnityEngine;
namespace ProjectileComponents {
    [Serializable] public enum DamageFilter { Player, Enemy }
    public class EntityDamager : MonoBehaviour {
        [SerializeField] private DamageFilter _filter;
        [SerializeField] private float _damage;

        public void Init(DamageFilter filter, float damage) {
            _filter = filter;
            _damage = damage;
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (
                _filter switch { 
                    DamageFilter.Player => collision.gameObject.HasComponent<PlayerProjectile>(),
                    DamageFilter.Enemy => collision.gameObject.HasComponent<EnemyProjectile>(),
                    _ => false
                }
            ) {
                Destroy(collision.gameObject); //Don't need to destroy self as collided bullet will do it
            } else {
                Health entityHealth = _filter switch {
                    DamageFilter.Player => collision.gameObject.HasComponent<PlayerController>() ? collision.GetComponent<Health>() : null,
                    DamageFilter.Enemy => (1 << collision.gameObject.layer & Globals.Instance.EnemyLayer.value) > 0 ? collision.GetComponent<Health>() : null,
                    _ => null
                };
                
                if (entityHealth) {
                    entityHealth.Damage(_damage);
                    Destroy(gameObject);
                }
            }
        }
    }
}