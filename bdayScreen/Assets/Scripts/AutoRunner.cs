using UnityEngine;

public class AutoRunner : MonoBehaviour
{
    public GameTuning tuning;
    [Min(0f)] public float speed = 2.5f;
    public float minX = -9f;
    public float maxX = 9f;

    private void Update()
    {
        if (tuning != null)
        {
            speed = tuning.runnerSpeed;
            minX = tuning.runnerMinX;
            maxX = tuning.runnerMaxX;
        }

        var pos = transform.position;
        pos.x += speed * Time.deltaTime;

        if (pos.x > maxX)
        {
            pos.x = minX;
        }

        transform.position = pos;
    }
}
