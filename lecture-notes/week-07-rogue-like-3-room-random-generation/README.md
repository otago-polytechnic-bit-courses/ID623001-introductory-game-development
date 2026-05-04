# Week 07 — Rogue-Like: Room Random Generation

This week introduces procedural level generation. The `LevelGenerator` script places room markers at random, resolves overlaps, then spawns the correct wall-prefab variant for each room based on its neighbours. `CameraController` and `Room` are also introduced here.

---

## 1. Scene Setup

Remove the `SampleScene`. Create a new scene called `GenerationTest`.

In the **Assets > Art > Map** folder, drag `Map_Rooms_15` into the Hierarchy. Rename it `RoomLayout`, drag it into the **Prefabs > Rooms** folder to make it a prefab, then delete the instance from the Hierarchy.

In the Hierarchy, create an empty GameObject called `LevelGenerator`. Add a child empty GameObject called `GenerationPoint`. The `LevelGenerator` manages room generation; `GenerationPoint` tracks where the next room will be placed.

---

## 2. CameraController Script

The camera follows a target transform, smoothly moving toward it each physics tick. It is a singleton so any script can call `CameraController.Instance.ChangeTarget(...)`.

```csharp
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    [SerializeField] private float speed = 30f;
    [SerializeField] private Transform target;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(target.position.x, target.position.y, transform.position.z),
                speed * Time.fixedDeltaTime
            );
        }
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
```

Attach `CameraController` to the **Main Camera** GameObject.

---

## 3. Room Script

When the player enters a room's trigger collider, the camera should re-target the **player** — not the room — so it continues to follow the player as they move through the room.

```csharp
using UnityEngine;

public class Room : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && PlayerController.Instance != null)
        {
            CameraController.Instance.ChangeTarget(PlayerController.Instance.transform);
        }
    }
}
```

Add `Room` to each room prefab. The trigger collider on the prefab should be large enough to cover the room area so the camera retargets as soon as the player enters.

---

## 4. LevelGenerator Script

`LevelGenerator` works in three stages inside `Start()`:

1. **Place markers** — temporary GameObjects with small circle colliders are placed at every room position, including the start and end rooms.
2. **Resolve overlaps** — before moving to the next position the generator checks with `Physics2D.OverlapCircle`. If the target position is occupied it picks a new random direction and tries again.
3. **Spawn outlines** — once all markers are in place, `CreateRoomOutline` checks each cardinal neighbour and selects the matching wall prefab. Markers are destroyed afterwards.

**Key data structures:**

| Element                                 | Purpose                                                                                |
| --------------------------------------- | -------------------------------------------------------------------------------------- |
| `roomPositions`                         | World positions of all mid-path rooms                                                  |
| `startRoomPosition` / `endRoomPosition` | Tracked separately from mid rooms                                                      |
| `markers`                               | Temporary GameObjects used for overlap detection; destroyed after outlines are created |
| `roomParent`                            | A parent `Transform` created at runtime to keep the Hierarchy tidy                     |

