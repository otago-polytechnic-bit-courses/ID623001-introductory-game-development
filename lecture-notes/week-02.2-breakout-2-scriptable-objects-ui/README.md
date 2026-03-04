# Week 02.2

---

## Important Links

| Section        | Link                                                                                                    |
| -------------- | ------------------------------------------------------------------------------------------------------- |
| Previous Class | [Week 02.1](lecture-notes/week-02.1-breakout-2-textures-prefabs-prefab-variants-input-system/README.md) |
| Next Class     | [Week 03]()                                                                                             |

---

## Scriptable Objects

A Scriptable Object is a data container that allows you to store large amounts of shared data independent from script instances. They are useful for storing data that needs to be accessed by multiple objects or scenes.

1. In the `Scripts` folder, right-click and select **Create > Scriptable Object**. Name it `BrickData`. Double-click the script to open it in your code editor and add the following code:

```csharp
using UnityEngine;

[CreateAssetMenu(fileName = "BrickData", menuName = "Scriptable Objects/BrickData")]
public class BrickData : ScriptableObject
{
    public Sprite sprite;
}
```

2. In the `Assets` folder, create a new folder called `ScriptableObjects` and a new subfolder called `Brick`. In the `Brick` folder, right-click and select **Create > Scriptable Objects > BrickData**. Name the new asset `BlueBrickData`. In the Inspector panel for the `BlueBrickData` asset, set the **Sprite** field to the `element_blue_rectangle` sprite from the `Sprites` folder.

![](<../../resources (ignore)/img/week-02.2-breakout-2-scriptable-objects-ui/00.png>)

3. Update the `Brick` script to use the `BrickData` Scriptable Object to set the sprite of the brick. You can do this by adding a public field for the `BrickData` Scriptable Object and then setting the sprite of the `Sprite Renderer` component in the `Start` method. Here is an example of how you can update the `Brick` script:

```csharp
using UnityEngine;

public class Brick : MonoBehaviour
{
    private BrickData data;

    public void Initialise(BrickData brickData)
    {
        data = brickData;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = data.sprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
            Destroy(gameObject);
    }
}
```

4. In the `Assets` folder, create a new script called `BrickController`. Open the script in your code editor and add the following code:

```csharp
using UnityEngine;

public class BrickController : MonoBehaviour
{
    [Header("Prefab Settings")]
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private BrickData[] bricks = new BrickData[4];

    [Header("Grid Settings")]
    [SerializeField] private int bricksPerRow = 8;
    [SerializeField] private float brickWidth = 1f;
    [SerializeField] private float brickHeight = 0.5f;
    [SerializeField] private float padding = 0.1f;
    [SerializeField] private float topOffset = 2f;

    [Header("References")]
    [SerializeField] private Transform bricksParent;

    private void Start()
    {
        Camera camera = Camera.main;

        if (camera == null)
        {
            Debug.LogError("Main Camera not found. Please ensure there is a camera tagged as 'MainCamera' in the scene");
            return;
        }

        float screenTop = camera.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;

        float totalWidth = // TODO: Calculate the total width of the grid of bricks based on the number of bricks per row, the width of each brick and the padding between bricks

        float startX = // TODO: Calculate the starting X position based on the total width and the camera's view

        float startY = // TODO: Calculate the starting Y position based on the top of the screen and the top offset

        for (int row = 0; row < bricks.Length; row++)
        {
            for (int col = 0; col < bricksPerRow; col++)
            {
                float x = // TODO: Calculate the X position based on the starting X position, the column index, the width of each brick and the padding between bricks

                float y = // TODO: Calculate the Y position based on the starting Y position, the row index, the height of each brick and the padding between bricks

                GameObject brick = Instantiate(brickPrefab, new Vector2(x, y), Quaternion.identity, bricksParent);

                Brick brickScript = brick.GetComponent<Brick>();
                brickScript?.Initialise(rowData[row]);

                SpriteRenderer sr = brick.GetComponent<SpriteRenderer>();

                if (sr != null)
                {
                    Vector2 spriteSize = sr.bounds.size;
                    brick.transform.localScale = new Vector2(
                        brickWidth / spriteSize.x,
                        brickHeight / spriteSize.y
                    );
                }
            }
        }
    }
}
```

