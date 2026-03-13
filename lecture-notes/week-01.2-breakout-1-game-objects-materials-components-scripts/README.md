# Week 01.2 — Breakout: Materials, Components & Scripts

## Navigation

|            | Link                                                                                                                                                                                        |
| ---------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| ← Previous | [Week 01.1 — GitHub, Unity & Basic Game Mathematics](ID623001-introductory-game-development/./lecture-notes/week-01.1-github-unity-basic-game-mathematics/README.md)                          |
| → Next     | [Week 02.1 — Breakout: Textures, Prefabs & Input System](ID623001-introductory-game-development/./lecture-notes/week-02.1-breakout-2-textures-prefabs-prefab-variants-input-system/README.md) |

---

## 1. Breakout

Breakout is a classic arcade game where the player controls a paddle to bounce a ball and break bricks. The goal is to clear all the bricks without letting the ball fall below the paddle.

---

## 2. Project Setup

**Step 1** — Open Unity Hub and create a new **2D Universal** project. Name it `Breakout`.

**Step 2** — In the root directory of your project, add a Unity `.gitignore` file to exclude unnecessary files from version control. Add, commit, and push the `.gitignore` file to your GitHub repository. Then add, commit, and push the Unity project files.

**Step 3** — In the `Assets` folder, create two new folders: `Materials` and `Scripts`.

---

## 3. Ball GameObject

**Step 1** — In the Hierarchy panel, right-click and select **2D Object > Sprite > Circle**. Name the GameObject `Ball`.

**Step 2** — In the `Materials` folder, right-click and select **Create > 2D > Physics Material 2D**. Name it `BallBounce`.

**Step 3** — In the Inspector panel, configure the `BallBounce` material:

| Property           | Value     | Reason                                         |
| ------------------ | --------- | ---------------------------------------------- |
| `Friction`         | `0`       | Prevents the ball from slowing down on contact |
| `Bounciness`       | `1`       | Ball rebounds at the same speed it arrived     |
| `Bounce Combine`   | `Maximum` | Ensures maximum bounciness when colliding      |
| `Friction Combine` | `Minimum` | Ensures no friction is applied on contact      |

**Step 4** — Add two components to the Ball GameObject:

| Component          | Purpose                                                         |
| ------------------ | --------------------------------------------------------------- |
| `Rigidbody2D`      | Allows the ball to be affected by physics (gravity, collisions) |
| `CircleCollider2D` | Defines the ball's shape for collision detection                |

**Step 5** — In the Inspector panel, configure the `Rigidbody2D` component:

| Property                        | Value         | Reason                                                        |
| ------------------------------- | ------------- | ------------------------------------------------------------- |
| `Gravity Scale`                 | `0`           | Prevents the ball from falling                                |
| `Collision Detection`           | `Continuous`  | Prevents the ball from passing through objects at high speeds |
| `Interpolate`                   | `Interpolate` | Smooths out the ball's movement                               |
| `Constraints > Freeze Rotation` | `true`        | Prevents the ball from spinning                               |

**Step 6** — In the Inspector panel, set the `CircleCollider2D` component's `Material` to `BallBounce`.

**Step 7** — In the `Scripts` folder, create a new script called `BallController`. Open it in your code editor and add the following:

```csharp
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float minHorizontalVelocity = 0.3f;

    [Header("Physics Settings")]
    private Rigidbody2D rb;

    private void Awake()
    {
        // Awake() runs before Start() — get component references here
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Pick a random upward direction, then launch the ball at the configured speed
        Vector2 direction = new Vector2(
            Random.Range(-1f, 1f), 1f).normalized;

        rb.linearVelocity = direction * speed;
    }

    private void FixedUpdate()
    {
        // Recalculate velocity each physics step to maintain a constant speed
        Vector2 velocity = rb.linearVelocity.normalized * speed;

        // Clamp horizontal component — prevents the ball from travelling almost vertically,
        // which would make the game unplayable
        if (Mathf.Abs(velocity.x) < minHorizontalVelocity)
        {
            velocity.x = minHorizontalVelocity * Mathf.Sign(velocity.x == 0 ? 1 : velocity.x);
            velocity = velocity.normalized * speed;
        }

        rb.linearVelocity = velocity;
    }
}
```

After attaching the script, the Inspector panel for the Ball GameObject will show a **Ball Controller** component with a **Speed** field you can adjust without editing code.