```csharp
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomPrefabs
{
    public GameObject roomDown, roomLeft, roomLeftDown, roomLeftRight,
        roomLeftRightDown, roomRight, roomRightDown, roomUp, roomUpDown,
        roomUpLeft, roomUpLeftDown, roomUpLeftRight, roomUpLeftRightDown,
        roomUpRight, roomUpRightDown;
}

public class LevelGenerator : MonoBehaviour
{
    private enum Direction { Up, Down, Left, Right }

    [SerializeField] private RoomPrefabs roomPrefabs;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private int distanceToEnd;
    [SerializeField] private Transform generationPoint;
    [SerializeField] private Direction direction;
    [SerializeField] private float xOffset = 18f;
    [SerializeField] private float yOffset = 10f;
    [SerializeField] private LayerMask roomLayerMask;

    private Vector3 startRoomPosition;
    private Vector3 endRoomPosition;
    private List<Vector3> roomPositions = new List<Vector3>();
    private List<GameObject> markers = new List<GameObject>();
    private Transform roomParent;

    void Start()
    {
        roomParent = new GameObject("Rooms").transform;

        // Place the start room marker at the generation point's initial position.
        startRoomPosition = generationPoint.position;
        PlaceMarker(startRoomPosition);

        direction = (Direction)Random.Range(0, 4);
        MoveGenerationPoint();

        for (int i = 0; i < distanceToEnd; i++)
        {
            Vector3 pos = generationPoint.position;
            PlaceMarker(pos);

            // The last iteration's position becomes the end room.
            if (i == distanceToEnd - 1)
                endRoomPosition = pos;
            else
                roomPositions.Add(pos);

            // Pick a direction and move, retrying if the target spot is occupied.
            Vector3 lastValidPosition = generationPoint.position;
            direction = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();

            int safetyLimit = 100;
            while (Physics2D.OverlapCircle(generationPoint.position, 0.2f, roomLayerMask))
            {
                generationPoint.position = lastValidPosition;
                direction = (Direction)Random.Range(0, 4);
                MoveGenerationPoint();

                if (--safetyLimit <= 0)
                {
                    Debug.LogWarning("LevelGenerator: could not find a free position.");
                    break;
                }
            }
        }

        // Spawn room outlines now that all markers are in position.
        CreateRoomOutline(startRoomPosition);
        foreach (Vector3 pos in roomPositions)
            CreateRoomOutline(pos);
        CreateRoomOutline(endRoomPosition);

        // Spawn the player at the start room and hand the camera its transform.
        if (playerPrefab != null)
        {
            GameObject player = Instantiate(playerPrefab, startRoomPosition, Quaternion.identity);
            CameraController.Instance.ChangeTarget(player.transform);
        }

        // Markers have served their purpose; remove them before baking the grid.
        foreach (GameObject marker in markers)
            Destroy(marker);

        GridManager.Instance.BakeWalls();
    }

    private void PlaceMarker(Vector3 position)
    {
        GameObject marker = new GameObject("RoomMarker");
        marker.transform.position = position;
        marker.layer = GetLayerFromMask(roomLayerMask);

        CircleCollider2D col = marker.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = 0.1f;

        markers.Add(marker);
    }

    private int GetLayerFromMask(LayerMask mask)
    {
        int value = mask.value;
        int layer = 0;
        while (value > 1) { value >>= 1; layer++; }
        return layer;
    }

    private void MoveGenerationPoint()
    {
        switch (direction)
        {
            case Direction.Up:    generationPoint.position += new Vector3(0,  yOffset, 0); break;
            case Direction.Down:  generationPoint.position += new Vector3(0, -yOffset, 0); break;
            case Direction.Left:  generationPoint.position += new Vector3(-xOffset, 0, 0); break;
            case Direction.Right: generationPoint.position += new Vector3( xOffset, 0, 0); break;
        }
    }

    public void CreateRoomOutline(Vector3 roomPosition)
    {
        bool isRoomAbove = Physics2D.OverlapCircle(roomPosition + new Vector3(0,  yOffset, 0), 0.2f, roomLayerMask);
        bool isRoomBelow = Physics2D.OverlapCircle(roomPosition + new Vector3(0, -yOffset, 0), 0.2f, roomLayerMask);
        bool isRoomLeft  = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0, 0), 0.2f, roomLayerMask);
        bool isRoomRight = Physics2D.OverlapCircle(roomPosition + new Vector3( xOffset, 0, 0), 0.2f, roomLayerMask);

        GameObject prefabToSpawn = GetRoomPrefab(isRoomAbove, isRoomBelow, isRoomLeft, isRoomRight);

        if (prefabToSpawn != null)
            Instantiate(prefabToSpawn, roomPosition, Quaternion.identity, roomParent);
        else
            Debug.LogWarning($"No prefab found at {roomPosition} — above:{isRoomAbove} below:{isRoomBelow} left:{isRoomLeft} right:{isRoomRight}");
    }

    private GameObject GetRoomPrefab(bool up, bool down, bool left, bool right)
    {
        // Single exit
        if ( up && !down && !left && !right) return roomPrefabs.roomUp;
        if (!up &&  down && !left && !right) return roomPrefabs.roomDown;
        if (!up && !down &&  left && !right) return roomPrefabs.roomLeft;
        if (!up && !down && !left &&  right) return roomPrefabs.roomRight;

        // Two exits
        if ( up &&  down && !left && !right) return roomPrefabs.roomUpDown;
        if ( up && !down &&  left && !right) return roomPrefabs.roomUpLeft;
        if ( up && !down && !left &&  right) return roomPrefabs.roomUpRight;
        if (!up &&  down &&  left && !right) return roomPrefabs.roomLeftDown;
        if (!up &&  down && !left &&  right) return roomPrefabs.roomRightDown;
        if (!up && !down &&  left &&  right) return roomPrefabs.roomLeftRight;

        // Three exits
        if ( up &&  down &&  left && !right) return roomPrefabs.roomUpLeftDown;
        if ( up &&  down && !left &&  right) return roomPrefabs.roomUpRightDown;
        if ( up && !down &&  left &&  right) return roomPrefabs.roomUpLeftRight;
        if (!up &&  down &&  left &&  right) return roomPrefabs.roomLeftRightDown;

        // Four exits
        if ( up &&  down &&  left &&  right) return roomPrefabs.roomUpLeftRightDown;

        return null;
    }
}
```

---

## 5. Room Outline Prefabs

Create the following 15 room prefabs, each using its corresponding map sprite. Use the `BasicRoom` prefab as a base, swap in the correct `Maps_Rooms_*` sprite, and save each to **Assets > Prefabs > Rooms**:

`RoomDown`, `RoomLeft`, `RoomLeftDown`, `RoomLeftRight`, `RoomLeftRightDown`, `RoomRight`, `RoomRightDown`, `RoomUp`, `RoomUpDown`, `RoomUpLeft`, `RoomUpLeftDown`, `RoomUpLeftRight`, `RoomUpLeftRightDown`, `RoomUpRight`, `RoomUpRightDown`

---

## 6. Inspector Setup

Select the `LevelGenerator` GameObject and configure the following fields:

| Field              | Value                                   |
| ------------------ | --------------------------------------- |
| `Room Prefabs`     | All 15 room prefabs assigned            |
| `Player Prefab`    | The Player prefab                       |
| `Distance To End`  | e.g. `10`                               |
| `Generation Point` | The `GenerationPoint` child transform   |
| `X Offset`         | `18` (match room width in world units)  |
| `Y Offset`         | `10` (match room height in world units) |
| `Room Layer Mask`  | The `RoomLayout` layer                  |

Create a new layer called `RoomLayout` and assign it to the `RoomLayout` prefab. Assign this layer to the **Room Layer Mask** field.

---

## Exercises

### Task 1 — Visualise the Generation Path

Draw lines between consecutive room positions using `Debug.DrawLine` in `Update` so you can see the generation path during Play mode.

> **Hint:** Loop through `roomPositions` and draw a line between each pair of adjacent positions.

---

### Task 2 — Tilemap Room Outlines

Use the `BasicRoom` prefab as a base to create tilemap versions of each directional room. Replace the sprite-based outlines with tilemap rooms that include proper floors, walls, and colliders.

> **Hint:** Each tilemap room prefab should contain its own `Grid` and `Tilemap` GameObjects, painted with the correct wall openings to match the exit directions.
