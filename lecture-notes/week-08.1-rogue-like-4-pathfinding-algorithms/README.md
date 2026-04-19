# Week 08.1 — Rogue-Like: Pathfinding Algorithms

Pathfinding algorithms are used in games to find a route between two points in a world. They power enemy navigation, NPC movement, and any system that needs to avoid obstacles and reach a goal.

---

## 1. How This Fits

Three scripts work together to make pathfinding work:

| Script           | Where it goes                    | What it does                                    |
| ---------------- | -------------------------------- | ----------------------------------------------- |
| `GridManager`    | An empty GameObject in the scene | Stores which tiles are walkable                 |
| `Pathfinder`     | The enemy GameObject             | Finds a route from A to B through the grid      |
| `EnemyNavigator` | The enemy GameObject             | Moves the enemy along the found path each frame |

One important concept to understand is **coordinate conversion**. Unity positions (like `transform.position`) are `Vector2` values in world units. The grid works in `Vector2Int` tile coordinates — whole numbers like `(3, 5)`. To convert between them, divide the world position by `cellSize` and round to the nearest integer:

```csharp
Vector2Int WorldToGrid(Vector2 worldPos)
{
    return new Vector2Int(
        Mathf.RoundToInt(worldPos.x / cellSize),
        Mathf.RoundToInt(worldPos.y / cellSize)
    );
}
```

You would call `FindPath` like this from `EnemyNavigator`:

```csharp
Vector2Int startTile = WorldToGrid(transform.position);
Vector2Int endTile   = WorldToGrid(PlayerController.Instance.transform.position);
List<Vector2Int> path = GetComponent<Pathfinder>().FindPath(startTile, endTile);
SetPath(path);
```

---

## 2. The Problem

When an enemy needs to move toward the player, it cannot simply walk in a straight line — walls, gaps, and other obstacles block the way. A pathfinding algorithm solves this by searching through available positions in the world to find a valid route.

The world is usually represented as a **grid** or **graph** of nodes. Each node represents a position, and connections between nodes represent valid moves.

---

## 3. Breadth-First Search (BFS)

**Breadth-First Search (BFS)** is the simplest pathfinding approach. It explores all neighbouring nodes level by level until it finds the target.

**Step 1** — Add a `GridManager` script to manage the grid. Each cell stores whether it is walkable.

```csharp
public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float cellSize = 1f;

    private bool[,] walkable;

    void Awake()
    {
        Instance = this;
        walkable = new bool[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                walkable[x, y] = true;
    }

    public bool IsWalkable(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height) return false;
        return walkable[x, y];
    }
}
```

**Step 2** — Implement BFS in a separate `Pathfinder` script:

```csharp
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        queue.Enqueue(start);
        cameFrom[start] = start;

        Vector2Int[] directions = {
            Vector2Int.up, Vector2Int.down,
            Vector2Int.left, Vector2Int.right
        };

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            if (current == end)
                return ReconstructPath(cameFrom, start, end);

            foreach (Vector2Int dir in directions)
            {
                Vector2Int neighbour = current + dir;
                if (!cameFrom.ContainsKey(neighbour) && GridManager.Instance.IsWalkable(neighbour.x, neighbour.y))
                {
                    queue.Enqueue(neighbour);
                    cameFrom[neighbour] = current;
                }
            }
        }

        return null; // No path found
    }

    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int current = end;

        while (current != start)
        {
            path.Add(current);
            current = cameFrom[current];
        }

        path.Reverse();
        return path;
    }
}
```

What is happening in the code above?

| Element             | Purpose                                                                 |
| ------------------- | ----------------------------------------------------------------------- |
| `Queue<Vector2Int>` | Stores nodes to be explored, in the order they were discovered          |
| `cameFrom`          | Records where each node was reached from, so the path can be retraced   |
| `directions`        | The four cardinal moves available from any position                     |
| `ReconstructPath`   | Traces backwards from the end node to the start, then reverses the list |

> **Note:** BFS always finds the _shortest_ path on a uniform grid (where all moves cost the same). However, it explores in all directions equally, which can be slow on large maps.

---

## 4. A\* (A-Star)

