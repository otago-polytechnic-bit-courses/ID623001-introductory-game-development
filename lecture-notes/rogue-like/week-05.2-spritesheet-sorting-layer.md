## Rogue-Like Game

In this module, you will develop **Rogue-Like** using **Unity**. Create a new **Unity** project using the **2D (Built-In Render Pipeline)** template. Name your project `rogue-like` and select a location to save your project. Click on the **Create project** button.

---

## Spritesheet

**Spritesheet** is a collection of images combined into a single image. It is used to reduce the number of draw calls in a game, which can improve performance. In this module, you will use a **spritesheet** to create a character for your game.

In the **lecture-notes** folder, you will find a zip folder called `07-assets`. Download and extract the folder. Copy the folders into your **Unity** project. The folder should be placed in the `Assets` folder of your project.

![](../../resources%20(ignore)/img/07-images/07-image-1.png)

In the **Assets > Art Characters** folder, you will find a spritesheet called `Characters`. This spritesheet contains all different characters for your game. 

Click on the `Characters` spritesheet in the **Project** window. In the **Inspector** window, change the following settings:

- **Sprite Mode**: `Multiple`. This allows you to slice the spritesheet into individual sprites.
- **Pixels Per Unit**: `16`. This determines how many pixels in the spritesheet correspond to one unit in the game world.
- **Filter Mode**: `Point (no filter)`. This setting is used to determine how the sprite is rendered. Point filtering is used for pixel art to avoid blurring.
- **Max Size**: `64`. This setting determines the maximum size of the sprite. If the sprite is larger than this size, it will be scaled down to fit.

![](../../resources%20(ignore)/img/07-images/07-image-2.png)

Click on the **Edit Sprite** button in the **Inspector** window. This will open the **Sprite Editor** window. 

![](../../resources%20(ignore)/img/07-images/07-image-3.png)

In the **Sprite Editor** window, click on the **Slice** button in the top left corner. This will open the **Slice** window. Set the following settings:

- **Type**: `Grid By Cell Size`. This will slice the spritesheet into a grid based on the cell size.
- **Pixel Size**: `16`. This will slice the spritesheet into 16x16 pixel sprites.

Click on the **Slice** button in the **Slice** window. This will slice the spritesheet into individual sprites. You should see a grid of sprites in the **Sprite Editor** window.

Click on the **Apply** button in the top right corner of the **Sprite Editor** window. This will apply the changes to the spritesheet. You should now see individual sprites in the **Project** window.

![](../../resources%20(ignore)/img/07-images/07-image-4.png)

Drag and drop a character sprite into the **Scene** window. This will create a new **GameObject** in the **Hierarchy** window.

![](../../resources%20(ignore)/img/07-images/07-image-5.png)

Similar to above, drag and drop a gun sprite into the **Scene** window. This will create a new **GameObject** in the **Hierarchy** window. Create a new `Empty GameObject` in the **Hierarchy** window and name it `Player`. Drag and drop the character and gun sprites into the `Player` GameObject. This will make the character and gun sprites children of the `Player` GameObject.

> **Note:** You do not need to slice the gun spritesheet. The gun sprites are already sliced and ready to use.

![](../../resources%20(ignore)/img/07-images/07-image-6.png)

---

## Sorting Layer

**Sorting Layer** is used to determine the order in which sprites are rendered. Sprites with a higher sorting layer will be rendered on top of sprites with a lower sorting layer.

In the **Inspector** window, create a new sorting layer called `Player`. You should now have two sorting layers: `Default` and `Player`.

![](../../resources%20(ignore)/img/07-images/07-image-7.png)

Click on the `Player` GameObject in the **Hierarchy** window. In the **Inspector** window, set the **Sprite Renderer > Additional Settings > Sorting Layer** to `Player` and **Sprite Renderer > Additional Settings > Sorting Layer Order** to `0`. This will set the sorting layer of the `Player` GameObject to `Player`.

![](../../resources%20(ignore)/img/07-images/07-image-8.png)

Apply the same settings to the gun sprite. This will set the sorting layer of the gun sprite to `Player`. However, set the **Sorting Layer Order** to `1`. This will set the sorting layer of the gun sprite to be rendered on top of the character sprite. This will make the gun sprite appear on top of the character sprite.

![](../../resources%20(ignore)/img/07-images/07-image-9.png)

In the **Assets** folder, create a new folder called `Scripts`. In the `Scripts` folder, create a new C# script called `PlayerController`. This script will be used to control the player character. 

> **Note:** In the formative assessment, you will write the code to control the player character.

![](../../resources%20(ignore)/img/07-images/07-image-10.png)

Add a `Circle Collider 2D` component to the `Player` GameObject.

![](../../resources%20(ignore)/img/07-images/07-image-11.png)

Add a `Rigidbody2D` component to the `Player` GameObject.

![](../../resources%20(ignore)/img/07-images/07-image-12.png)

---

## Formative Assessment

Learning to use AI tools is an important skill. While AI tools are powerful, you **must** be aware of the following:

- If you provide an AI tool with a prompt that is not refined enough, it may generate a not-so-useful response
- Do not trust the AI tool's responses blindly. You **must** still use your judgement and may need to do additional research to determine if the response is correct
- Acknowledge what AI tool you have used. In the assessment's repository **README.md** file, please include what prompt(s) you provided to the AI tool and how you used the response(s) to help you with your work

---

### Task 1

In the `PlayerController` script, write the code to move the player character using both the **WASD** keys and the **Arrow** keys. The player character should move in the direction of the key pressed.

---

### Task 2

In the `PlayerController` script, write the code to dash the player character using the **Space** key. The player character should dash in the direction it is currently moving. The dash should last for 0.5 seconds and should have a cooldown of 2 seconds.

> **Note:** The player character should not be able to dash again until the cooldown is over.

---

### Task 3

Write the code that allows the player character to swap between two weapons. The player character should be able to swap between the gun and a sword. The player character should be able to swap weapons using the **Q** key. 
