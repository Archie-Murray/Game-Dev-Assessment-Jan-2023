using ProjectileComponents;

using UnityEngine;

[CreateAssetMenu(fileName = "Shotgun Spawn Strategy", menuName = "Projectile Spawn Strategy/Shotgun")]
public class ShotgunSpawnStrategy : ProjectileSpawnStrategy {
    public GameObject Projectile;

    public ShotgunSpawnStrategy(GameObject projectile) {
        Projectile = projectile;
    }

    public float Count;
    public float SpreadAngle;

    public override void Fire(Transform origin) {
        float startAngle = Vector2.SignedAngle(origin.up, Vector3.up);
        float halfSpread = SpreadAngle * 0.5f;
        for (float angle = startAngle - halfSpread; angle <= startAngle + halfSpread; angle += SpreadAngle / Count) {
            GameObject projectileInstance = Instantiate(Projectile, origin.position + (Vector3) (Helpers.FromRadians(angle * Mathf.Deg2Rad) * 2f), Quaternion.AngleAxis(angle, Vector3.back));
            projectileInstance.GetOrAddComponent<AutoDestroy>().Duration = Duration;
            projectileInstance.GetOrAddComponent<ProjectileMover>().Speed = Speed;
            projectileInstance.GetOrAddComponent<PlayerProjectile>();
            projectileInstance.GetOrAddComponent<EntityDamager>().Init(DamageFilter.Enemy, Damage);
        }
    }
}