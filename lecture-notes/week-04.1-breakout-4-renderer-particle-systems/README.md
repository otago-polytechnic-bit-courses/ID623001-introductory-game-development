# Week 04.1 - Breakout: Renderer and Particle Systems

## Navigation

|            | Link                                                                                                                                                           |
| ---------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| ← Previous | [Week 03.1 - Breakout: Audio, Scene Management, Player Prefs and Coroutines](../week-03.1-breakout-3-audio-scene-management-player-prefs-coroutines/README.md) |
| → Next     | [Week 04.2 - Breakout: Optimisation, Build and Itch.io](../week-04.2-breakout-4-optimisation-build-itch.io/README.md)                                          |

---

## 1. Renderer

The **Sprite Renderer** component controls how a 2D sprite appears on screen - including its colour, sorting layer, and material. By accessing it through code, you can dynamically change a sprite's appearance at runtime, for example to tint a brick when it takes a hit or flash the paddle when the ball passes.

---

### 1.1 Sorting Layers and Order in Layer

Unity renders 2D sprites in a defined order. **Sorting Layers** are named groups (e.g. `Background`, `Gameplay`, `UI`) and **Order in Layer** is a number within that group - higher numbers render on top.

**Step 1** - Go to **Edit > Project Settings > Tags and Layers**. In the **Sorting Layers** section, click **+** and add the following layers in order:

| Layer      |
| ---------- |
| Background |
| Gameplay   |
| Effects    |

**Step 2** - In the Hierarchy, select the `Brick` prefab. In the `Sprite Renderer` component, set:

| Property           | Value      |
| ------------------ | ---------- |
| **Sorting Layer**  | `Gameplay` |
| **Order in Layer** | `0`        |

**Step 3** - Select the `Paddle` GameObject and apply the same settings.

**Step 4** - Select the `Ball` prefab and set **Order in Layer** to `1` so it always renders in front of bricks and the paddle.

