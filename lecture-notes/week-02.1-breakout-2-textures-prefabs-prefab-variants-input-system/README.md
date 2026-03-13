# Week 02.1 — Breakout: Textures, Prefabs & Input System

## Navigation

|            | Link                                                                                                                                                                                   |
| ---------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| ← Previous | [Week 01.2 — Breakout: Materials, Components & Scripts](https://github.com/otago-polytechnic-bit-courses/ID623002-introductory-game-development/tree/s1-26/lecture-notes/week-01.2-breakout-1-game-objects-materials-components-scripts/README.md) |
| → Next     | [Week 02.2 — Breakout: Scriptable Objects & UI](https://github.com/otago-polytechnic-bit-courses/ID623002-introductory-game-development/tree/s1-26/lecture-notes/week-02.2-breakout-2-scriptable-objects-ui/README.md)                             |

---

## 1. Walls GameObject

**Step 1** — In the Hierarchy panel, right-click and select **2D Object > Sprite > Rectangle**. Name the GameObject `Walls`. This will be the parent for all wall GameObjects in the scene.

**Step 2** — In the `Scripts` folder, create a new script called `WallController`. Open it and add the following:

```csharp
using UnityEngine;

public class WallController : MonoBehaviour
{
    [Header("Wall Settings")]
    [SerializeField] private float wallThickness = 0.5f;

    [Header("Physics Settings")]
    [SerializeField] private PhysicsMaterial2D ballBounceMaterial;

    private void Start()
    {
        Camera camera = Camera.main;

        // orthographicSize is half the screen height in world units
        float height = camera.orthographicSize * 2f;
        float width  = height * camera.aspect;

        CreateWall("Left Wall",
            new Vector2(-width / 2f - wallThickness / 2f, 0f),
            new Vector2(wallThickness, height));

        // TODO: Create the right wall and the top wall using the same pattern
    }

    private void CreateWall(string name, Vector2 position, Vector2 size)
    {
        // Create a plain GameObject — no sprite needed, just a collider
        GameObject wall      = new GameObject(name);
        wall.transform.parent   = transform;
        wall.transform.position = position;

        BoxCollider2D collider = wall.AddComponent<BoxCollider2D>();
        collider.size = size;

        // Apply the bounce material only if one has been assigned in the Inspector
        if (ballBounceMaterial != null)
            collider.sharedMaterial = ballBounceMaterial;
    }
}
```

**Step 3** — Drag and drop the `WallController` script onto the `Walls` GameObject in the Hierarchy.

**Step 4** — In the Inspector for the `Walls` GameObject, drag the `BallBounce` physics material from the `Materials` folder into the **Ball Bounce Material** field.

**Step 5** — Click **Play**. The `Walls` GameObject should now have a child called `Left Wall` with a `BoxCollider2D` component.

---

## 2. Textures

A **texture** is an image applied to a GameObject to add colour and detail. Unity represents textures with the `Texture` class — the most common variant for 2D games is `Texture2D`.

We will use sprites from **Kenney's Assets**, a collection of free, high-quality game assets.

📖 Reference: [Kenney's Assets](https://kenney.nl/assets)

**Step 1** — Copy the provided `Sprites` folder into the `Assets` folder of your Unity project.

**Step 2** — In the `Sprites` folder, select `ballBlue.png`. In the Inspector, set the following import settings and click **Apply**:

| Property            | Value               |
| ------------------- | ------------------- |
| **Sprite Mode**     | `Single`            |
| **Pixels Per Unit** | `22`                |
| **Filter Mode**     | `Point (no filter)` |
| **Max Size**        | `32`                |
| **Format**          | `RGBA 32 bit`       |

**Step 3** — In the Hierarchy, select the `Ball` GameObject. In the Inspector, drag the `ballBlue` sprite onto the **Sprite** field of the `Sprite Renderer` component.

**Step 4** — Click **Play** to verify the ball now uses the new sprite.

📖 Reference: [Unity — Textures](https://docs.unity3d.com/Manual/class-Texture.html)

---

## 3. Prefabs

A **prefab** is a reusable GameObject template. You create it once, then instantiate as many copies as you need at runtime. Any change made to the prefab is automatically reflected in all instances.

**Step 1** — In the Project panel, create a new folder called `Prefabs`.

**Step 2** — Drag the `Ball` GameObject from the Hierarchy into the `Prefabs` folder. This converts it into a prefab asset.

📖 Reference: [Unity — Prefabs](https://docs.unity3d.com/Manual/Prefabs.html)

---

## 4. Prefab Variants

A **prefab variant** inherits from a base prefab but can override specific properties. Changes made to the base prefab still flow through to all variants — unless a variant has overridden that property.

**Step 1** — In the `Prefabs` folder, right-click the `Ball` prefab and select **Create > Prefab Variant**. Name it `FastBall`.

**Step 2** — Open `FastBall` and set the **Speed** field in the `Ball Controller` component to a higher value.

**Step 3** — Create a second variant called `SlowBall` and set its **Speed** to a lower value.

📖 Reference: [Unity — Prefab Variants](https://docs.unity3d.com/Manual/PrefabVariants.html)

---

## 5. Instantiating Prefabs

Instead of placing the ball directly in the scene, we will spawn it at runtime using a script. This makes it easy to swap in different ball variants without touching the scene.

**Step 1** — Delete the `Ball` GameObject from the Hierarchy.

**Step 2** — In the `Scripts` folder, create a new script called `GameManager`. Open it and add the following:

```csharp
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Prefab Settings")]
    [SerializeField] private BallController fastBallPrefab;
    [SerializeField] private BallController slowBallPrefab;

    private void Start()
    {
        // Instantiate at the world origin with no rotation
        // Swap slowBallPrefab for fastBallPrefab to change which ball spawns
        Instantiate(slowBallPrefab, Vector2.zero, Quaternion.identity);
    }
}
```

**Step 3** — In the Hierarchy, create an empty GameObject named `GameManager`. Attach the `GameManager` script to it.

**Step 4** — In the Inspector for the `GameManager` GameObject, drag the `FastBall` prefab into **Fast Ball Prefab** and the `SlowBall` prefab into **Slow Ball Prefab**.

**Step 5** — Click **Play**. A slow ball should appear at the centre of the scene.

📖 Reference: [Unity — Instantiating Prefabs](https://docs.unity3d.com/Manual/InstantiatingPrefabs.html)

---

## 6. Paddle GameObject

**Step 1** — In the Hierarchy, right-click and select **2D Object > Sprite > Square**. Name the GameObject `Paddle`.

**Step 2** — Set the paddle's scale and position. You can do this manually in the Inspector for now, but consider doing it programmatically in `PaddleController.Start()` later.

**Step 3** — Add a `BoxCollider2D` and a `Rigidbody2D` component. Think carefully about which `Rigidbody2D` properties to set — for example, should gravity affect the paddle?

---

### 6.1 Input System

The **Input System** package provides a flexible way to handle input from keyboards, mice, gamepads, and touchscreens. You define **input actions** and bind them to controls, then respond to those actions in code.

**Step 1** — In the `Assets` folder, double-click `InputSystem_Actions` to open the Input Actions editor.

**Step 2** — Create a new action map called `Paddle`. You can delete the default `Player` and `UI` maps if you do not need them.

**Step 3** — Inside the `Paddle` map, create an action called `Move`. Set the action type to `Value` and the control type to `Axis`.

**Step 4** — Under `Move`, add a **1D positive/negative** binding. Set the positive binding to the `D` key and the negative binding to the `A` key.

**Step 5** — Save the asset and close the editor.

**Step 6** — Select the `InputSystem_Actions` asset in the Project panel. In the Inspector, click **Generate C# Class**. This creates a typed C# wrapper you can use in scripts.

**Step 7** — In the `Scripts` folder, create a script called `PaddleController`. Open it and add the following:

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PaddleController : MonoBehaviour
{
    [Header("Paddle Settings")]
    [SerializeField] private float speed = 5f;

    private Rigidbody2D rb;
    private float moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // TODO: Set the paddle's scale and starting position here
    }

    // Called by the Player Input component when the Move action fires
    // context.ReadValue<float>() returns -1 (left), 0 (none) or 1 (right)
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<float>();
    }

    private void FixedUpdate()
    {
        // MovePosition respects physics — safer than setting transform.position directly
        Vector2 position = rb.position;
        position.x += moveInput * speed * Time.fixedDeltaTime;
        rb.MovePosition(position);
    }
}
```

**Step 8** — Attach the `PaddleController` script to the `Paddle` GameObject.

**Step 9** — In the Inspector for the `Paddle` GameObject, add a **Player Input** component. Configure it as follows:

| Property     | Value                 |
| ------------ | --------------------- |
| **Actions**  | `InputSystem_Actions` |
| **Behavior** | `Invoke Unity Events` |

Expand **Events > Paddle > Move**. Click **+** to add a listener. Drag the `Paddle` GameObject into the object field and select **PaddleController > OnMove** from the function dropdown.

**Step 10** — Click **Play**. You should be able to move the paddle left and right with `A` and `D`.

📖 Reference: [Unity — Input System](https://docs.unity3d.com/Manual/com.unity.inputsystem.html)

---

## 7. Tags

**Tags** label GameObjects so scripts can identify them without relying on names. For example, the `Brick` script uses the `Ball` tag to check what it collided with.

**Step 1** — Go to **Edit > Project Settings > Tags and Layers**. In the **Tags** section, click **+** and add a tag named `Ball`.

**Step 2** — Select the `Ball` prefab in the Project panel. In the Inspector, set the **Tag** field to `Ball`.

---

## 8. Collision Detection

Unity's physics engine detects when two colliders make contact and calls `OnCollisionEnter2D` (or `OnTriggerEnter2D` for triggers) on the involved GameObjects. We use this to destroy a brick when the ball hits it.

**Step 1** — In the Hierarchy, right-click and select **2D Object > Sprite > Square**. Name it `Brick`.

**Step 2** — Add a `BoxCollider2D` component to the `Brick` GameObject.

**Step 3** — In the `Scripts` folder, create a script called `Brick`. Open it and add the following:

```csharp
using UnityEngine;

public class Brick : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // CompareTag is faster than comparing tag strings directly
        // and avoids typos causing silent failures
        if (collision.gameObject.CompareTag("Ball"))
            Destroy(gameObject);
    }
}
```

**Step 4** — Attach the `Brick` script to the `Brick` GameObject.

**Step 5** — Click **Play** and verify the brick is destroyed when the ball hits it.

**Step 6** — Drag the `Brick` GameObject from the Hierarchy into the `Prefabs` folder to create a prefab, then delete the original from the Hierarchy.

📖 Reference: [Unity — Physics 2D](https://docs.unity3d.com/Manual/Physics2DReference.html)

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

### Task 1 — Paddle Prefab Variants

Create two new prefab variants of the `Paddle` GameObject, each with a different sprite and speed:

| Variant      | Sprite                  | Speed               |
| ------------ | ----------------------- | ------------------- |
| `FastPaddle` | A sprite of your choice | Higher than default |
| `SlowPaddle` | A different sprite      | Lower than default  |

Update `GameManager` to expose a **Paddle Prefab** field and instantiate one of the new variants at the start of the game.

> **Hint:** position the paddle near the bottom of the screen using `Camera.main.ScreenToWorldPoint()` — the same technique used in Week 01.2 Task 4.

---

### Task 2 — Game Input Actions

Add three new input actions to the `Paddle` action map and implement them in `GameManager`:

| Action        | Key     | Behaviour                                  |
| ------------- | ------- | ------------------------------------------ |
| `Start Game`  | `Space` | Instantiate the ball prefab and begin play |
| `Pause Game`  | `P`     | Pause by setting `Time.timeScale = 0`      |
| `Resume Game` | `R`     | Resume by setting `Time.timeScale = 1`     |

> **Hint:** set each action's type to `Button`. Wire them to `GameManager` via **Player Input > Invoke Unity Events**, the same way `OnMove` is wired to `PaddleController`.
