# Week 08.1 — Rogue-Like: Pathfinding Algorithms

Pathfinding algorithms are used in games to find a route between two points in a world. They power enemy navigation, NPC movement, and any system that needs to avoid obstacles and reach a goal.

---

## 1. How This Fits

Three scripts work together to make pathfinding work:

| Script           | Where it goes                    | What it does                                    |
| ---------------- | -------------------------------- | ----------------------------------------------- |
| `GridManager`    | An empty GameObject in the scene | Stores which tiles are walkable                 |
| `PathFinder`     | The enemy GameObject             | Finds a route from A to B through the grid      |
| `EnemyNavigator` | The enemy GameObject             | Moves the enemy along the found path each frame |

`EnemyController` sits alongside these and handles health, attacking, and death independently of navigation.

---

## 2. Coordinate Conversion

Unity uses `Vector2` world positions; the grid uses `Vector2Int` tile coordinates. `GridManager` converts between them by dividing by `cellSize` and rounding. The `width / 2` and `height / 2` offsets centre the grid on world origin `(0, 0)`:

```csharp
Vector2Int WorldToGrid(Vector2 worldPos)
{
    return new Vector2Int(
        Mathf.RoundToInt(worldPos.x / cellSize) + width  / 2,
        Mathf.RoundToInt(worldPos.y / cellSize) + height / 2
    );
}

Vector2 GridToWorld(Vector2Int gridPos)
{
    return new Vector2(
        (gridPos.x - width  / 2) * cellSize,
        (gridPos.y - height / 2) * cellSize
    );
}
```

`EnemyNavigator` calls these methods each recalculation interval:

```csharp
Vector2Int startTile = GridManager.Instance.WorldToGrid(rb.position);
Vector2Int endTile   = GridManager.Instance.WorldToGrid(PlayerController.Instance.rb.position);
List<Vector2Int> path = pathfinder.FindPath(startTile, endTile);
```

---

## 3. The Problem

When an enemy needs to move toward the player it cannot simply walk in a straight line — walls, gaps, and other obstacles block the way. A pathfinding algorithm solves this by searching through available positions in the world to find a valid route. The world is represented as a **grid** of nodes, where each node stores whether it is walkable.

---

## 4. GridManager Script

`GridManager` allocates a `bool[,]` walkability map at startup. After all rooms are spawned, `LevelGenerator.Start()` calls `BakeWalls()`, which uses `Physics2D.OverlapCircle` to mark any cell overlapping a wall collider as unwalkable.

```csharp
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
```

> `BakeWalls` is called once at the end of `LevelGenerator.Start()`, after all room prefabs are instantiated. Calling it before rooms exist will produce an incorrect wall map.

---

## 5. Breadth-First Search (BFS)

**Breadth-First Search** explores all neighbouring nodes level by level until it finds the target. It always finds the shortest path on a uniform grid where all moves cost the same.

| Element             | Purpose                                                                  |
| ------------------- | ------------------------------------------------------------------------ |
| `Queue<Vector2Int>` | Stores nodes to explore in discovery order                               |
| `cameFrom`          | Records where each node was reached from so the path can be retraced     |
| `directions`        | The four cardinal moves available from any position                      |
| `ReconstructPath`   | Traces backwards from the end node to the start, then reverses the list  |

> BFS explores in all directions equally, which can be slow on large maps.

---

## 6. A* (A-Star)

**A\*** improves on BFS by using a **heuristic** — an estimate of how far each node is from the goal. This guides the search toward the target rather than expanding in every direction equally.

The key formula is:

```
f(n) = g(n) + h(n)
```

| Term   | Meaning                                                  |
| ------ | -------------------------------------------------------- |
| `g(n)` | Actual cost to reach node `n` from the start             |
| `h(n)` | Estimated cost from `n` to the goal (Manhattan distance) |
| `f(n)` | Total estimated cost — A* always expands the lowest `f`  |

| Aspect                     | BFS                 | A\*                             |
| -------------------------- | ------------------- | ------------------------------- |
| Order of exploration       | By discovery order  | By lowest `f` cost              |
| Uses a heuristic           | No                  | Yes                             |
| Speed on large maps        | Slower              | Much faster                     |
| Always finds shortest path | Yes (uniform grids) | Yes (with admissible heuristic) |

---

## 7. PathFinder Script

`PathFinder` implements A* and is attached to the enemy alongside `EnemyNavigator`. `EnemyNavigator` calls `FindPath` each recalculation interval and receives a `List<Vector2Int>` of tile coordinates to follow.

