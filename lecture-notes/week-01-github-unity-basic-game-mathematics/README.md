# Week 01

---

## Important Links

| Section      | Link                                                                                     |
| ------------ | ---------------------------------------------------------------------------------------- |
| GitHub       | [GitHub Classroom - ID623002-S1-26](https://classroom.github.com/a/your-assignment-link) |
| Code Example | [Code Example](code-example)                                                             |
| Next Class   | [Week 02](../week-02-unity-physics-collision)                                            |

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

C# is a strongly-typed, object-oriented programming language used as Unity's primary scripting language. All game logic, e.g., movement, collision, animation, UI, etc., are expressed through C# scripts attached to **GameObjects** via **MonoBehaviour**. 

> Resource: <https://docs.unity3d.com/Manual/index.html>

---

### MonoBehaviour Lifecycle

```csharp
using UnityEngine;

public class Demo : MonoBehaviour
{
    void Start()
    {
        // Runs once before the first frame. This is used for initialisation
        Debug.Log("Hello, Unity!");
    }

    void Update()
    {
        // Runs once per frame. This is used for input handling and non-physics updates
    }

    void FixedUpdate()
    {
        // Runs at a fixed physics timestep. This is used for physics updates and consistent movement
    }
}
```

---

## Cartesian Coordinate Systems

### 1D Mathematics

The simplest coordinate system is the **number line**: a single axis with a defined origin (0), a positive direction and a unit of length. Every real number corresponds to exactly one point on this line.

In Unity, individual axes behave like number lines. A GameObject's x-position is a signed scalar value on the x-axis of the world coordinate system.

```csharp
float xPos = transform.position.x;
float yPos = transform.position.y;
float zPos = transform.position.z;
```

---

### 2D Cartesian Space

Two perpendicular number lines - the **x-axis** (horizontal) and **y-axis** (vertical) intersect at the **origin** (0, 0) to form a 2D coordinate system. Any point P in the plane is specified uniquely by an ordered pair **(x, y)**.

In Unity 2D projects positions are represented as `Vector2`:

```csharp
Vector2 playerPos = new Vector2(3f, -1.5f);

Debug.Log($"X: {playerPos.x}, Y: {playerPos.y}"); // X: 3.0, Y: -1.5
```

---

### 3D Cartesian Space

3D space adds a third axis, the **z-axis** perpendicular to both x and y. Every point in 3D is an ordered triple **(x, y, z)**. Unity uses a **left-handed** coordinate system:

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

### Angles, Degrees and Radians

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
float radians = degrees * Mathf.Deg2Rad;
float back = radians * Mathf.Rad2Deg;

Debug.Log($"Degrees: {degrees}, Radians: {radians}, Back to Degrees: {back}"); // Degrees: 90, Radians: 1.5708, Back to Degrees: 90
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
sin²(θ) + cos²(θ) = 1
sin(−θ) = −sin(θ)
cos(−θ) = cos(θ)
```

In Unity these are used constantly. For circular motion, aiming and computing angles between directions:

```csharp
float angle = 45f * Mathf.Deg2Rad;
float s = Mathf.Sin(angle);
float c = Mathf.Cos(angle);

Debug.Log($"sin(45°) = {s:F3}, cos(45°) = {c:F3}"); // sin(45°) = 0.707, cos(45°) = 0.707

// Inverse trig functions return angles in radians
float theta = Mathf.Asin(0.5f)  * Mathf.Rad2Deg;
float phi   = Mathf.Atan2(1f, 1f) * Mathf.Rad2Deg;

Debug.Log($"Asin(0.5) = {theta}°, Atan2(1, 1) = {phi}°"); // Asin(0.5) = 30°, Atan2(1, 1) = 45°
```

> **Use `Mathf.Atan2(y, x)` instead of `Mathf.Atan(y/x)`** — it correctly handles all four quadrants and avoids division by zero when x = 0.

> Resource: <https://docs.unity3d.com/ScriptReference/Mathf.html>

---

## Vectors

### What is a Vector?

A **vector** is a quantity with both **magnitude** (size) and **direction**. This distinguishes it from a **scalar**, which has magnitude only.

- **Scalar example:** The player's health (e.g. 75 HP) is a scalar. It has no direction, just a value.
- **Vector example:** The enemy's velocity (e.g. (3, 0, 0) m/s) is a vector. It has a speed of 3 m/s and points in the positive x direction.

Vectors are written in bold (**v**) or with an arrow (v→). In Unity, `Vector2` and `Vector3` are the built-in vector types.

---

### Vectors vs. Points

This is a conceptually important distinction that trips people up early:

- A **point** describes a **location** in space. It has no length or direction on its own.
- A **vector** describes a **displacement** or **direction**. It has no fixed position.

In Unity both are stored as `Vector3`, but they mean different things depending on context. `transform.position` is a point. `transform.forward` is a vector.

```csharp
Vector3 pointA = new Vector3(1f, 0f, 0f); // A point at (1, 0, 0) in world space
Vector3 pointB = new Vector3(4f, 3f, 0f); // Another point at (4, 3, 0) in world space
Vector3 displacement = pointB - pointA; // A vector from A to B, with magnitude equal to the distance between them

Debug.Log($"Point A: {pointA}, Point B: {pointB}, Displacement: {displacement}");
```

---

### Vector Negation

Negating a vector **reverses its direction** while keeping its magnitude:

```
−v = (−x, −y, −z)
```

Formula breakdown:

1. Each component of the vector v is multiplied by -1, which flips the sign of each component.

```csharp
Vector3 v = new Vector3(1f, -2f, 3f);
Vector3 neg = -v; // (-1, 2, -3)
```

---

### Scalar Multiplication

Multiplying a vector by a scalar **k** scales its length by |k|. If k < 0 the direction is also reversed:

```
k·v = (k·x,  k·y,  k·z)
```

Formula breakdown:

1. Each component of the vector v is multiplied by the scalar k.
2. If k > 1, the vector is stretched (longer). If 0 < k < 1, the vector is shrunk (shorter). If k < 0, the vector is reversed and scaled by |k|.

```csharp
Vector3 forward = Vector3.forward;
Vector3 fast = forward * 10f;
Vector3 back = forward * -1f;

Debug.Log($"Forward: {forward}, Fast: {fast}, Back: {back}"); // Forward: (0, 0, 1), Fast: (0, 0, 10), Back: (0, 0, -1)
```

---

### Vector Addition and Subtraction

Vectors add and subtract **component-wise**. Geometrically, addition places vectors head-to-tail; the result is the **resultant** vector:

```
a + b = (ax + bx,  ay + by,  az + bz)
a − b = (ax − bx,  ay − by,  az − bz)
```

Forumla breakdown:

1. For addition, add the corresponding components of vectors a and b to get the components of the resultant vector.
2. For subtraction, subtract the corresponding components of vector b from vector a to get the components of the resultant vector.

The vector **from point A to point B** is always `B − A`:

```csharp
Vector3 A = new Vector3(1f, 0f, 0f);
Vector3 B = new Vector3(4f, 3f, 0f);
Vector3 AtoB = B - A;

Debug.Log($"Vector from A to B: {AtoB}"); // Vector from A to B: (3, 3, 0)
```

---

### Vector Magnitude (Length)

The magnitude (length) of a vector is calculated using the **Pythagorean theorem** extended to 3D:

```
|v| = √(x² + y² + z²)
```

Formula breakdown:

1. Square each component of the vector to get x², y² and z².
2. Sum these squared components to get x² + y² + z².
3. Take the square root of this sum to get the magnitude |v|.

```csharp
Vector3 v = new Vector3(3f, 4f, 0f);
float mag = v.magnitude;
float magSq = v.sqrMagnitude;

Debug.Log($"Magnitude: {mag}, Squared Magnitude: {magSq}"); // Magnitude: 5, Squared Magnitude: 25
```

---

### Unit Vectors and Normalisation

A **unit vector** has a magnitude of exactly 1. It expresses a **pure direction** with no scale information. Any non-zero vector can be normalised by dividing by its magnitude:

```
v̂ = v / |v|
```

Formula breakdown:

1. Calculate the magnitude |v| of the vector v.
2. Divide each component of v by its magnitude to get the normalised vector v̂.

```csharp
Vector3 v = new Vector3(3f, 4f, 0f);
Vector3 vHat = v.normalized;
Vector3 dir = (target.position - transform.position).normalized;
transform.position += dir * speed * Time.deltaTime;

Debug.Log($"Original: {v}, Normalised: {vHat}"); // Original: (3, 4, 0), Normalised: (0.6, 0.8, 0)
```

> **Note:** Never normalise the zero vector. It has no direction. `Vector3.zero.normalized` returns `Vector3.zero` in Unity but mathematically is undefined. Check `v.magnitude > 0f` before normalising if the vector may be zero.

---

### The Distance Formula

The distance between two points P and Q is the magnitude of the displacement vector between them:

```
d(P, Q) = |Q − P| = √((Qx−Px)² + (Qy−Py)² + (Qz−Pz)²)
```

Formula breakdown:

1. Subtract the coordinates of P from Q to get the displacement vector Q − P.
2. Calculate the magnitude of this displacement vector using the formula for vector magnitude.

```csharp
Vector3 P = new Vector3(1f, 2f, 3f);
Vector3 Q = new Vector3(4f, 6f, 3f);
float dist = Vector3.Distance(P, Q);

Debug.Log($"Distance from P to Q: {dist}"); // Distance from P to Q: 5 
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
    Debug.Log("Enemy is in front");
else if (dot < 0f)
    Debug.Log("Enemy is behind");

float angle = Mathf.Acos(Mathf.Clamp(dot, -1f, 1f)) * Mathf.Rad2Deg;

Debug.Log($"Angle to enemy: {angle}°");
```

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

The direction follows the **left-hand rule** in Unity. The cross product is used to find surface normals, determine left/right relative orientation and build rotation axes.

```csharp
Vector3 a = new Vector3(1f, 0f, 0f);
Vector3 b = new Vector3(0f, 1f, 0f);
Vector3 c = Vector3.Cross(a, b);

Debug.Log($"Cross Product: {c}"); // Cross Product: (0, 0, 1)

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

| Space            | Description                                                            |
| ---------------- | ---------------------------------------------------------------------- |
| **World space**  | The global fixed coordinate system. All objects ultimately exist here. |
| **Local space**  | Relative to a specific GameObject's own position and orientation.      |
| **Camera space** | Relative to the camera — used in rendering and screen-space effects.   |

---

### World vs. Local in Unity

```csharp
Vector3 worldPos = transform.position;
Vector3 worldFwd = transform.forward;
Vector3 localPos = transform.localPosition;
Vector3 worldPoint = transform.TransformPoint(new Vector3(0f, 1f, 0f));
Vector3 localPoint = transform.InverseTransformPoint(worldPoint);

Debug.Log($"World Position: {worldPos}, Local Position: {localPos}, Local Point: {localPoint}"); // World Position: (x, y, z), Local Position: (0, 0, 0)
```

Understanding which space a vector lives in is one of the most common sources of bugs in Unity. Always be explicit about whether you are working in world or local space.

> Resource: <https://docs.unity3d.com/ScriptReference/Transform.html>

---

## Useful Mathf Functions

Unity's `Mathf` class provides game-relevant maths utilities. All values are `float`.

```csharp
float health = Mathf.Clamp(currentHealth, 0f, 100f);
float dist = Mathf.Abs(-5f);
float squared = Mathf.Pow(4f, 2f);
float root = Mathf.Sqrt(16f);
float a = Mathf.Round(3.7f);
float b = Mathf.Floor(3.9f);
float c = Mathf.Ceil(3.1f);
float roll = Random.Range(0f, 1f);
float lowest = Mathf.Min(3f, 7f, 1f);
float highest = Mathf.Max(3f, 7f, 1f);

Debug.Log($"Clamped Health: {health}, Abs: {dist}, Pow: {squared}, Sqrt: {root}, Round: {a}, Floor: {b}, Ceil: {c}, Random: {roll}, Min: {lowest}, Max: {highest}"); // Clamped Health: (clamped value), Abs: 5, Pow: 16, Sqrt: 4, Round: 4, Floor: 3, Ceil: 4, Random: (random value), Min: 1, Max: 7
```

> Resource: <https://docs.unity3d.com/ScriptReference/Mathf.html>

---

## Interpolation

Interpolation computes a value that sits **between** two known values. It is used constantly in games for smooth movement, fading effects and animation blending.

---

### Linear Interpolation (Lerp)

**Linear interpolation** between values a and b by parameter t (where t ∈ [0, 1]) is:

```
Lerp(a, b, t) = a + t·(b − a) = (1−t)·a + t·b
```

When t = 0 the result is a; when t = 1 the result is b; when t = 0.5 the result is the midpoint.

```csharp
float health = Mathf.Lerp(0f, 100f, 0.25f);
transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 5f);
Color col = Color.Lerp(Color.red, Color.blue, 0.5f);

Debug.Log($"Health: {health}, Position: {transform.position}, Colour: {col}"); // Health: 25, Position: (interpolated position), Colour: (0.5, 0, 0.5)
```

> **Note:** Using `Time.deltaTime * speed` as the t value each frame produces an exponential ease-out rather than a constant-speed move. For constant speed use `Vector3.MoveTowards` instead.

---

### Smooth Interpolation

`Mathf.SmoothDamp` and `Vector3.SmoothDamp` give a physically natural ease-in/ease-out feel and are the go-to for camera following and UI animations:

```csharp
private Vector3 velocity = Vector3.zero; // SmoothDamp requires a reference velocity variable that it updates each frame

void Update()
{
    transform.position = Vector3.SmoothDamp(
        transform.position, // Current position
        targetPos,          // Target position
        ref velocity,       // Reference to velocity, modified by SmoothDamp
        0.3f                // Smooth time, i.e. how long it takes to reach the target approximately
    );
}
```

> Resource: <https://docs.unity3d.com/ScriptReference/Vector3.Lerp.html>

---

## Circular Motion

Uniform circular motion, i.e., moving in a circle at constant speed, is one of the most common patterns in games. Given a radius **r** and an angle **θ** (in radians), a point on a circle centred at the origin is:

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
public float radius = 3f;
public float angularSpeed = 2f; // Radians per second

private float _angle = 0f;

void Update()
{
    _angle += angularSpeed * Time.deltaTime;
    float x = radius * Mathf.Cos(_angle);
    float z = radius * Mathf.Sin(_angle);
    transform.position = new Vector3(x, 0f, z); // Moves the object in a circle on the XZ plane
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
    // Write your code here
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
    // Write your code here
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
    // Write your code here
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
    // Write your code here
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
    // Write your code here
}
```

Then write a second method:

```csharp
float AngleBetween(Vector3 a, Vector3 b)
{
    // Write your code here
}
```

> Hint: `a · b = ax·bx + ay·by + az·bz`. For the angle, normalise both vectors first and clamp the dot product to [−1, 1] before passing to `Mathf.Acos`.
