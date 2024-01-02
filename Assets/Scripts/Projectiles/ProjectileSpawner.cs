using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ProjectileSpawner {

    [SerializeField] private ProjectileSpawnStrategy _spawnStrategy;
    [SerializeField] private Transform _firePoint;

    public ProjectileSpawnStrategy Strategy { get => _spawnStrategy; }

    public ProjectileSpawner(ProjectileSpawnStrategy spawnStrategy, Transform firePoint) {
        _spawnStrategy = spawnStrategy;
        _firePoint = firePoint;
    }

    public void Fire() {
        _spawnStrategy.Fire(_firePoint);
    }
}