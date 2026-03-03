# Week 02.1

---

## Important Links

| Section        | Link                                                                                                                |
| -------------- | ------------------------------------------------------------------------------------------------------------------- |
| Previous Class | [Week 01.2](lecture-notes/week-01.2-breakout-1-game-objects-materials-components-scripts/README.md)                 |
| Next Class     | [Week 02.2](lecture-notes/week-02.2-breakout-2-scriptable-objects-ui-audio-scene-management-player-prefs/README.md) |

---

## Walls Game Objects

1. In the Hierarchy panel, right-click and select "2D Object > Sprite > Rectangle". Name the GameObject "Walls". This will be the parent GameObject for all the wall GameObjects in the scene.
2. In the Scripts folder, create a new script called "WallController". Open the script in your code editor and add the following code:

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

        float height = camera.orthographicSize * 2f;
        float width = height * camera.aspect;

        CreateWall("Left Wall",
            new Vector2(-width / 2f - wallThickness / 2f, 0f),
            new Vector2(wallThickness, height));

        // TODO: Create the right and top walls
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

3. Drag and drop the `WallController` script from the Project panel onto the Walls GameObject in the Hierarchy panel.
4. In the Inspector panel for the Walls GameObject, you should see a new component called "Wall Controller" with a "Wall Thickness" field and a "Ball Bounce Material" field. Drag and drop the "BallBounce" physics material from the Materials folder into the "Ball Bounce Material" field.

![](<../../resources (ignore)/img/week-02.1-breakout-2-textures-prefabs-prefab-variants-input-system/00.png>)

5. Click the Play button at the top of the Unity Editor to run the game. In the Hierarchy panel, you should see that the Walls GameObject has a child GameObject called "Left Wall" with a BoxCollider2D component.

![](<../../resources (ignore)/img/week-02.1-breakout-2-textures-prefabs-prefab-variants-input-system/01.png>)

---

## Kenny's Assets

Kenny's Assets is a collection of free game assets that you can use in your projects. It includes a wide variety of textures, sprites, audio files and more.

In the week-02.1-breakout-2-textures-prefabs-prefab-variants-input-system folder, you will find a subfolder called "Sprites". This folder contains a collection of textures that you can use to replace the default sprites for the Ball, Paddle and Bricks GameObjects.

