using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] private int width = 2000;
    [SerializeField] private int height = 2000;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private LayerMask wallLayerMask;

    private bool[,] walkable;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        walkable = new bool[width, height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                walkable[x, y] = true;
    }

    public void BakeWalls()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 worldPos = GridToWorld(new Vector2Int(x, y));
                if (Physics2D.OverlapCircle(worldPos, cellSize * 0.4f, wallLayerMask))
                    walkable[x, y] = false;
            }
        }
    }

    public bool IsWalkable(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height) return false;
        return walkable[x, y];
    }

    public Vector2Int WorldToGrid(Vector2 worldPos)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPos.x / cellSize) + width  / 2,
            Mathf.RoundToInt(worldPos.y / cellSize) + height / 2
        );
    }

    public Vector2 GridToWorld(Vector2Int gridPos)
    {
        return new Vector2(
            (gridPos.x - width  / 2) * cellSize,
            (gridPos.y - height / 2) * cellSize
        );
    }
}