# Week 05.2 — Spritesheets and Sorting Layers

In this module you will set up the rogue-like project: import and slice a spritesheet, configure sorting layers, add physics components, and write the `PlayerController` script.

---

## 1. Project Setup

Create a new Unity project named `rogue-like`. In the **lecture-notes > rogue-like** folder, find the zip folder called `assets`. Download and extract it, then copy the folders into the `Assets` folder of your Unity project.

---

## 2. Spritesheet

A **spritesheet** is a collection of images combined into a single image file. It reduces draw calls, which can improve performance.

**Step 1** — In the **Assets > Art > Characters** folder, click on the `Characters` spritesheet. In the Inspector, configure the following settings:

| Property          | Value               | Reason                                                      |
| ----------------- | ------------------- | ----------------------------------------------------------- |
| `Sprite Mode`     | `Multiple`          | Allows the spritesheet to be sliced into individual sprites |
| `Pixels Per Unit` | `16`                | Matches the pixel art scale to one Unity world unit         |
| `Filter Mode`     | `Point (no filter)` | Prevents blurring on pixel art sprites                      |
| `Max Size`        | `64`                | Scales down the texture if it exceeds this size             |

**Step 2** — Click the **Sprite Editor** button. In the Slice popup, set `Type` to `Grid By Cell Size` and `Pixel Size` to `16`. Click **Slice**, then **Apply**.

**Step 3** — Drag a character sprite into the Scene. Drag a gun sprite in as well. Create an empty GameObject named `Player` and make both sprites its children.

> You do not need to slice the gun spritesheet — the gun sprites are already ready to use.

---

## 3. Sorting Layers

**Sorting Layers** determine the render order of sprites. Sprites on a higher sorting layer appear in front of sprites on a lower layer.

**Step 1** — Create a new sorting layer called `Player`. You should now have two sorting layers: `Default` and `Player`.

**Step 2** — Select the character sprite child of the `Player` GameObject. Set **Sprite Renderer > Additional Settings > Sorting Layer** to `Player` and **Order in Layer** to `0`.

**Step 3** — Select the gun sprite child. Apply the same `Player` sorting layer, but set **Order in Layer** to `1`. This ensures the gun renders on top of the character sprite.

---

## 4. Physics Components

**Step 1** — Select the `Player` GameObject. Add a `CircleCollider2D` component to define the player's collision shape.

**Step 2** — Add a `Rigidbody2D` component. Configure it as follows:

| Property        | Value     | Reason                           |
| --------------- | --------- | -------------------------------- |
| `Gravity Scale` | `0`       | Prevents the player from falling |
| `Body Type`     | `Dynamic` | Allows physics-driven movement   |

---

## 5. PlayerController Script

Create a `Scripts` folder. Inside it, create a `PlayerController` script and attach it to the `Player` GameObject.

`PlayerController` uses Unity's **Input System** (`InputActionAsset`) for movement. It exposes a public `rb` property so other scripts such as `EnemyNavigator` can read the player's physics position directly without an additional `GetComponent` call.

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField] private float speed = 5f;
    [SerializeField] private InputActionAsset inputActions;

    public Rigidbody2D rb { get; private set; }
    private Vector2 moveInput;
    private InputAction moveAction;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        moveAction = inputActions.FindActionMap("Player").FindAction("Move");
    }

    private void OnEnable()  => moveAction.Enable();
    private void OnDisable() => moveAction.Disable();

    private void FixedUpdate()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * moveInput);
    }

    public void TakeDamage(float amount)
    {
        // Hook up to your health system here.
        Debug.Log($"Player took {amount} damage");
    }
}
```

In the Inspector, assign your `InputActionAsset` to the **Input Actions** field on the `PlayerController` component.

---

## 6. MonoBehaviour Lifecycle

Unity calls special methods on a `MonoBehaviour` at defined points during the game's lifetime:

| Method          | When it is called                       | Typical use                             |
| --------------- | --------------------------------------- | --------------------------------------- |
| `Awake()`       | When the script loads, before `Start()` | Initialise component references         |
| `Start()`       | Before the first frame update           | Set up initial state                    |
| `Update()`      | Once per frame                          | Input handling, non-physics movement    |
| `FixedUpdate()` | At a fixed interval (default 50×/sec)   | Physics — always use this for Rigidbody |

> Always apply forces and movement in `FixedUpdate()` to keep physics deterministic. The frame rate affects `Update()` but never `FixedUpdate()`.

---

## Exercises

Learning to use AI tools is an important skill. While AI tools are powerful, you must be aware of the following:

- Refine your prompts — vague prompts yield vague responses
- Validate AI output — don't trust it blindly
- Acknowledge AI usage at the top of any AI-assisted file:

```csharp
/// <summary>
/// Brief description of what this script does.
/// </summary>
/// <remarks>
/// AI-Assisted: This file was developed with assistance from [AI Tool Name]
/// Prompts:
///   - "Your first prompt here"
///   - "Your second prompt here"
/// Usage: Describe how you used the AI responses.
/// </remarks>
```

---

### Task 1 — Player Movement

Configure the player to move using either the **WASD** keys or the **Arrow** keys.

---

### Task 2 — Dash

Write the code to dash the player using the **Space** key. The player should dash in the direction it is currently moving. The dash should last `0.5` seconds and have a cooldown of `2` seconds.

> The player should not be able to dash again until the cooldown has expired.

> **Hint:** Use a `Coroutine` to handle the dash duration and cooldown. Use a `bool` flag to track whether a dash is available.

📖 Reference: [Unity — Coroutines](https://docs.unity3d.com/Manual/Coroutines.html)

---

### Task 3 — Weapon Swap

Write the code to swap between two weapons using the **Q** key. Pressing **Q** should toggle which weapon GameObject is active using `SetActive(true)` / `SetActive(false)`.