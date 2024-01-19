using ProjectileComponents;

using UnityEngine;

[CreateAssetMenu(fileName = "Shield Projectile Strategy", menuName = "Projectile Spawn Strategy/Shield")]
public class ShieldProjectileSpawnStrategy : ProjectileSpawnStrategy {
    public float AngleDelta;
    public float RotationSpeed;
    public override void Fire(Transform origin) {
        GameObject shield = Instantiate(Projectile, origin.position, origin.rotation);
        shield.GetOrAddComponent<AutoDestroy>().Duration = Duration;
        shield.GetOrAddComponent<ShieldController>().Init(AngleDelta, RotationSpeed);
    }
}