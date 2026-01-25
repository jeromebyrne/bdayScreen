using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public GameTuning tuning;
    public Transform target;
    public Vector3 offset = new Vector3(0f, 0f, -10f);
    [Min(0f)] public float smoothTime = 0f;

    private Vector3 velocity;

    private void LateUpdate()
    {
        if (tuning != null)
        {
            offset = tuning.cameraOffset;
            smoothTime = tuning.cameraSmoothTime;
        }

        if (target == null)
        {
            var found = GameObject.Find("Runner");
            if (found != null)
            {
                target = found.transform;
            }
            else
            {
                return;
            }
        }

        Vector3 desired = new Vector3(target.position.x, target.position.y, 0f) + offset;

        if (smoothTime <= 0f)
        {
            transform.position = desired;
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);
        }
    }
}
