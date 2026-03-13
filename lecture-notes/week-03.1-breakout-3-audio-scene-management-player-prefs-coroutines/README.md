# Week 03.1 — Breakout: Audio, Scene Management, Player Prefs & Coroutines

## Navigation

|            | Link                                                                                                                                                               |
| ---------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| ← Previous | [Week 02.2 — Breakout: Scriptable Objects & UI](https://github.com/otago-polytechnic-bit-courses/ID623002-introductory-game-development/tree/s1-26/lecture-notes/week-02.2-breakout-2-scriptable-objects-ui/README.md)         |
| → Next     | [Week 04.1 — Breakout: Renderer & Particle Systems](https://github.com/otago-polytechnic-bit-courses/ID623002-introductory-game-development/tree/s1-26/lecture-notes/week-04.1-breakout-4-renderer-particle-systems/README.md) |

---

## 1. Audio

Audio sets the tone and atmosphere of a game and gives players feedback on their actions. In Unity, audio files are played through **AudioSource** components. The `AudioManager` below uses the **Singleton pattern** so any script in any scene can trigger sounds through a single shared instance.

**Step 1** — Copy the provided `Audio` folder into the `Assets` folder of your Unity project.

**Step 2** — In the `Scripts` folder, create a new script called `AudioManager`. Open it and add the following:

```csharp
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    // Static reference — accessible from any script via AudioManager.Instance
    public static AudioManager Instance { get; private set; }

    [Header("Ball Sound Settings")]
    [SerializeField] private AudioClip wallHitSound;

    private AudioSource audioSource;

    // Maps a GameObject tag to its corresponding sound clip
    private Dictionary<string, AudioClip> tagToSound;

    private void Awake()
    {
        // Singleton enforcement — destroy any duplicate that loads later
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);   // persist across scene loads

        audioSource = GetComponent<AudioSource>();

        tagToSound = new Dictionary<string, AudioClip>
        {
            { "Walls", wallHitSound }
            // Add more tag → clip pairs here as you add sounds
        };
    }

    // Look up the clip for the given tag and play it, if one exists
    public void PlayCollisionSound(string tag)
    {
        if (tagToSound.TryGetValue(tag, out AudioClip clip))
            PlaySound(clip);
    }

    // PlayOneShot allows multiple overlapping sounds without cutting each other off
    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }
}
```

**Step 3** — Update `BallController` to call `AudioManager` when the ball collides with something:

```csharp
// Omitted for brevity

public class BallController : MonoBehaviour
{
    // Omitted for brevity

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;

        // Wall colliders are children of the Walls GameObject — check the parent tag if the
        // child itself is untagged
        string tag = obj.CompareTag("Untagged")
            ? obj.transform.parent?.tag
            : obj.tag;

        if (tag != null)
            AudioManager.Instance?.PlayCollisionSound(tag);
    }
}
```

**Step 4** — In the Hierarchy, create an empty GameObject named `Audio Manager`. Attach the `AudioManager` script to it. In the Inspector, set the **Wall Hit Sound** field to an audio clip from the `Audio` folder.

---

## 2. Scene Management

Scene management is the process of loading and unloading scenes in a game. Unity's `SceneManager` class lets you load scenes by name or index and remove scenes that are no longer needed.

**Step 1** — In the `Scenes` folder, right-click and select **Create > Scene**. Name it `MainMenuScene`. Double-click to open it.

**Step 2** — Drag `MainMenuScene` from the `Scenes` folder into the Hierarchy panel.

**Step 3** — Delete the `Audio Manager` GameObject from `SampleScene`. The `AudioManager` uses `DontDestroyOnLoad`, so it will persist when the scene transitions — we will create a single instance in `MainMenuScene` instead.

**Step 4** — Right-click `SampleScene` in the Hierarchy and select **Remove Scene**.

**Step 5** — In the `Scripts` folder, create a new script called `MainMenuManager`. Open it and add the following:

```csharp
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button playButton;

    private void Start()
    {
        // AddListener wires the button click to our method without using the Inspector event
        playButton.onClick.AddListener(OnPlayClicked);
    }

    private void OnPlayClicked()
    {
        // LoadSceneAsync loads the scene in the background without freezing the game
        SceneManager.LoadSceneAsync("SampleScene");
    }

    private void OnDestroy()
    {
        // Always remove listeners when the object is destroyed to avoid memory leaks
        playButton.onClick.RemoveAllListeners();
    }
}
```

**Step 6** — In the Hierarchy, right-click and select **UI (Canvas) > Canvas**. Add the following UI elements inside an empty child GameObject named `Main Container`:

| Element         | Type        | Text       |
| --------------- | ----------- | ---------- |
| Title           | TextMeshPro | `Breakout` |
| Play button     | Button      | `Play`     |
| Settings button | Button      | `Settings` |
| Quit button     | Button      | `Quit`     |

Arrange them to your preference.

**Step 7** — Create an empty GameObject named `Main Menu Manager`. Attach the `MainMenuManager` script to it. In the Inspector, set the **Play Button** field to the Play button in the Canvas.

**Step 8** — Open **File > Build Profiles**. Click **Add Open Scenes** to add the currently open scene. Repeat for `SampleScene`. Ensure the build order is:

| Index | Scene           |
| ----- | --------------- |
| 0     | `MainMenuScene` |
| 1     | `SampleScene`   |

📖 Reference: [Unity — SceneManager](https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.html)

---

## 3. Player Prefs

`PlayerPrefs` saves and loads small amounts of player data — integers, floats, and strings — to disk. This is useful for settings like volume that should persist between play sessions.

**Step 1** — In the Hierarchy for `MainMenuScene`, create an empty GameObject named `Audio Manager`. Attach the `AudioManager` script to it.

**Step 2** — Update `AudioManager` to load the saved volume on startup and save it whenever it changes:

```csharp
// Omitted for brevity

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    // Omitted for brevity

    // Public property so other scripts can read the current volume
    public float SoundXFVolume { get; private set; }

    private void Awake()
    {
        // Omitted for brevity

        // Load the saved volume; default to 1 (full volume) if no value has been saved yet
        SoundXFVolume = PlayerPrefs.GetFloat("SoundXFVolume", 1f);
    }

    // Omitted for brevity

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
            // Pass volume as the second argument to PlayOneShot
            audioSource.PlayOneShot(clip, SoundXFVolume);
    }

    public void SetSoundXFVolume(float volume)
    {
        SoundXFVolume = volume;
        PlayerPrefs.SetFloat("SoundXFVolume", volume);
        PlayerPrefs.Save();   // flush to disk immediately
    }
}
```

**Step 3** — In the `Scripts` folder, create a new script called `SettingsController`. Open it and add the following:

```csharp
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private Slider soundXFSlider;

    private void Start()
    {
        soundXFSlider.minValue = 0f;
        soundXFSlider.maxValue = 1f;

        // Initialise the slider to the currently saved volume
        soundXFSlider.value = AudioManager.Instance.SoundXFVolume;

        soundXFSlider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void OnSliderChanged(float value)
    {
        AudioManager.Instance.SetSoundXFVolume(value);
    }

    private void OnBackClicked()
    {
        // Hide the panel rather than destroying it so it can be re-opened
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        soundXFSlider.onValueChanged.RemoveAllListeners();
    }
}
```

**Step 4** — In the Hierarchy, right-click the Canvas and select **UI (Canvas) > Panel**. Name it `Settings Panel`.

**Step 5** — Attach the `SettingsController` script to `Settings Panel`. Add a **Slider** as a child of `Settings Panel` and assign it to the **Sound XF Slider** field in the Inspector.

**Step 6** — Wire up the **Settings** button in `MainMenuManager` to show `Settings Panel` (`SetActive(true)`), and the **Back** button inside `Settings Panel` to call `OnBackClicked`.

📖 Reference: [Unity — PlayerPrefs](https://docs.unity3d.com/ScriptReference/PlayerPrefs.html)

---

## 4. Coroutines

A **coroutine** is a method that can pause its own execution and resume later, across multiple frames. This makes them ideal for timed sequences — countdowns, delays, fade effects — that would be awkward to implement inside `Update`.

Coroutines return `IEnumerator` and use `yield return` to pause:

| Yield statement                              | Effect                                                      |
| -------------------------------------------- | ----------------------------------------------------------- |
| `yield return new WaitForSeconds(t)`         | Pause for `t` seconds (affected by `Time.timeScale`)        |
| `yield return new WaitForSecondsRealtime(t)` | Pause for `t` real-time seconds (unaffected by `timeScale`) |
| `yield return null`                          | Pause for exactly one frame                                 |

**Step 1** — Drag `SampleScene` from the `Scenes` folder into the Hierarchy. Right-click `MainMenuScene` and select **Remove Scene**.

**Step 2** — In the Canvas, add a TextMeshPro text element as a child. Name it `Go Text` and set the initial text to `Press Space to Start`.

**Step 3** — Update `GameManager` to start a countdown coroutine when the player presses Space:

```csharp
// Omitted for brevity
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Omitted for brevity

    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI goText;

    private void Start()
    {
        goText.text = "Press Space to Start";
        // Omitted for brevity
    }

    public void OnStartGame(InputAction.CallbackContext context)
    {
        // Omitted for brevity

        // StartCoroutine begins the IEnumerator — Unity resumes it each frame
        StartCoroutine(GoRoutine());
    }

    private IEnumerator GoRoutine()
    {
        gameManagerMap.Disable();        // prevent the player from starting again mid-countdown
        goText.gameObject.SetActive(true);

        string[] sequence = { "3", "2", "1", "Go!" };

        foreach (string step in sequence)
        {
            goText.text = step;
            yield return new WaitForSeconds(1f);   // pause here; resume after 1 second
        }

        goText.gameObject.SetActive(false);
        paddleMap.Enable();
        Instantiate(slowBallPrefab, Vector2.zero, Quaternion.identity);
    }
}
```

> The `GoRoutine` coroutine takes approximately 4 seconds to complete, as it waits 1 second between each step in the countdown sequence (`"3"`, `"2"`, `"1"`, `"Go!"`). The exact number of frames depends on the frame rate — at 60 fps this is around 240 frames — but `WaitForSeconds` is time-based so the behaviour is consistent regardless of frame rate.

**Step 4** — In the Inspector for the `Game Manager` GameObject, set the **Go Text** field to the `Go Text` TextMeshPro element.

**Step 5** — Click **Play**. You should see `Press Space to Start`. Pressing Space triggers the `3 → 2 → 1 → Go!` countdown before the ball spawns.

📖 Reference: [Unity — Coroutines](https://docs.unity3d.com/Manual/Coroutines.html)

---

## Exercises

Learning to use AI tools is an important skill. While AI tools are powerful, you must be aware of the following:

- Refine your prompts — vague prompts yield vague responses
- Validate AI output — don't trust it blindly
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

### Task 1 — Background Music

Add a looping background music clip that plays when `MainMenuScene` loads. Make sure the music stops or transitions when `SampleScene` is loaded. Use `AudioSource.loop = true` and consider adding a separate `AudioSource` component dedicated to music.

> **Hint:** set `audioSource.loop = true` and call `audioSource.Play()` in `Awake`. Use `OnSceneLoaded` from `SceneManager.sceneLoaded` to detect when `SampleScene` loads and stop the music accordingly.

---

### Task 2 — Lives Flash Effect

Add a lives display to the `SampleScene` canvas using a `TextMeshProUGUI` element. Update the text whenever the player loses a life using a coroutine that briefly flashes the text red using `Color.Lerp` before returning to white, giving the player clear visual feedback.

> **Hint:** `yield return null` advances one frame at a time — use it inside a loop with `Color.Lerp(Color.red, Color.white, t)` where `t` increases each frame.

---

### Task 3 — Music Volume Setting

Extend `SettingsController` to control music volume separately. Add a second `Slider` to `Settings Panel` for music volume, stored under the key `"MusicVolume"` in `PlayerPrefs`. Update `AudioManager` to expose a `SetMusicVolume(float volume)` method that adjusts the music `AudioSource`'s `.volume` property.

> **Hint:** store a reference to the music `AudioSource` as a separate private field in `AudioManager`, distinct from the sound effects source.

---

### Task 4 — Pause Panel

In the `SampleScene` canvas, create a `PausePanel` that appears when the player presses **Escape**. The panel should contain a **Resume** button and a **Main Menu** button. While paused, set `Time.timeScale = 0f` to freeze gameplay and restore it to `1f` on resume.

> **Hint:** use `WaitForSecondsRealtime` instead of `WaitForSeconds` in any coroutines that need to run while paused — `Time.timeScale = 0` stops `WaitForSeconds`.

---

### Task 5 — Game Over and Win Panels

In the `SampleScene` canvas, add two new panels — `GameOverPanel` and `WinPanel`. When all lives are lost, disable gameplay input and show `GameOverPanel`. When all bricks are destroyed, show `WinPanel`. Each panel should have a **Play Again** button that reloads `SampleScene` and a **Main Menu** button that loads `MainMenuScene`.

> **Hint:** use `SceneManager.LoadSceneAsync("SampleScene")` for Play Again. Disable paddle input when either panel is shown by calling `paddleMap.Disable()`.

---

### Task 6 — Speed Up Coroutine

Write a coroutine in `GameManager` called `SpeedUpRoutine` that gradually increases the ball's speed every 30 seconds using `WaitForSeconds`. Cap the speed at a `[SerializeField] private float maxBallSpeed` value set in the Inspector. Display a **"Speed Up!"** message in the canvas for 2 seconds each time the speed increases, using a `TextMeshProUGUI` element that fades out with `Color.Lerp`.

> **Hint:** `yield return new WaitForSeconds(30f)` waits between increases. Fade the text alpha from `1f` to `0f` over the 2-second window using `Color.Lerp(visibleColour, transparentColour, t)`.

---

### Task 7 — Quit Button

Wire up the **Quit** button using `Application.Quit()`. Since `Application.Quit()` has no effect in the Unity Editor, use a coroutine to display a `"Quitting..."` message in the canvas for 2 seconds using `WaitForSeconds` before calling it, so the behaviour can still be observed during testing.

> **Hint:** `yield return new WaitForSeconds(2f)` then `Application.Quit()`. Wrap the call in `#if !UNITY_EDITOR` / `#endif` if you want to suppress it entirely when running in the Editor.

---

### Task 8 — Paddle and Brick Sounds

Add a paddle hit sound and a brick hit sound to `AudioManager`. Extend the `tagToSound` dictionary with entries for `"Paddle"` and `"Brick"`. In `BallController.OnCollisionEnter2D`, call `AudioManager.Instance?.PlayCollisionSound("Paddle")` or `PlayCollisionSound("Brick")` based on the tag of the collided object. Assign the new clips in the Inspector.

> **Hint:** make sure the `Paddle` and `Brick` GameObjects (and their prefabs) have their **Tag** fields set to `"Paddle"` and `"Brick"` respectively — otherwise `CompareTag` will not match.
