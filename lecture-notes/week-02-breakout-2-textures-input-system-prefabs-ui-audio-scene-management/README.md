# Week 02

---

## Important Links

| Section    | Link                                                                                            |
| ---------- | ----------------------------------------------------------------------------------------------- |
| Next Class | [Week 03](lecture-notes/week-03-breakout-3-scriptable-objects-player-preferences-build-release) |

---

## Ball Game Object

---

### Textures

> Resource: [Unity Manual: Textures](https://docs.unity3d.com/Manual/class-Texture.html)

---

### Sprite Atlas

> Resource: [Unity Manual: Sprite Atlas](https://docs.unity3d.com/Manual/class-SpriteAtlas.html)

---

### Kenny's Assets

> Resource: [Kenney's Assets](https://kenney.nl/assets)

---

## Walls Game Objects

1. In the Hierarchy panel, right-click and select "2D Object > Sprite > Rectangle". Name the GameObject "Walls". This will be the parent GameObject for all the wall GameObjects in the scene.
2. In the Scripts folder, right-click and select "Create > MonoBehaviour Script". Name it "WallsController". Double-click the script to open it in your code editor, e.g., Microsoft Visual Studio or Microsoft Visual Studio Code. Add the following code:

```csharp
using UnityEngine;

public class WallController : MonoBehaviour
{
    [SerializeField] private float wallThickness = 0.5f;
    [SerializeField] private PhysicsMaterial2D ballBounceMaterial;

    private void Start()
    {
        Camera camera = Camera.main;

        float height = camera.orthographicSize * 2f;
        float width = height * camera.aspect;

        CreateWall("Left Wall",
            new Vector2(-width / 2f - wallThickness / 2f, 0f),
            new Vector2(wallThickness, height));

        // TODO: Create the right, top and bottom walls
    }

    private void CreateWall(string name, Vector2 position, Vector2 size)
    {
        GameObject wall = new GameObject(name); // Child of the Walls GameObject
        wall.transform.parent = transform;
        wall.transform.position = position;

        BoxCollider2D collider = wall.AddComponent<BoxCollider2D>();
        collider.size = size;

        if (ballBounceMaterial != null)
        {
            collider.sharedMaterial = ballBounceMaterial;
        }
    }
}
```

3. Drag and drop the `WallsController` script from the Project panel onto the Walls GameObject in the Hierarchy panel.
4. In the Inspector panel for the Walls GameObject, you should see a new component called "Wall Controller" with a "Wall Thickness" field and a "Ball Bounce Material" field. Drag and drop the "BallBounce" physics material from the Materials folder into the "Ball Bounce Material" field.
5. Click the Play button at the top of the Unity Editor to run the game. In the Hierarchy panel, you should see that the Walls GameObject has a child GameObject called "Left Wall" with a BoxCollider2D component. 

---

## Paddle Game Object

---

### Input System

> Resource: [Unity Manual: Input System](https://docs.unity3d.com/Manual/com.unity.inputsystem.html)

---

## Bricks Game Objects

---

### Prefabs

> Resource: [Unity Manual: Prefabs](https://docs.unity3d.com/Manual/Prefabs.html)

---

### Prefab Variants

> Resource: [Unity Manual: Prefab Variants](https://docs.unity3d.com/Manual/PrefabVariants.html)

---

## UI and Audio

---

### UI

> Resource: [Unity Manual: UI](https://docs.unity3d.com/Manual/UISystem.html)

---

### Audio

> Resource: [Unity Manual: Audio](https://docs.unity3d.com/Manual/Audio.html)

---

## Scene Management

> Resource: [Unity Manual: Scene Management](https://docs.unity3d.com/Manual/SceneManagement.html)

---

## Exercises

Learning to use AI tools is an important skill. While AI tools are powerful, you must be aware of the following:

- If you provide an AI tool with a prompt that is not refined enough, it may generate a not-so-useful response
- Do not trust the AI tool's responses blindly. You must still use your judgement and may need to do additional research to determine if the response is correct
- Acknowledge what AI tool you have used. If you use AI to help you with a file, include an XML doc comment at the top of the file

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

### Task 1

---

### Task 2

---

### Task 3

---

### Task 4

---

### Task 5