```csharp
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    private static readonly Vector2Int[] Directions =
        { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
    {
        List<NodeData> openList = new List<NodeData>();
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();
        Dictionary<Vector2Int, NodeData> nodeMap = new Dictionary<Vector2Int, NodeData>();

        NodeData startNode = new NodeData
        {
            position = start,
            gCost = 0,
            hCost = Heuristic(start, end),
            parent = start
        };
        openList.Add(startNode);
        nodeMap[start] = startNode;

        while (openList.Count > 0)
        {
            NodeData current = openList[0];
            foreach (NodeData node in openList)
                if (node.FCost < current.FCost) current = node;

            openList.Remove(current);
            closedSet.Add(current.position);

            if (current.position == end)
                return ReconstructPath(nodeMap, start, end);

            foreach (Vector2Int dir in Directions)
            {
                Vector2Int neighbour = current.position + dir;

                if (closedSet.Contains(neighbour)) continue;
                if (!GridManager.Instance.IsWalkable(neighbour.x, neighbour.y)) continue;

                float newG = current.gCost + 1;
                if (nodeMap.ContainsKey(neighbour) && newG >= nodeMap[neighbour].gCost) continue;

                openList.Add(new NodeData
                {
                    position = neighbour,
                    gCost = newG,
                    hCost = Heuristic(neighbour, end),
                    parent = current.position
                });
                nodeMap[neighbour] = openList[^1];
            }
        }

        return null;
    }

    private List<Vector2Int> ReconstructPath(
        Dictionary<Vector2Int, NodeData> nodeMap, Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int current = end;

        while (current != start)
        {
            path.Add(current);
            current = nodeMap[current].parent;
        }

        path.Reverse();
        return path;
    }

    private float Heuristic(Vector2Int a, Vector2Int b) =>
        Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
}

public class NodeData
{
    public Vector2Int position;
    public float gCost;
    public float hCost;
    public float FCost => gCost + hCost;
    public Vector2Int parent;
}
```

---

## 8. EnemyNavigator Script

`EnemyNavigator` periodically recalculates the path to the player via a `Coroutine` and steps the enemy along it each `FixedUpdate`. It waits until both `PlayerController.Instance` and `GridManager.Instance` are ready before beginning.

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNavigator : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float recalculateInterval = 0.5f;
    [SerializeField] private float waypointReachedDistance = 0.1f;

    private List<Vector2Int> path = new List<Vector2Int>();
    private int currentStep = 0;
    private PathFinder pathfinder;
    private Rigidbody2D rb;

    void Awake()
    {
        pathfinder = GetComponent<PathFinder>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        StartCoroutine(RecalculatePath());
    }

    private IEnumerator RecalculatePath()
    {
        // Wait until both singletons are available.
        while (PlayerController.Instance == null || GridManager.Instance == null)
            yield return new WaitForSeconds(0.1f);

        while (true)
        {
            Vector2Int startTile = GridManager.Instance.WorldToGrid(rb.position);
            Vector2Int endTile   = GridManager.Instance.WorldToGrid(PlayerController.Instance.rb.position);

            if (GridManager.Instance.IsWalkable(startTile.x, startTile.y) &&
                GridManager.Instance.IsWalkable(endTile.x, endTile.y))
            {
                List<Vector2Int> newPath = pathfinder.FindPath(startTile, endTile);

                if (newPath != null && newPath.Count > 0)
                {
                    path = newPath;
                    currentStep = 0;
                }
            }

            yield return new WaitForSeconds(recalculateInterval);
        }
    }

    void FixedUpdate()
    {
        if (path == null || path.Count == 0 || currentStep >= path.Count) return;

        Vector2 target = GridManager.Instance.GridToWorld(path[currentStep]);
        rb.MovePosition(Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime));

        if (Vector2.Distance(rb.position, target) < waypointReachedDistance)
            currentStep++;
    }

    void Update()
    {
        if (path == null || path.Count == 0) return;

        for (int i = currentStep; i < path.Count - 1; i++)
        {
            Debug.DrawLine(
                GridManager.Instance.GridToWorld(path[i]),
                GridManager.Instance.GridToWorld(path[i + 1]),
                Color.red
            );
        }
    }
}
```

---

## 9. EnemyController Script

`EnemyController` handles health, attacking, and death. It uses `sqrMagnitude` instead of `Vector2.Distance` for the range check to avoid an unnecessary square root each frame. The sprite flips to always face the player.

```csharp
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 1f;

    private float attackTimer;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (PlayerController.Instance == null) return;

        attackTimer -= Time.deltaTime;

        Vector2 toPlayer = (Vector2)(PlayerController.Instance.transform.position - transform.position);
        spriteRenderer.flipX = toPlayer.x < 0f;

        if (toPlayer.sqrMagnitude <= attackRange * attackRange && attackTimer <= 0f)
        {
            Attack();
            attackTimer = attackCooldown;
        }
    }

    private void Attack()
    {
        PlayerController.Instance.TakeDamage(damage);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
```

---

## 10. Enemy GameObject Setup

Attach all four scripts to the enemy GameObject and configure the Rigidbody2D as **Kinematic** so the enemy moves only via `MovePosition` and cannot be pushed by physics impulses from other colliders.

| Component         | Required fields                        |
| ----------------- | -------------------------------------- |
| `Rigidbody2D`     | Body Type: **Kinematic**               |
| `EnemyNavigator`  | Speed, Recalculate Interval            |
| `PathFinder`      | *(no Inspector fields)*                |
| `EnemyController` | Health, Damage, Attack Range, Cooldown |

---

## Exercises

### Task 1 — Visualise the Path

`EnemyNavigator.Update()` already draws the active path with `Debug.DrawLine`. Enter Play mode and open the Scene view to confirm the red path lines appear and update as the player moves.

---

### Task 2 — Mark Obstacles

Add a method to `GridManager` that sets a cell to unwalkable at runtime. Call it from a click handler that converts `Input.mousePosition` → `Camera.main.ScreenToWorldPoint` → `WorldToGrid`. Pathfinding should automatically route around marked cells.

> **Hint:** After marking a cell, the next `RecalculatePath` interval will pick up the change automatically.

---

### Task 3 — Recalculate on Move

The `RecalculatePath` coroutine in `EnemyNavigator` already handles this. Tune `recalculateInterval` in the Inspector to balance responsiveness against performance — lower values react faster but call `FindPath` more often.