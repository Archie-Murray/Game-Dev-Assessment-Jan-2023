

using UnityEngine;

namespace ProjectileComponents {
    [CreateAssetMenu(fileName = "Saw Blade Spawn Strategy", menuName = "Projectile Spawn Strategy/Saw Blade")]
    public class SawBladeSpawnStrategy : ProjectileSpawnStrategy {
        public float RotationSpeed;
        public float DamageRate;
        public override void Fire(Transform origin) {
            GameObject sawBlade = Instantiate(Projectile, origin.position, origin.rotation);
            sawBlade.GetOrAddComponent<SawBladeProjectile>().Init(Damage, DamageRate, RotationSpeed);
            sawBlade.GetOrAddComponent<AutoDestroy>().Duration = Duration;
            sawBlade.GetOrAddComponent<ProjectileMover>().Speed = Speed;
            sawBlade.GetOrAddComponent<PlayerProjectile>();
        }
    }
}
