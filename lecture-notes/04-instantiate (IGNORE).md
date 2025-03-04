## Week 04

## Previous Class

Link to the previous class: [Week 03]()

---

## Breakout

In this module, you will continue to develop **Breakout** using Unity.

> **Note:** The following content does not take into account last week's formative assessment. Please ensure you have completed the formative assessment before continuing with this week's content. 

---

## Instantiate Bricks

Last week, you created `Brick`, `Brick1`, and `Brick2` prefabs. You then added these **Prefabs** to the scene. This is not a scalable solution. If you want to add more bricks to the scene, you would have to manually add them to the scene. You can use **Instantiate** to create bricks at runtime.

Delete all the `Brick` **GameObjects** from your scene.

![](../resources/img/04-images/04-image-1.png)

In the **Assets > Scripts** folder, create a new folder called `Brick`. In the `Brick` folder, create a new script called `BrickSpawner`. 

![](../resources/img/04-images/04-image-2.png)


In the `BrickSpawner` script, add the following code:

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] brickPrefabs;
    [SerializeField] private int rows = 3;
    [SerializeField] private int columns = 3;

    void Start()
    {
        GenerateBasicFormation();
    }

    private void GenerateBasicBrickFormation()
    {
        GameObject firstBrick = brickPrefabs[0];
        float brickWidth = firstBrick.GetComponent<SpriteRenderer>().bounds.size.x;
        float brickHeight = firstBrick.GetComponent<SpriteRenderer>().bounds.size.y;

        float gap = 0.5f;
        float totalWidth = columns * brickWidth + (columns - 1) * gap;
        
        float startX = -totalWidth / 2 + brickWidth / 2;
        float startY = 5f - (brickHeight / 2) - 0.5f;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int randomIndex = Random.Range(0, brickPrefabs.Length);
                float xPosition = startX + col * (brickWidth + gap);
                float yPosition = startY - row * (brickHeight + gap);
                Vector2 position = new Vector2(xPosition, yPosition);
                Instantiate(brickPrefabs[randomIndex], position, Quaternion.identity);
            }
        }
    }
}
```

Let's break down the code:

1. `GameObject firstBrick = brickPrefabs[0];` - We get the first `Brick` **Prefab** to calculate the width and height of the bricks.
2. `float brickWidth = firstBrick.GetComponent<SpriteRenderer>().bounds.size.x;` - This is the width of the `Brick` **Prefab**.
3. `float brickHeight = firstBrick.GetComponent<SpriteRenderer>().bounds.size.y;` - This is the height of the `Brick` **Prefab**.
4. `float gap = 0.5f;` - This is the gap between the bricks.
5. `float totalWidth = columns * brickWidth + (columns - 1) * gap;` - This is the total width of the bricks.
6. `float startX = -totalWidth / 2 + brickWidth / 2;` - This is the starting X position of the bricks. We subtract `totalWidth / 2` to align the bricks correctly. We then add `brickWidth / 2` to align the bricks correctly.
7. `float startY = 5f - (brickHeight / 2) - 0.5f;` - This is the starting Y position of the bricks. The `5f` is the height of the `TopWall` **GameObject**. We subtract `brickHeight / 2` to align the bricks correctly. We then subtract `0.5f` to give some space between the bricks and the `TopWall` **GameObject**.
8. Loop through the rows and columns to create the bricks. We calculate the X and Y positions of the bricks. We then instantiate the bricks at the calculated positions.

Attach the `BrickSpawner` script to the `Bricks` **GameObject**. Set the `Brick Prefabs` size to `3` and add the `Brick`, `Brick1`, and `Brick2` **Prefabs** to the `Brick Prefabs` array. 

![](../resources/img/04-images/04-image-3.png)

Click the **Play** button to test the game. You should see the bricks being created at runtime.

---

## Formative Assessment

Learning to use AI tools is an important skill. While AI tools are powerful, you **must** be aware of the following:

- If you provide an AI tool with a prompt that is not refined enough, it may generate a not-so-useful response
- Do not trust the AI tool's responses blindly. You **must** still use your judgement and may need to do additional research to determine if the response is correct
- Acknowledge what AI tool you have used. In the assessment's repository **README.md** file, please include what prompt(s) you provided to the AI tool and how you used the response(s) to help you with your work

---

### Submission

Create a new pull request and assign **grayson-orr** to review your practical submission. Please do not merge your own pull request.

---

## Next Class

Link to the next class: [Week 05]()
