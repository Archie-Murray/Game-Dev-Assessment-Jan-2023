using UnityEngine;

[CreateAssetMenu(fileName = "Linear Spawn Strategy", menuName = "Projectile Spawn Strategy/Linear")]
public class LinearProjectileSpawnStrategy : ProjectileSpawnStrategy {
    public GameObject projectile;
    public float OffsetMagnitude = 2f;
    public override void Fire(Transform origin) {
        float rotation = Vector2.SignedAngle(origin.up, Vector2.up);
        GameObject projectileInstance = Instantiate(projectile, origin.position + (Vector3) (Helpers.FromRadians(rotation * Mathf.Deg2Rad) * OffsetMagnitude), origin.rotation);
        projectileInstance.GetOrAddComponent<ProjectileMover>().Speed = Speed;
        projectileInstance.GetOrAddComponent<AutoDestroy>().Duration = Duration;
    }
}
