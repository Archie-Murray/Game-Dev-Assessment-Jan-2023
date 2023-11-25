using System;
using UnityEngine;

[Serializable] public enum ProjectileSpawnStrategyType { LIGHT, HEAVY, ELITE }
public abstract class ProjectileSpawnStrategy : ScriptableObject {
    public ProjectileSpawnStrategyType Type;
    public int Cost;
    public float Duration;
    public float Speed;
    public float Damage;

    public abstract void Fire(Transform origin);

    public static string Display(Type type) {
        return type.Name.Replace("SpawnStrategy", "");
    }
}
