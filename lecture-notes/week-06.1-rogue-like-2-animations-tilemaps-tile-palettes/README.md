# Week 06.1 - Rogue-Like: Animations, Tilemaps and Tile Palettes

## 1. Rotating the Gun

To make the gun point toward the mouse cursor, update `PlayerController` with the following:

```csharp
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Omitted for brevity

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform gunTransform;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Omitted for brevity

        Vector3 mousePosition = Input.mousePosition;
        Vector3 cursorPoint = mainCamera.WorldToScreenPoint(transform.localPosition);
        Vector2 offset = new Vector2(mousePosition.x - cursorPoint.x, mousePosition.y - cursorPoint.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        gunTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
```

What is happening in the code above?

| Expression              | Purpose                                                                         |
| ----------------------- | ------------------------------------------------------------------------------- |
| `mousePosition`         | Stores the current mouse position in screen space                               |
| `cursorPoint`           | Converts the player's world position to screen space using `WorldToScreenPoint` |
| `offset`                | The vector from the player to the mouse in screen space                         |
| `angle`                 | The angle between the player and the mouse, calculated with `Mathf.Atan2`       |
| `gunTransform.rotation` | Applies the angle as a rotation using `Quaternion.Euler`                        |

**Step 1** - In the Hierarchy, select the `Player` GameObject. In the Inspector, drag the `Gun` child GameObject into the **Gun Transform** field of the `PlayerController` component.

![](<../../resources%20(ignore)/img/08-images/08-image-1.png>)

**Step 2** - Click **Play** and move the mouse. The gun should rotate to face the cursor.

![](<../../resources%20(ignore)/img/08-images/08-image-2.png>)

---

## 2. Facing Direction

When the mouse is to the left of the player, both the player sprite and gun should flip horizontally. Update the `Update()` method in `PlayerController`:

```csharp
void Update()
{
    // Omitted for brevity

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

Click **Play** and move the mouse to each side of the player. The character and gun should flip to face the cursor direction.

![](<../../resources%20(ignore)/img/08-images/08-image-3.png>)

---

## 3. Animation

### Opening the Animation Window

Go to **Window > Animation > Animation** to open the Animation window.

![](<../../resources%20(ignore)/img/08-images/08-image-4.png>)

---

### Idle Animation

**Step 1** - In the Hierarchy, select the `Player` GameObject. In the Animation window, click **Create**. In the `Assets` folder, create a new folder called `Animations`. Name the new animation clip `PlayerIdle`.

![](<../../resources%20(ignore)/img/08-images/08-image-5.png>)

**Step 2** - In the Hierarchy, select the `Guns_0` child GameObject. Move the timeline slider to `0.2`. Click the **Record** button in the Animation window.

![](<../../resources%20(ignore)/img/08-images/08-image-6.png>)

**Step 3** - In the Inspector, set the `Y` position of `Guns_0` to `-0.1`. You should see a `Guns_0 : Position` property appear in the Animation window with two keyframes - one at `0` and one at `0.2`.

![](<../../resources%20(ignore)/img/08-images/08-image-9.png>)

**Step 4** - Move the slider to `0.4`, create a new keyframe, and set the `Y` position back to `0`.

**Step 5** - Go back to the first keyframe (at `0`) and confirm the `Y` position is `0`.

![](<../../resources%20(ignore)/img/08-images/08-image-7.png>)

---

### Walk Animation

**Step 1** - Create a new animation clip called `PlayerWalk` in the `Animations` folder.

![](<../../resources%20(ignore)/img/08-images/08-image-11.png>)

**Step 2** - Move the slider to `0.2`. Click **Record**. Select the `Characters_0` child GameObject. Set its `Z` rotation to `7`. On the first keyframe (at `0`), set the `Z` rotation to `-7`. Add a keyframe at `0.1` and set `Z` rotation to `0`.

![](<../../resources%20(ignore)/img/08-images/08-image-12.png>)

---

### Animator

**Step 1** - Go to **Window > Animation > Animator** to open the Animator window.

![](<../../resources%20(ignore)/img/08-images/08-image-13.png>)

**Step 2** - In the Animator window, click the **Parameters** tab, then click **+** and create a new **Bool** parameter named `isMoving`.

![](<../../resources%20(ignore)/img/08-images/08-image-14.png>)

![](<../../resources%20(ignore)/img/08-images/08-image-15.png>)

**Step 3** - Right-click on the `PlayerIdle` state and select **Make Transition**. Drag the arrow to the `PlayerWalk` state.

![](<../../resources%20(ignore)/img/08-images/08-image-16.png>)

![](<../../resources%20(ignore)/img/08-images/08-image-17.png>)

**Step 4** - Select the transition arrow. In the Inspector, uncheck **Has Exit Time** and add a condition: `isMoving` is `true`.

> **Has Exit Time** controls whether an animation must finish before transitioning. Unchecking it allows an immediate transition as soon as the condition is met.

![](<../../resources%20(ignore)/img/08-images/08-image-18.png>)

**Step 5** - Create a transition from `PlayerWalk` back to `PlayerIdle`. Set the condition to `isMoving` is `false`, and also uncheck **Has Exit Time**.

![](<../../resources%20(ignore)/img/08-images/08-image-19.png>)

![](<../../resources%20(ignore)/img/08-images/08-image-20.png>)

---

### Driving the Animator from Script

Add the following to `PlayerController` to update the animator parameter based on movement:

```csharp
public class PlayerController : MonoBehaviour
{
    // Omitted for brevity

