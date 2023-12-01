using UnityEngine;
namespace BossAttack {

    public abstract class BossAttack : ScriptableObject {
        public abstract void Attack(Transform origin);
    }
}