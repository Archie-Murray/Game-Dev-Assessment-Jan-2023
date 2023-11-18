using UnityEngine;
using UnityEngine.Events;

public class TickSystem : MonoBehaviour {
    [SerializeField] private float _tickDelay = 0.1f;
    [SerializeField] private float _currentTime = 0f;
    public UnityAction<float> TickLoop;

    private void FixedUpdate() {
        _currentTime += Time.fixedDeltaTime;
        if (_currentTime >= _tickDelay) {
            TickLoop?.Invoke(_currentTime);
            _currentTime = 0f;
        }
    }
}