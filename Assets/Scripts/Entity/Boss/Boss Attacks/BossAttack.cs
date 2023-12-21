using UnityEngine;
namespace Boss {

    public abstract class BossAttack : ScriptableObject {
        public float Cooldown = 1f;
        public abstract void Attack(Transform origin);
    }
}