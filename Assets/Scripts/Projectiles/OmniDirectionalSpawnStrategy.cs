using UnityEngine;

[CreateAssetMenu(fileName = "Omni-Directional Spawn Strategy", menuName = "Projectile Spawn Strategy/Omni-Directional")]
public class OmniDirectionalSpawnStrategy : ProjectileSpawnStrategy {
    public float Count;
    public GameObject projectile;
    public override void Fire(Transform origin) {
        Transform player = origin.parent;
        for (float angle = -180f; angle < 180f; angle += 360f / Count) {
            GameObject projectileInstance = Instantiate(projectile, player.position + (Vector3) (Helpers.FromRadians(angle * Mathf.Deg2Rad) * 2f), Quaternion.AngleAxis(angle, Vector3.back));
            projectileInstance.GetOrAddComponent<AutoDestroy>().Duration = Duration;
            projectileInstance.GetOrAddComponent<ProjectileMover>().Speed = Speed;
        }
    }
}