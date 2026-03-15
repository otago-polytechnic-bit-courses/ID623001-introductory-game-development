# Week 02.2 - Breakout: Scriptable Objects and UI

## Navigation

|            | Link                                                                                                                                                                                                                                                    |
| ---------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| ← Previous | [Week 02.1 - Breakout: Textures, Prefabs and Input System](../week-02.1-breakout-2-textures-prefabs-prefab-variants-input-system/README.md) |
| → Next     | [Week 03.1. - Audio, Scene Management, Player Prefs and Coroutines](../week-03.1-breakout-3-audio-scene-management-player-prefs-coroutines/README.md)                                           |

---

## 1. Scriptable Objects

A **Scriptable Object** is a data container that stores shared data independently from script instances. They are useful for data that needs to be accessed by multiple objects or scenes - for example, storing the sprite and point value for each brick type in one place rather than duplicating it across every brick instance.

---

### 1.1 Creating a Scriptable Object

**Step 1** - In the `Scripts` folder, right-click and select **Create > Scriptable Object**. Name it `BrickData`. Open it in your code editor and add the following:

```csharp
using UnityEngine;

// CreateAssetMenu lets you right-click in the Project panel to create a BrickData asset
[CreateAssetMenu(fileName = "BrickData", menuName = "Scriptable Objects/BrickData")]
public class BrickData : ScriptableObject
{
    public Sprite sprite;
}
```

**Step 2** - In the `Assets` folder, create a new folder called `ScriptableObjects` with a subfolder called `Brick`. Inside `Brick`, right-click and select **Create > Scriptable Objects > BrickData**. Name the asset `BlueBrickData`. In the Inspector, set the **Sprite** field to the `element_blue_rectangle` sprite from the `Sprites` folder.

**Step 3** - Update the `Brick` script to accept a `BrickData` asset and apply its sprite:

```csharp
using UnityEngine;

public class Brick : MonoBehaviour
{
    private BrickData data;

    // Called by BrickController after Instantiate() - passes in the data for this brick
    public void Initialise(BrickData brickData)
    {
        data = brickData;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = data.sprite;   // apply the sprite defined in the Scriptable Object
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
            Destroy(gameObject);
    }
}
```

**Step 4** - In the `Scripts` folder, create a new script called `BrickController`. Open it and add the following:

```csharp
using UnityEngine;

public class BrickController : MonoBehaviour
{
    [Header("Prefab Settings")]
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private BrickData[] rowData = new BrickData[4];  // one entry per row

    [Header("Grid Settings")]
    [SerializeField] private int   bricksPerRow = 8;
    [SerializeField] private float brickWidth   = 1f;
    [SerializeField] private float brickHeight  = 0.5f;
    [SerializeField] private float padding      = 0.1f;
    [SerializeField] private float topOffset    = 2f;

    [Header("References")]
    [SerializeField] private Transform bricksParent;

    private void Start()
    {
        Camera camera = Camera.main;

        if (camera == null)
        {
            Debug.LogError("Main Camera not found. Ensure a camera is tagged 'MainCamera'.");
            return;
        }

        // Convert the top of the screen from screen space to world space
        float screenTop = camera.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;

        float totalWidth = // TODO: bricksPerRow * (brickWidth + padding) - padding

        float startX = // TODO: centre the grid - (-totalWidth / 2f) + (brickWidth / 2f)

        float startY = // TODO: screenTop - topOffset

        for (int row = 0; row < rowData.Length; row++)
        {
            for (int col = 0; col < bricksPerRow; col++)
            {
                float x = // TODO: startX + col * (brickWidth + padding)

                float y = // TODO: startY - row * (brickHeight + padding)

                GameObject brick = Instantiate(
                    brickPrefab,
                    new Vector2(x, y),
                    Quaternion.identity,
                    bricksParent
                );

                // Pass the row's BrickData to the brick so it can apply the correct sprite
                brick.GetComponent<Brick>()?.Initialise(rowData[row]);

                // Scale the brick sprite to match the configured width and height
                SpriteRenderer sr = brick.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    Vector2 spriteSize = sr.bounds.size;
                    brick.transform.localScale = new Vector2(
                        brickWidth  / spriteSize.x,
                        brickHeight / spriteSize.y
                    );
                }
            }
        }
    }
}
```

**Step 5** - In the Hierarchy panel, create an empty GameObject and name it `Bricks`. Drag and drop the `BrickController` script onto it. In the Inspector, set:

| Field             | Value                                                                       |
| ----------------- | --------------------------------------------------------------------------- |
| **Brick Prefab**  | The `Brick` prefab                                                          |
| **Row Data**      | Set the array size and assign a different `BrickData` asset to each element |
| **Bricks Parent** | The `Bricks` GameObject itself                                              |

**Step 6** - Click **Play**. You should see a grid of bricks, each using the sprite from its assigned `BrickData` asset.

---

## 2. UI

User interfaces let players interact with the game and provide feedback on their actions. Unity's UI system is built around a **Canvas** - the root of all UI elements.

---

### 2.1 Canvas and TextMeshPro

**Step 1** - In the Hierarchy panel, right-click and select **UI (Canvas) > Canvas**. This creates a `Canvas` GameObject that renders all UI elements on screen.

