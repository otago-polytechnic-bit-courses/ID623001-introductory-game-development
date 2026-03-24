## 1. Rogue-Like Game

In this module, you will develop a **Rogue-Like** game using **Unity**. Create a new Unity project. Name your project `rogue-like` and select a location to save it. Click the **Create project** button.

---

## 2. Spritesheet

A **spritesheet** is a collection of images combined into a single image file. It is used to reduce the number of draw calls in a game, which can improve performance.

**Step 1** - In the **lecture-notes > rogue-like** folder, find the zip folder called `assets`. Download and extract it, then copy the folders into the `Assets` folder of your Unity project.

![](../../resources%20(ignore)/img/07-images/07-image-1.png)

**Step 2** - In the **Assets > Art > Characters** folder, click on the `Characters` spritesheet. In the Inspector panel, configure the following settings:

| Property         | Value               | Reason                                                        |
| ---------------- | ------------------- | ------------------------------------------------------------- |
| `Sprite Mode`    | `Multiple`          | Allows the spritesheet to be sliced into individual sprites   |
| `Pixels Per Unit`| `16`                | Matches the pixel art scale to one Unity world unit           |
| `Filter Mode`    | `Point (no filter)` | Prevents blurring on pixel art sprites                        |
| `Max Size`       | `64`                | Scales down the texture if it exceeds this size               |

![](../../resources%20(ignore)/img/07-images/07-image-2.png)

**Step 3** - Click the **Sprite Editor** button in the Inspector panel to open the Sprite Editor window.

![](../../resources%20(ignore)/img/07-images/07-image-3.png)

**Step 4** - In the Sprite Editor window, click the **Slice** button. In the Slice popup, configure the following:

| Property     | Value               |
| ------------ | ------------------- |
| `Type`       | `Grid By Cell Size` |
| `Pixel Size` | `16`                |

Click **Slice**, then click **Apply** in the top-right corner. You should now see individual sprites in the Project panel.

![](../../resources%20(ignore)/img/07-images/07-image-4.png)

**Step 5** - Drag a character sprite into the Scene window. This creates a new GameObject in the Hierarchy.

![](../../resources%20(ignore)/img/07-images/07-image-5.png)

**Step 6** - Drag a gun sprite into the Scene window. Create an **Empty GameObject** in the Hierarchy and name it `Player`. Drag both the character sprite and gun sprite onto the `Player` GameObject to make them its children.

> **Note:** You do not need to slice the gun spritesheet — the gun sprites are already ready to use.

![](../../resources%20(ignore)/img/07-images/07-image-6.png)

---

## 3. Sorting Layers

**Sorting Layers** determine the render order of sprites. Sprites on a higher sorting layer appear in front of sprites on a lower layer.

**Step 1** - In the Inspector panel, create a new sorting layer called `Player`. You should now have two sorting layers: `Default` and `Player`.

![](../../resources%20(ignore)/img/07-images/07-image-7.png)

**Step 2** - Select the character sprite child of the `Player` GameObject. In the Inspector panel, set **Sprite Renderer > Additional Settings > Sorting Layer** to `Player` and **Order in Layer** to `0`.

![](../../resources%20(ignore)/img/07-images/07-image-8.png)

**Step 3** - Select the gun sprite child. Apply the same `Player` sorting layer, but set **Order in Layer** to `1`. This ensures the gun renders on top of the character sprite.

![](../../resources%20(ignore)/img/07-images/07-image-9.png)

---

## 4. Physics Components

**Step 1** - Select the `Player` GameObject in the Hierarchy. Add a `CircleCollider2D` component to define the player's collision shape.

![](../../resources%20(ignore)/img/07-images/07-image-11.png)

**Step 2** - Add a `Rigidbody2D` component to the `Player` GameObject to enable physics simulation. In Unity 6, configure the following:

| Property        | Value      | Reason                                  |
| --------------- | ---------- | --------------------------------------- |
| `Gravity Scale` | `0`        | Prevents the player from falling        |
| `Body Type`     | `Dynamic`  | Allows physics-driven movement          |

![](../../resources%20(ignore)/img/07-images/07-image-12.png)

---

## 5. PlayerController Script

**Step 1** - In the `Assets` folder, create a new folder called `Scripts`. Inside `Scripts`, create a new C# script called `PlayerController` and attach it to the `Player` GameObject.

![](../../resources%20(ignore)/img/07-images/07-image-10.png)

---

## 6. MonoBehaviour Lifecycle

Unity calls special methods on a `MonoBehaviour` at defined points during the game's lifetime. The most important ones are:

| Method          | When it's called                                      | Typical use                                       |
| --------------- | ----------------------------------------------------- | ------------------------------------------------- |
| `Awake()`       | When the script instance is loaded — before `Start()` | Initialise component references (`GetComponent`)  |
| `Start()`       | Before the first frame update                         | Set up initial state (position, velocity, colour) |
| `Update()`      | Once per frame                                        | Input handling, non-physics movement              |
| `FixedUpdate()` | At a fixed time interval (default 50×/sec)            | Physics updates — always use this for `Rigidbody` |

> The frame rate affects `Update()` but never `FixedUpdate()`. Always apply forces and velocity changes in `FixedUpdate()` to keep physics deterministic.

📖 Reference: [Unity — MonoBehaviour](https://docs.unity3d.com/ScriptReference/MonoBehaviour.html)

---

## 7. Unity 6 Physics Note

In Unity 6, `Rigidbody2D.velocity` has been renamed to `Rigidbody2D.linearVelocity`. Always use `linearVelocity` when setting or reading a Rigidbody2D's velocity:

```csharp
// Unity 6 — correct
rb.linearVelocity = direction * speed;

// Legacy (Unity 2022 and earlier) — avoid
rb.velocity = direction * speed;
```

---

## Exercises

Learning to use AI tools is an important skill. While AI tools are powerful, you must be aware of the following:

- Refine your prompts — vague prompts yield vague responses
- Validate AI output — don't trust it blindly
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

### Task 1 — Player Movement

Configure the player to move using either the **WASD** keys or the **Arrow** keys. The player should move in the direction of the key pressed.

---

### Task 2 — Dash

In the `PlayerController` script, write the code to dash the player using the **Space** key. The player should dash in the direction it is currently moving. The dash should last `0.5` seconds and have a cooldown of `2` seconds.

> **Note:** The player should not be able to dash again until the cooldown has expired.

> **Hint:** Use a `Coroutine` to handle the dash duration and cooldown. Use a `bool` flag to track whether a dash is available.

📖 Reference: [Unity — Coroutines](https://docs.unity3d.com/Manual/Coroutines.html)

---

### Task 3 — Weapon Swap

Write the code that allows the player to swap between two weapons using the **Q** key. The player should be able to toggle between a gun and a sword.

> **Hint:** Store references to both weapon GameObjects and toggle `SetActive(true)` / `SetActive(false)` on each when **Q** is pressed.