    [SerializeField] private Animator animator;

    void Update()
    {
        // Omitted for brevity

        if (movement != Vector2.zero)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }
}
```

> **Note:** `movement` should match whatever variable name you used for player input in your earlier tasks.

In the Inspector, drag the `Animator` component from the `Player` GameObject into the **Animator** field of the `PlayerController` script.

![](<../../resources%20(ignore)/img/08-images/08-image-21.png>)

Click **Play**. The player should play the walk animation while moving and the idle animation when still.

---

## 4. Tilemaps

**Tilemaps** provide a built-in system for constructing 2D environments from small, reusable tile images.

**Step 1** - Go to **GameObject > 2D Object > Tilemap > Rectangular**. This creates a `Grid` GameObject in the Hierarchy with a `Tilemap` child.

![](<../../resources%20(ignore)/img/08-images/08-image-22.png>)

![](<../../resources%20(ignore)/img/08-images/08-image-23.png>)

---

### Tile Palette

A **Tile Palette** is a collection of tiles used to paint a tilemap.

**Step 1** - Go to **Window > 2D > Tile Palette** to open the Tile Palette window.

![](<../../resources%20(ignore)/img/08-images/08-image-24.png>)

**Step 2** - Click **Create New Palette**. Name the palette `Dungeon 3` and click **Create**. Save it in the `Assets > Tilesets > Dungeon 3` folder.

![](<../../resources%20(ignore)/img/08-images/08-image-25.png>)

![](<../../resources%20(ignore)/img/08-images/08-image-26.png>)

![](<../../resources%20(ignore)/img/08-images/08-image-27.png>)

**Step 3** - Drag the `Dungeon 3 Tiles` sprite sheet into the Tile Palette window. The tiles will be extracted and displayed in the palette.

![](<../../resources%20(ignore)/img/08-images/08-image-28.png>)

![](<../../resources%20(ignore)/img/08-images/08-image-29.png>)

---

## Exercises

---

### Task 1 - Paint a Room

Select **Paint with basic brush** in the Tile Palette and paint a room using tiles from the palette. Here is an example layout:

![](<../../resources%20(ignore)/img/08-images/08-image-30.png>)

---

### Task 2 - Tilemap Collider

Add a `Tilemap Collider 2D` component to the `Tilemap` GameObject. Enable the **Used By Composite** checkbox. Then add a `Composite Collider 2D` component to the same GameObject.

> **Note:** Adding `Composite Collider 2D` will automatically add a `Rigidbody2D`. Set its **Body Type** to `Kinematic` so gravity does not affect the tilemap.

---

### Task 3 - Floor Tiles

Click on `Dungeon 3 Tiles_24` in the `Assets > Tilesets > Dungeon 3` folder. In the Inspector, set its **Collider Type** to `None`. This allows the player to walk across floor tiles without colliding with them.

![](<../../resources%20(ignore)/img/08-images/08-image-31.png>)

---

### Task 4 - Normalise Diagonal Movement

You will notice that the player moves faster when moving diagonally, because both the X and Y axes contribute simultaneously. Write the code to normalise the movement vector so the player moves at a constant speed in all directions.

> **Hint:** Call `.normalized` on your movement `Vector2` before multiplying by speed. Only normalise when the magnitude is greater than zero to avoid dividing by zero.

📖 Reference: [Unity - Vector2.normalized](https://docs.unity3d.com/ScriptReference/Vector2-normalized.html)
