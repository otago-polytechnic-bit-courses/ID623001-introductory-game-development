# Week 04.2 - Breakout: Build and Itch.io

## Navigation

|            | Link                                                                                                                                                           |
| ---------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| ← Previous | [Week 04.1 - Breakout: Renderer and Particle Systems](../week-04.1-breakout-4-renderer-particle-systems/README.md) |
| → Next     | [Week 05.1 - Breakout: Polish and Juice](../week-05.1-breakout-5-polish-and-juice/README.md)                                                                 |

---

## 1. Build Settings

Before exporting your game, Unity needs to know which scenes to include and what platform to target. The **Build Profiles** window manages this.

---

### 1.1 Configuring Scenes

**Step 1** - Open **File > Build Profiles**.

**Step 2** - Confirm both scenes are listed in the correct order:

| Index | Scene           |
| ----- | --------------- |
| 0     | `MainMenuScene` |
| 1     | `SampleScene`   |

If either scene is missing, open it and click **Add Open Scenes**.

**Step 3** - Ensure `MainMenuScene` is at index `0` - this is the scene Unity loads first when the game launches. Drag scenes to reorder them if needed.

📖 Reference: [Unity - Build Settings](https://docs.unity3d.com/Manual/BuildSettings.html)

---

### 1.2 Selecting a Platform

Unity can build for many platforms. For Itch.io, **WebGL** is the best choice - it runs in any modern browser without requiring the player to download or install anything.

**Step 1** - In **Build Profiles**, click **WebGL** in the platform list on the left. If it is not installed, click **Install with Unity Hub** and follow the prompts.

**Step 2** - Click **Switch Platform**. Unity will reimport assets for the WebGL target - this may take a moment.

📖 Reference: [Unity - WebGL](https://docs.unity3d.com/Manual/webgl-building.html)

---

## 2. Player Settings

**Player Settings** control the game's name, icon, and how it behaves on a given platform.

**Step 1** - In **Build Profiles**, click **Player Settings** (bottom left).

**Step 2** - Under **Player**, configure the following:

| Property             | Value                          | Reason                                              |
| -------------------- | ------------------------------ | --------------------------------------------------- |
| **Product Name**     | `Breakout` (or your own title) | Shown in the browser tab and on Itch.io             |
| **Default Icon**     | Your game icon sprite          | Displayed on Itch.io and in the browser             |
| **Version**          | `1.0.0`                        | Useful for tracking builds                          |

**Step 3** - Expand the **Resolution and Presentation** section. Set:

| Property                  | Value       | Reason                                              |
| ------------------------- | ----------- | --------------------------------------------------- |
| **Default Canvas Width**  | `960`       | A safe width for embedded Itch.io iframes           |
| **Default Canvas Height** | `600`       | Matches a standard widescreen aspect ratio          |
| **Run In Background**     | Enabled     | Keeps the game running if the player clicks away    |
| **WebGL Template**        | `Minimal`   | Removes Unity's default logo and loading screen     |

📖 Reference: [Unity - Player Settings](https://docs.unity3d.com/Manual/class-PlayerSettings.html)

---

## 3. WebGL Build

### 3.1 Publishing Settings

**Step 1** - In **Player Settings**, expand the **Publishing Settings** section.

**Step 2** - Set **Compression Format** to **Disabled**. Itch.io's server configuration can prevent compressed WebGL builds from loading correctly in some browsers.

> Some hosting platforms support Gzip or Brotli compression and can serve it correctly - but disabling compression is the safest default for Itch.io.

**Step 3** - Leave all other publishing settings at their defaults.

📖 Reference: [Unity - WebGL Deployment](https://docs.unity3d.com/Manual/webgl-deploying.html)

---

### 3.2 Building the Game

**Step 1** - In **Build Profiles**, click **Build**.

**Step 2** - A file dialog will open. Create a new folder called `Build` inside your project folder (but **outside** the `Assets` folder) and select it.

> Unity generates several files and folders. Do not move or rename files inside the build output - the browser requires them all to be present and correctly referenced.

**Step 3** - Wait for the build to complete. Unity will open the output folder automatically when it finishes. The output will contain:

| File / Folder       | Purpose                                                   |
| ------------------- | --------------------------------------------------------- |
| `index.html`        | The page the browser loads                                |
| `Build/`            | Compiled game code, data, and framework files             |
| `TemplateData/`     | CSS and assets for the loading screen (Minimal template)  |

**Step 4** - Open `index.html` in a browser to do a quick local test. Note that some browsers block local file access - if the game does not load, use the Unity Editor's **Build and Run** option instead, which starts a local server automatically.

📖 Reference: [Unity - Building for WebGL](https://docs.unity3d.com/Manual/webgl-building.html)

---

## 4. Itch.io

[Itch.io](https://itch.io) is an open platform for indie games. It supports WebGL games played directly in the browser, making it ideal for sharing Unity projects.

---

### 4.1 Creating an Itch.io Account

**Step 1** - Go to [https://itch.io](https://itch.io) and click **Register**.

**Step 2** - Choose a username, enter your email, and set a password. Click **Create account**.

**Step 3** - Verify your email address using the link sent to your inbox.

---

### 4.2 Creating a New Project Page

**Step 1** - Log in and click your profile icon in the top right. Select **Dashboard**.

**Step 2** - Click **Create new project**.

**Step 3** - Fill in the project page:

| Field              | Value                                                              |
| ------------------ | ------------------------------------------------------------------ |
| **Title**          | `Breakout` (or your own title)                                     |
| **Project URL**    | Auto-filled from the title - edit if needed                        |
| **Kind of project**| `HTML`                                                             |
| **Classification** | `Games`                                                            |
| **Genre**          | `Action` (or the closest match)                                    |
| **Description**    | A short summary of your game                                       |

---

### 4.3 Uploading the Build

**Step 1** - Scroll to the **Uploads** section. Click **Upload files**.

**Step 2** - Select all files in your `Build` output folder, compress them into a single `.zip` file, and upload that. The zip must contain `index.html` at its **root** - not inside a subfolder.

> On Windows: select all files in the Build folder, right-click and choose **Compress to ZIP file**. On macOS: select all files, right-click and choose **Compress**.

**Step 3** - Once uploaded, tick the checkbox **This file will be played in the browser**.

**Step 4** - Under **Embed options**, set the frame dimensions to match your Player Settings:

| Property  | Value |
| --------- | ----- |
| **Width** | `960` |
| **Height**| `600` |

---

### 4.4 Publishing

**Step 1** - Scroll to the **Visibility** section. Set it to **Public** when you are ready to share, or **Restricted** to share only with people who have the link.

**Step 2** - Click **Save & view page** to preview how the page looks.

**Step 3** - Click **Run game** on your project page to verify the build loads and plays correctly in the browser.

> If the game does not load, return to **Edit game** and confirm:
> - Compression is set to **Disabled** in Unity Player Settings
> - `index.html` is at the root of the uploaded zip
> - The file is marked as **played in the browser**

📖 Reference: [Itch.io - Uploading HTML5 Games](https://itch.io/docs/creators/html5)

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

### Task 1 - Custom WebGL Template

Create a custom WebGL template that replaces Unity's default loading screen with a styled page matching your game's colour scheme. In the `Assets` folder, create a `WebGLTemplates/Breakout` directory containing an `index.html` and a `thumbnail.png`. Select it under **Player Settings > Resolution and Presentation > WebGL Template** and rebuild.

> **Hint:** copy Unity's built-in `Minimal` template from the Unity Editor installation folder as a starting point. The template uses `{{{ TOTAL_MEMORY }}}` and `{{{ DATA_FILENAME }}}` placeholder variables - do not remove them.

---

### Task 2 - Application Version Display

Display the build version string on the Main Menu screen using a `TextMeshProUGUI` element. In a `Start` method, set the text to `Application.version`. Update the **Version** field in Player Settings and rebuild to confirm the displayed string changes.

> **Hint:** `Application.version` reads directly from the **Version** field in Player Settings and requires no additional setup.

---

### Task 3 - Platform-Specific Quit Button

The **Quit** button from Week 03.1 has no effect in WebGL because browsers do not allow JavaScript to close tabs programmatically. Hide the Quit button at runtime when the build target is WebGL. Use `Application.platform` to detect the platform and call `gameObject.SetActive(false)` on the button if the platform is `RuntimePlatform.WebGLPlayer`.

> **Hint:** `Application.platform == RuntimePlatform.WebGLPlayer` returns `true` only in a live WebGL build, not in the Unity Editor, so you can still test the button normally in Play mode.

---

### Task 4 - Itch.io Page Polish

Add a cover image, screenshots, and a description to your Itch.io project page. The cover image should be exactly **630 × 500 px**. Add at least two screenshots from Play mode (use Unity's **Game** view screenshot tool or a system screenshot). Write a description that includes the controls, objective, and credits. Set at least two tags (e.g. `breakout`, `arcade`, `unity`) to help players discover the game.

> **Hint:** screenshots can be taken directly from the Unity **Game** view by right-clicking the tab and selecting **Save screenshot**. Resize in any image editor to meet Itch.io's recommended dimensions.