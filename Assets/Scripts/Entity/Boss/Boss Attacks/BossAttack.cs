using UnityEngine;
namespace Boss {

    public abstract class BossAttack : ScriptableObject {
        public float Duration;
        public abstract void Attack(Transform origin);
    }
}