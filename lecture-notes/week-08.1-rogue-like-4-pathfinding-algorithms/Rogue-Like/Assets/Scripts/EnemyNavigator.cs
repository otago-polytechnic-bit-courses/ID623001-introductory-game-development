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
        while (PlayerController.Instance == null || GridManager.Instance == null)
            yield return new WaitForSeconds(0.1f);

        while (true)
        {
            Vector2Int startTile = GridManager.Instance.WorldToGrid(rb.position);
            Vector2Int endTile = GridManager.Instance.WorldToGrid(PlayerController.Instance.rb.position);

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