> Resource: [Kenney's Assets](https://kenney.nl/assets)

---

## Textures

Textures are images that can be applied to GameObjects in Unity. They can be used to add colour, detail and realism to your game. In Unity, textures are represented by the `Texture` class. There are different types of textures, such as `Texture2D`, `Texture3D` and `Cubemap`, each with its own specific use cases.

1. Copy and paste the "Sprites" folder from the week-02.1-breakout-2-textures-prefabs-prefab-variants-input-system folder into the Assets folder of your Unity project.

2. In the "Sprites" folder, you will find a texture called "ballBlue.png". Set the following:
   - Sprite Mode: Single
   - Pixels Per Unit: 22
   - Filter Mode: Point (no filter)
   - Max Size: 32
   - Format: RGBA 32 bit

Click the Apply button to save the changes.

![](<../../resources (ignore)/img/week-02.1-breakout-2-textures-prefabs-prefab-variants-input-system/02.png>)

3. In the Hierarchy panel, select the Ball GameObject. Drag and drop the "ballBlue" sprite from the Sprites folder in the Project panel onto the Sprite Renderer component of the Ball GameObject in the Inspector panel.
4. Click the Play button at the top of the Unity Editor to run the game.

![](<../../resources (ignore)/img/week-02.1-breakout-2-textures-prefabs-prefab-variants-input-system/03.png>)

> Resource: [Unity Manual: Textures](https://docs.unity3d.com/Manual/class-Texture.html)

---

## Prefabs

Prefabs are reusable GameObjects that can be easily instantiated in your scene. They allow you to create a template for a GameObject and then create multiple instances of that template throughout your game. This is especially useful for objects that you want to create multiple times.

We are going to create a prefab for the Ball GameObject. In the next section, we will create prefab variants for the Ball GameObject to create different types of balls. For example, a blue and fast ball, and a grey and slow ball.

In the Project panel, create a new folder called "Prefabs". Drag and drop the Ball GameObject from the Hierarchy panel into the Prefabs folder in the Project panel. This will create a prefab of the Ball GameObject.

![](<../../resources (ignore)/img/week-02.1-breakout-2-textures-prefabs-prefab-variants-input-system/04.png>)

> Resource: [Unity Manual: Prefabs](https://docs.unity3d.com/Manual/Prefabs.html)

---

## Prefab Variants

Prefab variants are a special type of prefab that allows you to create variations of a prefab while still maintaining a connection to the original prefab. This means that if you make changes to the original prefab, those changes will be reflected in all of its variants.

To create a prefab variant, right-click on the Ball prefab in the Prefabs folder and select "Create > Prefab Variant". Name the new prefab "FastBall".

![](<../../resources (ignore)/img/week-02.1-breakout-2-textures-prefabs-prefab-variants-input-system/05.png>)

In the Inspector panel, change the "Speed" field of the Ball Controller component to a higher value. Create another prefab variant called "SlowBall" and change the "Speed" field of the Ball Controller component to a lower value.

![](<../../resources (ignore)/img/week-02.1-breakout-2-textures-prefabs-prefab-variants-input-system/06.png>)

> Resource: [Unity Manual: Prefab Variants](https://docs.unity3d.com/Manual/PrefabVariants.html)

---

## Instantiating Prefabs

In the Hierarchy panel, delete the Ball GameObject. We will instantiate the Ball prefab at runtime using a script.

In the Scripts folder, create a new script called "GameManager". Open the script in your code editor and add the following code:

```csharp
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Prefab Settings")]
    [SerializeField] private BallController fastBallPrefab;
    [SerializeField] private BallController slowBallPrefab;

    private BallController currentBall;

    private void Start()
    {
        currentBall = Instantiate(slowBallPrefab, Vector2.zero, Quaternion.identity);
    }
}
```

In the Hierarchy panel, create an empty GameObject and name it "GameManager". Drag and drop the `GameManager` script from the Project panel onto the GameManager GameObject in the Hierarchy panel. In the Inspector panel for the GameManager GameObject, you should see a new component called "Game Manager" with two fields: "Fast Ball Prefab" and "Slow Ball Prefab". Drag and drop the FastBall prefab into the "Fast Ball Prefab" field and the SlowBall prefab into the "Slow Ball Prefab" field.

![](<../../resources (ignore)/img/week-02.1-breakout-2-textures-prefabs-prefab-variants-input-system/07.png>)

Click the Play button at the top of the Unity Editor to run the game. You should see a slow ball instantiated in the scene. You can stop the game and change the prefab that is instantiated in the `GameManager` script to the fast ball prefab to see a fast ball instantiated in the scene.

> Resource: [Unity Manual: Instantiating Prefabs](https://docs.unity3d.com/Manual/InstantiatingPrefabs.html)

---

## Paddle Game Object

1. In the Hierarchy panel, right-click and select "2D Object > Sprite > Square". Name the GameObject "Paddle".
2. Change the Paddle GameObject's scale and position. It is recommended you do it programmatically in a script, but for now, you can do it manually in the Inspector panel.
3. Do not forget to add a BoxCollider2D component and a Rigidbody2D component. Think about what properties you need to set for the Rigidbody2D component.

---

### Input System

The Input System is a package in Unity that provides a new way to handle input from various devices, such as keyboards, mice, gamepads and touchscreens. It offers a more flexible and powerful way to manage input compared to the old Input Manager. It allows you to define input actions and bind them to specific controls, making it easier to handle complex input scenarios.

1. In the Assets folder, double-click the InputSystem_Actions file to open the Input Actions editor. In the editor, you can define input actions and bind them to specific controls.
2. Create a new action map called "Paddle". You should see two default action maps called "Player" and "UI". Feel free to delete these action maps if you do not need them.
3. In the Paddle action map, create a new action called "Move". Set the action type to "Value" and the control type to "Axis".
4. In the "Move" action, add a 1D positive/negative binding. Set the positive binding to the D key and the negative binding to the A key.
5. Save the asset and close the Input Actions editor.

![](<../../resources (ignore)/img/week-02.1-breakout-2-textures-prefabs-prefab-variants-input-system/08.png>)

6. In the Project panel, click on the InputSystem_Actions asset to select it. In the Inspector panel, click the "Generate C# Class" button. This will generate a C# script that you can use to access the input actions defined in the Input Actions editor.

![](<../../resources (ignore)/img/week-02.1-breakout-2-textures-prefabs-prefab-variants-input-system/09.png>)

7. If you have not already, create a new script called "PaddleController". Open the script in your code editor and add the following code:

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
    private float screenHalfWidth;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // TODO: Set scale and position
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<float>();
    }

    private void FixedUpdate()
    {
        Vector2 position = rb.position;
        position.x += moveInput * speed * Time.fixedDeltaTime;
        rb.MovePosition(position);
    }
}
```

8. Drag and drop the `PaddleController` script from the Project panel onto the Paddle GameObject in the Hierarchy panel.
9. In the Inspector panel for the Paddle GameObject, add a new component called "Player Input". Set the "Behavior" field to "Invoke Unity Events". Expand the "Events > Paddle" section. You should see the "Move" action that you created in the Input Actions editor. Click the "+" button to add a new event listener for the "Move" action. Drag and drop the Paddle GameObject from the Hierarchy panel into the object field of the new event listener. In the function dropdown, select "PaddleController > OnMove".

![](<../../resources (ignore)/img/week-02.1-breakout-2-textures-prefabs-prefab-variants-input-system/10.png>)

10. Click the Play button at the top of the Unity Editor to run the game. You should be able to move the paddle left and right using the A and D keys.

> Resource: [Unity Manual: Input System](https://docs.unity3d.com/Manual/com.unity.inputsystem.html)

---

## Tags

Tags are a way to categorise GameObjects in Unity. They allow you to assign a label to a GameObject, which can be used to identify and group GameObjects in your scripts. For example, you can assign the "Ball" tag to the Ball GameObject, and then use that tag to identify the ball in your scripts.

1. In the Unity Editor, go to "Edit > Project Settings > Tags and Layers". In the "Tags" section, click the "+" button to add a new tag. Name the new tag "Ball".
2. In the Hierarchy panel, select the Ball GameObject. In the Inspector panel, set the "Tag" field to "Ball".

![](<../../resources (ignore)/img/week-02.1-breakout-2-textures-prefabs-prefab-variants-input-system/10.png>)

---

## Collision Detection

Collision detection is the process of detecting when two or more GameObjects in a game collide with each other. In Unity, collision detection is handled by the physics engine. When two GameObjects with colliders come into contact with each other, the physics engine detects the collision and can trigger events or apply forces based on the collision. For example, you can use collision detection to destroy a brick when the ball collides with it.

1. In the Hierarchy panel, right-click and select "2D Object > Sprite > Square". Name the GameObject "Brick".
2. Add a BoxCollider2D component to the Brick GameObject.
3. Create a new script called "Brick". Open the script in your code editor and add the following code:

```csharp
using UnityEngine;

public class Brick : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball")) // Destroy the brick if it collides with the ball
            Destroy(gameObject);
    }
}
```

4. Drag and drop the `Brick` script from the Project panel onto the Brick GameObject in the Hierarchy panel.
5. Click the Play button at the top of the Unity Editor to run the game. You should see that when the ball collides with the brick, the brick is destroyed.
6. Drag and drop the Brick GameObject from the Hierarchy panel into the Prefabs folder in the Project panel to create a prefab of the Brick GameObject. You can then delete the Brick GameObject from the Hierarchy panel.

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

Create two new prefab variants for the Paddle GameObject with different sprites and speeds. For example, a red and fast paddle, and a blue and slow paddle. Instantiate one of the new prefab variants in the scene using the `GameManager` script.

---

### Task 2

Create the following input actions:

- "Start Game" action that is triggered by the Space key. When this action is triggered, it should instantiate the ball prefab and start the game.
- "Pause Game" action that is triggered by the P key. When this action is triggered, it should pause the game by setting `Time.timeScale` to 0.
- "Resume Game" action that is triggered by the R key. When this action is triggered, it should resume the game by setting `Time.timeScale` back to 1.
