using ProjectileComponents;

using UnityEngine;

namespace Boss {
    [CreateAssetMenu(menuName = "Boss Attack/Homing Missile")]
    public class HomingMissile : BossAttack {

        public int MissileCount;
        public float MissileDuration;
        public float MissileSpeed;
        public float MissileDamage;
        public GameObject Prefab;

        public override void Attack(Transform origin) {
            Transform target = FindFirstObjectByType<PlayerController>().OrNull()?.transform ?? null;
            for (int i = 0; i < MissileCount; i++) {
                if (target == null) {
                    break;
                }
                GameObject missile = Instantiate(Prefab, origin.position, origin.rotation);
                missile.AddComponent<EntityDamager>().Init(DamageFilter.Player, MissileDamage);
                missile.AddComponent<AutoDestroy>().Duration = MissileDuration;
                missile.AddComponent<HomingProjectileMover>().Init(target, MissileSpeed);
            }
        }
    }
}