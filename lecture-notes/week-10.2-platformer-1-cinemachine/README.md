# Week 10.2 — Platformer: Cinemachine

**Cinemachine** is a Unity package that handles camera behaviour through data-driven virtual cameras. Instead of writing camera follow scripts by hand, you configure how the camera should behave in the Inspector — smoothing, look-ahead, confining to a room — and Cinemachine handles the rest.

---

## 1. How This Fits

The `CameraController` script from the rogue-like project moved the camera manually using `Vector3.MoveTowards`. For a platformer, Cinemachine replaces that script entirely. A **Virtual Camera** targets the player and a **Cinemachine Brain** on the Main Camera reads from it each frame.

| Component               | Where it goes     | What it does                                               |
| ----------------------- | ----------------- | ---------------------------------------------------------- |
| `CinemachineBrain`      | Main Camera       | Reads from the active Virtual Camera and drives the camera |
| `CinemachineCamera`     | Empty GameObject  | Defines how the camera follows and looks at the player     |
| `CinemachineConfiner2D` | CinemachineCamera | Restricts the camera to a defined boundary polygon         |

---

## 2. Installing Cinemachine

**Step 1** — Go to **Window > Package Manager**. In the **Unity Registry**, search for `Cinemachine` and click **Install**.

**Step 2** — Select the **Main Camera** in the Hierarchy. Click **Add Component** and add a **Cinemachine Brain** component.

The Brain has no required configuration — it automatically blends between all active Virtual Cameras in the scene.

---

## 3. Creating a Virtual Camera

**Step 1** — Go to **GameObject > Cinemachine > Cinemachine Camera**. This adds a new GameObject called `CM Camera` to the Hierarchy.

**Step 2** — With `CM Camera` selected, find the **Follow** field in the Inspector and drag the `Player` GameObject into it.

**Step 3** — Find the **Look At** field and drag the `Player` GameObject in as well.

Click **Play**. The camera should now follow the player.

---

## 4. Position Control — Framing Transposer

The **Position Control** component on the Virtual Camera determines how the camera positions itself relative to the Follow target.

Select `CM Camera`. Under **Position Control**, set the type to **Framing Transposer**. Configure the following:

| Property              | Recommended Value | Purpose                                                        |
| --------------------- | ----------------- | -------------------------------------------------------------- |
| `Lookahead Time`      | `0.3`             | Anticipates player movement and shifts the frame ahead         |
| `Lookahead Smoothing` | `5`               | Smooths the lookahead so it does not jerk on direction changes |
| `X Damping`           | `0.5`             | How long the camera takes to catch up horizontally             |
| `Y Damping`           | `0.3`             | How long the camera takes to catch up vertically               |
| `Screen X`            | `0.5`             | Keeps the player centred horizontally                          |
| `Screen Y`            | `0.4`             | Shifts the player slightly below centre — shows more above     |

> **Lookahead** moves the camera in the direction the player is moving before they get there, so the player can see what is ahead. Increase `Lookahead Time` for a more cinematic feel, or reduce it to keep the camera tighter on the player.

---

## 5. Rotation Control

For a fixed 2D platformer camera, set **Rotation Control** to **None** to lock the camera's rotation.

---

## 6. Lens Settings

Under **Lens**, adjust the **Orthographic Size** to control how much of the scene is visible.

| Orthographic Size | Effect                                              |
| ----------------- | --------------------------------------------------- |
| Smaller value     | Camera is zoomed in — the player appears larger     |
| Larger value      | Camera is zoomed out — more of the level is visible |

A value of `5` to `8` works well for most platformers. Tune this to match your tile size and level layout.

---

## 7. Confining the Camera

A **Cinemachine Confiner 2D** prevents the camera from showing areas outside the level boundary — for example, empty space beyond the edges of a room.

**Step 1** — Create a new empty GameObject called `CameraBounds`. Add a **Polygon Collider 2D** component to it. Edit the polygon points to trace the boundary of your level. Enable **Is Trigger** so the collider does not affect physics.

**Step 2** — Select `CM Camera`. Click **Add Extension** and choose **Cinemachine Confiner 2D**.

**Step 3** — Drag the `CameraBounds` Polygon Collider 2D into the **Bounding Shape 2D** field.

