# Week 06.1 — Animations, Tilemaps and Tile Palettes

This week adds gun rotation, sprite flipping, idle and walk animations driven by the Animator, and a tilemap-based room environment.

---

## 1. Rotating the Gun

Update `PlayerController` to rotate the gun transform toward the mouse cursor each frame.

```csharp
[SerializeField] private Transform gunTransform;
private Camera mainCamera;

void Start()
{
    mainCamera = Camera.main;
}

void Update()
{
    Vector3 mousePosition = Input.mousePosition;
    Vector3 cursorPoint = mainCamera.WorldToScreenPoint(transform.localPosition);
    Vector2 offset = new Vector2(mousePosition.x - cursorPoint.x, mousePosition.y - cursorPoint.y);
    float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
    gunTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
}
```

| Expression              | Purpose                                                           |
| ----------------------- | ----------------------------------------------------------------- |
| `mousePosition`         | Current mouse position in screen space                            |
| `cursorPoint`           | Player's world position converted to screen space                 |
| `offset`                | Vector from the player to the mouse in screen space               |
| `angle`                 | Angle between player and mouse, calculated with `Mathf.Atan2`     |
| `gunTransform.rotation` | Angle applied as a rotation via `Quaternion.Euler`                |

In the Inspector, drag the `Gun` child GameObject into the **Gun Transform** field of `PlayerController`.

Click **Play** and move the mouse. The gun should rotate to face the cursor.

---

## 2. Facing Direction

When the mouse is to the left of the player, both the player sprite and gun should flip horizontally. Add the following to the `Update()` method in `PlayerController`, before the rotation calculation:

```csharp
void Update()
{
    Vector3 mousePosition = Input.mousePosition;
    Vector3 cursorPoint = mainCamera.WorldToScreenPoint(transform.localPosition);

    if (mousePosition.x < cursorPoint.x)
    {
        transform.localScale = new Vector3(-1f, 1f, 1f);
        gunTransform.localScale = new Vector3(-1f, -1f, 1f);
    }
    else
    {
        transform.localScale = Vector3.one;
        gunTransform.localScale = Vector3.one;
    }

    Vector2 offset = new Vector2(mousePosition.x - cursorPoint.x, mousePosition.y - cursorPoint.y);
    float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
    gunTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
}
```

Click **Play** and move the mouse to each side of the player. The character and gun should flip to face the cursor.

---

## 3. Animation

### Opening the Animation Window

Go to **Window > Animation > Animation** to open the Animation window.

---

### Idle Animation

**Step 1** — Select the `Player` GameObject. In the Animation window, click **Create**. In the `Assets` folder, create a new folder called `Animations`. Name the clip `PlayerIdle`.

**Step 2** — Select the `Guns_0` child GameObject. Move the timeline slider to `0.2`. Click the **Record** button.

**Step 3** — In the Inspector, set the Y position of `Guns_0` to `-0.1`. Two keyframes should appear in the Animation window — one at `0` and one at `0.2`.

**Step 4** — Move the slider to `0.4`, create a new keyframe, and set the Y position back to `0`.

**Step 5** — Go back to the first keyframe at `0` and confirm the Y position is `0`.

---

### Walk Animation

**Step 1** — Create a new animation clip called `PlayerWalk` in the `Animations` folder.

**Step 2** — Move the slider to `0.2`. Click **Record**. Select the `Characters_0` child GameObject and set its Z rotation to `7`. At the keyframe at `0`, set Z rotation to `-7`. Add a keyframe at `0.1` and set Z rotation to `0`.

---

### Animator Setup

**Step 1** — Go to **Window > Animation > Animator** to open the Animator window.

**Step 2** — In the **Parameters** tab, click **+** and create a new **Bool** parameter named `isMoving`.

**Step 3** — Right-click the `PlayerIdle` state and select **Make Transition**. Drag the arrow to the `PlayerWalk` state.

**Step 4** — Select the transition arrow. In the Inspector, uncheck **Has Exit Time** and add a condition: `isMoving` is `true`.

> **Has Exit Time** controls whether an animation must finish before transitioning. Unchecking it allows an immediate transition as soon as the condition is met.

**Step 5** — Create a transition from `PlayerWalk` back to `PlayerIdle`. Set the condition to `isMoving` is `false` and also uncheck **Has Exit Time**.

---

### Driving the Animator from Script

Add an `Animator` field to `PlayerController` and set the `isMoving` parameter based on the current movement input:

```csharp
[SerializeField] private Animator animator;

void Update()
{
    // ... gun rotation and flip code above ...

    animator.SetBool("isMoving", moveInput != Vector2.zero);
}
```

In the Inspector, drag the `Animator` component from the `Player` GameObject into the **Animator** field of `PlayerController`.

Click **Play**. The player should play the walk animation while moving and the idle animation when still.

---

## 4. Tilemaps

**Tilemaps** provide a built-in system for constructing 2D environments from small, reusable tile images.

**Step 1** — Go to **GameObject > 2D Object > Tilemap > Rectangular**. This creates a `Grid` GameObject in the Hierarchy with a `Tilemap` child.

---

### Tile Palette

A **Tile Palette** is a collection of tiles used to paint onto a tilemap.

**Step 1** — Go to **Window > 2D > Tile Palette** to open the Tile Palette window.

**Step 2** — Click **Create New Palette**. Name the palette `Dungeon 3` and click **Create**. Save it in the `Assets > Tilesets > Dungeon 3` folder.

**Step 3** — Drag the `Dungeon 3 Tiles` sprite sheet into the Tile Palette window. The tiles will be extracted and displayed in the palette.

---

## Exercises

### Task 1 — Paint a Room

Select **Paint with basic brush** in the Tile Palette and paint a room using tiles from the palette.

---

### Task 2 — Tilemap Collider

Add a `Tilemap Collider 2D` component to the `Tilemap` GameObject. Enable the **Used By Composite** checkbox. Then add a `Composite Collider 2D` component to the same GameObject.

> Adding `Composite Collider 2D` will automatically add a `Rigidbody2D`. Set its **Body Type** to `Kinematic` so gravity does not affect the tilemap.

---

### Task 3 — Floor Tiles

Click on `Dungeon 3 Tiles_24` in the `Assets > Tilesets > Dungeon 3` folder. In the Inspector, set its **Collider Type** to `None`. This allows the player to walk across floor tiles without colliding with them.

---

### Task 4 — Normalise Diagonal Movement

The player moves faster diagonally because both X and Y axes contribute simultaneously. Call `.normalized` on the movement vector before multiplying by speed. Only normalise when the magnitude is greater than zero to avoid dividing by zero.

📖 Reference: [Unity — Vector2.normalized](https://docs.unity3d.com/ScriptReference/Vector2-normalized.html)