5. In the Hierarchy panel, create an empty GameObject and name it `Bricks`. Drag and drop the `BrickController` script from the Project panel onto the `Bricks` GameObject in the Hierarchy panel. In the Inspector panel for the `Bricks` GameObject, you should see a new component called **Brick Controller** with several fields. Set the **Brick Prefab** field to the `Brick` prefab. Set the **Bricks** array size to `2` and set each element to a different `BrickData` Scriptable Object that you have created.

![](<../../resources (ignore)/img/week-02.2-breakout-2-scriptable-objects-ui/01.png>)

6. Click the **Play** button at the top of the Unity Editor to run the game. You should see a grid of bricks with different sprites based on the `BrickData` Scriptable Objects that you assigned to each brick.

![](<../../resources (ignore)/img/week-02.2-breakout-2-scriptable-objects-ui/02.png>)

---

## UI

User interfaces (UI) are an important part of any game. They allow players to interact with the game and provide feedback on their actions.

---

### Canvas and TextMeshPro

Canvases are the root of all UI elements in Unity. They are responsible for rendering UI elements on the screen. TextMeshPro is a powerful text rendering system that allows you to create high-quality text with advanced formatting options.

1. In the Hierarchy panel, right-click and select **UI > Canvas**. This will create a new `Canvas` GameObject in the scene. The Canvas is the root of all UI elements and is responsible for rendering them on the screen.
2. With the `Canvas` GameObject selected, right-click on it in the Hierarchy panel and select **UI > Text - TextMeshPro**. This will create a new TextMeshPro text element as a child of the Canvas. This text element will be used to display the player's score. You may need to import the **TextMeshPro Essentials** package and **TextMeshPro Examples and Extras** package if you have not already done so. Name the TextMeshPro text element `Score Text`.

![](<../../resources (ignore)/img/week-02.2-breakout-2-scriptable-objects-ui/03.png>)

3. In the Inspector panel for the new TextMeshPro text element, set the **Text** field to `Score: 0`. This will be the initial text that is displayed on the screen.
4. In the `Assets` folder, create a new script called `UIManager`. Open the script in your code editor and add the following code:

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

    public void AddScore(int points)
    {
        score += points;
        scoreText.text = $"Score: {score}";
    }
}
```

5. In the Hierarchy panel, create an empty GameObject and name it `UI Manager`. Drag and drop the `UIManager` script from the Project panel onto the `UI Manager` GameObject in the Hierarchy panel. In the Inspector panel for the `UI Manager` GameObject, you should see a new component called **UI Manager** with a field for **Score Text**. Set this field to the `Score Text` TextMeshPro text element that you created earlier.

![](<../../resources (ignore)/img/week-02.2-breakout-2-scriptable-objects-ui/04.png>)

6. In the `Brick` script, add a reference to the `UIManager` and update the score when the ball collides with a brick. Here is an example of how you can update the `Brick` script:

```csharp
using UnityEngine;

public class Brick : MonoBehaviour
{
    // Omitted for brevity

