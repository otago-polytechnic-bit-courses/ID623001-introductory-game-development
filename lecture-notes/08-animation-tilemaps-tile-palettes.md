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

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private Transform gunTransform;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Omitted for brevity

        Vector3 mousePosition = Input.mousePosition;
        Vector3 cursorPoint = mainCamera.WorldToScreenPoint(transform.localPosition);
        Vector2 offset = new Vector2(mousePosition.x - cursorPoint.x, mousePosition.y - cursorPoint.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        gunTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
```

What is happening in the code above?

- `Vector3 mousePosition` variable stores the current position of the mouse in screen space.
- `Vector3 cursorPoint` variable converts the player's position from world space to screen space using the `WorldToScreenPoint` method of the camera.
- `Vector2 offset` variable calculates the difference between the mouse position and the player position.
- `float angle` variable calculates the angle between the player and the mouse position using the `Mathf.Atan2` method.
- `gunTransform.rotation` variable sets the rotation of the gun to the angle calculated above using the `Quaternion.Euler` method.
- `Quaternion.Euler` method converts the angle from degrees to radians.

In the **Hierarchy** window, select the `Player` object. In the **Inspector** window, drag and drop the `Gun` object into the `Gun Transform` field of the `PlayerController` script.

![](../resources/img/08-images/08-image-1.png)

Click on the **Play** button. Move the mouse around the screen. You should see the gun rotate to face the mouse position.

![](../resources/img/08-images/08-image-2.png)

---

## Direction

When the player moves left, the player and gun should flip to face the left direction. When the player moves right, the player and gun should flip to face the right direction. To do this, we need to check the mouse position and flip the player and gun accordingly.

In the `PlayerController` script, update to the following:

```cs
// Omitted for brevity

public class PlayerController : MonoBehaviour
{
    // Omitted for brevity

    void Update()
    {
        // Omitted for brevity

        Vector3 mousePosition = Input.mousePosition;
        Vector3 cursorPoint = mainCamera.WorldToScreenPoint(transform.localPosition);

        if (mousePosition.x < cursorPoint.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            gunTransform.localScale = new Vector3(-1f, -1f, 1f);
        }
        else 
        {
            transform.localScale = Vector3.one;
            gunTransform.localScale = Vector3.one;
        }

        Vector2 offset = new Vector2(mousePosition.x - cursorPoint.x, mousePosition.y - cursorPoint.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        gunTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
```

Click on the **Play** button. Move the mouse around the screen. You should see the player and gun flip to face the mouse position.

![](../resources/img/08-images/08-image-3.png)

---

## Animation

Currently, the player is not animated. To animate the player, we need to create an animation controller and animation clips.

To open the **Animation** window, go to **Window > Animation > Animation**. This will open the **Animation** window.

![](../resources/img/08-images/08-image-4.png)

---

### Idle Animation

In the **Hierarchy** window, select the `Player` object. In the **Animation** window, click on the **Create** button. In the **Assets** folder, create a new folder called `Animations`. Name the animation `PlayerIdle`. This will create a new animation clip called `PlayerIdle` in the `Animations` folder.

![](../resources/img/08-images/08-image-5.png)

In this animation clip, the gun will move up and down while the player is idle. In the **Hierarchy** window, select the `Guns_0` **Game Object**. Drag the slider from `0` to `0.2`. 

![](../resources/img/08-images/08-image-6.png)

Click the **Record** button in the **Animation** window. This will start recording the animation.

In the **Inspector** window, change the `Y` position of the `Guns_0` **Game Object** to `-0.1`. This will move the gun down.

You should see a new **Property** in the **Animation** window called `Guns_0 : Position` and two keyframes. The first keyframe is at `0` and the second keyframe is at `0.2`.

![](../resources/img/08-images/08-image-9.png)

Move the slider to `0.4` and create a new keyframe. Change the `Y` position of the `Guns_0` **Game Object** to `0`.  

![](../resources/img/08-images/08-image-10.png)

In the first keyframe, change the `Y` position of the `Guns_0` **Game Object** to `0`. 

![](../resources/img/08-images/08-image-7.png)

---

### Walk Animation

Create a new animation clip called `PlayerWalk` in the `Animations` folder. 

![](../resources/img/08-images/08-image-11.png)

Drag the slider from `0` to `0.2`. Click the **Record** button in the **Animation** window. In the **Hierarchy** window, select the `Characters_0` **Game Object**. In the **Inspector** window, change the `Z` rotation of the `Characters_0` **Game Object** to `7`. In the first keyframe, change the `Z` rotation of the `Characters_0` **Game Object** to `-7`. Add another keyframe at `0.1` and change the `Z` rotation of the `Characters_0` **Game Object** to `0`.

![](../resources/img/08-images/08-image-12.png)

---

### Animator

To open the **Animator** window, go to **Window > Animation > Animator**. This will open the **Animator** window.

You should see the following:

![](../resources/img/08-images/08-image-13.png)

In the **Animator** window, click on the **Parameter** tab. 

![](../resources/img/08-images/08-image-14.png)

Click on the **+** button and create a new **Bool** parameter called `isMoving`. This will be used to determine if the player is moving or not.

![](../resources/img/08-images/08-image-15.png)

Left-click on the `PlayerIdle` animation clip and select **Make Transition**. 

![](../resources/img/08-images/08-image-16.png)


Drag the arrow to the `PlayerWalk` animation clip. This will create a transition from the `PlayerIdle` animation clip to the `PlayerWalk` animation clip.

![](../resources/img/08-images/08-image-17.png)

Click on the transition arrow. In the **Inspector** window, uncheck the **Has Exit Time** checkbox and set the **Condition** to `isMoving` is `true`. **Has Exit Time** is used to determine if the animation should exit or not. By unchecking this, we are telling Unity that the animation should exit immediately when the condition is met.

![](../resources/img/08-images/08-image-18.png)

Left-click on the `PlayerWalk` animation clip and select **Make Transition**. Drag the arrow to the `PlayerIdle` animation clip. 

![](../resources/img/08-images/08-image-19.png)

Click on the transition arrow. In the **Inspector** window, uncheck the **Has Exit Time** checkbox and set the **Condition** to `isMoving` is `false`.

![](../resources/img/08-images/08-image-20.png)

In the `PlayerController` script, update to the following:

```cs
// Omitted for brevity

public class PlayerController : MonoBehaviour
{
    // Omitted for brevity

    [SerializeField]
    private Animator animator;

    // Omitted for brevity

    void Update()
    {
        // Omitted for brevity

        if (movement != Vector2.zero)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }
}
```

> **Note:** You may have a different variable name for the `movement` variable.

In the **Hierarchy** window, select the `Player` object. In the **Inspector** window, drag and drop the `Animator` component into the `Animator` field of the `PlayerController` script.

![](../resources/img/08-images/08-image-21.png)

Click on the **Play** button. Move the player around the screen. You should see the player animate when moving and idle when not moving.

---

## Tilemaps

**Tilemaps** are a way to create 2D environments using tiles. Tiles are small images that can be used to create larger images. Unity has a built-in tilemap system that allows you to create tilemaps easily.

To create a tilemap, go to **GameObject > 2D Object > Tilemap > Rectangular**. 

![](../resources/img/08-images/08-image-22.png)

This will create a new `Grid` **Game Object** with a `Tilemap` **Game Object** inside it. The `Grid` **Game Object** is used to define the size of the tilemap and the `Tilemap` **Game Object** is used to store the tiles.

![](../resources/img/08-images/08-image-23.png)

---

### Tile Palette

**Tile Palettes** are used to create and edit tiles. A tile palette is a collection of tiles that can be used to create a tilemap. You can create a tile palette by dragging and dropping images into the tile palette.

To create a tile palette, go to **Window > 2D > Tile Palette**. This will open the **Tile Palette** window.

![](../resources/img/08-images/08-image-24.png)

Create a new tile palette by clicking on the **Create New Palette** button. 

![](../resources/img/08-images/08-image-25.png)

In the **Create New Palette** window, name the tile palette `Dungeon 3` and click on the **Create** button.

![](../resources/img/08-images/08-image-26.png)

Save the tile palette in the `Assets > Tilesets > Dungeon 3` folder. 

![](../resources/img/08-images/08-image-27.png)

Drag and drop the `Dungeon 3 Tiles` **Sprite Sheet** into the tile palette. This will create a new tile palette with the tiles from the sprite sheet.

![](../resources/img/08-images/08-image-28.png)

You should see the `Dungeon 3` tile palette in the **Tile Palette** window.

![](../resources/img/08-images/08-image-29.png)

Click on **Paint with basic brush** and select a tile from the tile palette. 

**Tasks:** 

1. Create a room using the tiles from the tile palette. Here is an example of a room created using the tiles from the tile palette.

![](../resources/img/08-images/08-image-30.png)

2. Add a `Tilemap Collider 2D` component to the `Tilemap` **Game Object**. This will allow the player to collide with the tiles in the tilemap. Set the **Used By Composite** checkbox to `true`. This will allow the `Tilemap Collider 2D` to be used by the `Composite Collider 2D` component.

3. Add a `Composite Collider 2D` component to the `Tilemap` **Game Object**. This will allow the player to collide with the tiles in the tilemap as a single collider. 

> **Note:** When you add a `Composite Collider 2D` component, a `Rigidbody 2D` component will be added automatically. 

4. In the `Rigidbody 2D` component, set the **Body Type** to `Kinematic`. This will allow the player to collide with the tiles in the tilemap without being affected by gravity.

5. Click on `Dungeon 3 Tiles_24` in the `Assets > Tilesets > Dungeon 3` folder. In the **Inspector** window, set the **Collider Type** to `None`. This will allow the player to walk on the tiles in the tilemap without colliding with them.

![](../resources/img/08-images/08-image-31.png)

---

## Exercises

Learning to use AI tools is an important skill. While AI tools are powerful, you **must** be aware of the following:

- If you provide an AI tool with a prompt that is not refined enough, it may generate a not-so-useful response
- Do not trust the AI tool's responses blindly. You **must** still use your judgement and may need to do additional research to determine if the response is correct
- Acknowledge what AI tool you have used. In the assessment's repository **README.md** file, please include what prompt(s) you provided to the AI tool and how you used the response(s) to help you with your work

---

### Task 1

You will notice that the player moves faster when moving diagonally. This is because the player is moving in both the `X` and `Y` directions at the same time. Write the code to normalise the movement vector so that the player moves at a constant speed in all directions.

---

## Next Class

Link to the next class: [Week 09]()