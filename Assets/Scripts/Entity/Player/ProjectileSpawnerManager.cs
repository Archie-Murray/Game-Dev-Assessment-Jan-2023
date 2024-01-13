using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawnerManager : MonoBehaviour {
    [SerializeField] private Dictionary<ProjectileSpawnStrategyType, Transform[]> _sockets;
    [SerializeField] private Dictionary<ProjectileSpawnStrategyType, List<ProjectileSpawner>> _projectileSpawners;

    private void Awake() {
        _sockets = new Dictionary<ProjectileSpawnStrategyType, Transform[]>();
        _projectileSpawners = new Dictionary<ProjectileSpawnStrategyType, List<ProjectileSpawner>>();
        foreach (ProjectileSpawnStrategyType type in Enum.GetValues(typeof(ProjectileSpawnStrategyType))) {
            _projectileSpawners.Add(type, new List<ProjectileSpawner>());
        }

        InitSockets<LightSocket>(ProjectileSpawnStrategyType.LIGHT);
        InitSockets<HeavySocket>(ProjectileSpawnStrategyType.HEAVY);
        InitSockets<EliteSocket>(ProjectileSpawnStrategyType.ELITE);
    }

    private void InitSockets<T>(ProjectileSpawnStrategyType strategyType) where T : Component {
        Transform socketRoot = FindAnyObjectByType<T>().OrNull()?.transform ?? null;
        if (socketRoot == null) { 
            Debug.LogWarning("Could not find socket of type: " + typeof(T));
            return;
        }
        List<Transform> sockets = new List<Transform>();
        for (int i = 0; i < socketRoot.childCount; i++) {
            sockets.Add(socketRoot.GetChild(i));
        }
        _sockets.Add(strategyType, sockets.ToArray());
    }

    public void TryAddSpawner(ProjectileSpawnStrategy spawnStrategy, bool overrideCost = false) {
        _sockets.TryGetValue(spawnStrategy.Type, out Transform[] sockets);
        if (sockets == null) {
            Debug.LogWarning("Could not find sockets for type: " + spawnStrategy.Type);
            return;
        }
        foreach (Transform socket in sockets) { 
            if (socket.childCount == 0) {
                GameObject firePoint = new GameObject($"{spawnStrategy.Type}");
                firePoint.transform.parent = socket;
                firePoint.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                _projectileSpawners[spawnStrategy.Type].Add(new ProjectileSpawner(spawnStrategy, firePoint.transform));
                if (!overrideCost) {
                    Globals.Instance.AddMoney(-spawnStrategy.Cost);
                }
                return;
            }
        }
    }

    public bool Fire(ProjectileSpawnStrategyType type) {
        foreach (ProjectileSpawner projectileSpawner in _projectileSpawners[type]) {
            projectileSpawner.Fire();
        }
        return _projectileSpawners[type].Count > 0;
    }
}