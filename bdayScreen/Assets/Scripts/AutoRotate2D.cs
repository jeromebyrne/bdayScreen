using UnityEngine;

public class AutoRotate2D : MonoBehaviour
{
    [Tooltip("Degrees per second.")]
    public float degreesPerSecond = 45f;
    public bool useUnscaledTime = false;

    private void Update()
    {
        float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        transform.Rotate(0f, 0f, degreesPerSecond * dt);
    }
}
