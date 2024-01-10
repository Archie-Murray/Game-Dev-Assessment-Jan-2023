using UnityEngine;
namespace Boss {

    public abstract class BossAttack : ScriptableObject {
        public float Cooldown = 1f;
        public SoundEffectType AbilitySoundEffect;
        public abstract void Attack(Transform origin);
    }
}