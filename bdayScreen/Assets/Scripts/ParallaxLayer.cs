using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Header("Follow")]
    public Transform followTarget;
    [Range(0f, 1f)] public float followFactor = 0.5f;

    [Header("Tile")]
    public Vector2 tileSize = new Vector2(12f, 6f);
    public Color tileColor = new Color(0.6f, 0.7f, 0.9f, 1f);
    public int sortingOrder = 0;
    public Sprite tileSprite;
    public bool useTiledDrawMode = true;
    public bool autoCreateTile = true;

    private Transform tile;
    private bool hasFollowOffset;
    private float followOffsetX;
    private Vector2 lastSize;
    private Color lastColor;
    private int lastSortingOrder;

    private void Awake()
    {
        EnsureTile();
    }

    private void OnEnable()
    {
        EnsureTile();
    }

    private void Update()
    {
        if (autoCreateTile)
        {
            if (tile == null)
            {
                EnsureTile();
                if (tile == null) return;
            }
        }

        bool shouldRebuild = false;

        if (autoCreateTile && (shouldRebuild || tileSize != lastSize || tileColor != lastColor || sortingOrder != lastSortingOrder))
        {
            RebuildTile();
        }

        if (followTarget == null)
        {
            var cam = Camera.main;
            if (cam != null) followTarget = cam.transform;
        }

        if (followTarget != null)
        {
            if (!hasFollowOffset)
            {
                followOffsetX = transform.position.x - (followTarget.position.x * followFactor);
                hasFollowOffset = true;
            }

            var pos = transform.position;
            pos.x = followTarget.position.x * followFactor + followOffsetX;
            transform.position = pos;
        }
    }

    private void EnsureTile()
    {
        if (!autoCreateTile) return;
        if (tile == null)
        {
            tile = CreateTile("Tile");
        }

        tile.localPosition = Vector3.zero;

        lastSize = tileSize;
        lastColor = tileColor;
        lastSortingOrder = sortingOrder;
    }

    private void RebuildTile()
    {
        if (!autoCreateTile) return;
        if (tile != null) DestroyImmediateSafe(tile.gameObject);

        tile = null;
        EnsureTile();
    }

    private void DestroyImmediateSafe(GameObject go)
    {
        if (Application.isPlaying)
        {
            Destroy(go);
        }
        else
        {
            DestroyImmediate(go);
        }
    }

    private Transform CreateTile(string name)
    {
        var go = new GameObject(name);
        go.transform.SetParent(transform, false);

        var renderer = go.AddComponent<SpriteRenderer>();
        renderer.sortingOrder = sortingOrder;

        if (tileSprite != null)
        {
            renderer.sprite = tileSprite;
            if (useTiledDrawMode)
            {
                renderer.drawMode = SpriteDrawMode.Tiled;
                renderer.size = tileSize;
            }
        }

        return go.transform;
    }
}
