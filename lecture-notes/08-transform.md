## Week 08

## Previous Class

Link to the previous class: [Week 07](https://github.com/otago-polytechnic-bit-courses/ID623001-introductory-game-development/blob/main-s1-25/lecture-notes/07-spritesheet-sorting-layer.md)

---

## Rogue-Like Game

In this module, you will continue to develop **Rogue-Like** using Unity.

> **Note:** The following content does not take into account last week's formative assessment. Please ensure you have completed the formative assessment before continuing with this week's content. 
---

## Rotating The Gun

In the `PlayerController` script, update to the following:

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{    
    // Omitted for brevity

    [SerializeField] 
    private Rigidbody2D rb;

    [SerializeField]
    private Transform gunTransform;

    void Update()
    {
        // Omitted for brevity

        Vector3 mousePosition = Input.mousePosition;
        Vector3 cursorPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
        Vector2 offset = new Vector2(mousePosition.x - cursorPoint.x, mousePosition.y - cursorPoint.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        gunTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
```

What is happening in the code above?

- The `Vector3 mousePosition` variable stores the current position of the mouse in screen space.
- The `Vector3 cursorPoint` variable stores the position of the player in screen space.
- The `Vector2 offset` variable calculates the difference between the mouse position and the player position.
- The `float angle` variable calculates the angle between the player and the mouse position using the `Mathf.Atan2` method.
- The `gunTransform.rotation` variable sets the rotation of the gun to the angle calculated above using the `Quaternion.Euler` method.
- The `Quaternion.Euler` method converts the angle from degrees to radians.

In the **Hierarchy** window, select the `Player` object. In the **Inspector** window, drag and drop the `Gun` object into the `Gun Transform` field of the `PlayerController` script.

![](../resources/img/08-images/08-image-1.png)

Click on the **Play** button. Move the mouse around the screen. You should see the gun rotate to face the mouse position.

![](../resources/img/08-images/08-image-2.png)

---

## Formative Assessment

Learning to use AI tools is an important skill. While AI tools are powerful, you **must** be aware of the following:

- If you provide an AI tool with a prompt that is not refined enough, it may generate a not-so-useful response
- Do not trust the AI tool's responses blindly. You **must** still use your judgement and may need to do additional research to determine if the response is correct
- Acknowledge what AI tool you have used. In the assessment's repository **README.md** file, please include what prompt(s) you provided to the AI tool and how you used the response(s) to help you with your work

---

### Task 1

---

## Next Class

Link to the next class: [Week 09]()