using UnityEngine;

public class SawBladeProjectile : MonoBehaviour {
    [SerializeField] private float _rotationSpeed;

    public void Init(float damage, float tickRate, float rotationSpeed) {
        _rotationSpeed = rotationSpeed;
        SawBladeController[] sawBladeControllers = GetComponentsInChildren<SawBladeController>();
        for (int i = 0; i < Mathf.Min(2, sawBladeControllers.Length); i++) {
            sawBladeControllers[i].Init(damage, tickRate, i % 2 == 0 ? rotationSpeed * 0.5f : -rotationSpeed * 0.5f);
        }
    }

    private void FixedUpdate() {
        transform.rotation = Quaternion.AngleAxis(transform.rotation.eulerAngles.z + _rotationSpeed * Mathf.PI * 2f * Time.fixedDeltaTime, Vector3.forward);
    }
}