**Step 8** — Drag and drop the `BallController` script from the Project panel onto the Ball GameObject in the Hierarchy.

**Step 9** — Click the **Play** button. You should see the ball bouncing around the scene.

---

## 4. MonoBehaviour Lifecycle

Unity calls special methods on a `MonoBehaviour` script at defined points during the game's lifetime. The most important ones are:

| Method          | When it's called                                      | Typical use                                       |
| --------------- | ----------------------------------------------------- | ------------------------------------------------- |
| `Awake()`       | When the script instance is loaded — before `Start()` | Initialise component references (`GetComponent`)  |
| `Start()`       | Before the first frame update                         | Set up initial state (position, velocity, colour) |
| `Update()`      | Once per frame                                        | Input handling, non-physics movement              |
| `FixedUpdate()` | At a fixed time interval (default 50×/sec)            | Physics updates — always use this for `Rigidbody` |

> The frame rate affects `Update()` but never `FixedUpdate()`. Always apply forces and velocity changes in `FixedUpdate()` to keep physics deterministic.

📖 Reference: [Unity — MonoBehaviour](https://docs.unity3d.com/ScriptReference/MonoBehaviour.html)

---

## 5. Useful Attributes

Attributes are placed in square brackets above a field or class to change how Unity handles them.

| Attribute                       | Effect                                                                                    |
| ------------------------------- | ----------------------------------------------------------------------------------------- |
| `[SerializeField]`              | Makes a `private` field visible and editable in the Inspector, without making it `public` |
| `[Header("Label")]`             | Adds a bold label above a field in the Inspector to group related fields                  |
| `[RequireComponent(typeof(T))]` | Automatically adds component `T` if it is missing when the script is attached             |

```csharp
[RequireComponent(typeof(Rigidbody2D))]   // Unity adds Rigidbody2D automatically
public class BallController : MonoBehaviour
{
    [Header("Movement Settings")]         // Bold label in the Inspector
    [SerializeField] private float speed = 5f;  // Private but editable in Inspector
}
```

📖 Reference: [Unity — Attributes](https://docs.unity3d.com/Manual/Attributes.html)

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

### Task 1 — Refactor `Start()`

The `Start()` method currently has two responsibilities: choosing the initial direction and applying the initial velocity. Refactor it by extracting these into two separate private methods.

> **Hint:** create `GetLaunchDirection()` returning a `Vector2` and `LaunchBall(Vector2 direction)` applying the velocity, then call both from `Start()`.

---

### Task 2 — Random Ball Size

Add a serialized field to `BallController` to represent the ball's size range. In `Start()`, set the ball to a random size within that range by assigning a new `Vector3` to `transform.localScale`.

> **Hint:** `transform.localScale = new Vector3(size, size, 1f)` where `size` is a random float. Use the same value for x and y to keep the ball circular.

📖 Reference: [Unity — Transform.localScale](https://docs.unity3d.com/ScriptReference/Transform-localScale.html)

---

### Task 3 — Background Colour

Change the background colour of the scene to a colour of your choice.

> **Hint:** select the **Main Camera** GameObject in the Hierarchy and change the **Background** colour field in the Inspector panel.

📖 Reference: [Unity — Camera](https://docs.unity3d.com/Manual/class-Camera.html)

---

### Task 4 — Spawn Position

The ball always spawns at `(0, 0)`. Modify `Start()` to spawn the ball 20 units above the bottom of the screen, regardless of resolution. Use `Camera.main.ScreenToWorldPoint()` to convert screen coordinates to world coordinates.

> **Hint:** `Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, 20f, camera.nearClipPlane))` gives you a world-space point 20 screen units from the bottom centre.

📖 Reference: [Unity — Camera.ScreenToWorldPoint](https://docs.unity3d.com/ScriptReference/Camera.ScreenToWorldPoint.html)

---

### Task 5 — Random Ball Colour

The ball is currently white. In `Start()`, set the ball to a random colour each time the game starts by accessing its `SpriteRenderer` component and setting the `color` property. Ensure there is enough contrast between the ball and the background colour for the ball to be visible.

> **Hint:** `new Color(Random.value, Random.value, Random.value)` gives a random RGB colour. Access the renderer with `GetComponent<SpriteRenderer>()`.

📖 Reference: [Unity — SpriteRenderer](https://docs.unity3d.com/Manual/class-SpriteRenderer.html)