**Step 2** - Right-click the `Canvas` GameObject and select **UI (Canvas) > Text - TextMeshPro**. This creates a TextMeshPro text element as a child of the Canvas. Name it `Score Text`. Import the **TextMeshPro Essentials** package if prompted.

**Step 3** - In the Inspector for `Score Text`, set the **Text** field to `Score: 0`.

**Step 4** - In the `Scripts` folder, create a new script called `UIManager`. Open it and add the following:

```csharp
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI scoreText;

    private int score = 0;

    private void Start()
    {
        scoreText.text = $"Score: {score}";
    }

    // Called by Brick when the ball collides with it
    public void AddScore(int points)
    {
        score += points;
        scoreText.text = $"Score: {score}";
    }
}
```

**Step 5** - In the Hierarchy, create an empty GameObject named `UI Manager`. Attach the `UIManager` script to it. In the Inspector, set the **Score Text** field to the `Score Text` element.

**Step 6** - Update the `Brick` script to find the `UIManager` and call `AddScore` when a brick is destroyed:

```csharp
using UnityEngine;

public class Brick : MonoBehaviour
{
    // Omitted for brevity

    [Header("References")]
    private UIManager uiManager;

    private void Start()
    {
        // FindObjectOfType searches the scene for an active UIManager instance
        uiManager = FindObjectOfType<UIManager>();
    }

    // Omitted for brevity

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // TODO: call uiManager.AddScore() with the brick's point value
            Destroy(gameObject);
        }
    }
}
```

**Step 7** - Click **Play**. The score should increase each time the ball hits a brick.

---

### 2.2 Vertical Layout Group

A **Vertical Layout Group** automatically stacks its child UI elements in a column, adapting to different screen sizes. We will use one to keep the score and lives display neatly aligned.

**Step 1** - In the Hierarchy, select the `Canvas` GameObject, right-click and select **Create Empty**. Name it `Info Container`.

**Step 2** - Select `Info Container`. In the Inspector, click **Add Component** and add a **Vertical Layout Group**. Configure the `Rect Transform` anchor and the layout group padding and spacing to position it on the right side of the screen.

**Step 3** - Drag the `Score Text` element in the Hierarchy onto `Info Container` to make it a child. The Vertical Layout Group will automatically arrange it and any future children.

---

### 2.3 Fonts

Custom fonts set the visual tone of your game. Unity uses **TMP Font Assets** - a compiled format that TextMeshPro can render with high quality at any size.

**Step 1** - Copy the provided `Fonts` folder (found in the `week-02.2` resources) into your project's `Assets` folder.

**Step 2** - Select all `.ttf` font files in the `Fonts` folder, right-click and select **Create > TextMeshPro > Font Asset > SDF**. This generates a `TMP_FontAsset` for each font.

**Step 3** - To assign a font in the Inspector, select `Score Text` in the Hierarchy and set the **Font Asset** field to one of the new `TMP_FontAsset` files.

**Step 4** - To assign a font through code, update `UIManager`:

```csharp
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Settings")]
    // Omitted for brevity
    [SerializeField] private TMP_FontAsset customFont;   // drag a TMP_FontAsset here

    private void Start()
    {
        // Apply the custom font to the score text at runtime
        scoreText.font = customFont;
        // Omitted for brevity
    }

    // Omitted for brevity
}
```

**Step 5** - In the Inspector for the `UI Manager` GameObject, drag a `TMP_FontAsset` from the Project panel onto the **Custom Font** field.

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

### Task 1 - Brick Variety

Create a new `BrickData` Scriptable Object for each brick type below and assign the matching sprite:

| Asset name        | Sprite                     |
| ----------------- | -------------------------- |
| `RedBrickData`    | `element_red_rectangle`    |
| `GreenBrickData`  | `element_green_rectangle`  |
| `YellowBrickData` | `element_yellow_rectangle` |

Assign them to different rows in the `BrickController`'s **Row Data** array and verify the correct sprites appear in Play mode.

---

### Task 2 - Brick Stats

Extend `BrickData` with two new fields and update `Brick` to use them:

| Field        | Type  | Description                                  |
| ------------ | ----- | -------------------------------------------- |
| `pointValue` | `int` | Points awarded when this brick is destroyed  |
| `hitPoints`  | `int` | Number of hits required to destroy the brick |

Update `Brick` so it tracks remaining hit points, decrements on each collision, and only destroys itself (and awards points) when hit points reach zero.

> **Hint:** store the current hit points in a private field initialised from `data.hitPoints` in `Initialise()`.

---

### Task 3 - Lives, Game Over and Win

Add three new UI features to the game:

| Feature               | Description                                                                       |
| --------------------- | --------------------------------------------------------------------------------- |
| **Lives counter**     | Starts at `3`; decrements by `1` each time the ball hits the bottom of the screen |
| **Game over message** | Displayed when lives reach `0`                                                    |
| **Win message**       | Displayed when all bricks are destroyed; include the player's final score         |

> **Hint:** detect the ball hitting the bottom by adding a thin collider tagged `"BottomWall"` along the bottom edge of the screen. Have `UIManager` expose `LoseLife()`, `ShowGameOver()` and `ShowWin(int score)` methods.
