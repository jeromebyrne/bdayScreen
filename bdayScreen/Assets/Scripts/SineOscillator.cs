using UnityEngine;

public class SineOscillator : MonoBehaviour
{
    public enum Axis
    {
        X,
        Y
    }

    [Header("Motion")]
    public Axis axis = Axis.Y;
    public float amplitude = 0.5f;
    public float frequency = 1f;
    public float phaseOffset = 0f;
    public bool useUnscaledTime = false;

    private float lastOffset;

    private void Awake()
    {
        lastOffset = 0f;
    }

    private void OnEnable()
    {
        lastOffset = 0f;
    }

    private void Update()
    {
        float time = useUnscaledTime ? Time.unscaledTime : Time.time;
        float offset = Mathf.Sin((time * frequency + phaseOffset) * Mathf.PI * 2f) * amplitude;

        float delta = offset - lastOffset;
        lastOffset = offset;

        var pos = transform.localPosition;
        if (axis == Axis.X)
        {
            pos.x += delta;
        }
        else
        {
            pos.y += delta;
        }

        transform.localPosition = pos;
    }
}
