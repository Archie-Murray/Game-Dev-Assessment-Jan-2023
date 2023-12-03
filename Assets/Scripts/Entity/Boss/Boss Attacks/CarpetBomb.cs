using System.Collections.Generic;

using ProjectileComponents;

using UnityEngine;

namespace Boss {
    [CreateAssetMenu(menuName = "Boss Attack/Carpet Bomb")]
    public class CarpetBomb : BossAttack {
        public int Points = 3;
        public float Damage = 5f;
        public float Speed = 5f;
        public float Height = 1f;
        public GameObject MissilePrefab;
        public float MaxDistance = 2f;
        public override void Attack(Transform origin) {
            Transform playerTransform = FindFirstObjectByType<PlayerController>().transform;
            for (int i = 0; i < Points; i++) {
                Vector3 targetPos = playerTransform.position + (Vector3) (Random.insideUnitCircle * MaxDistance);
                GameObject projectile = Instantiate(MissilePrefab, origin.position, origin.rotation);
                projectile.GetOrAddComponent<ArcProjectileController>().Init(Speed, Damage, Height, targetPos);
                projectile.GetOrAddComponent<EnemyProjectile>();
            }
        }

    }

}