📖 Reference: [Unity - Sprite Renderer](https://docs.unity3d.com/Manual/class-SpriteRenderer.html)

---

### 1.2 Tinting Sprites at Runtime

You can read and write the `color` property on a `SpriteRenderer` to tint a sprite without replacing it. A white sprite tinted with `Color.red` appears red; tinting with `Color.white` restores the original appearance.

**Step 1** - Update the `Brick` script to flash the sprite when it is hit but not yet destroyed:

```csharp
using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour
{
    // Omitted for brevity

    private SpriteRenderer sr;
    private Coroutine flashCoroutine;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ball"))
            return;

        currentHitPoints--;

        if (currentHitPoints <= 0)
        {
            uiManager?.AddScore(data.pointValue);
            Destroy(gameObject);
        }
        else
        {
            // Cancel any in-progress flash before starting a new one
            if (flashCoroutine != null)
                StopCoroutine(flashCoroutine);

            flashCoroutine = StartCoroutine(FlashRoutine());
        }
    }

    private IEnumerator FlashRoutine()
    {
        sr.color = Color.white * new Color(1f, 0.3f, 0.3f);   // tint red
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;                                // restore
    }
}
```

**Step 2** - Click **Play**. Bricks with more than one hit point should briefly flash red when struck before turning white again.

---

### 1.3 Materials and Shaders

A **Material** defines how a surface is rendered - which shader to use and what parameters (colour, texture, etc.) to pass to it. In 2D games the default **Sprite-Lit-Default** shader is sufficient for most cases, but swapping to a custom material lets you add effects like outlines or glow.

**Step 1** - In the `Materials` folder, right-click and select **Create > 2D > Lit > Sprite Lit Default**. Name it `BrickMaterial`.

**Step 2** - Select the `Brick` prefab. In the `Sprite Renderer` component, set the **Material** field to `BrickMaterial`.

**Step 3** - In the `Scripts` folder, create a script called `RendererController`. Open it and add the following:

```csharp
using UnityEngine;

/// <summary>
/// Demonstrates runtime material property changes on a SpriteRenderer.
/// Attach to any GameObject with a SpriteRenderer.
/// </summary>
public class RendererController : MonoBehaviour
{
    [Header("Renderer Settings")]
    [SerializeField] private Color highlightColour = Color.yellow;
    [SerializeField] private Color defaultColour   = Color.white;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Call this from other scripts or UI events to highlight the object
    public void SetHighlight(bool on)
    {
        sr.color = on ? highlightColour : defaultColour;
    }
}
```

📖 Reference: [Unity - Materials](https://docs.unity3d.com/Manual/Materials.html)

---

## 2. Particle Systems

A **Particle System** emits and simulates many small sprites or meshes to create visual effects such as explosions, sparks, smoke, or trails. Each emitted object is called a **particle** and has its own position, velocity, colour, and lifetime.

---

### 2.1 Creating a Particle System

**Step 1** - In the Hierarchy, right-click and select **Effects > Particle System**. Name it `BrickExplosion`. Unity creates a looping burst of particles by default.

**Step 2** - In the Inspector, configure the **Particle System** component. Each collapsible section below is a **module**:

| Module              | Property           | Value                                        | Reason                                       |
| ------------------- | ------------------ | -------------------------------------------- | -------------------------------------------- |
| Main                | **Duration**       | `0.5`                                        | Short burst rather than a sustained effect   |
| Main                | **Loop**           | `false`                                      | The effect plays once and stops              |
| Main                | **Start Lifetime** | `0.4 – 0.8` (Random Between Two Constants)   | Particles fade at different times            |
| Main                | **Start Speed**    | `3 – 6` (Random Between Two Constants)       | Varies the burst spread                      |
| Main                | **Start Size**     | `0.05 – 0.15` (Random Between Two Constants) | Small chips of varying size                  |
| Main                | **Start Color**    | A colour of your choice                      | Matches the brick sprite                     |
| Emission            | **Rate over Time** | `0`                                          | We use bursts instead of a continuous stream |
| Emission > Burst    | **Count**          | `12`                                         | Number of particles per burst                |
| Shape               | **Shape**          | `Rectangle`                                  | Particles originate from the brick's face    |
| Shape               | **Scale**          | Match brick width and height                 | Spread across the whole brick                |
| Color over Lifetime | (Enable module)    | Fade alpha from `255` to `0`                 | Particles fade out gracefully                |
| Renderer            | **Sorting Layer**  | `Effects`                                    | Renders on top of gameplay sprites           |

**Step 3** - Click the **Play** button in the Particle System preview at the bottom of the Inspector to preview the effect.

**Step 4** - Drag the `BrickExplosion` GameObject from the Hierarchy into the `Prefabs` folder to create a prefab. Delete the original from the Hierarchy.

📖 Reference: [Unity - Particle System](https://docs.unity3d.com/Manual/class-ParticleSystem.html)

---

### 2.2 Spawning Particles from Code

Rather than placing particle effects in the scene manually, we spawn them at runtime from a script using `Instantiate`, then destroy them once they finish playing.

**Step 1** - Update `Brick` to spawn the explosion effect when it is destroyed:

```csharp
using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour
{
    // Omitted for brevity

    [Header("Effects")]
    [SerializeField] private GameObject explosionPrefab;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ball"))
            return;

        currentHitPoints--;

        if (currentHitPoints <= 0)
        {
            SpawnExplosion();
            uiManager?.AddScore(data.pointValue);
            Destroy(gameObject);
        }
        else
        {
            if (flashCoroutine != null)
                StopCoroutine(flashCoroutine);

            flashCoroutine = StartCoroutine(FlashRoutine());
        }
    }

    private void SpawnExplosion()
    {
        if (explosionPrefab == null)
            return;

        // Spawn at the brick's position with no rotation
        GameObject fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Destroy the effect GameObject after it has finished playing
        ParticleSystem ps = fx.GetComponent<ParticleSystem>();
        if (ps != null)
            Destroy(fx, ps.main.duration + ps.main.startLifetime.constantMax);
    }

    // Omitted for brevity
}
```

**Step 2** - Select the `Brick` prefab in the Project panel. In the Inspector, drag the `BrickExplosion` prefab into the **Explosion Prefab** field.

**Step 3** - Click **Play**. Each brick should spawn a burst of particles when destroyed.

---

### 2.3 Matching Particle Colour to Brick Data

To make the explosion colour match each brick type, read the colour from the `BrickData` Scriptable Object and apply it to the spawned `ParticleSystem` at runtime.

**Step 1** - Add a `particleColour` field to `BrickData`:

```csharp
using UnityEngine;

[CreateAssetMenu(fileName = "BrickData", menuName = "Scriptable Objects/BrickData")]
public class BrickData : ScriptableObject
{
    public Sprite sprite;
    public int    pointValue;
    public int    hitPoints;
    public Color  particleColour = Color.white;   // new field
}
```

**Step 2** - Update `SpawnExplosion` in `Brick` to apply the colour:

```csharp
private void SpawnExplosion()
{
    if (explosionPrefab == null)
        return;

    GameObject fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

    ParticleSystem ps = fx.GetComponent<ParticleSystem>();

    if (ps != null)
    {
        // ParticleSystem.MainModule is a struct - must modify a local copy then assign it back
        ParticleSystem.MainModule main = ps.main;
        main.startColor = data.particleColour;

        Destroy(fx, ps.main.duration + ps.main.startLifetime.constantMax);
    }
}
```

**Step 3** - In the Project panel, select each `BrickData` asset (e.g. `BlueBrickData`) and set the **Particle Colour** field to match the brick's sprite colour.

**Step 4** - Click **Play**. Each brick colour should now produce a matching explosion.

---

### 2.4 Particle Trail on the Ball

A **Trail** sub-emitter (or the built-in **Trails** module) leaves a fading path behind a moving object, giving the ball a sense of speed.

**Step 1** - Select the `Ball` prefab in the Project panel and open it for editing.

**Step 2** - In the Hierarchy (with the prefab open), right-click the `Ball` root and select **Effects > Particle System**. Name it `BallTrail`.

**Step 3** - Configure the `BallTrail` Particle System:

| Module              | Property             | Value                  | Reason                                           |
| ------------------- | -------------------- | ---------------------- | ------------------------------------------------ |
| Main                | **Duration**         | `1`                    | Trail refreshes continuously                     |
| Main                | **Loop**             | `true`                 | Runs for the lifetime of the ball                |
| Main                | **Start Lifetime**   | `0.15`                 | Particles disappear quickly                      |
| Main                | **Start Speed**      | `0`                    | Trail particles should not move on their own     |
| Main                | **Start Size**       | `0.15`                 | Slightly smaller than the ball                   |
| Main                | **Simulation Space** | `World`                | Particles stay at the position they were emitted |
| Emission            | **Rate over Time**   | `40`                   | Dense trail                                      |
| Shape               | (Disable module)     | —                      | Particles emit from the ball's exact centre      |
| Color over Lifetime | (Enable)             | Fade alpha `255` → `0` | Trail fades behind the ball                      |
| Renderer            | **Sorting Layer**    | `Effects`              | Renders above gameplay sprites                   |

**Step 4** - Save the prefab. Click **Play** and verify the ball leaves a fading trail.

📖 Reference: [Unity - Particle System Trails](https://docs.unity3d.com/Manual/PartSysTrailsModule.html)

---

## Exercises

Learning to use AI tools is an important skill. While AI tools are powerful, you must be aware of the following:

- Refine your prompts - vague prompts yield vague responses
- Validate AI output - don't trust it blindly
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

### Task 1 - Hit Tint from BrickData

Currently all bricks flash the same hard-coded red. Extend `BrickData` with a `hitColour` field (`Color`) and update `FlashRoutine` in `Brick` to use `data.hitColour` instead. Assign a different hit colour to each `BrickData` asset and verify each brick flashes its own colour.

> **Hint:** `sr.color = data.hitColour;` inside `FlashRoutine`, then `sr.color = Color.white;` to restore.

---

### Task 2 - Paddle Hit Effect

Create a new particle prefab called `PaddleHit` that emits a short upward spray of particles when the ball bounces off the paddle. Spawn it from `BallController.OnCollisionEnter2D` when the collided object is tagged `"Paddle"`.

> **Hint:** set the **Shape** module to `Edge` and orient it horizontally so particles spray upward. Use `collision.contacts[0].point` for the spawn position so the effect appears at the exact contact point.

---

### Task 3 - Speed-Based Trail Colour

Change the `BallTrail` particle colour based on the ball's current speed. Use the `Gradient` mode in the **Start Color** field at runtime by accessing `ps.main.startColor` in `BallController.Update`. Map a slow speed to blue and a high speed to orange using `Color.Lerp`.

> **Hint:** `float t = Mathf.InverseLerp(minSpeed, maxSpeed, rb.linearVelocity.magnitude)` gives a 0–1 value you can pass into `Color.Lerp(slowColour, fastColour, t)`.

---

### Task 4 - Background Renderer

Add a background image to the scene using a **Sprite Renderer** on a new `Background` GameObject. Set its **Sorting Layer** to `Background` and **Order in Layer** to `0` so it always renders behind all other objects. Scale it to fill the camera's viewport using `Camera.main.orthographicSize` and `camera.aspect` in a `Start` method.

> **Hint:** `transform.localScale = new Vector3(width, height, 1f)` where `width = camera.orthographicSize * 2f * camera.aspect` and `height = camera.orthographicSize * 2f`.

---

### Task 5 - Game Over Particle Burst

When the player loses their last life, trigger a large particle burst across the whole screen. Create a new particle prefab called `GameOverBurst` using a **Rectangle** shape scaled to fill the screen. Spawn it from `UIManager.ShowGameOver()` at the world origin. Ensure the burst uses the `Effects` sorting layer and is destroyed after it finishes.

> **Hint:** set **Emission > Burst Count** to `80` or higher. Use `Camera.main.ScreenToWorldPoint` to calculate the correct rectangle scale to match the screen bounds.
