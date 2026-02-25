# Week 01.2

---

## Important Links

| Section    | Link                                                                         |
| ---------- | ---------------------------------------------------------------------------- |
| Next Class | [Week 02]()                               |

---

## Breakout

Breakout is a classic arcade game where the player controls a paddle to bounce a ball and break bricks. The goal is to break all the bricks without letting the ball fall below the paddle.

---

### Project Setup

1. Open Unity Hub and create a new "2D Universal" project. Name it "Breakout".
2. In the root directory of your project, add a Unity `.gitignore` file to exclude unnecessary files from version control. Add, commit and push the `.gitignore` file to your GitHub repository. Then add, commit and push the Unity project files to your GitHub repository.
3. In the Assets folder, create two new folders: "Materials" and "Scripts". 

---

### Ball 

1. In the Hierarchy panel, right-click and select "2D Object > Sprite > Circle". Name the GameObject "Ball". 
2. In the Materials folder, right-click and select "Create > 2D > Physics Material 2D". Name it "BallBounce". 
3. In the Inspector panel, set the:
    - `Friction` to 0 to prevent the ball from slowing down when it collides with other objects
    - `Bounciness` to 1 to make the ball bounce back with the same speed after colliding with other objects
    - `Bounce Combine` to "Maximum" to ensure the ball bounces as much as possible when colliding with other objects
    - `Friction Combine` to "Minimum" to ensure the ball does not experience any friction when colliding with other objects
4. Add two components to the Ball GameObject: `Rigidbody2D` and `CircleCollider2D`. 
    - `Rigidbody2D` allows the ball to be affected by physics, such as gravity and collisions.
    - `CircleCollider2D` defines the shape of the ball for collision detection.
5. In the Inspector panel, set the `Rigidbody2D` component's:
    - `Gravity Scale` to 0 so the ball does not fall due to gravity
    - `Collision Detection` to "Continuous" to prevent the ball from passing through objects at high speeds
    - `Interpolate` to "Interpolate" to smooth out the ball's movement
    - `Constraints > Freeze Rotation` to true to prevent the ball from spinning
6. In the Inspector panel, set the `CircleCollider2D` component's `Material` to "BallBounce" to apply the physics material.
7. In the Scripts folder, right-click and select "Create > MonoBehaviour Script". Name it "BallController". Double-click the script to open it in your code editor, e.g., Microsoft Visual Studio or Microsoft Visual Studio Code. Add the following code:

```csharp
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] 
public class BallController : MonoBehaviour
{
    [Header("Movement Settings")] 
    [SerializeField] private float speed = 5f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    private void Start()
    {
        // Set the initial direction of the ball to a random direction
        Vector2 direction = new Vector2(
            Random.Range(-1f, 1f), 1f).normalized; // Normalise to ensure the direction has a magnitude of 1

        // Set the initial velocity of the ball based on the direction and speed
        rb.linearVelocity = direction * speed;
    }

    private void FixedUpdate()
    {
        // Ensure the ball maintains a constant speed by normalising the velocity and multiplying by the speed
        rb.linearVelocity = rb.linearVelocity.normalized * speed;
    }
}
```

If you look at the Inspector panel for the Ball GameObject, you should see a new component called "Ball Controller" with a "Speed" field. You can adjust the speed of the ball by changing the value in this field.

8. Drag and drop the `BallController` script from the Project panel onto the Ball GameObject in the Hierarchy panel.
9. Click the Play button at the top of the Unity Editor to run the game. You should see the ball bouncing around the scene.

---

### Life Cycle of a MonoBehaviour Script

In a MonoBehaviour script, there are several special methods that are called by Unity at specific points in the game's lifecycle. These methods include:

- `Awake()`: Called when the script instance is being loaded. This happens before the `Start()` method and is used to initialise variables or states before the game starts. It is called only once during the lifetime of the script instance.
- `Start()`: Called before the first frame update. This is used to set up the initial state of the game. It is called only once during the lifetime of the script instance.
- `Update()`: Called once per frame. This is used for regular updates, such as checking for input or moving objects. The frequency of this method depends on the frame rate of the game.
- `FixedUpdate()`: Called at a fixed time interval and is used for physics updates. It is called multiple times per second, depending on the physics settings of the project.

---

### Useful Attributes

- `[SerializeField]`: This attribute allows you to serialize a private field, making it visible and editable in the Unity Inspector. This is useful for keeping variables private while still allowing designers to tweak values in the editor.
- `[Header("Header Name")]`: This attribute adds a header above the field in the Unity Inspector, which can be used to group related fields together for better organisation and readability.
- `[RequireComponent(typeof(ComponentType))]`: This attribute ensures that the specified component is added to the GameObject when the script is attached. If the component is not already present, Unity will automatically add it. This is useful for ensuring that necessary components are always present on a GameObject.

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

The `Start()` method has two concerns: setting the initial direction of the ball and setting the initial speed of the ball. Refactor the `Start()` method to separate these concerns into two methods: `SetInitialDirection()` and `SetInitialSpeed()`. The `Start()` method should call these two methods to set up the ball's initial movement.

---

### Task 2

Create a variable called `size` in the `BallController` script to represent the size of the ball. In the `Start()` method, set the ball's scale to a random value between 0.5 and 1 using this variable. You can do this by setting the `transform.localScale` property of the Ball GameObject to a new `Vector2` with the random size for both the x and y axes.

---

### Task 3

Change the background color of the scene to a color of your choice. You can do this by selecting the "Main Camera" GameObject in the Hierarchy panel and changing the "Background" colour in the Inspector panel.

---

### Task 4

Currently, the Ball GameObject position is set to (0, 0) in the scene. This means the ball will always start at the center of the scene. Modify the `Start()` method in the `BallController` script to set the ball's initial position to 20 units above the bottom of the screen. You can use `Camera.main.ScreenToWorldPoint()` to convert screen coordinates to world coordinates. 

---

### Task 5

Currently, the Ball GameObject colour is white. In the `Start()` method of the `BallController` script, set the ball's colour to a random colour each time the game starts. You can do this by accessing the `SpriteRenderer` component of the Ball GameObject and setting its `color` property to a new `Color` with random RGB values. Ensure there is contrast between the ball and the background colour for better visibility.

