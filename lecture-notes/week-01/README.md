# Week 01

---

## Important Links

| Section       | Link                                                                                     |
| ------------- | ---------------------------------------------------------------------------------------- |
| GitHub        | [GitHub Classroom - ID607001-S1-26](https://classroom.github.com/a/your-assignment-link) |
| Lecture Video | [Week 01 Lecture Video]()                                                                |
| Code Example  | [Code Example](code-example)                                                             |
| Next Class    | [Week 02](../week-02-unity-physics-collision)                                            |

---

## Useful Git Commands

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

> Resource: <https://guides.github.com/introduction/git-handbook/>

---

## C# and Unity

C# is a strongly-typed, object-oriented programming language used as Unity's primary scripting language. All game logic — movement, collision, animation, UI — is expressed through C# scripts attached to **GameObjects** via **MonoBehaviour**. This week we cover the mathematical foundations that underpin every system you will build in Unity.

> Resource: <https://docs.unity3d.com/Manual/index.html>

---

### MonoBehaviour Lifecycle

```csharp
using UnityEngine;

public class MathDemo : MonoBehaviour
{
    void Start()
    {
        // Runs once before the first frame — good for initialisation
        Debug.Log("Hello from Unity!");
    }

    void Update()
    {
        // Runs once per frame — home of most per-frame maths and input
    }

    void FixedUpdate()
    {
        // Runs at a fixed physics timestep — use for Rigidbody and force calculations
    }
}
```

---

## Cartesian Coordinate Systems

### 1D Mathematics

The simplest coordinate system is the **number line**: a single axis with a defined origin (0), a positive direction, and a unit of length. Every real number corresponds to exactly one point on this line.

In Unity, individual axes behave like number lines. A GameObject's x-position is a signed scalar value on the x-axis of the world coordinate system.

```csharp
float xPos = transform.position.x; // a single value on the x number line
float yPos = transform.position.y;
float zPos = transform.position.z;
```

---

### 2D Cartesian Space

Two perpendicular number lines — the **x-axis** (horizontal) and **y-axis** (vertical) — intersect at the **origin** (0, 0) to form a 2D coordinate system. Any point P in the plane is specified uniquely by an ordered pair **(x, y)**.

In Unity 2D projects positions are represented as `Vector2`:

```csharp
Vector2 playerPos = new Vector2(3f, -1.5f);
Debug.Log(playerPos.x); // 3
Debug.Log(playerPos.y); // -1.5
```

---

### 3D Cartesian Space

3D space adds a third axis — the **z-axis** — perpendicular to both x and y. Every point in 3D is an ordered triple **(x, y, z)**. Unity uses a **left-handed** coordinate system:

| Axis | Direction                 |
| ---- | ------------------------- |
| +x   | Right                     |
| +y   | Up                        |
| +z   | Forward (into the screen) |

> **Left-handed vs Right-handed:** In a right-handed system (standard mathematics and OpenGL), +z points _out of_ the screen. Unity's left-handed system has +z pointing _into_ the screen. This affects how cross products work — the result points in the opposite direction compared to a right-handed system. Keep this in mind when working with physics or importing assets from other tools.

Unity provides convenient constants for the cardinal directions:

```csharp
Vector3 right   = Vector3.right;    // ( 1,  0,  0)
Vector3 up      = Vector3.up;       // ( 0,  1,  0)
Vector3 forward = Vector3.forward;  // ( 0,  0,  1)
Vector3 origin  = Vector3.zero;     // ( 0,  0,  0)
```

> Resource: <https://docs.unity3d.com/Manual/class-Transform.html>

---

### Angles, Degrees, and Radians

Angles measure the amount of rotation between two directions. Two units are commonly used:

- **Degrees:** A full rotation is 360°.
- **Radians:** A full rotation is 2π ≈ 6.2832 rad. One radian is the angle formed when the arc length along a circle equals the radius of that circle.

The conversion between them is:

```
radians = degrees × (π / 180)
degrees = radians × (180 / π)
```

Unity's trigonometric functions (`Mathf.Sin`, `Mathf.Cos`, etc.) take values in **radians**. Use `Mathf.Deg2Rad` and `Mathf.Rad2Deg` to convert:

```csharp
float degrees = 90f;
float radians = degrees * Mathf.Deg2Rad; // 1.5708...
float back    = radians * Mathf.Rad2Deg; // 90

Debug.Log(Mathf.PI); // 3.14159...
```

---

### Trigonometric Functions

For a right triangle with angle θ, the three core trig functions are:

```
sin(θ) = opposite / hypotenuse
cos(θ) = adjacent / hypotenuse
tan(θ) = opposite / adjacent = sin(θ) / cos(θ)
```

Key identities to remember:

```
sin²(θ) + cos²(θ) = 1 (Pythagorean identity)
sin(−θ) = −sin(θ)     (sin is an odd function)
cos(−θ) = cos(θ)      (cos is an even function)
```

In Unity these are used constantly — for circular motion, aiming, and computing angles between directions:

```csharp
float angle = 45f * Mathf.Deg2Rad;

float s = Mathf.Sin(angle);  // ~0.707
float c = Mathf.Cos(angle);  // ~0.707

// Inverse trig: recover an angle from a known ratio
float theta = Mathf.Asin(0.5f)  * Mathf.Rad2Deg; // 30°
float phi   = Mathf.Atan2(1f, 1f) * Mathf.Rad2Deg; // 45°
```

> **Use `Mathf.Atan2(y, x)` instead of `Mathf.Atan(y/x)`** — it correctly handles all four quadrants and avoids division by zero when x = 0.

> Resource: <https://docs.unity3d.com/ScriptReference/Mathf.html>

---

## Vectors

### What is a Vector?

A **vector** is a quantity with both **magnitude** (size) and **direction**. This distinguishes it from a **scalar**, which has magnitude only.

- **Scalar example:** "The enemy has 80 health points."
- **Vector example:** "The projectile is moving 12 m/s to the north-east."

Vectors are written in bold (**v**) or with an arrow (v→). In Unity, `Vector2` and `Vector3` are the built-in vector types.

---

### Vectors vs. Points

This is a conceptually important distinction that trips people up early:

- A **point** describes a **location** in space — it has no length or direction on its own.
- A **vector** describes a **displacement** or **direction** — it has no fixed position.

In Unity both are stored as `Vector3`, but they mean different things depending on context. `transform.position` is a point. `transform.forward` is a vector.

```csharp
Vector3 pointA = new Vector3(1f, 0f, 0f); // a position in space
Vector3 pointB = new Vector3(4f, 3f, 0f); // another position

// Subtracting two points gives a displacement VECTOR from A to B
Vector3 displacement = pointB - pointA;   // (3, 3, 0)
```

---

### Vector Negation

Negating a vector **reverses its direction** while keeping its magnitude:

```
−v = (−x, −y, −z)
```

```csharp
Vector3 v = new Vector3(1f, -2f, 3f);
Vector3 neg = -v; // (-1, 2, -3)
```

Useful for finding the direction _away_ from a target, or reversing an applied force.

---

### Scalar Multiplication

Multiplying a vector by a scalar **k** scales its length by |k|. If k < 0 the direction is also reversed:

```
k·v = (k·x,  k·y,  k·z)
```

```csharp
Vector3 forward = Vector3.forward;   // (0, 0, 1)
Vector3 fast = forward * 10f;     // (0, 0, 10) — 10 units forward
Vector3 back = forward * -1f;     // (0, 0, -1) — reversed
```

---

### Vector Addition and Subtraction

Vectors add and subtract **component-wise**. Geometrically, addition places vectors head-to-tail; the result is the **resultant** vector:

```
a + b = (ax + bx,  ay + by,  az + bz)
a − b = (ax − bx,  ay − by,  az − bz)
```

The vector **from point A to point B** is always `B − A`:

```csharp
Vector3 A = new Vector3(1f, 0f, 0f);
Vector3 B = new Vector3(4f, 3f, 0f);

Vector3 AtoB = B - A; // (3, 3, 0) — direction and distance from A to B
```

---

### Vector Magnitude (Length)

The magnitude (length) of a vector is calculated using the **Pythagorean theorem** extended to 3D:

```
|v| = √(x² + y² + z²)
```

```csharp
Vector3 v = new Vector3(3f, 4f, 0f);

float mag   = v.magnitude;      // 5.0  (classic 3-4-5 right triangle)
float magSq = v.sqrMagnitude;   // 25.0 — no square root, cheaper to compute
```

> **Performance tip:** `sqrMagnitude` avoids the costly square root. When comparing distances (e.g. "is the enemy within range?"), compare squared distances instead:
>
> ```csharp
> // Instead of: Vector3.Distance(a, b) < 5f
> if ((B - A).sqrMagnitude < 25f) { /* within range */ }
> ```

---

### Unit Vectors and Normalisation

A **unit vector** has a magnitude of exactly 1. It expresses a **pure direction** with no scale information. Any non-zero vector can be normalised by dividing by its magnitude:

```
v̂ = v / |v|
```

```csharp
Vector3 v    = new Vector3(3f, 4f, 0f);
Vector3 vHat = v.normalized; // (0.6, 0.8, 0.0) — magnitude is 1

// Common pattern — move toward a target at a fixed speed:
Vector3 dir = (target.position - transform.position).normalized;
transform.position += dir * speed * Time.deltaTime;
```

> **Warning:** Never normalise the zero vector — it has no direction. `Vector3.zero.normalized` returns `Vector3.zero` in Unity but mathematically is undefined. Check `v.magnitude > 0f` before normalising if the vector may be zero.

---

### The Distance Formula

The distance between two points P and Q is the magnitude of the displacement vector between them:

```
d(P, Q) = |Q − P| = √((Qx−Px)² + (Qy−Py)² + (Qz−Pz)²)
```

```csharp
Vector3 P = new Vector3(1f, 2f, 3f);
Vector3 Q = new Vector3(4f, 6f, 3f);

float dist = Vector3.Distance(P, Q); // 5.0
// Equivalent to: (Q - P).magnitude
```

---

### The Dot Product

The **dot product** of two vectors is defined algebraically as:

```
a · b = ax·bx + ay·by + az·bz
```

It also has a geometric form:

```
a · b = |a| |b| cos(θ)
```

where θ is the angle between the two vectors. Rearranging gives the angle directly:

```
θ = arccos( (a · b) / (|a| |b|) )
```

For **unit vectors** this simplifies to `θ = arccos(a · b)`, which is why normalising before a dot product is so common.

| Value of `a · b` (unit vectors) | Meaning                          |
| ------------------------------- | -------------------------------- |
| = 1                             | θ = 0° — same direction          |
| > 0                             | θ < 90° — roughly same direction |
| = 0                             | θ = 90° — perpendicular          |
| < 0                             | θ > 90° — roughly opposite       |
| = −1                            | θ = 180° — exactly opposite      |

```csharp
Vector3 forward = transform.forward;
Vector3 toEnemy = (enemy.position - transform.position).normalized;

float dot = Vector3.Dot(forward, toEnemy);

if (dot > 0.5f)
    Debug.Log("Enemy is in front (within ~60°)");
else if (dot < 0f)
    Debug.Log("Enemy is behind us");

// Recover the angle in degrees:
float angle = Mathf.Acos(Mathf.Clamp(dot, -1f, 1f)) * Mathf.Rad2Deg;
Debug.Log($"Angle to enemy: {angle:F1}°");
```

> **`Mathf.Clamp`** is required before `Acos` — floating-point rounding can push the dot product slightly outside [−1, 1], which would cause `Acos` to return `NaN`.

> Resource: <https://docs.unity3d.com/ScriptReference/Vector3.Dot.html>

---

### The Cross Product

The **cross product** is a 3D-only operation. It takes two vectors and returns a third vector that is **perpendicular to both**:

```
a × b = (ay·bz − az·by,
         az·bx − ax·bz,
         ax·by − ay·bx)
```

The magnitude of the result equals:

```
|a × b| = |a| |b| sin(θ)
```

The direction follows the **left-hand rule** in Unity (since Unity is left-handed). The cross product is used to find surface normals, determine left/right relative orientation, and build rotation axes.

```csharp
Vector3 a = new Vector3(1f, 0f, 0f); // right
Vector3 b = new Vector3(0f, 1f, 0f); // up

Vector3 c = Vector3.Cross(a, b);
Debug.Log(c); // (0, 0, 1) — forward, perpendicular to both

// Practical use: determine if an enemy is to the left or right of the player
Vector3 toEnemy = (enemy.position - transform.position).normalized;
Vector3 cross   = Vector3.Cross(transform.forward, toEnemy);

if (cross.y > 0f)
    Debug.Log("Enemy is to the right");
else
    Debug.Log("Enemy is to the left");
```

> Resource: <https://docs.unity3d.com/ScriptReference/Vector3.Cross.html>

---

## Multiple Coordinate Spaces

### Why Multiple Spaces?

A single global coordinate system quickly becomes impractical. When a sword is attached to a character's hand, it is far easier to describe the sword's position _relative to the hand_ than relative to the entire world. Unity maintains several coordinate spaces simultaneously:

| Space                    | Description                                                            |
| ------------------------ | ---------------------------------------------------------------------- |
| **World space**          | The global fixed coordinate system. All objects ultimately exist here. |
| **Object (local) space** | Relative to a specific GameObject's own position and orientation.      |
| **Camera space**         | Relative to the camera — used in rendering and screen-space effects.   |

---

### World vs. Local in Unity

```csharp
// World space — absolute position and direction in the scene
Vector3 worldPos = transform.position;
Vector3 worldFwd = transform.forward; // the object's forward in world space

// Local space — position relative to this object's parent
Vector3 localPos = transform.localPosition;

// Convert between spaces
Vector3 worldPoint = transform.TransformPoint(new Vector3(0f, 1f, 0f));  // local → world
Vector3 localPoint = transform.InverseTransformPoint(worldPoint);         // world → local
```

Understanding which space a vector lives in is one of the most common sources of bugs in Unity — always be explicit about whether you are working in world or local space.

> Resource: <https://docs.unity3d.com/ScriptReference/Transform.html>

---

## Useful Mathf Functions

Unity's `Mathf` class provides game-relevant maths utilities. All values are `float`.

```csharp
// Clamp — keep a value within a range (essential for health, speed limits, etc.)
float health = Mathf.Clamp(currentHealth, 0f, 100f);

// Absolute value
float dist = Mathf.Abs(-5f); // 5

// Powers and roots
float squared = Mathf.Pow(4f, 2f); // 16
float root    = Mathf.Sqrt(16f);   // 4

// Rounding
float a = Mathf.Round(3.7f); // 4
float b = Mathf.Floor(3.9f); // 3
float c = Mathf.Ceil(3.1f);  // 4

// Random float in range [min, max)
float roll = Random.Range(0f, 1f);

// Min and Max
float lowest  = Mathf.Min(3f, 7f, 1f); // 1
float highest = Mathf.Max(3f, 7f, 1f); // 7
```

> Resource: <https://docs.unity3d.com/ScriptReference/Mathf.html>

---

## Interpolation

Interpolation computes a value that sits **between** two known values. It is used constantly in games for smooth movement, fading effects, and animation blending.

---

### Linear Interpolation (Lerp)

**Linear interpolation** between values a and b by parameter t (where t ∈ [0, 1]) is:

```
Lerp(a, b, t) = a + t·(b − a) = (1−t)·a + t·b
```

When t = 0 the result is a; when t = 1 the result is b; when t = 0.5 the result is the midpoint.

```csharp
// Scalars
float health = Mathf.Lerp(0f, 100f, 0.25f); // 25 — 25% of the way from 0 to 100

// Vectors — smoothly move toward a target each frame
transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 5f);

// Colours
Color col = Color.Lerp(Color.red, Color.blue, 0.5f); // purple
```

> **Note:** Using `Time.deltaTime * speed` as the t value each frame produces an exponential ease-out (the object slows as it approaches the target) rather than a constant-speed move. For constant speed use `Vector3.MoveTowards` instead.

---

### Smooth Interpolation

`Mathf.SmoothDamp` and `Vector3.SmoothDamp` give a physically natural ease-in/ease-out feel and are the go-to for camera following and UI animations:

```csharp
// SmoothDamp — requires a velocity variable that it manages internally
private Vector3 _velocity = Vector3.zero;

void Update()
{
    transform.position = Vector3.SmoothDamp(
        transform.position, // current
        targetPos,          // target
        ref _velocity,      // internal velocity (managed by Unity)
        0.3f                // approximate time to reach target in seconds
    );
}
```

> Resource: <https://docs.unity3d.com/ScriptReference/Vector3.Lerp.html>

---

## Circular Motion

Uniform circular motion — moving along a circle at constant speed — is one of the most common patterns in games (orbiting cameras, patrol paths, spinning objects). Given a radius **r** and an angle **θ** (in radians), a point on a circle centred at the origin is:

```
x = r · cos(θ)
y = r · sin(θ)
```

The angle advances over time as:

```
θ(t) = θ₀ + ω·t
```

where **ω** (omega) is the **angular velocity** in radians per second.

```csharp
public float radius       = 3f;
public float angularSpeed = 2f; // radians per second

private float _angle = 0f;

void Update()
{
    _angle += angularSpeed * Time.deltaTime;

    float x = radius * Mathf.Cos(_angle);
    float z = radius * Mathf.Sin(_angle);

    transform.position = new Vector3(x, 0f, z); // orbit in the XZ plane
}
```

> Resource: <https://docs.unity3d.com/ScriptReference/Mathf.Sin.html>

---

## Exercises

Create a Unity project and attach a C# script called `Week01Tasks.cs` to a GameObject in your scene. Implement each method below and verify your output in the Unity Console using `Debug.Log()`.

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

Write a method that converts an angle in degrees to radians **without** using `Mathf.Deg2Rad`, then verify against Unity's constant.

Test cases:

- `DegreesToRadians(0f)` should return `0`
- `DegreesToRadians(180f)` should return approximately `3.14159`
- `DegreesToRadians(360f)` should return approximately `6.28318`
- `DegreesToRadians(90f)` should return approximately `1.5708`

```csharp
float DegreesToRadians(float degrees)
{
    // Your code here — use Mathf.PI
}
```

> Hint: The formula is `radians = degrees × (π / 180)`.

---

### Task 2

Write a method that computes the **magnitude** of a 3D vector **without** using `.magnitude` or `Vector3.Distance`.

Test cases:

- `VectorMagnitude(new Vector3(3f, 4f, 0f))` should return `5`
- `VectorMagnitude(new Vector3(0f, 0f, 0f))` should return `0`
- `VectorMagnitude(new Vector3(1f, 1f, 1f))` should return approximately `1.732`

```csharp
float VectorMagnitude(Vector3 v)
{
    // Your code here — use Mathf.Sqrt
}
```

> Hint: `|v| = √(x² + y² + z²)`

---

### Task 3

Write a method that **normalises** a vector without using `.normalized`.

Test cases:

- `Normalise(new Vector3(3f, 4f, 0f))` should return approximately `(0.6, 0.8, 0)`
- `Normalise(new Vector3(0f, 5f, 0f))` should return `(0, 1, 0)`

```csharp
Vector3 Normalise(Vector3 v)
{
    // Your code here — guard against the zero vector
}
```

> Hint: Divide each component by the magnitude. Return `Vector3.zero` if the magnitude is 0.

---

### Task 4

Write a method that computes the **distance** between two 3D points without using `Vector3.Distance`.

Test cases:

- `Distance(new Vector3(1f, 2f, 3f), new Vector3(1f, 2f, 3f))` should return `0`
- `Distance(new Vector3(0f, 0f, 0f), new Vector3(3f, 4f, 0f))` should return `5`

```csharp
float Distance(Vector3 a, Vector3 b)
{
    // Your code here — use your VectorMagnitude method or Mathf.Sqrt
}
```

> Hint: `d = |b − a|`

---

### Task 5

Write a method that computes the **dot product** of two vectors without using `Vector3.Dot`, then uses the result to determine the angle between them.

Test cases:

- `DotProduct(Vector3.right, Vector3.right)` should return `1`
- `DotProduct(Vector3.right, Vector3.up)` should return `0`
- `DotProduct(Vector3.right, Vector3.left)` should return `-1`

```csharp
float DotProduct(Vector3 a, Vector3 b)
{
    // Your code here
}
```

Then write a second method:

```csharp
float AngleBetween(Vector3 a, Vector3 b)
{
    // Use DotProduct to compute the angle in degrees
    // Hint: θ = arccos(a · b) for unit vectors — use Mathf.Acos and Mathf.Clamp
}
```

> Hint: `a · b = ax·bx + ay·by + az·bz`. For the angle, normalise both vectors first and clamp the dot product to [−1, 1] before passing to `Mathf.Acos`.
