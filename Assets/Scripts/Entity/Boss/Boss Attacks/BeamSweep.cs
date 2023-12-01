using UnityEngine;

namespace BossAttack {
    public class BeamSweep : BossAttack {
        public GameObject BeamPrefab;
        public float Damage = 10f;
        public float DamageCooldown = 0.5f;
        public float TurnSpeed = 1.5f;
        public float TotalDegrees = 360f;
        public override void Attack(Transform origin) {
            GameObject instance = Instantiate(BeamPrefab, origin.position, origin.rotation);
            instance.GetOrAddComponent<BeamController>().Init(Damage, DamageCooldown, TurnSpeed, TotalDegrees);
        }
    }
}
