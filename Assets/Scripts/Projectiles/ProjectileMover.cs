using UnityEngine;

public class ProjectileMover : MonoBehaviour {

    public float Speed = 0f;

    private void FixedUpdate() {
        transform.position += Speed * Time.fixedDeltaTime * transform.up;
    }
}