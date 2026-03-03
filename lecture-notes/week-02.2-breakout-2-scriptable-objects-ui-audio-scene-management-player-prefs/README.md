using UnityEngine;

public class BrickController : MonoBehaviour
{
    [Header("Prefab Settings")]
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private BrickData[] rowData = new BrickData[4];

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

        float totalWidth = bricksPerRow * (brickWidth + padding) - padding;
        float startX = -totalWidth / 2f + brickWidth / 2f;
        float startY = screenTop - topOffset;

        for (int row = 0; row < rowData.Length; row++)
        {
            for (int col = 0; col < bricksPerRow; col++)
            {
                float x = startX + col * (brickWidth + padding);
                float y = startY - row * (brickHeight + padding);

                GameObject brick = Instantiate(brickPrefab, new Vector2(x, y), Quaternion.identity, bricksParent);

                Brick brickScript = brick.GetComponent<Brick>();
                if (brickScript != null)
                {
                    brickScript.Initialize(rowData[row]);
                }

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


# Week 02.2

---

## Important Links

| Section        | Link                                                                                     |
| -------------- | ---------------------------------------------------------------------------------------- |
| Previous Class | [Week 02.1](lecture-notes/week-02.1-breakout-2-textures-prefabs-prefab-variants-input-system/README.md) |
| Next Class     | [Week 03]()                                                                            |

---

 





---

## UI

---

## Audio

---

## Scene Management

---

## Player Prefs

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

---

### Task 3


---

### Task 4

