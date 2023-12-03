using UnityEngine;

namespace Boss {
    [CreateAssetMenu(menuName = "Boss Attack/Beam Sweep")]
    public class BeamSweep : BossAttack {
        public GameObject BeamPrefab;
        public float Damage = 10f;
        public float DamageCooldown = 0.5f;
        public float TurnDirection = 1f;
        public float TotalDegrees = 360f;
        public override void Attack(Transform origin) {
            GameObject instance = Instantiate(BeamPrefab, origin.position, origin.rotation);
            instance.GetOrAddComponent<BeamController>().Init(Damage, DamageCooldown, TurnDirection, TotalDegrees, Duration);
        }
    }
}
