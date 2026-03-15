# Week 04.1 - Breakout: Renderer and Particle Systems

## Navigation

|            | Link                                                                                                                                                           |
| ---------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| ← Previous | [Week 03.1 - Breakout: Audio, Scene Management, Player Prefs and Coroutines](../week-03.1-breakout-3-audio-scene-management-player-prefs-coroutines/README.md) |
| → Next     | []()                                             |

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

**Step 2** - Select the `Brick` prefab. In the `Sprite Renderer` component, set:

| Property           | Value      |
| ------------------ | ---------- |
| **Sorting Layer**  | `Gameplay` |
| **Order in Layer** | `0`        |

**Step 3** - Select the `Paddle` GameObject and apply the same settings.

**Step 4** - Select the `Ball` prefab and set **Order in Layer** to `1` so it always renders in front of bricks and the paddle.

📖 Reference: [Unity - Sprite Renderer](https://docs.unity3d.com/Manual/class-SpriteRenderer.html)

---

## 2. Particle Systems

A **Particle System** emits and simulates many small sprites or meshes to create visual effects such as explosions, sparks, smoke, or trails. Each emitted object is called a **particle** and has its own position, velocity, colour, and lifetime.

---

### 2.1 Creating a Particle System

We create the explosion prefab **first**, before writing any code that references it.

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

### 1.2 Flashing Sprites at Runtime

Now that the explosion prefab exists, we can write the `Brick` script that references it. Rather than tinting the sprite's colour (which looks wrong on coloured artwork), we swap the `SpriteRenderer`'s sprite to a plain flash sprite for a brief moment, then swap back. This works reliably regardless of what the original sprite looks like.

**Step 1** - Create a plain white sprite to use as the flash. In your art assets, add a solid white rectangle the same dimensions as your brick sprite. Import it into Unity and place it in your `Sprites` folder. Name it something like `BrickFlash`.

**Step 2** - Update the `Brick` script:

```csharp
using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour
{
    private BrickData data;
    private UIManager uiManager;
    private SpriteRenderer sr;
    private Coroutine flashCoroutine;
    private int currentHitPoints;

    [Header("Effects Settings")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private Sprite flashSprite;

    private Sprite defaultSprite;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        uiManager = FindAnyObjectByType<UIManager>();
    }

    public void Initialize(BrickData brickData)
    {
        data = brickData;
        currentHitPoints = data.hitPoints;

        if (sr != null && data.sprite != null)
        {
            sr.sprite = data.sprite;
            defaultSprite = data.sprite;
        }
    }

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

    private IEnumerator FlashRoutine()
    {
        if (flashSprite != null)
            sr.sprite = flashSprite;

        yield return new WaitForSeconds(0.1f);

        sr.sprite = defaultSprite;
    }

    private void SpawnExplosion()
    {
        if (explosionPrefab == null)
            return;

        GameObject fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        ParticleSystem ps = fx.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ParticleSystem.MainModule main = ps.main;
            main.startColor = data.particleColour;
            Destroy(fx, ps.main.duration + ps.main.startLifetime.constantMax);
        }
    }
}
```

**Step 3** - Select the `Brick` prefab in the Project panel. Drag your `BrickFlash` sprite into the **Flash Sprite** field in the Inspector.

**Step 4** - Click **Play**. Bricks with more than one hit point should briefly flash white when struck before returning to their original sprite.

---

### 2.2 Spawning Particles from Code

Rather than placing particle effects in the scene manually, we spawn them at runtime from a script using `Instantiate`, then destroy them once they finish playing.

**Step 1** - Select the `Brick` prefab in the Project panel. In the Inspector, drag the `BrickExplosion` prefab into the **Explosion Prefab** field.

**Step 2** - Click **Play**. Each brick should spawn a burst of particles when destroyed.

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
    public int pointValue = 100;
    public int hitPoints = 1;
    public Color particleColour = Color.white;
}
```

**Step 2** - `SpawnExplosion` in `Brick` already applies `data.particleColour`. For reference, the relevant lines are:

```csharp
ParticleSystem.MainModule main = ps.main;
main.startColor = data.particleColour;
```

> `ParticleSystem.MainModule` is a struct - Unity requires you to copy it into a local variable, modify it, and the assignment back to `main` updates the system automatically.

**Step 3** - In the Project panel, select each `BrickData` asset (e.g. `BlueBrickData`) and set the **Particle Colour** field to match the brick's sprite colour.

**Step 4** - Click **Play**. Each brick colour should now produce a matching explosion.

---

### 2.4 Particle Trail on the Ball

A **Trail** sub-emitter (or the built-in **Trails** module) leaves a fading path behind a moving object, giving the ball a sense of speed.

**Step 1** - In the Project panel, double-click the `Ball` prefab to open it for editing. The Hierarchy will show just the `Ball` root.

**Step 2** - Right-click the `Ball` root in the Hierarchy and select **Effects > Particle System**. Name it `BallTrail`.

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
| Shape               | (Disable module)     | -                      | Particles emit from the ball's exact centre      |
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

### Task 1 - Per-Brick Flash Sprite

Add a `flashSprite` field (`Sprite`) to `BrickData`. Update `Brick` to read `data.flashSprite` instead of using the serialized field on the prefab, so each brick type can define its own flash sprite. Assign a different flash sprite to each `BrickData` asset and verify each brick uses its own flash in Play mode.

> **Hint:** In `FlashRoutine`, replace `flashSprite` with `data.flashSprite`. You can remove the `[SerializeField] private Sprite flashSprite` field from `Brick` once `BrickData` owns it.

---

### Task 2 - Paddle Hit Effect

Create a new particle prefab called `PaddleHit` that emits a short upward spray of particles when the ball bounces off the paddle. In `BallController`, add `OnCollisionEnter2D` and spawn the prefab when the collided object is tagged `"Paddle"`.

> **Hint:** set the **Shape** module to `Edge` and orient it horizontally so particles spray upward. Use `collision.contacts[0].point` for the spawn position so the effect appears at the exact contact point.

---

### Task 3 - Speed-Based Trail Colour

Change the `BallTrail` particle colour based on the ball's current speed. In `BallController.Update`, read `rb.linearVelocity.magnitude` and map it to a colour between blue (slow) and orange (fast) using `Color.Lerp`. Apply the result to `ps.main.startColor`.

> **Hint:** `float t = Mathf.InverseLerp(minSpeed, maxSpeed, rb.linearVelocity.magnitude)` gives a 0–1 value to pass into `Color.Lerp`.

---

### Task 4 - Background Renderer

Add a background sprite to the scene using a **Sprite Renderer** on a new `Background` GameObject. Set its **Sorting Layer** to `Background` and **Order in Layer** to `0`. In a `Start` method, scale it to fill the camera's viewport using `Camera.main.orthographicSize` and `camera.aspect`.

> **Hint:** `transform.localScale = new Vector3(width, height, 1f)` where `width = camera.orthographicSize * 2f * camera.aspect` and `height = camera.orthographicSize * 2f`.

---

### Task 5 - Screen Burst Effect

Create a particle prefab called `ScreenBurst` using a **Rectangle** shape. Add a public method to `UIManager` called `ShowBurst()` that spawns it at the world origin and destroys it after it finishes playing. Call `ShowBurst()` from a button in the Canvas to test it independently of any game event.

> **Hint:** set **Emission > Burst Count** to `80` or higher. Use `Camera.main.ScreenToWorldPoint` to size the rectangle to match the screen bounds. Destroy with `Destroy(fx, ps.main.duration + ps.main.startLifetime.constantMax)`.