## 1. Random Generation

**Random Generation** is a technique used in game development to create content that is not pre-defined — levels, items, enemies and more.

**Step 1** - Remove the `SampleScene`. Create a new scene called `GenerationTest`. In the **Assets > Art > Map** folder, drag `Map_Rooms_15` into the Hierarchy. This creates a new GameObject in the scene.

![](../../resources%20(ignore)/img/10-images/10-image-1.png)

**Step 2** - In the Hierarchy, create a new empty GameObject called `LevelGenerator`. Add a child empty GameObject called `GenerationPoint`. The `LevelGenerator` will manage room generation; `GenerationPoint` tracks where the next room will be placed.

![](../../resources%20(ignore)/img/10-images/10-image-2.png)

**Step 3** - Rename `Map_Rooms_15` to `RoomLayout`. Drag it into the **Prefabs > Rooms** folder to make it a prefab. Delete the instance from the Hierarchy.

![](../../resources%20(ignore)/img/10-images/10-image-3.png)

---

## 2. LevelGenerator Script

**Step 1** - In the `Scripts` folder, create a new script called `LevelGenerator` and attach it to the `LevelGenerator` GameObject.

![](../../resources%20(ignore)/img/10-images/10-image-4.png)

**Step 2** - Open the script and add the following code to generate the first room:

```csharp
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject roomLayout;
    [SerializeField] private int distanceToEnd;
    [SerializeField] private Transform generationPoint;

    private Color startColor = Color.blue;
    private Color endColor = Color.red;

    void Start()
    {
        GameObject room = Instantiate(roomLayout, generationPoint.position, generationPoint.rotation);
        SpriteRenderer sr = room.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = startColor;
        }
    }
}
```

**Step 3** - Select the `LevelGenerator` GameObject. In the Inspector, drag the `RoomLayout` prefab into the **Room Layout** field, set **Distance To End** to `10`, and drag the `GenerationPoint` child into the **Generation Point** field.

![](../../resources%20(ignore)/img/10-images/10-image-5.png)

**Step 4** - Click **Play**. A single blue room should appear in the scene.

---

## 3. Generating More Rooms

