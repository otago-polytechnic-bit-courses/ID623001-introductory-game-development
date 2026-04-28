# Week 09.2 - Rogue-Like: Field of View

**Field of View (FOV)** is a technique used to limit what an enemy can detect. Rather than always knowing where the player is, an enemy only reacts when the player is within a certain angle and distance - and not hidden behind a wall.

This is a core mechanic in stealth and roguelike games.

---

## 1. How This Fits

Field of view is added to the existing `EnemyController`. Two checks are combined: an **angle check** (is the player within the enemy's viewing cone?) and a **raycast check** (is there a wall between the enemy and the player?). Both must pass before the enemy reacts.

| Element                 | Where it lives              | What it does                                                       |
| ----------------------- | --------------------------- | ------------------------------------------------------------------ |
| Angle and range fields  | `EnemyController` Inspector | Define how wide and far the enemy can see                          |
| `CanSeePlayer()` method | `EnemyController`           | Returns true if the player is in range, in angle, and not occluded |
| `Physics2D.Raycast`     | Inside `CanSeePlayer()`     | Checks whether a wall is blocking the line of sight                |
| `OnDrawGizmosSelected`  | `EnemyController`           | Draws the FOV cone in the Scene view for debugging                 |

---

## 2. The Problem

Without FOV, enemies always know where the player is regardless of distance or direction. This removes any sense of stealth or tension. A field of view system means the player can sneak past enemies that are looking the other way, or break line of sight by ducking behind a wall.

---

## 3. Angle and Range Check

The first step is checking whether the player is close enough and within the viewing angle.

**Step 1** - Add the following fields to `EnemyController`:

```csharp
[SerializeField] private float viewRange = 8f;
[SerializeField] private float viewAngle = 90f;
```

**Step 2** - Add a `CanSeePlayer()` method:

```csharp
public bool CanSeePlayer()
{
    Vector2 directionToPlayer = PlayerController.Instance.transform.position - transform.position;
    float distanceToPlayer = directionToPlayer.magnitude;

    // Range check
    if (distanceToPlayer > viewRange)
        return false;

    // Angle check
    float angleBetween = Vector2.Angle(transform.right, directionToPlayer);
    if (angleBetween > viewAngle / 2f)
        return false;

    return true;
}
```

What is happening here?

| Element                       | Purpose                                                                                            |
| ----------------------------- | -------------------------------------------------------------------------------------------------- |
| `directionToPlayer.magnitude` | The straight-line distance to the player                                                           |
| `Vector2.Angle`               | Returns the angle in degrees between the enemy's forward direction and the direction to the player |
| `viewAngle / 2f`              | The angle is compared to half the total cone width, since the cone extends equally on both sides   |
| `transform.right`             | The enemy's forward-facing direction in a 2D top-down game                                         |

**Step 3** - Update `EnemyController` to use `CanSeePlayer()` instead of a plain distance check:

```csharp
void Update()
{
    if (CanSeePlayer())
    {
        // Chase or attack logic here
        Vector2 direction = (PlayerController.Instance.transform.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }
    else
    {
        rb.linearVelocity = Vector2.zero;
    }
}
```

Click **Play**. The enemy should only move when the player is within its viewing cone. Walk around the enemy and notice it ignores you when you are behind it.

---

## 4. Line of Sight Raycast

The angle check alone does not account for walls. A **raycast** fires an invisible ray from the enemy toward the player and checks what it hits first. If it hits a wall before reaching the player, line of sight is blocked.

**Step 1** - Create a new layer called `Wall` and assign it to all wall GameObjects and tilemaps.

**Step 2** - Add a `wallLayer` field to `EnemyController`:

```csharp
[SerializeField] private LayerMask wallLayer;
```

**Step 3** - Update `CanSeePlayer()` to include the raycast:

```csharp
public bool CanSeePlayer()
{
    Vector2 directionToPlayer = PlayerController.Instance.transform.position - transform.position;
    float distanceToPlayer = directionToPlayer.magnitude;

    // Range check
    if (distanceToPlayer > viewRange)
        return false;

    // Angle check
    float angleBetween = Vector2.Angle(transform.right, directionToPlayer);
    if (angleBetween > viewAngle / 2f)
        return false;

    // Line of sight check
    RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, distanceToPlayer, wallLayer);
    if (hit.collider != null)
        return false; // Something is blocking the view

    return true;
}
```

**Step 4** - Select the `EnemyController` GameObject. In the Inspector, set the **Wall Layer** field to the `Wall` layer.

Click **Play**. The enemy should now lose sight of the player when a wall comes between them.

> **Note:** Make sure the enemy's own collider is not on the `Wall` layer, otherwise the raycast will hit the enemy itself immediately.

---

## 5. Visualising the FOV in the Editor

It is difficult to tune `viewRange` and `viewAngle` without being able to see the cone. `OnDrawGizmosSelected` draws debug shapes in the Scene view whenever the GameObject is selected - it has no effect in the built game.

Add this to `EnemyController`:

```csharp
void OnDrawGizmosSelected()
{
    // Draw the view range circle
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, viewRange);

    // Draw the left and right edges of the viewing cone
    float halfAngle = viewAngle / 2f;

    Vector3 leftBoundary  = Quaternion.Euler(0, 0,  halfAngle) * transform.right * viewRange;
    Vector3 rightBoundary = Quaternion.Euler(0, 0, -halfAngle) * transform.right * viewRange;

    Gizmos.color = Color.cyan;
    Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
    Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
}
```

Select the enemy GameObject in the Hierarchy. You should see a yellow circle and two cyan lines forming the viewing cone in the Scene view.

> **Tip:** Adjust `viewAngle` and `viewRange` in the Inspector while in Play mode to find values that feel right for your game, then copy the values out before stopping Play mode.

---

## 6. Connecting to the State Machine

If you have implemented the state machine from Week 08.2, replace the plain distance check in the `Idle` and `Chase` states with `CanSeePlayer()`:

```csharp
// In the Idle state
if (CanSeePlayer())
    currentState = EnemyState.Chase;

// In the Chase state
if (!CanSeePlayer())
    currentState = EnemyState.Idle;
```

The enemy will now only begin chasing when it can actually see the player, and will give up the chase if line of sight is broken.

---

## Exercises

---

### Task 1 - Alert State

Add an `Alert` state between `Idle` and `Chase`. When the enemy spots the player it enters `Alert` for 1 second (playing a visual indicator such as a colour change) before transitioning to `Chase`. If it loses sight during the alert period, it returns to `Idle`.

> **Hint:** Use a timer float that counts down in `Update()`. Start it at `1f` when entering the `Alert` state.

---

### Task 2 - Last Known Position

When the enemy loses sight of the player, make it walk to the player's last known position before giving up and returning to `Idle`.

> **Hint:** Store `PlayerController.Instance.transform.position` in a `Vector2 lastKnownPosition` variable each frame that `CanSeePlayer()` returns true. Move toward it when sight is lost.

---

### Task 3 - FOV Mesh

Instead of the Gizmo lines, draw the actual FOV cone as a filled `Mesh` in the game view using `MeshFilter` and `MeshRenderer`. Cast rays at small angle increments within the cone and build triangles between them.

> **Hint:** This is an extension task. Look up Unity's `Mesh` class. Create vertices at the enemy position and at each raycast hit point, then connect them into triangles using the `triangles` array.
