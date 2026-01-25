using System.Collections.Generic;
using UnityEngine;

public class ParallaxTiler : MonoBehaviour
{
    [Min(0f)] public float spacing = 0f;
    public bool useSpriteWidth = true;
    public bool alternateFlipX = false;
    public Camera targetCamera;

    private static bool isCreatingClone;
    private bool isClone;

    private readonly List<Transform> tiles = new List<Transform>();
    private float tileWidth = 1f;

    private void Awake()
    {
        if (isCreatingClone)
        {
            isClone = true;
        }

        if (isClone)
        {
            DestroyComponentSafe(this);
            return;
        }

        EnsureCamera();
        BuildTiles();
    }

    private void OnEnable()
    {
        if (isClone) return;
        EnsureCamera();
        if (tiles.Count == 0) BuildTiles();
    }

    private void Update()
    {
        if (tiles.Count == 0) return;
        EnsureCamera();
        if (targetCamera == null) return;

        float camHalfWidth = GetCameraHalfWidth();
        float leftBound = targetCamera.transform.position.x - camHalfWidth - tileWidth;

        Transform leftmost = tiles[0];
        Transform rightmost = tiles[0];
        for (int i = 1; i < tiles.Count; i++)
        {
            if (tiles[i].position.x < leftmost.position.x) leftmost = tiles[i];
            if (tiles[i].position.x > rightmost.position.x) rightmost = tiles[i];
        }

        if (leftmost.position.x + (tileWidth * 0.5f) < leftBound)
        {
            float newX = rightmost.position.x + tileWidth + spacing;
            var pos = leftmost.position;
            pos.x = newX;
            leftmost.position = pos;
        }
    }

    private void BuildTiles()
    {
        tiles.Clear();
        tiles.Add(transform);

        tileWidth = GetTileWidth();

        for (int i = 1; i <= 2; i++)
        {
            isCreatingClone = true;
            var clone = Instantiate(gameObject, transform.parent);
            isCreatingClone = false;
            clone.name = $"{name}_Tile_{i}";

            var t = clone.transform;
            var pos = transform.position;
            pos.x += (tileWidth + spacing) * i;
            t.position = pos;

            if (alternateFlipX)
            {
                float xScale = (i % 2 == 1) ? -1f : 1f;
                t.localScale = new Vector3(xScale, t.localScale.y, t.localScale.z);
            }

            tiles.Add(t);
        }
    }

    private float GetTileWidth()
    {
        if (!useSpriteWidth) return 1f;

        var renderer = GetComponent<SpriteRenderer>();
        if (renderer != null && renderer.sprite != null)
        {
            return renderer.bounds.size.x;
        }

        return 1f;
    }

    private void EnsureCamera()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    private float GetCameraHalfWidth()
    {
        if (targetCamera == null) return 5f;
        if (targetCamera.orthographic)
        {
            return targetCamera.orthographicSize * targetCamera.aspect;
        }

        float distance = Mathf.Abs(targetCamera.transform.position.z - transform.position.z);
        return Mathf.Tan(targetCamera.fieldOfView * 0.5f * Mathf.Deg2Rad) * distance;
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