Update `LevelGenerator` to generate a chain of rooms in random directions:

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    // Omitted for brevity

    private enum Direction { Up, Down, Left, Right }

    [SerializeField] private Direction direction;
    [SerializeField] private float xOffset = 18f;
    [SerializeField] private float yOffset = 10f;

    void Start()
    {
        // Omitted for brevity — spawn the start room here

        direction = (Direction)Random.Range(0, 4);
        MoveGenerationPoint();

        for (int i = 0; i < distanceToEnd; i++)
        {
            Instantiate(roomLayout, generationPoint.position, generationPoint.rotation);

            direction = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void MoveGenerationPoint()
    {
        switch (direction)
        {
            case Direction.Up:
                generationPoint.position += new Vector3(0, yOffset, 0);
                break;
            case Direction.Down:
                generationPoint.position += new Vector3(0, -yOffset, 0);
                break;
            case Direction.Left:
                generationPoint.position += new Vector3(-xOffset, 0, 0);
                break;
            case Direction.Right:
                generationPoint.position += new Vector3(xOffset, 0, 0);
                break;
        }
    }
}
```

What is happening in the code above?

| Element | Purpose |
| --- | --- |
| `Direction` enum | Defines the four cardinal directions a room can be placed in |
| `xOffset` / `yOffset` | The world-space distance between adjacent rooms |
| `MoveGenerationPoint` | Moves `generationPoint` by the appropriate offset based on the current direction |
| `R` key reload | Reloads the active scene so the layout can be regenerated at runtime |

Click **Play**. You should see a chain of rooms generated in random directions. Press **R** to regenerate.

![](../../resources%20(ignore)/img/10-images/10-image-6.png)

> **Note:** Some rooms may overlap. This is because the generator picks directions randomly without checking whether a room already exists at that position.

---

## 4. Preventing Overlapping Rooms

Use `Physics2D.OverlapCircle` to detect existing rooms before placing a new one.

**Step 1** - Create a new layer called `RoomLayout` and assign it to the `RoomLayout` prefab.

**Step 2** - Add a `BoxCollider2D` component to the `RoomLayout` prefab. Set **Size X** to `3` and **Size Y** to `3`.

**Step 3** - Update `LevelGenerator`:

```csharp
public class LevelGenerator : MonoBehaviour
{
    // Omitted for brevity

    [SerializeField] private LayerMask roomLayerMask;

    void Start()
    {
        // Omitted for brevity

        for (int i = 0; i < distanceToEnd; i++)
        {
            Instantiate(roomLayout, generationPoint.position, generationPoint.rotation);

            direction = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();

            while (Physics2D.OverlapCircle(generationPoint.position, 0.2f, roomLayerMask))
            {
                direction = (Direction)Random.Range(0, 4);
                MoveGenerationPoint();
            }
        }
    }
}
```

`Physics2D.OverlapCircle` checks whether a collider exists within a small radius at `generationPoint`. If one is found, `MoveGenerationPoint` is called again with a new random direction. This continues until a free position is found.

**Step 4** - Select the `LevelGenerator` GameObject. In the Inspector, set the **Room Layer Mask** field to the `RoomLayout` layer.

Click **Play** and press **R** several times. Rooms should no longer overlap.

![](../../resources%20(ignore)/img/10-images/10-image-7.png)

---

## 5. Tracking Generated Rooms

Use a `List` to track all generated rooms and identify the start and end rooms:

```csharp
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    // Omitted for brevity

    private GameObject endRoom;
    private List<GameObject> layoutRoomGOs = new List<GameObject>();

    void Start()
    {
        // Omitted for brevity

        for (int i = 0; i < distanceToEnd; i++)
        {
            GameObject newRoom = Instantiate(roomLayout, generationPoint.position, generationPoint.rotation);
            layoutRoomGOs.Add(newRoom);

            // Mark the final room as the end room
            if (i + 1 == distanceToEnd)
            {
                SpriteRenderer newRoomSr = newRoom.GetComponent<SpriteRenderer>();
                if (newRoomSr != null)
                {
                    newRoomSr.color = endColor;
                    layoutRoomGOs.RemoveAt(layoutRoomGOs.Count - 1); // Keep end room separate
                    endRoom = newRoom;
                }
            }

            direction = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();

            while (Physics2D.OverlapCircle(generationPoint.position, 0.2f, roomLayerMask))
            {
                direction = (Direction)Random.Range(0, 4);
                MoveGenerationPoint();
            }
        }
    }
}
```

Click **Play**. The first room should be blue, the last room red, and all others white. Press **R** to regenerate.

![](../../resources%20(ignore)/img/10-images/10-image-8.png)

---

## 6. Room Outline Prefabs

Each room needs walls that match its surrounding connections — e.g., a room with exits to the north and east needs a different wall layout to one with only a southern exit.

**Step 1** - In the Hierarchy, unpack the `BasicRoom` prefab and rename it `RoomRight`. Remove the `Grid` GameObject. From the **Assets > Art > Map** folder, drag `Maps_Rooms_0` into the `RoomRight` GameObject.

![](../../resources%20(ignore)/img/10-images/10-image-9.png)

**Step 2** - Drag `RoomRight` into the **Assets > Prefabs > Rooms** folder.

![](../../resources%20(ignore)/img/10-images/10-image-10.png)

**Step 3** - Repeat this process to create the following room prefabs, each using its corresponding map sprite:

`RoomDown`, `RoomLeft`, `RoomLeftDown`, `RoomLeftRight`, `RoomLeftRightDown`, `RoomRight`, `RoomRightDown`, `RoomUp`, `RoomUpDown`, `RoomUpLeft`, `RoomUpLeftDown`, `RoomUpLeftRight`, `RoomUpLeftRightDown`, `RoomUpRight`, `RoomUpRightDown`

![](../../resources%20(ignore)/img/10-images/10-image-11.png)

**Step 4** - Add a serialisable class to hold references to all room prefabs, and add it to `LevelGenerator`:

```csharp
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
    // Omitted for brevity

    [SerializeField] private RoomPrefabs roomPrefabs;
}
```

**Step 5** - Select the `LevelGenerator` GameObject. In the Inspector, assign all 15 room prefabs to the corresponding fields in the **Room Prefabs** section.

![](../../resources%20(ignore)/img/10-images/10-image-12.png)

---

## 7. Creating Room Outlines

Add the following to `LevelGenerator` to generate the correct wall prefab for each layout room based on which adjacent rooms exist:

```csharp
public class LevelGenerator : MonoBehaviour
{
    // Omitted for brevity

