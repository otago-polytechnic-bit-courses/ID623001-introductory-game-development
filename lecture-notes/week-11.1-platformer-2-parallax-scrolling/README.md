# Week 11.1 — Platformer: Parallax Scrolling

**Parallax scrolling** is a technique where background layers move at different speeds relative to the camera, creating the illusion of depth. Layers that are further away move slowly; layers that are closer move faster. It is one of the most effective ways to give a 2D game a sense of three-dimensional space without any 3D geometry.

---

## 1. How This Fits

Parallax is handled by a `ParallaxLayer` script attached to each background layer GameObject. Each frame, the script reads how much the camera has moved and shifts the layer by a fraction of that amount, determined by a `parallaxFactor` you set per layer in the Inspector.

| Script          | Where it goes         | What it does                                           |
| --------------- | --------------------- | ------------------------------------------------------ |
| `ParallaxLayer` | Each background layer | Moves the layer relative to camera movement each frame |

---

## 2. The Problem

A static background makes a 2D game feel flat — everything appears to be on the same plane. Parallax exploits how our eyes perceive depth: when you move your head, nearby objects shift quickly across your vision while distant objects barely move. Replicating this in 2D creates a strong sense of distance and atmosphere.

---

## 3. Scene Setup

**Step 1** — In this assset pack - [Parallex Background Plains](https://vvvka.itch.io/parallex-background-plains), you should have several layers — for example: sky, far mountains, near mountains, trees, and foreground bushes. Each should be a wide sprite that spans the visible width of the camera.

**Step 2** — In the Hierarchy, create an empty GameObject called `Background`. Create a child GameObject for each layer and name them clearly:

```
Background
├── clouds
├── forrest
├── grass
├── hill
├── plains
└── sky
```

**Step 3** — Add a `SpriteRenderer` to each layer child and assign the corresponding sprite. Stack them in the Scene view from back to front using their Z position or **Sorting Layers**.

> Set up a `Background` sorting layer and give each layer an **Order in Layer** value — `0` for the furthest back, increasing toward the front.

---

## 4. ParallaxLayer Script

Create a new script called `ParallaxLayer` in the `Scripts` folder and attach it to each background layer child GameObject.

```csharp
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] private float parallaxFactor;
    [SerializeField] private bool parallaxVertical = true;

    private Transform cam;
    private Vector3 lastCameraPosition;

    private void Start()
    {
        cam = Camera.main.transform;
        lastCameraPosition = cam.position;
    }

    private void LateUpdate()
    {
        // How far the camera moved this frame
        Vector3 deltaMovement = cam.position - lastCameraPosition;

        float yShift = parallaxVertical ? deltaMovement.y * parallaxFactor : 0f;
        transform.position += new Vector3(deltaMovement.x * parallaxFactor, yShift, 0f);

        lastCameraPosition = cam.position;
    }
}
```

What is happening in this script?

| Element              | Purpose                                                                                                             |
| -------------------- | ------------------------------------------------------------------------------------------------------------------- |
| `parallaxFactor`     | Controls how much this layer moves relative to the camera. `0` = fixed; `1` = moves with camera; between = parallax |
| `parallaxVertical`   | Toggle to disable vertical parallax on layers like a sky gradient that should not shift up and down                 |
| `lastCameraPosition` | Stores the camera's position from the previous frame to calculate how far it moved                                  |
| `deltaMovement`      | The camera's movement this frame — the layer moves by this scaled by `parallaxFactor`                               |
| `LateUpdate()`       | Runs after all other `Update()` calls, ensuring the camera has finished moving before the layer repositions         |

> **Why LateUpdate?** If Cinemachine is used (from Week 10.2), it updates the camera position during its own late update pass. Using `LateUpdate` here ensures the parallax layers always read the camera's final position for that frame rather than its position before Cinemachine has moved it.
>
> In the Cinemachine Brain Inspector, confirm **Update Method** is set to `Late Update` to keep everything in sync.

---

## 5. Choosing Parallax Factors

The `parallaxFactor` for each layer should decrease as the layer gets further from the camera. A layer that does not move at all gets `0`. A layer just behind the foreground tiles gets a value close to `1`.

| Layer               | Example parallaxFactor | Effect                                   |
| ------------------- | ---------------------- | ---------------------------------------- |
| sky and clouds | `0.0`                  | Does not move — feels infinitely distant |
| hill       | `0.1`                  | Barely drifts — very far away            |
| forrest      | `0.3`                  | Slow drift — mid distance                |
| plains               | `0.5`                  | Noticeable movement — close background   |
| grass   | `0.8`                  | Fast movement — just behind the player   |

Select each layer in the Hierarchy and set the `parallaxFactor` on its `ParallaxLayer` component in the Inspector.

---

## 6. Infinite Scrolling

For levels that scroll horizontally for long distances, a single wide sprite will eventually run out. **Infinite scrolling** tiles the background by snapping the sprite back into position once it has drifted far enough.

Update `ParallaxLayer` to support tiling:

```csharp
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] private float parallaxFactor;
    [SerializeField] private bool parallaxVertical = true;
    [SerializeField] private bool infiniteHorizontal = true;

    private Transform cam;
    private Vector3 lastCameraPosition;
    private float spriteWidth;

    private void Start()
    {
        cam = Camera.main.transform;
        lastCameraPosition = cam.position;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            spriteWidth = sr.bounds.size.x;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = cam.position - lastCameraPosition;

        float yShift = parallaxVertical ? deltaMovement.y * parallaxFactor : 0f;
        transform.position += new Vector3(deltaMovement.x * parallaxFactor, yShift, 0f);

        lastCameraPosition = cam.position;

        if (infiniteHorizontal && spriteWidth > 0f)
        {
            float distanceFromCamera = cam.position.x - transform.position.x;

            // If the layer has drifted more than half its width from the camera, snap it back
            if (Mathf.Abs(distanceFromCamera) >= spriteWidth * 0.5f)
            {
                float snapAmount = distanceFromCamera > 0 ? spriteWidth : -spriteWidth;
                transform.position += new Vector3(snapAmount, 0f, 0f);
            }
        }
    }
}
```

> For the infinite scroll to look seamless, the background sprite must tile cleanly — its left and right edges must match. Enable **Wrap Mode > Repeat** on the texture in the Inspector, or use a sprite that is specifically designed to loop.

---

## 7. Foreground Layers

A `parallaxFactor` greater than `1` makes a layer move **faster** than the camera, simulating objects very close to the viewer — such as tall grass, vines, or cave stalactites overlaid in front of the player.

Set the sorting order of these layers **above** the player sprite so they render in front, and ensure the sprite has gaps or is semi-transparent so the player remains visible.

---

## 8. Auto-Scrolling Layers

Some layers should scroll automatically regardless of camera movement — for example, clouds drifting in the wind. Add a `scrollSpeed` field and apply it each frame alongside the parallax offset:

```csharp
[SerializeField] private float scrollSpeed = 0f;

private void LateUpdate()
{
    Vector3 deltaMovement = cam.position - lastCameraPosition;

    float yShift = parallaxVertical ? deltaMovement.y * parallaxFactor : 0f;

    // Auto-scroll is added on top of the parallax offset
    float xShift = deltaMovement.x * parallaxFactor + scrollSpeed * Time.deltaTime;

    transform.position += new Vector3(xShift, yShift, 0f);

    lastCameraPosition = cam.position;

    if (infiniteHorizontal && spriteWidth > 0f)
    {
        float distanceFromCamera = cam.position.x - transform.position.x;
        if (Mathf.Abs(distanceFromCamera) >= spriteWidth * 0.5f)
        {
            float snapAmount = distanceFromCamera > 0 ? spriteWidth : -spriteWidth;
            transform.position += new Vector3(snapAmount, 0f, 0f);
        }
    }
}
```

Set `scrollSpeed` to a small positive value (e.g. `0.5`) in the Inspector and enable `infiniteHorizontal` so the layer loops seamlessly.

---

## 9. Inspector Summary

Each `ParallaxLayer` component exposes the following fields:

| Field                 | Type    | Description                                                                        |
| --------------------- | ------- | ---------------------------------------------------------------------------------- |
| `Parallax Factor`     | `float` | How much this layer moves relative to the camera (0 = fixed, 1 = locked to camera) |
| `Parallax Vertical`   | `bool`  | Whether to apply vertical parallax (disable for sky gradients)                     |
| `Infinite Horizontal` | `bool`  | Whether to loop the layer horizontally                                             |
| `Scroll Speed`        | `float` | Automatic horizontal scroll per second (0 = no auto-scroll)                        |

