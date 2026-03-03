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

1. In the Scripts folder, right-click and select "Create > Scriptable Object". Name it "BrickData". Double-click the script to open it in your code editor and add the following code:

```csharp
using UnityEngine;

[CreateAssetMenu(fileName = "BrickData", menuName = "Scriptable Objects/BrickData")]
public class BrickData : ScriptableObject
{
    public Sprite sprite;
}
```

2. In the Assets folder, create a new folder called "ScriptableObjects" and a new subfolder called "Brick". In the Brick folder, right-click and select "Create > Scriptable Objects > BrickData". Name the new asset "BlueBrickData". In the Inspector panel for the BlueBrickData asset, set the "Sprite" field to the "element_blue_rectangle" sprite from the Sprites folder.

![](<../../resources (ignore)/img/week-02.2-breakout-2-scriptable-objects-ui-audio-scene-management-player-prefs/00.png>)

3. Update the Brick script to use the BrickData Scriptable Object to set the sprite of the brick. You can do this by adding a public field for the BrickData Scriptable Object and then setting the sprite of the Sprite Renderer component in the Start method. Here is an example of how you can update the Brick script:

```csharp
using UnityEngine;

public class Brick : MonoBehaviour
{
    private BrickData data;

    public void Initialize(BrickData brickData)
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

4. In the Assets folder, create a new script called "BrickController". Open the script in your code editor and add the following code:

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
                brickScript?.Initialize(rowData[row]);
            }
        }
    }
}
```

5. In the Hierarchy panel, create an empty GameObject and name it "Bricks". Drag and drop the `BrickController` script from the Project panel onto the Bricks GameObject in the Hierarchy panel. In the Inspector panel for the Bricks GameObject, you should see a new component called "Brick Controller" with several fields. Set the "Brick Prefab" field to the Brick prefab. Set the "Bricks" array size to 2 and set each element to a different BrickData Scriptable Object that you have created.

![](<../../resources (ignore)/img/week-02.2-breakout-2-scriptable-objects-ui-audio-scene-management-player-prefs/01.png>)

6. Click the Play button at the top of the Unity Editor to run the game. You should see a grid of bricks with different sprites based on the BrickData Scriptable Objects that you assigned to each brick.

![](<../../resources (ignore)/img/week-02.2-breakout-2-scriptable-objects-ui-audio-scene-management-player-prefs/02.png>)

---

## UI

Unity's UI system allows you to display information on the screen such as scores, health bars, menus, etc. It is built on top of the Canvas system, which is a special type of GameObject that renders UI elements.

1. In the Hierarchy panel, right-click and select "UI > Canvas". This will create a new Canvas GameObject in the scene. The Canvas is the root of all UI elements and is responsible for rendering them on the screen.
2. With the Canvas GameObject selected, right-click on it in the Hierarchy panel and select "UI > Text - TextMeshPro". This will create a new TextMeshPro text element as a child of the Canvas. This text element will be used to display the player's score. You may need to import the TextMeshPro Essentials package and, TextMeshPro Examples and Extras package if you have not already to access the TextMeshPro text element. Name the TextMeshPro text element "Score Text".

![](<../../resources (ignore)/img/week-02.2-breakout-2-scriptable-objects-ui-audio-scene-management-player-prefs/03.png>)

3. In the Inspector panel for the new TextMeshPro text element, set the "Text" field to "Score: 0". This will be the initial text that is displayed on the screen.
4. In the Assets folder, create a new script called "UIManager". Open the script in your code editor and add the following code:

```csharp
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;

    public void AddScore(int points)
    {
        score += points;
        scoreText.text = $"Score: {score}";
    }
}
```

5. In the Hierarchy panel, create an empty GameObject and name it "UI Manager". Drag and drop the `UIManager` script from the Project panel onto the UI Manager GameObject in the Hierarchy panel. In the Inspector panel for the UI Manager GameObject, you should see a new component called "UI Manager" with a field for "Score Text". Set this field to the TextMeshPro text element that you created earlier.

6. In the Ball script, add a reference to the UIManager and update the score when the ball collides with a brick. Here is an example of how you can update the Ball script:

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

7. Click the Play button at the top of the Unity Editor to run the game. When the ball collides with a brick, the score should increase by the amount of points. 

![](<../../resources (ignore)/img/week-02.2-breakout-2-scriptable-objects-ui-audio-scene-management-player-prefs/05.png>)

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

Create three new BrickData Scriptable Objects for different types of bricks, e.g. RedBrickData, GreenBrickData, YellowBrickData and set their sprites to different brick sprites from the Sprites folder. Update the BrickController to use these new BrickData Scriptable Objects to create a more varied grid of bricks in the scene.

---

### Task 2

Update the `BrickData` Scriptable Object script to include two new fields for point value and hit points. The point value field will represent how many points the player gets for destroying the brick, and the hit points field will represent how many times the brick needs to be hit before it is destroyed. Update the `Brick` script to use these new fields to determine how many points to add to the score when a brick is hit and when it is destroyed. 

---

### Task 3

Add a new UI element to display the player's remaining lives. When the ball falls below the paddle and is destroyed, decrease the player's lives by one and update the UI element to reflect the new number of lives. 

---

### Task 4

Add a new UI element to display a "Game Over" message when the player runs out of lives. When the player's lives reach zero, display the "Game Over" message and stop the game from running.

---

### Task 5

Add a new UI element to display a "You Win!" message when the player destroys all the bricks. When there are no more bricks in the scene, display the "You Win!" message and stop the game from running.
