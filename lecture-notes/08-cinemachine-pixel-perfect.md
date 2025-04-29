## Week 08

## Previous Class

Link to the previous class: [Week 07](https://github.com/otago-polytechnic-bit-courses/ID623001-introductory-game-development/blob/main-s1-25/lecture-notes/07-custom-editor-window-scriptable-objects.md)

---

## Rouge-Like Game

In this module, you will continue to develop **Rogue-Like** using Unity.

> **Note:** The following content does not take into account last week's formative assessment. Please ensure you have completed the formative assessment before continuing with this week's content. 

---


---

## Cinemachine

**Cinemachine** is a powerful suite of tools for creating dynamic and complex camera systems in **Unity**. It allows you to create cameras that can follow, look at, and react to game objects in a variety of ways, making it easier to create cinematic experiences in your games.

Install **Cinemachine** from the **Package Manager**. In the **Hierarchy**, right-click and select **Cinemachine > Virtual Camera**. This will create a new **Cinemachine Virtual Camera** in your scene.

In the **Hierarchy**, create a new empty **Game Object** and name it `Cinemachine Target Group`. Add a **Cinemachine Target Group** component to the new game object. 

In the **Inspector**, set the **Cinemachine Virtual Camera** to follow the `Cinemachine Target Group` game object. This will make the camera follow the target group instead of a single game object. For example, the player and cursor game objects can be added to the target group, allowing the camera to follow both of them.

![](../resources/img/08-images/08-image-1.png)

Set the `Lens > Ortho Size` value to `8.4375`, `Lens > Advanced > Mode Override` to `Orthographic` and `Body` to `Framing Transposer`. This will ensure that the camera is orthographic and that it will follow the target group correctly.

![](../resources/img/08-images/08-image-2.png)

Set the following values in `Body`:

- `Lookahead Smoothing`: `10`
- `X Damp`: `0.7`
- `Y Damp`: `0.7`
- `Screen X`: `0.5`
- `Screen Y`: `0.5`
- `Soft Zone Width`: `2`
- `Soft Zone Height`: `1`
- `Bias X`: `0.183`
- `Group Framing Size`: `1`
- `Minimum Ortho Size`: `8.4375`
- `Maximum Ortho Size`: `8.4375`

![](../resources/img/08-images/08-image-3.png)

What do each of these values do?

- **Lookahead Smoothing**: This value determines how quickly the camera will react to the target's movement. A higher value will make the camera follow the target more smoothly, while a lower value will make it follow more quickly.
- **X and Y Damp**: These values determine how quickly the camera will follow the target in the X and Y directions. A higher value will make the camera follow the target more smoothly, while a lower value will make it follow more quickly.
- **Screen X and Y**: These values determine the position of the target on the screen. A value of `0.5` will centre the target in the screen, while a value of `0` will place it at the left or bottom edge of the screen.
- **Soft Zone Width and Height**: These values determine the size of the area around the target where the camera will follow the target smoothly. If the target moves outside of this area, the camera will move more quickly to catch up to it.
- **Bias X**: This value determines the bias of the target's position in the X direction. A positive value will move the target to the right, while a negative value will move it to the left.
- **Group Framing Size**: This value determines the size of the area around the target that the camera will frame. A higher value will make the camera frame a larger area, while a lower value will make it frame a smaller area.
- **Minimum and Maximum Ortho Size**: These values determine the minimum and maximum size of the camera's orthographic view. A higher value will make the camera's view larger, while a lower value will make it smaller.

---

## Pixel Perfect

**Pixel Perfect** is a component in **Unity** that helps you achieve pixel-perfect rendering in your 2D games. It ensures that your sprites are rendered at the correct size and position on the screen, preventing any blurriness or distortion that can occur when scaling or moving sprites.

Set the `Extensions > Set Extensions` to `CinemachinePixelPerfect`. This will add a **Cinemachine Pixel Perfect** component to the virtual camera, which will ensure that the camera's view is pixel-perfect.

![](../resources/img/08-images/08-image-4.png)

---

## Universal Render Pipeline

**Universal Render Pipeline** (URP) is a scriptable render pipeline that is designed to be lightweight and efficient. It is ideal for 2D games and provides a variety of features that can help improve the performance and visual quality of your game.

![](../resources/img/08-images/08-image-5.png)

![](../resources/img/08-images/08-image-6.png)

---

## Main Camera

Set the **Main Camera** to the following values:

- `Clear Flags`: `Solid Color`
- `Background`: `#000000`

---

## Other Packages and Assets

Ensure you have installed the following packages from the **Package Manager**:

- **2D Sprite**
- **2D Tilemap Editor**
- **2D Tilemap Extras**
- **TextMesh Pro**
- **Unity UI**
- **Universal RP**

In the **lecture-notes**, unzip the **08-assets.zip** folder and copy the contents into the `Assets` folder.

---

## Formative Assessment

Learning to use AI tools is an important skill. While AI tools are powerful, you **must** be aware of the following:

- If you provide an AI tool with a prompt that is not refined enough, it may generate a not-so-useful response
- Do not trust the AI tool's responses blindly. You **must** still use your judgement and may need to do additional research to determine if the response is correct
- Acknowledge what AI tool you have used. In the assessment's repository **README.md** file, please include what prompt(s) you provided to the AI tool and how you used the response(s) to help you with your work

---

## Next Class

Link to the next class: [Week 09]()