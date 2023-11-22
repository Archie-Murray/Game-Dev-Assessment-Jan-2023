using System.Linq;

using UnityEngine;

public class EnemyManager : MonoBehaviour {
    [SerializeField] private Transform[] _wanderPoints;
    [SerializeField] private Transform _target;

    private void Awake() {
        _target = FindFirstObjectByType<PlayerController>().transform;
        _wanderPoints ??= FindObjectsOfType<WanderPoint>().ToList<WanderPoint>().ConvertAll(converter: (WanderPoint point) => point.transform).ToArray();
    }
}