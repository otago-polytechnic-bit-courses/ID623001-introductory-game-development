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

public class BallController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    private void Start()
    {
        Vector2 direction = new Vector2(
            Random.Range(-1f, 1f), 1f).normalized; 

        rb.linearVelocity = direction * speed;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = rb.linearVelocity.normalized * speed;
    }
}
```

8. Drag and drop the `BallController` script from the Project panel onto the Ball GameObject in the Hierarchy panel.
9. Click the Play button at the top of the Unity Editor to run the game. You should see the ball bouncing around the scene.

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
