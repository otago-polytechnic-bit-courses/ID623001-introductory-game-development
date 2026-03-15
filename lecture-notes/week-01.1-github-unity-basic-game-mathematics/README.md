# Week 01.1 - GitHub, Unity and Basic Game Mathematics

## Navigation

|                  | Link                                                                                                                                                                                                                                               |
| ---------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| GitHub Classroom | [ID623002-S1-26](https://classroom.github.com/a/tSkpt5Ho)                                                                                                                                                                                          |
| → Next           | [Week 01.2 - Breakout: Materials, Components and Scripts](../week-01.2-breakout-1-game-objects-materials-components-scripts/README.md) |

---

## 1. Git

### 1.1 Useful Git Commands

| Command                          | Description                                         |
| -------------------------------- | --------------------------------------------------- |
| `git clone <repository-url>`     | Clone a repository to your local machine            |
| `git status`                     | Check the status of your local repository           |
| `git add <file>`                 | Stage changes for the next commit                   |
| `git commit -m "commit message"` | Commit staged changes with a descriptive message    |
| `git push`                       | Push committed changes to the remote repository     |
| `git pull`                       | Pull the latest changes from the remote repository  |
| `git branch`                     | List all branches in the repository                 |
| `git switch <branch>`            | Switch to a different branch                        |
| `git restore <file>`             | Discard changes in the working directory for a file |
| `git checkout <branch>`          | Switch to a different branch (older command)        |
| `git fetch`                      | Fetch changes from the remote repository            |
| `git merge <branch>`             | Merge a branch into the current branch              |
| `git log`                        | View the commit history                             |

📖 Reference: [GitHub Git Handbook](https://guides.github.com/introduction/git-handbook/)

---

## 2. C# and Unity

C# is a strongly-typed, object-oriented programming language used as Unity's primary scripting language. All game logic - movement, collision, animation, UI - is expressed through C# scripts attached to GameObjects via `MonoBehaviour`.

📖 Reference: [Unity Manual](https://docs.unity3d.com/Manual/index.html)

---

### 2.1 Installer and Project Setup

**Step 1** - Download and install Unity Hub from [unity.com/download](https://unity.com/download).

**Step 2** - In Unity Hub, go to the **Installs** tab and click **Install Editor**. Install the recommended LTS (Long Term Support) version. This ensures you have a stable, widely supported version for development.

**Step 3** - Go to the **Projects** tab and click **New Project**. Choose the **Universal 2D** template, name your project, select a save location, then click **Create project**.

**Step 4** - Unity will open with a default scene loaded. The editor has five main panels:

| Panel         | Purpose                                     |
| ------------- | ------------------------------------------- |
| **Scene**     | View and edit your game world               |
| **Game**      | Preview your game as it will run            |
| **Hierarchy** | Lists all GameObjects in the current scene  |
| **Inspector** | Shows properties of the selected GameObject |
| **Project**   | Shows all assets in your project            |

**Step 5** - In the Hierarchy panel, right-click and select **2D Object > Sprite > Circle**.

**Step 6** - In the `Assets` folder, right-click and select **Create > Folder**. Name it `Scripts`.

**Step 7** - In the `Scripts` folder, right-click and select **Create > MonoBehaviour Script**. Name it `Demo`. Double-click to open it in your code editor and add the following to the `Start()` method:

```csharp
using UnityEngine;

public class Demo : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Hello, World!");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
```

**Step 8** - Drag and drop the `Demo` script from the Project panel onto the `Circle` GameObject in the Hierarchy. This attaches the script as a component of that GameObject.

**Step 9** - Click the **Play** button at the top of the editor. You should see `"Hello, World!"` printed in the Console panel.

**Step 10** - You may notice an asterisk (`*`) next to the scene name in the Hierarchy. This means the scene has unsaved changes. Save regularly as you work.

---

## 3. Cartesian Coordinate Systems

### 3.1 1D Mathematics

The simplest coordinate system is the number line: a single axis with a defined origin (`0`), a positive direction, and a unit of length. Every real number corresponds to exactly one point on this line.

In Unity, individual axes behave like number lines. A GameObject's position on any axis is a signed scalar value.

```csharp
float xPos = transform.position.x;
float yPos = transform.position.y;
float zPos = transform.position.z;
```

---

### 3.2 2D Cartesian Space

Two perpendicular number lines - the x-axis (horizontal) and y-axis (vertical) - intersect at the origin `(0, 0)` to form a 2D coordinate system. Any point P in the plane is specified uniquely by an ordered pair `(x, y)`.

In Unity 2D projects, positions are represented as `Vector2`:

```csharp
Vector2 playerPos = new Vector2(3f, -1.5f);

Debug.Log($"X: {playerPos.x}, Y: {playerPos.y}"); // X: 3, Y: -1.5
```

---

### 3.3 3D Cartesian Space

3D space adds a third axis - the z-axis - perpendicular to both x and y. Every point in 3D is an ordered triple `(x, y, z)`. Unity uses a **left-handed** coordinate system:

| Axis | Direction                 |
| ---- | ------------------------- |
| +x   | Right                     |
| +y   | Up                        |
| +z   | Forward (into the screen) |

> **Left-handed vs right-handed:** In a right-handed system (standard mathematics and OpenGL), +z points _out of_ the screen. Unity's left-handed system has +z pointing _into_ the screen. This affects how cross products work - results point in the opposite direction compared to a right-handed system. Keep this in mind when working with physics or importing assets from other tools.

Unity provides convenient constants for the cardinal directions:

```csharp
Vector3 right   = Vector3.right;    // ( 1,  0,  0)
Vector3 up      = Vector3.up;       // ( 0,  1,  0)
Vector3 forward = Vector3.forward;  // ( 0,  0,  1)
Vector3 origin  = Vector3.zero;     // ( 0,  0,  0)
```

How would you represent the left, down and back directions? Try it yourself before checking:

```csharp
Vector3 left = Vector3.left;  // (-1,  0,  0)
Vector3 down = Vector3.down;  // ( 0, -1,  0)
Vector3 back = Vector3.back;  // ( 0,  0, -1)
```

📖 Reference: [Unity - Transform](https://docs.unity3d.com/Manual/class-Transform.html)

---

### 3.4 Angles, Degrees and Radians

Angles measure the amount of rotation between two directions. Two units are commonly used:

- **Degrees:** a full rotation is 360°
- **Radians:** a full rotation is 2π ≈ 6.2832 rad. One radian is the angle formed when the arc length along a circle equals the radius of that circle.

The conversion between them:

```
radians = degrees × (π / 180)
degrees = radians × (180 / π)
```

Unity's trigonometric functions (`Mathf.Sin`, `Mathf.Cos`, etc.) take values in **radians**. Use `Mathf.Deg2Rad` and `Mathf.Rad2Deg` to convert:

```csharp
float degrees = 90f;
float radians = degrees * Mathf.Deg2Rad;
float back    = radians * Mathf.Rad2Deg;

Debug.Log($"Degrees: {degrees}, Radians: {radians}, Back: {back}");
// Degrees: 90, Radians: 1.5708, Back: 90
```

---

### 3.5 Trigonometric Functions

For a right triangle with angle θ, the three core trig functions are:

```
sin(θ) = opposite / hypotenuse
cos(θ) = adjacent / hypotenuse
tan(θ) = opposite / adjacent  =  sin(θ) / cos(θ)
```

Key identities:

```
sin²(θ) + cos²(θ) = 1
sin(−θ) = −sin(θ)
cos(−θ) =  cos(θ)
```

In Unity these are used constantly - for circular motion, aiming, and computing angles between directions:

```csharp
float angle = 45f * Mathf.Deg2Rad;
float s = Mathf.Sin(angle);
float c = Mathf.Cos(angle);

Debug.Log($"sin(45°) = {s:F3}, cos(45°) = {c:F3}"); // sin(45°) = 0.707, cos(45°) = 0.707

// Inverse trig functions return angles in radians
float theta = Mathf.Asin(0.5f)    * Mathf.Rad2Deg;
float phi   = Mathf.Atan2(1f, 1f) * Mathf.Rad2Deg;

Debug.Log($"Asin(0.5) = {theta}°, Atan2(1,1) = {phi}°"); // Asin(0.5) = 30°, Atan2(1,1) = 45°
```

> Use `Mathf.Atan2(y, x)` instead of `Mathf.Atan(y/x)` - it correctly handles all four quadrants and avoids division by zero when `x = 0`.

📖 Reference: [Unity - Mathf](https://docs.unity3d.com/ScriptReference/Mathf.html)

---

## 4. Vectors

### 4.1 What is a Vector?

A **vector** is a quantity with both magnitude (size) and direction. This distinguishes it from a **scalar**, which has magnitude only.

- **Scalar example:** a player's health (e.g. 75 HP) has no direction - just a value.
- **Vector example:** an enemy's velocity (e.g. `(3, 0, 0)` m/s) has a speed of 3 m/s pointing in the +x direction.

Vectors are written in bold (**v**) or with an arrow (v→). In Unity, `Vector2` and `Vector3` are the built-in vector types.

---

### 4.2 Vectors vs. Points

This distinction is conceptually important:

- A **point** describes a _location_ in space. It has no length or direction on its own.
- A **vector** describes a _displacement_ or direction. It has no fixed position.

If you are describing **where** something is, use a point. If you are describing **how** something moves or faces, use a vector.

In Unity both are stored as `Vector3`, but they mean different things depending on context. `transform.position` is a point; `transform.forward` is a vector.

```csharp
Vector3 pointA       = new Vector3(1f, 0f, 0f);   // a point at (1, 0, 0)
Vector3 pointB       = new Vector3(4f, 3f, 0f);   // a point at (4, 3, 0)
Vector3 displacement = pointB - pointA;            // vector from A to B

Debug.Log($"Displacement: {displacement}"); // Displacement: (3, 3, 0)
```

---

### 4.3 Vector Negation

Negating a vector reverses its direction while keeping its magnitude:

```
−v = (−x, −y, −z)
```

Each component is multiplied by `−1`, flipping its sign.

```csharp
Vector3 v   = new Vector3(1f, -2f, 3f);
Vector3 neg = -v;   // (-1, 2, -3)
```

Common uses in games: applying knockback (opposite direction to impact), computing the vector from B to A as `A − B`.

---

### 4.4 Scalar Multiplication

Multiplying a vector by a scalar `k` scales its length by `|k|`. If `k < 0` the direction is also reversed:

```
k · v = (k·x,  k·y,  k·z)
```

- `k > 1` - vector is stretched (longer)
- `0 < k < 1` - vector is shrunk (shorter)
- `k < 0` - vector is reversed and scaled by `|k|`

```csharp
Vector3 forward = Vector3.forward;
Vector3 fast    = forward * 10f;    // (0, 0, 10)
Vector3 back    = forward * -1f;    // (0, 0, -1)

Debug.Log($"Fast: {fast}, Back: {back}");
```

Common uses in games: controlling movement speed, applying forces in a given direction.

---

### 4.5 Vector Addition and Subtraction

Vectors add and subtract **component-wise**. Geometrically, addition places vectors head-to-tail; the result is the resultant:

```
a + b = (ax+bx,  ay+by,  az+bz)
a − b = (ax−bx,  ay−by,  az−bz)
```

The vector from point A to point B is always `B − A`:

```csharp
Vector3 A    = new Vector3(1f, 0f, 0f);
Vector3 B    = new Vector3(4f, 3f, 0f);
Vector3 AtoB = B - A;

Debug.Log($"Vector from A to B: {AtoB}"); // Vector from A to B: (3, 3, 0)
```

> Why `B − A` and not `A − B`? Because we want a vector that _starts at A and points toward B_. `A − B` would point in the opposite direction, from B toward A.

---

### 4.6 Vector Magnitude (Length)

The magnitude (length) of a vector uses the Pythagorean theorem extended to 3D:

```
|v| = √(x² + y² + z²)
```

Step by step for `v = (3, 4, 0)`:

1. Square each component: `3² = 9`, `4² = 16`, `0² = 0`
2. Sum: `9 + 16 + 0 = 25`
3. Square root: `√25 = 5`

```csharp
Vector3 v    = new Vector3(3f, 4f, 0f);
float   mag  = v.magnitude;       // 5
float   magSq = v.sqrMagnitude;   // 25 - cheaper, avoids sqrt; use for comparisons

Debug.Log($"Magnitude: {mag}, Squared Magnitude: {magSq}");
```

---

### 4.7 Unit Vectors and Normalisation

A **unit vector** has a magnitude of exactly `1`. It expresses a pure direction with no scale. Any non-zero vector can be normalised by dividing by its magnitude:

```
v̂ = v / |v|
```

```csharp
Vector3 v    = new Vector3(3f, 4f, 0f);
Vector3 vHat = v.normalized;   // (0.6, 0.8, 0)

// Move toward a target at constant speed
Vector3 dir = (target.position - transform.position).normalized;
transform.position += dir * speed * Time.deltaTime;

Debug.Log($"Original: {v}, Normalised: {vHat}");
```

> Never normalise the zero vector - it has no direction. `Vector3.zero.normalized` returns `Vector3.zero` in Unity but is mathematically undefined. Check `v.magnitude > 0f` before normalising if the vector may be zero.

**Diagonal movement gotcha:** if you do not normalise an 8-directional input vector, diagonal movement will be faster than axis-aligned movement. The diagonal vector `(1, 1)` has magnitude `√2 ≈ 1.414`. Normalising gives a unit vector in the same direction, ensuring consistent speed in all directions.

Common uses in games: moving toward a target at constant speed, computing aim direction.

---

### 4.8 The Distance Formula

The distance between two points P and Q is the magnitude of the displacement vector between them:

```
d(P, Q) = |Q − P| = √((Qx−Px)² + (Qy−Py)² + (Qz−Pz)²)
```

For `P = (1, 2, 3)` and `Q = (4, 6, 3)`:

```
d = √((4−1)² + (6−2)² + (3−3)²) = √(9 + 16 + 0) = √25 = 5
```

```csharp
Vector3 P    = new Vector3(1f, 2f, 3f);
Vector3 Q    = new Vector3(4f, 6f, 3f);
float   dist = Vector3.Distance(P, Q);

Debug.Log($"Distance: {dist}"); // Distance: 5
```

Common uses in games: checking if a player is within range of an enemy, proximity triggers.

---

### 4.9 The Dot Product

The dot product is defined algebraically as:

```
a · b = ax·bx + ay·by + az·bz
```

And geometrically as:

```
a · b = |a| |b| cos(θ)
```

Rearranging gives the angle directly:

```
θ = arccos( (a · b) / (|a| |b|) )
```

For **unit vectors** this simplifies to `θ = arccos(a · b)`, which is why normalising before a dot product is so common.

| `a · b` (unit vectors) | Meaning                          |
| ---------------------- | -------------------------------- |
| `= 1`                  | θ = 0° - same direction          |
| `> 0`                  | θ < 90° - roughly same direction |
| `= 0`                  | θ = 90° - perpendicular          |
| `< 0`                  | θ > 90° - roughly opposite       |
| `= −1`                 | θ = 180° - exactly opposite      |

```csharp
Vector3 forward = transform.forward;
Vector3 toEnemy = (enemy.position - transform.position).normalized;
float   dot     = Vector3.Dot(forward, toEnemy);

if (dot > 0.5f)
    Debug.Log("Enemy is in front");
else if (dot < 0f)
    Debug.Log("Enemy is behind");

// Clamp to [-1, 1] before Acos to avoid NaN from floating-point error
float angle = Mathf.Acos(Mathf.Clamp(dot, -1f, 1f)) * Mathf.Rad2Deg;
Debug.Log($"Angle to enemy: {angle}°");
```

Common uses in games: detecting whether an enemy is in front of or behind the player, field-of-view checks, aiming.

📖 Reference: [Unity - Vector3.Dot](https://docs.unity3d.com/ScriptReference/Vector3.Dot.html)

---

### 4.10 The Cross Product

The cross product is a **3D-only** operation. It takes two vectors and returns a third vector perpendicular to both:

```
a × b = (ay·bz − az·by,
         az·bx − ax·bz,
         ax·by − ay·bx)
```

The magnitude of the result is:

```
|a × b| = |a| |b| sin(θ)
```

The direction follows the **left-hand rule** in Unity.

```csharp
Vector3 a = new Vector3(1f, 0f, 0f);
Vector3 b = new Vector3(0f, 1f, 0f);
Vector3 c = Vector3.Cross(a, b);

Debug.Log($"Cross Product: {c}"); // Cross Product: (0, 0, 1)

// Determine if an enemy is to the left or right of the player
Vector3 toEnemy = (enemy.position - transform.position).normalized;
Vector3 cross   = Vector3.Cross(transform.forward, toEnemy);

if (cross.y > 0f) Debug.Log("Enemy is to the right");
else              Debug.Log("Enemy is to the left");
```

Common uses in games: computing surface normals for lighting and physics, determining left/right relative orientation, building rotation axes.

📖 Reference: [Unity - Vector3.Cross](https://docs.unity3d.com/ScriptReference/Vector3.Cross.html)

---

## 5. Multiple Coordinate Spaces

### 5.1 Why Multiple Spaces?

A single global coordinate system quickly becomes impractical. When a sword is attached to a character's hand, it is far easier to describe the sword's position _relative to the hand_ than relative to the entire world. Unity maintains several coordinate spaces simultaneously:

| Space            | Description                                                            |
| ---------------- | ---------------------------------------------------------------------- |
| **World space**  | The global fixed coordinate system. All objects ultimately exist here. |
| **Local space**  | Relative to a specific GameObject's own position and orientation.      |
| **Camera space** | Relative to the camera - used in rendering and screen-space effects.   |

---

### 5.2 World vs. Local in Unity

```csharp
Vector3 worldPos  = transform.position;
Vector3 worldFwd  = transform.forward;
Vector3 localPos  = transform.localPosition;

// TransformPoint converts a local-space point to world space
Vector3 worldPoint = transform.TransformPoint(new Vector3(0f, 1f, 0f));

// InverseTransformPoint converts a world-space point to local space
Vector3 localPoint = transform.InverseTransformPoint(worldPoint);

Debug.Log($"World: {worldPos}, Local: {localPos}");
```

> Understanding which space a vector lives in is one of the most common sources of bugs in Unity. Always be explicit about whether you are working in world or local space.

📖 Reference: [Unity - Transform](https://docs.unity3d.com/ScriptReference/Transform.html)

---

## 6. Useful `Mathf` Functions

Unity's `Mathf` class provides game-relevant maths utilities. All values are `float`.

| Function                   | Description                             | Example                          |
| -------------------------- | --------------------------------------- | -------------------------------- |
| `Mathf.Clamp(v, min, max)` | Restricts `v` to the range `[min, max]` | Keeping health between 0 and 100 |
| `Mathf.Abs(v)`             | Absolute value - distance from zero     | `Abs(-5f)` → `5`                 |
| `Mathf.Pow(v, exp)`        | `v` raised to the power `exp`           | `Pow(4f, 2f)` → `16`             |
| `Mathf.Sqrt(v)`            | Square root                             | `Sqrt(16f)` → `4`                |
| `Mathf.Round(v)`           | Rounds to nearest integer               | `Round(3.7f)` → `4`              |
| `Mathf.Floor(v)`           | Rounds down                             | `Floor(3.9f)` → `3`              |
| `Mathf.Ceil(v)`            | Rounds up                               | `Ceil(3.1f)` → `4`               |
| `Random.Range(min, max)`   | Random float in `[min, max)`            | `Range(0f, 1f)` → random         |
| `Mathf.Min(a, b, ...)`     | Smallest value                          | `Min(3f, 7f, 1f)` → `1`          |
| `Mathf.Max(a, b, ...)`     | Largest value                           | `Max(3f, 7f, 1f)` → `7`          |

```csharp
float health  = Mathf.Clamp(currentHealth, 0f, 100f);
float dist    = Mathf.Abs(-5f);
float squared = Mathf.Pow(4f, 2f);
float root    = Mathf.Sqrt(16f);
float roll    = Random.Range(0f, 1f);

Debug.Log($"Health: {health}, Abs: {dist}, Pow: {squared}, Sqrt: {root}, Random: {roll}");
```

📖 Reference: [Unity - Mathf](https://docs.unity3d.com/ScriptReference/Mathf.html)

---

## 7. Interpolation

Interpolation computes a value that sits between two known values. It is used constantly in games for smooth movement, fading effects, and animation blending - for example: smoothly following a player with the camera, fading UI elements in and out, or blending between animation states.

---

### 7.1 Linear Interpolation (Lerp)

Linear interpolation between `a` and `b` by parameter `t` (where `t ∈ [0, 1]`):

```
Lerp(a, b, t) = a + t·(b − a) = (1−t)·a + t·b
```

When `t = 0` the result is `a`; when `t = 1` the result is `b`; when `t = 0.5` the result is the midpoint.

```csharp
float   health = Mathf.Lerp(0f, 100f, 0.25f);                           // 25
Vector3 pos    = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 5f);
Color   col    = Color.Lerp(Color.red, Color.blue, 0.5f);               // purple

Debug.Log($"Health: {health}"); // Health: 25
```

> Using `Time.deltaTime * speed` as the `t` value each frame produces an **exponential ease-out** rather than constant-speed movement. For constant speed, use `Vector3.MoveTowards` instead.

---

### 7.2 Smooth Interpolation

`Vector3.SmoothDamp` gives a physically natural ease-in/ease-out feel and is the go-to for camera following and UI animations:

```csharp
private Vector3 _velocity = Vector3.zero;   // SmoothDamp writes into this each frame

void Update()
{
    transform.position = Vector3.SmoothDamp(
        transform.position,   // current position
        targetPos,            // target position
        ref _velocity,        // modified by SmoothDamp each frame
        0.3f                  // approximately how long to reach the target
    );
}
```

📖 Reference: [Unity - Vector3.Lerp](https://docs.unity3d.com/ScriptReference/Vector3.Lerp.html)

---

## 8. Circular Motion

Uniform circular motion - moving in a circle at constant speed - is one of the most common patterns in games. Given a radius `r` and an angle θ (in radians), a point on a circle centred at the origin is:

```
x = r · cos(θ)
y = r · sin(θ)
```

The angle advances over time as:

```
θ(t) = θ₀ + ω·t
```

where ω (omega) is the angular velocity in radians per second.

```csharp
public float radius      = 3f;
public float angularSpeed = 2f;   // radians per second

private float _angle = 0f;

void Update()
{
    _angle += angularSpeed * Time.deltaTime;

    float x = radius * Mathf.Cos(_angle);
    float z = radius * Mathf.Sin(_angle);

    // Move the object in a circle on the XZ plane
    transform.position = new Vector3(x, 0f, z);
}
```

Common uses in games: enemy ships circling the player, planets orbiting a star, spinning collectibles.

📖 Reference: [Unity - Mathf.Sin](https://docs.unity3d.com/ScriptReference/Mathf.Sin.html)

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

### Task 1 - Degrees to Radians

Write a method that converts an angle in degrees to radians **without** using `Mathf.Deg2Rad`, then verify the result against Unity's constant.

```csharp
float DegreesToRadians(float degrees)
{
    // Write your code here
}
```

Test cases:

| Input  | Expected output |
| ------ | --------------- |
| `0f`   | `0`             |
| `90f`  | `≈ 1.5708`      |
| `180f` | `≈ 3.14159`     |
| `360f` | `≈ 6.28318`     |

> **Hint:** `radians = degrees × (π / 180)`

---

### Task 2 - Vector Magnitude

Write a method that computes the magnitude of a 3D vector **without** using `.magnitude` or `Vector3.Distance`.

```csharp
float VectorMagnitude(Vector3 v)
{
    // Write your code here
}
```

Test cases:

| Input       | Expected output |
| ----------- | --------------- |
| `(3, 4, 0)` | `5`             |
| `(0, 0, 0)` | `0`             |
| `(1, 1, 1)` | `≈ 1.732`       |

> **Hint:** `|v| = √(x² + y² + z²)`

---

### Task 3 - Normalise a Vector

Write a method that normalises a vector **without** using `.normalized`.

```csharp
Vector3 Normalise(Vector3 v)
{
    // Write your code here
}
```

Test cases:

| Input       | Expected output   |
| ----------- | ----------------- |
| `(3, 4, 0)` | `≈ (0.6, 0.8, 0)` |
| `(0, 5, 0)` | `(0, 1, 0)`       |

> **Hint:** divide each component by the magnitude. Return `Vector3.zero` if the magnitude is `0`.

---

### Task 4 - Distance Between Two Points

Write a method that computes the distance between two 3D points **without** using `Vector3.Distance`.

```csharp
float Distance(Vector3 a, Vector3 b)
{
    // Write your code here
}
```

Test cases:

| Input                   | Expected output |
| ----------------------- | --------------- |
| `(1,2,3)` and `(1,2,3)` | `0`             |
| `(0,0,0)` and `(3,4,0)` | `5`             |

> **Hint:** `d = |b − a|`

---

### Task 5 - Dot Product and Angle

Write a method that computes the dot product of two vectors **without** using `Vector3.Dot`, then use the result to find the angle between them.

```csharp
float DotProduct(Vector3 a, Vector3 b)
{
    // Write your code here
}

float AngleBetween(Vector3 a, Vector3 b)
{
    // Write your code here
}
```

Test cases:

| Input                               | Expected output       |
| ----------------------------------- | --------------------- |
| `Vector3.right` and `Vector3.right` | `0°` - same direction |
| `Vector3.right` and `Vector3.up`    | `90°` - perpendicular |
| `Vector3.right` and `Vector3.left`  | `180°` - opposite     |

> **Hint:** `a · b = ax·bx + ay·by + az·bz`. For the angle, normalise both vectors first and clamp the dot product to `[−1, 1]` before passing to `Mathf.Acos`. Remember to convert the result from radians to degrees.