**Step 4** — Set **Damping** to `0` for hard boundaries, or a small value like `0.5` to ease against the edge rather than stopping sharply.

Click **Play** and move the player to the edge of the level. The camera should stop at the boundary rather than revealing empty space.

---

## 8. Camera Shake

Cinemachine provides **Impulse Sources** and **Impulse Listeners** for camera shake — for example, when the player lands from a high fall.

**Step 1** — Select `CM Camera`. Click **Add Extension** and choose **Cinemachine Impulse Listener**.

**Step 2** — Add a `CinemachineImpulseSource` component to the `Player` GameObject.

**Step 3** — Create a script called `CameraShake` and attach it to the `Player` GameObject:

```csharp
using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource impulseSource;
    [SerializeField] private float shakeStrength = 1f;

    public void Shake()
    {
        impulseSource.GenerateImpulse(shakeStrength);
    }
}
```

Drag the `CinemachineImpulseSource` component into the **Impulse Source** field on `CameraShake` in the Inspector.

**Step 4** — Call `cameraShake.Shake()` from `PlayerController` on the frame the player lands. Landing is detected by comparing `isGrounded` between frames — no legacy input calls needed:

```csharp
private bool wasGrounded;
private CameraShake cameraShake;

private void Awake()
{
    rb          = GetComponent<Rigidbody2D>();
    cameraShake = GetComponent<CameraShake>();

    var playerMap = inputActions.FindActionMap("Player");
    moveAction    = playerMap.FindAction("Move");
    jumpAction    = playerMap.FindAction("Jump");
}

private void Update()
{
    moveInput    = moveAction.ReadValue<Vector2>();
    jumpPressed  = jumpAction.WasPressedThisFrame();
    jumpReleased = jumpAction.WasReleasedThisFrame();

    CheckGrounded();

    // Detect the landing frame by comparing to the previous frame's grounded state
    if (isGrounded && !wasGrounded)
        cameraShake.Shake();

    wasGrounded = isGrounded;

    HandleMovement();
    HandleJump();
}
```

---

## 9. Multiple Virtual Cameras

Cinemachine supports multiple Virtual Cameras with blended transitions — for example, switching to a wider camera when the player enters a large arena.

**Step 1** — Create a second Virtual Camera (`CM Camera 2`) and configure it differently (different zoom, different target).

**Step 2** — Each Virtual Camera has a **Priority** field. The Brain activates whichever camera has the highest priority. To switch, raise the new camera's priority or disable the old one.

```csharp
using Unity.Cinemachine;
using UnityEngine;

public class CameraZoneTrigger : MonoBehaviour
{
    [SerializeField] private CinemachineCamera targetCamera;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            targetCamera.Priority = 20; // Raise above the default camera's priority
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            targetCamera.Priority = 0; // Drop back below — Brain switches back automatically
    }
}
```

**Step 3** — Under **Brain > Blend Settings**, set the **Default Blend** type to `Ease In Out` and the duration to `1` second for a smooth transition.

---

## Exercises

---

### Task 1 — Dead Zone

In the **Framing Transposer**, find the **Dead Zone** settings. Set `Dead Zone Width` and `Dead Zone Height` to a small non-zero value (e.g. `0.1`). The player can now move within this zone without the camera following, which prevents jitter on small movements.

> **Hint:** The dead zone appears as an inner rectangle in the Game view camera overlay when `CM Camera` is selected.

---

### Task 2 — Look Ahead on Jump

When the player jumps, shift the camera's `Screen Y` value upward so the player can see more above them. When they land, return it to the default value.

> **Hint:** Get a reference to the `CinemachineCamera` component. Access the Framing Transposer via `GetCinemachineComponent<CinemachineFramingTransposer>()`. Lerp `m_ScreenY` in `Update()` based on `isGrounded`.

---

### Task 3 — Room-Based Camera Zones

Create two or more `CameraZoneTrigger` zones in the level, each with its own `CinemachineCamera` framing that area. The camera should blend smoothly between them as the player moves through the level.

> **Hint:** Use `BoxCollider2D` triggers to define each zone. Give each Virtual Camera a unique starting priority so only one is active at a time.
