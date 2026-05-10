# Week 10.1 — Platformer: Raycasting

**Raycasting** is a technique where an invisible ray is fired from a point in a direction, and Unity reports back what it hits. In a 2D platformer, raycasts are the standard way to check whether a character is standing on the ground, about to walk off a ledge, or about to hit a wall — without relying on collision callbacks alone.

---

## 1. How This Fits

Raycasting is added to `PlayerController`. Rather than using `OnCollisionEnter2D` to detect the ground, the player fires short downward rays each frame. This gives precise, frame-accurate ground detection that works reliably on slopes, moving platforms, and ledge edges.

| Script             | Where it goes         | What it does                                                 |
| ------------------ | --------------------- | ------------------------------------------------------------ |
| `PlayerController` | The player GameObject | Fires raycasts each frame to check ground, walls, and ledges |
| `GroundCheck`      | Inspector reference   | A `Transform` marking where ground rays originate            |

---

## 2. The Problem

Unity's built-in collision callbacks (`OnCollisionEnter2D`, `OnCollisionStay2D`) can miss fast-moving objects, fire a frame late, or behave unexpectedly when the player grazes the edge of a platform. A common result is coyote time breaking, double jumps triggering mid-air, or the player sticking to walls.

Raycasting solves this by asking each frame: "is there ground directly beneath me right now?" The answer is always current and precise.

---

## 3. Input System Setup

This project uses Unity's **Input System** package with an `InputActionAsset`. If you do not already have one set up from a previous week, follow these steps.

**Step 1** — Go to **Window > Package Manager**. In the **Unity Registry**, search for `Input System` and click **Install**. When prompted to switch the active input backend, click **Yes**.

**Step 2** — In the `Assets` folder, right-click and go to **Create > Input Actions**. Name the asset `PlayerInputActions`.

**Step 3** — Double-click the asset to open the Input Actions editor. Create an **Action Map** called `Player`. Inside it, create the following actions:

| Action | Action Type | Binding                       |
| ------ | ----------- | ----------------------------- |
| `Move` | `Value`     | WASD / Arrow Keys (2D Vector) |
| `Jump` | `Button`    | Space                         |

Click **Save Asset**.

**Step 4** — In `PlayerController`, declare and wire up the actions:

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 12f;

    private InputAction moveAction;
    private InputAction jumpAction;
    private Rigidbody2D rb;

    private Vector2 moveInput;
    private bool jumpPressed;   // true on the frame Jump was pressed
    private bool jumpReleased;  // true on the frame Jump was released

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        var playerMap = inputActions.FindActionMap("Player");
        moveAction    = playerMap.FindAction("Move");
        jumpAction    = playerMap.FindAction("Jump");
    }

    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }
}
```

In the Inspector, drag the `PlayerInputActions` asset into the **Input Actions** field.

---

## 4. Reading Input Each Frame

The Input System separates _reading_ input from _acting_ on it. Read all values at the top of `Update()` into local variables, then use those variables in your movement and jump methods.

```csharp
private void Update()
{
    moveInput    = moveAction.ReadValue<Vector2>();
    jumpPressed  = jumpAction.WasPressedThisFrame();
    jumpReleased = jumpAction.WasReleasedThisFrame();

    CheckGrounded();
    CheckWalls();
    CheckLedge();
    HandleMovement();
    HandleJump();
}
```

| Method                   | When it is true                                       |
| ------------------------ | ----------------------------------------------------- |
| `WasPressedThisFrame()`  | Only on the single frame the button was first pressed |
| `WasReleasedThisFrame()` | Only on the single frame the button was released      |
| `IsPressed()`            | Every frame the button is held down                   |
| `ReadValue<Vector2>()`   | The current axis value this frame                     |

---

## 5. Ground Detection

**Step 1** — Create an empty child GameObject on the `Player` called `GroundCheck`. Position it at the bottom centre of the player sprite, just below the feet.

**Step 2** — Add the following fields to `PlayerController`:

```csharp
[SerializeField] private Transform groundCheck;
[SerializeField] private float groundCheckRadius = 0.1f;
[SerializeField] private LayerMask groundLayer;

private bool isGrounded;
```

**Step 3** — Add the check method:

```csharp
private void CheckGrounded()
{
    isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
}
```

`Physics2D.OverlapCircle` checks a small circle for overlapping colliders. It is the most reliable method for ground detection because it catches platform edges even when the player is only partially overlapping.

**Step 4** — Create a new layer called `Ground` and assign it to all platform and floor GameObjects. Set the **Ground Layer** field on `PlayerController` in the Inspector.

---

## 6. Raycasting for Walls and Ledges

For wall detection and ledge checks, `Physics2D.Raycast` fires a ray from a point in a direction and returns information about the first collider it hits.

```csharp
[SerializeField] private float wallCheckDistance = 0.5f;

private bool isTouchingWall;
private bool isNearLedge;

private void CheckWalls()
{
    // Fire a ray horizontally in the direction the player is facing
    float direction = transform.localScale.x > 0 ? 1f : -1f;

    RaycastHit2D wallHit = Physics2D.Raycast(
        transform.position,
        Vector2.right * direction,
        wallCheckDistance,
        groundLayer
    );

    isTouchingWall = wallHit.collider != null;

    Debug.DrawRay(transform.position, Vector2.right * direction * wallCheckDistance, Color.red);
}

