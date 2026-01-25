using UnityEngine;

public class ParallaxRepeater : MonoBehaviour
{
    [Min(0f)] public float spacing = 0f;
    public bool useSpriteWidth = true;

    private static bool isCreatingClone;
    private bool isClone;

    private Transform cloneA;
    private Transform cloneB;

    private void Awake()
    {
        if (isCreatingClone)
        {
            isClone = true;
        }

        if (isClone)
        {
            DestroyComponentSafe(this);
        }
    }

    private void OnEnable()
    {
        if (isClone) return;
        EnsureClones();
    }

    private void Reset()
    {
        if (isClone) return;
        EnsureClones();
    }

    private void EnsureClones()
    {
        if (cloneA == null)
        {
            cloneA = CreateClone("Clone_A", -1f);
        }
        if (cloneB == null)
        {
            cloneB = CreateClone("Clone_B", 1f);
        }

        float width = GetWidth();
        cloneA.localPosition = new Vector3(width + spacing, 0f, 0f);
        cloneB.localPosition = new Vector3((width + spacing) * 2f, 0f, 0f);
    }

    private Transform CreateClone(string name, float xScale)
    {
        isCreatingClone = true;
        var go = Instantiate(gameObject, transform.parent);
        isCreatingClone = false;
        go.name = name;

        var t = go.transform;
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
        t.localScale = new Vector3(xScale, 1f, 1f);
        return t;
    }

    private float GetWidth()
    {
        if (!useSpriteWidth) return 1f;

        var renderer = GetComponent<SpriteRenderer>();
        if (renderer != null && renderer.sprite != null)
        {
            return renderer.bounds.size.x;
        }

        return 1f;
    }

    private void DestroyComponentSafe(Component component)
    {
        if (component == null) return;
        if (Application.isPlaying)
        {
            Destroy(component);
        }
        else
        {
            DestroyImmediate(component);
        }
    }
}