    [Header("References")]
    private UIManager uiManager;

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    // Omitted for brevity

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // TODO: Add score
            Destroy(gameObject);
        }
    }
}
```

7. Click the **Play** button at the top of the Unity Editor to run the game. When the ball collides with a brick, the score should increase by the amount of points.

![](<../../resources (ignore)/img/week-02.2-breakout-2-scriptable-objects-ui/05.png>)

---

### Vertical Layout Group

A Vertical Layout Group is a component that automatically arranges its child elements in a vertical column. It is useful for creating UI layouts that need to adapt to different screen sizes and resolutions. We will use a Vertical Layout Group to display the player's score and lives on the right side of the screen.

1. In the Hierarchy panel, select the `Canvas` GameObject. Right-click on it and select **UI > Panel**. This will create a new `Panel` GameObject as a child of the Canvas. Name the new panel `Top Right Panel`.
2. Select the `Top Right Panel` GameObject in the Hierarchy panel. In the Inspector panel, click the **Add Component** button and search for **Vertical Layout Group**. Add the `Vertical Layout Group` component to the `Top Right Panel` GameObject. Set the `Rect Transform` and `Vertical Layout Group` properties as follows:

![](<../../resources (ignore)/img/week-02.2-breakout-2-scriptable-objects-ui/06.png>)

3. Drag and drop the `Score Text` TextMeshPro text element from the Hierarchy panel onto the `Top Right Panel` GameObject in the Hierarchy panel. This will make `Score Text` a child of `Top Right Panel` and it will be automatically arranged by the `Vertical Layout Group`.

---

### Fonts

Fonts are an important part of any UI. They can help to set the tone and style of the game. You can use custom fonts in Unity by importing them into your project and then assigning them to your UI elements.

1. In the `week-02.2-breakout-2-scriptable-objects-ui` folder, there is a `Fonts` folder with a variety of different fonts. Copy and paste the `Fonts` folder into the `Assets` folder of your Unity project.

![](<../../resources (ignore)/img/week-02.2-breakout-2-scriptable-objects-ui/07.png>)

2. The fonts are in `.ttf` format, which is a common font format that can be used in Unity. Select all the font files in the `Fonts` folder, right-click and select **Create > TextMeshPro > Font Asset > SDF**. This will create a new `TMP_FontAsset` for each font file.

![](<../../resources (ignore)/img/week-02.2-breakout-2-scriptable-objects-ui/08.png>)

3. You should now have a new `TMP_FontAsset` for each font file in the `Fonts` folder.

![](<../../resources (ignore)/img/week-02.2-breakout-2-scriptable-objects-ui/09.png>)

4. You can assign these font assets to your TextMeshPro text elements to change their appearance. For example, you can assign a custom font to the `Score Text` TextMeshPro text element by selecting it in the Hierarchy panel and then setting the **Font Asset** field in the Inspector panel to one of the new `TMP_FontAsset` files that you created.

![](<../../resources (ignore)/img/week-02.2-breakout-2-scriptable-objects-ui/10.png>)

5. You can also assign the custom font to the `Score Text` TextMeshPro text element through code by adding a reference to the `TMP_FontAsset` in the `UIManager` script and then setting the font in the `Start()` method. Here is an example of how you can update the `UIManager` script:

```csharp
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Settings")]
    // Omitted for brevity
    [SerializeField] private TMP_FontAsset customFont;

    private void Start()
    {
        scoreText.font = customFont;
        // Omitted for brevity
    }

    // Omitted for brevity
}
```

6. Drag and drop a `TMP_FontAsset` from the Project panel onto the **Custom Font** field in the Inspector panel for the `UI Manager` GameObject.

![](<../../resources (ignore)/img/week-02.2-breakout-2-scriptable-objects-ui/11.png>)

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

Create a new `BrickData` Scriptable Object for each of the different types of bricks in the game. For example, you can create:

- Red brick with the `element_red_rectangle` sprite
- Green brick with the `element_green_rectangle` sprite
- Yellow brick with the `element_yellow_rectangle` sprite

---

### Task 2

Update the `BrickData` script to include:

- A point value field that represents how many points the player gets for destroying the brick
- A hit points field that represents how many times the brick needs to be hit before it is destroyed

Update the `Brick` script to use these new fields.

---

### Task 3

Add new UI elements for the following:

- **Lives** — set the initial value to `3` and decrease it by `1` each time the ball collides with the bottom of the screen
- **Game over message** — displayed when the player runs out of lives
- **Win message** — displayed when the player destroys all the bricks; include the player's score in the win message
