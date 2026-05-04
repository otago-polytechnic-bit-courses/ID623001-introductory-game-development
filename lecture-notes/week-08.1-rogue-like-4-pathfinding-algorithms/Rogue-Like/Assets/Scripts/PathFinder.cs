using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    private static readonly Vector2Int[] Directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
    {
        List<NodeData> openList = new List<NodeData>();
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();
        Dictionary<Vector2Int, NodeData> nodeMap = new Dictionary<Vector2Int, NodeData>();

        NodeData startNode = new NodeData { position = start, gCost = 0, hCost = Heuristic(start, end), parent = start };
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

                openList.Add(new NodeData { position = neighbour, gCost = newG, hCost = Heuristic(neighbour, end), parent = current.position });
                nodeMap[neighbour] = openList[^1];
            }
        }

        return null;
    }

    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, NodeData> nodeMap, Vector2Int start, Vector2Int end)
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