private void CheckLedge()
{
    // Fire a ray downward from slightly ahead of the player's feet
    float direction = transform.localScale.x > 0 ? 1f : -1f;
    Vector2 ledgeCheckOrigin = (Vector2)transform.position + Vector2.right * direction * 0.5f;

    RaycastHit2D ledgeHit = Physics2D.Raycast(
        ledgeCheckOrigin,
        Vector2.down,
        1f,
        groundLayer
    );

    isNearLedge = ledgeHit.collider == null;

    Debug.DrawRay(ledgeCheckOrigin, Vector2.down, Color.blue);
}
```

---

## 7. RaycastHit2D

`Physics2D.Raycast` returns a `RaycastHit2D` struct containing information about what was hit.

| Property       | Type         | What it contains                                     |
| -------------- | ------------ | ---------------------------------------------------- |
| `hit.collider` | `Collider2D` | The collider that was hit. `null` if nothing was hit |
| `hit.point`    | `Vector2`    | The world position where the ray made contact        |
| `hit.normal`   | `Vector2`    | The surface normal at the point of contact           |
| `hit.distance` | `float`      | How far along the ray the hit occurred               |

```csharp
RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, layerMask);

if (hit.collider != null)
{
    Debug.Log($"Hit {hit.collider.name} at distance {hit.distance}");
    Debug.Log($"Surface normal: {hit.normal}");
}
```

---

## 8. Movement

With `moveInput` read from the Input System, horizontal movement is applied in `FixedUpdate` to keep physics deterministic:

```csharp
private void HandleMovement()
{
    rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);

    // Flip the sprite to face the direction of movement
    if (moveInput.x != 0)
        transform.localScale = new Vector3(Mathf.Sign(moveInput.x), 1f, 1f);
}
```

---

## 9. Jumping with Ground Detection

With `isGrounded` reliable and `jumpPressed` read from the Input System, use both to gate the jump:

```csharp
private void HandleJump()
{
    if (jumpPressed && isGrounded)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }
}
```

---

## 10. Coyote Time

**Coyote time** gives the player a short window to jump after walking off a ledge, making controls feel more forgiving.

```csharp
[SerializeField] private float coyoteTime = 0.15f;
private float coyoteTimeCounter;

private void Update()
{
    moveInput    = moveAction.ReadValue<Vector2>();
    jumpPressed  = jumpAction.WasPressedThisFrame();
    jumpReleased = jumpAction.WasReleasedThisFrame();

    CheckGrounded();

    // Reset the counter when grounded; count down when airborne
    if (isGrounded)
        coyoteTimeCounter = coyoteTime;
    else
        coyoteTimeCounter -= Time.deltaTime;

    HandleMovement();
    HandleJump();
}

private void HandleJump()
{
    if (jumpPressed && coyoteTimeCounter > 0f)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        coyoteTimeCounter = 0f; // Prevent double-jumping after a coyote jump
    }
}
```

---

## 11. Jump Buffering

**Jump buffering** remembers a jump input for a short window before the player lands, so pressing jump just before touching the ground still triggers a jump.

```csharp
[SerializeField] private float jumpBufferTime = 0.1f;
private float jumpBufferCounter;

private void Update()
{
    moveInput    = moveAction.ReadValue<Vector2>();
    jumpPressed  = jumpAction.WasPressedThisFrame();
    jumpReleased = jumpAction.WasReleasedThisFrame();

    CheckGrounded();

    if (isGrounded)
        coyoteTimeCounter = coyoteTime;
    else
        coyoteTimeCounter -= Time.deltaTime;

    // Start the buffer window on the frame Jump is pressed
    if (jumpPressed)
        jumpBufferCounter = jumpBufferTime;
    else
        jumpBufferCounter -= Time.deltaTime;

    HandleMovement();
    HandleJump();
}

private void HandleJump()
{
    // Jump if the buffer is active and the player is in the coyote window
    if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        jumpBufferCounter = 0f;
        coyoteTimeCounter = 0f;
    }

    // Variable jump height — release early for a shorter jump
    if (jumpReleased && rb.linearVelocity.y > 0f)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
    }
}
```

---

## 12. Debug.DrawRay

`Debug.DrawRay` draws a ray in the Scene view during Play mode. It has no effect in the built game. Use it whenever you add a new raycast to confirm it is firing from the right position and in the right direction.

```csharp
// Signature: DrawRay(origin, direction, color)
Debug.DrawRay(transform.position, Vector2.down * groundCheckRadius, Color.green);
```

Open the **Scene** view while in Play mode to see the rays. Switch the Scene view gizmo display to **Shaded Wireframe** if the rays are hard to see.

---

## Exercises

---

### Task 1 — One-Way Platforms

Create a platform with a `PlatformEffector2D` component and enable **Use One Way**. Add a `Crouch` action (Button type) to your `Player` action map, bound to **S** and the **down arrow**. When the player presses crouch while standing on a one-way platform, let them drop through it.

> **Hint:** Read the action with `crouchAction.WasPressedThisFrame()`. To drop through, temporarily ignore the platform layer using `Physics2D.IgnoreLayerCollision`, then restore it after a short delay with a `Coroutine`.

---

### Task 2 — Slope Detection

Use `RaycastHit2D.normal` from the ground check to detect when the player is on a slope. If the surface normal is not `Vector2.up`, calculate the slope angle with `Vector2.Angle(hit.normal, Vector2.up)` and reduce the player's movement speed proportionally.

> **Hint:** A flat surface returns a normal of `(0, 1)`. A 45° slope returns approximately `(-0.71, 0.71)`.

---

### Task 3 — Multiple Ground Rays

Replace the single `OverlapCircle` ground check with three downward raycasts: one from the centre of the player's feet, one from the left edge, and one from the right edge. The player is considered grounded if **any** of the three rays hits.

> **Hint:** Use `Physics2D.Raycast` for each and combine the results with `||`. Use `Debug.DrawRay` on all three to confirm their positions in the Scene view.