    private List<GameObject> generatedOutlines = new List<GameObject>();

    void Start()
    {
        // Omitted for brevity — after generating all layout rooms:

        CreateRoomOutline(Vector3.zero); // Start room
        foreach (GameObject roomGO in layoutRoomGOs)
        {
            CreateRoomOutline(roomGO.transform.position);
        }
        CreateRoomOutline(endRoom.transform.position);
    }

    public void CreateRoomOutline(Vector3 roomPosition)
    {
        bool isRoomAbove = Physics2D.OverlapCircle(roomPosition + new Vector3(0, yOffset, 0), 0.2f, roomLayerMask);
        bool isRoomBelow = Physics2D.OverlapCircle(roomPosition + new Vector3(0, -yOffset, 0), 0.2f, roomLayerMask);
        bool isRoomLeft  = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0, 0), 0.2f, roomLayerMask);
        bool isRoomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0, 0), 0.2f, roomLayerMask);

        int directionCount = 0;
        if (isRoomAbove) directionCount++;
        if (isRoomBelow) directionCount++;
        if (isRoomLeft)  directionCount++;
        if (isRoomRight) directionCount++;

        switch (directionCount)
        {
            case 1:
                if (isRoomAbove) generatedOutlines.Add(Instantiate(roomPrefabs.roomUp, roomPosition, transform.rotation));
                if (isRoomBelow) generatedOutlines.Add(Instantiate(roomPrefabs.roomDown, roomPosition, transform.rotation));
                if (isRoomLeft)  generatedOutlines.Add(Instantiate(roomPrefabs.roomLeft, roomPosition, transform.rotation));
                if (isRoomRight) generatedOutlines.Add(Instantiate(roomPrefabs.roomRight, roomPosition, transform.rotation));
                break;
            case 2:
                // TODO: handle two adjacent rooms
                break;
            case 3:
                // TODO: handle three adjacent rooms
                break;
            case 4:
                // TODO: handle all four adjacent rooms
                break;
            default:
                Debug.Log($"No adjacent rooms found at {roomPosition}");
                break;
        }
    }
}
```

`CreateRoomOutline` uses `Physics2D.OverlapCircle` to check each of the four cardinal positions around a room. It counts how many adjacent rooms exist and selects the matching wall prefab from `roomPrefabs`.

Click **Play**. You should see wall outlines appear around each room. Press **R** to regenerate the layout.

![](../../resources%20(ignore)/img/10-images/10-image-13.png)

---

## Exercises

---

### Task 1 — Complete Room Outline Cases

The `switch` statement in `CreateRoomOutline` currently only handles `case 1`. Complete `case 2`, `case 3`, and `case 4` to instantiate the correct prefab for every possible combination of adjacent rooms.

> **Hint:** For `case 2`, use `if/else if` chains to check each combination of two directions (e.g. `isRoomAbove && isRoomRight` → `roomUpRight`). Repeat the pattern for three and four directions.

---

### Task 2 — Tilemap Room Outlines

Use the `BasicRoom` prefab as a base to create tilemap versions of each directional room. Replace the sprite-based room outlines with tilemap rooms that include proper floors, walls and colliders.

> **Hint:** Each tilemap room prefab should contain its own `Grid` and `Tilemap` GameObjects, painted with the correct wall openings to match the exit directions.