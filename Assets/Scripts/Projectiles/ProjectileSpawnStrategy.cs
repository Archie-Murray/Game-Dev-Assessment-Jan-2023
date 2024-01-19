using System;
using UnityEngine;

[Serializable] public enum ProjectileSpawnStrategyType { LIGHT, HEAVY, ELITE }
public abstract class ProjectileSpawnStrategy : ScriptableObject {
    public ProjectileSpawnStrategyType Type;
    public int Cost;
    public float Duration;
    public float Speed;
    public float Damage;
    public GameObject Projectile;

    public abstract void Fire(Transform origin);

    public string Display() {
        return $"{Cost}: {GetType().Name.Replace("SpawnStrategy", "")}";
    }
}