**A\*** improves on BFS by using a **heuristic** — an estimate of how far each node is from the goal. This guides the search toward the target rather than expanding in every direction equally.

The key formula is:

```
f(n) = g(n) + h(n)
```

- `g(n)` — the actual cost to reach node `n` from the start
- `h(n)` — the estimated cost from `n` to the goal (the heuristic)
- `f(n)` — the total estimated cost; A\* always expands the node with the lowest `f`

**Step 1** — Add a `NodeData` class to track costs:

```csharp
public class NodeData
{
    public Vector2Int position;
    public float gCost;
    public float hCost;
    public float FCost => gCost + hCost;
    public Vector2Int parent;
}
```

**Step 2** — Implement A\* in the `Pathfinder` script:

```csharp
public List<Vector2Int> FindPathAStar(Vector2Int start, Vector2Int end)
{
    List<NodeData> openList = new List<NodeData>();
    HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();
    Dictionary<Vector2Int, NodeData> nodeMap = new Dictionary<Vector2Int, NodeData>();

    NodeData startNode = new NodeData { position = start, gCost = 0, hCost = Heuristic(start, end), parent = start };
    openList.Add(startNode);
    nodeMap[start] = startNode;

    Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

    while (openList.Count > 0)
    {
        NodeData current = openList[0];
        foreach (NodeData node in openList)
            if (node.FCost < current.FCost) current = node;

        openList.Remove(current);
        closedSet.Add(current.position);

        if (current.position == end)
        {
            return ReconstructAStarPath(nodeMap, start, end);
        }

        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighbourPos = current.position + dir;

            if (closedSet.Contains(neighbourPos)) continue;
            if (!GridManager.Instance.IsWalkable(neighbourPos.x, neighbourPos.y)) continue;

            float newG = current.gCost + 1;

            if (!nodeMap.ContainsKey(neighbourPos) || newG < nodeMap[neighbourPos].gCost)
            {
                NodeData neighbour = new NodeData
                {
                    position = neighbourPos,
                    gCost = newG,
                    hCost = Heuristic(neighbourPos, end),
                    parent = current.position
                };
                openList.Add(neighbour);
                nodeMap[neighbourPos] = neighbour;
            }
        }
    }

    return null;
}

private float Heuristic(Vector2Int a, Vector2Int b)
{
    return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); // Manhattan distance
}
```

What is different between BFS and A\*?

| Aspect                     | BFS                 | A\*                             |
| -------------------------- | ------------------- | ------------------------------- |
| Order of exploration       | By discovery order  | By lowest `f` cost              |
| Uses a heuristic           | No                  | Yes                             |
| Speed on large maps        | Slower              | Much faster                     |
| Always finds shortest path | Yes (uniform grids) | Yes (with admissible heuristic) |

---

## 5. Following the Path

Once a path is found, the enemy should follow it step by step:

```csharp
public class EnemyNavigator : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    private List<Vector2Int> path;
    private int currentStep = 0;

    public void SetPath(List<Vector2Int> newPath)
    {
        path = newPath;
        currentStep = 0;
    }

    void Update()
    {
        if (path == null || currentStep >= path.Count) return;

        Vector2 target = new Vector2(path[currentStep].x, path[currentStep].y);
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target) < 0.05f)
            currentStep++;
    }
}
```

---

## Exercises

---

### Task 1 — Visualise the Path

Draw the found path on screen using `Debug.DrawLine` so you can see where the enemy intends to travel during Play mode.

> **Hint:** Loop through the path list and call `Debug.DrawLine` between each pair of consecutive positions inside `Update()`.

---

### Task 2 — Mark Obstacles

Add the ability to mark cells in the grid as unwalkable by clicking on them in Play mode. Pathfinding should automatically avoid these cells.

> **Hint:** Use `Camera.main.ScreenToWorldPoint` on `Input.mousePosition` to convert the click to a world position, then convert to grid coordinates.

---

### Task 3 — Recalculate on Move

Make the enemy recalculate its path every few seconds so it reacts if the player moves to a new position.

> **Hint:** Use a `Coroutine` with `WaitForSeconds(recalculateInterval)` that calls `FindPath` repeatedly.
