# Week 02.2

---

## Important Links

| Section        | Link                                                                            |
| -------------- | ------------------------------------------------------------------------------- |
| Previous Class | [Week 02.2](lecture-notes/week-02.2-breakout-2-scriptable-objects-ui/README.md) |
| Next Class     | [Week 03.1]()                                                                   |

---

## Audio

Audio is an important part of any game. It can help to set the tone and atmosphere of the game, as well as provide feedback to the player. You can use audio in Unity by importing audio files into your project and then playing them through audio sources.

1. In the `week-03-breakout-audio-scene-management-player-prefs-builds` folder, there is an `Audio` folder with a variety of different audio files. Copy and paste the `Audio` folder into the `Assets` folder of your Unity project. 

3. In the Assets folder, create a new script called `AudioManager`. Open the script in your code editor and add the following code:

```csharp
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Ball Sounds Settings")]
    [SerializeField] private AudioClip wallHitSound;

    private AudioSource audioSource;
    private Dictionary<string, AudioClip> tagToSound;

    private void Awake()
    {
        if (Instance != null) 
        { 
            Destroy(gameObject); 
            return; 
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        tagToSound = new Dictionary<string, AudioClip>
        {
            { "Walls",  wallHitSound }
        };
    }

    public void PlayCollisionSound(string tag)
    {
        if (tagToSound.TryGetValue(tag, out AudioClip clip))
            PlaySound(clip);
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }
}
```

4. In the `BallController` script, add a reference to the `AudioManager` and play a sound when the ball collides with a wall. 

```csharp
// Omitted for brevity

public class BallController : MonoBehaviour
{
    // Omitted for brevity

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.CompareTag("Walls"))
            AudioManager.Instance?.PlayCollisionSound("Walls");
    }
}
```

5. In the Hierarchy panel, create an empty GameObject and name it `Audio Manager`. Drag and drop the `AudioManager` script from the Project panel onto the `Audio Manager` GameObject in the Hierarchy panel. In the Inspector panel for the `Audio Manager` GameObject, you should see a new component called **Audio Manager** with a field for **Wall Hit Sound**. Set this field to one of the audio clips from the `Audio` folder.

![](<../../resources (ignore)/img/week-03.1-breakout-3-audio-scene-management-player-prefs-coroutines/00.png>)

---

## Scene Management

Scene management is the process of loading and unloading scenes in a game. In Unity, you can use the `SceneManager` class to manage scenes. You can load a scene by name or by index, and you can also Remove Scenes when they are no longer needed.

1. In the `Scenes` folder, right-click and select **Create > Scene**. Name the new scene `MainMenuScence`. Open the `MainMenuScence` scene.

![](<../../resources (ignore)/img/week-03.1-breakout-3-audio-scene-management-player-prefs-coroutines/01.png>)

2. Drag and drop the `MainMenuScene` from the `Scenes` folder into the Hierarchy panel. 

3. Delete the `Audio Manager` GameObject from the `SampleScene` since we want the audio manager to persist across scenes and we will be creating a new one in the `MainMenuScene`. 

4. Remove the `SampleScene` by right-clicking on it in the Hierarchy panel and selecting **Remove Scene**.

5. In the `Assets` folder, create a new script called `MainMenuManager`. Open the script in your code editor and add the following code:

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
        playButton.onClick.AddListener(OnPlayClicked);
    }

    private void OnPlayClicked()
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveAllListeners();
    }
}
```

6. In the Hierarchy panel, right-click and select **UI (Canvas) > Canvas**. Add text - "Breakout" and three buttons - "Play", "Settings", and "Quit" as children of the Canvas GameObject. These main menu UI elements are in an empty GameObject named `Main Container`. Arrange the text and buttons in a way that looks good to you.

7. In the Hierarchy panel, create an empty GameObject and name it `Main Menu Manager`. Drag and drop the `MainMenuManager` script from the Project panel onto the `Main Menu Manager` GameObject in the Hierarchy panel. Set the field for **Play Button** to the "Play" button that you created in the Canvas. 

![](<../../resources (ignore)/img/week-03.1-breakout-3-audio-scene-management-player-prefs-coroutines/02.png>)

8. In the `Build Profiles` window, make sure that both the `MainMenuScene` and the `SampleScene` are added to the build. You can open the `Build Profiles` window by going to **File > Build Profiles**. Click on the **Add Open Scenes** button to add the currently open scene to the build. Make sure that the `MainMenuScene` is at index 0 and the `SampleScene` is at index 1.

![](<../../resources (ignore)/img/week-03.1-breakout-3-audio-scene-management-player-prefs-coroutines/03.png>)

---

## Player Prefs

Player prefs are a way to save and load player data in Unity. You can use the `PlayerPrefs` class to save and load data such as high scores, settings, and other player preferences. The `PlayerPrefs` class provides methods for saving and loading data of different types, such as integers, floats and strings.

1. In the Hierarchy panel, create an empty GameObject and name it `Audio Manager`. The setuo is similar to the one we did in the `SampleScene` but this time we will be using player prefs to save and load the sound effects volume. Drag and drop the `AudioManager` script from the Project panel onto the `Audio Manager` GameObject in the Hierarchy panel. 

2. Update the `AudioManager` script to save and load the sound effects volume using player prefs. Add the following code to the `AudioManager` script:

```csharp
// Omitted for brevity

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    // Omitted for brevity
    public float SoundXFVolume { get; private set; }

    private void Awake()
    {
        // Omitted for brevity

        SoundXFVolume = PlayerPrefs.GetFloat("SoundXFVolume", 1f);
    }

    // Omitted for brevity

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip, SoundXFVolume);
    }

    public void SetSoundXFVolume(float volume)
    {
        SoundXFVolume = volume;
        PlayerPrefs.SetFloat("SoundXFVolume", volume);
        PlayerPrefs.Save();
    }
}

3. In the `Assets` folder, create a new script called `SettingsManager`. Open the script in your code editor and add the following code:

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
        soundXFSlider.value = AudioManager.Instance.SoundXFVolume;

        soundXFSlider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void OnSliderChanged(float value)
    {
        AudioManager.Instance.SetSoundXFVolume(value);
    }

    private void OnBackClicked()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        soundXFSlider.onValueChanged.RemoveAllListeners();
    }
}
```

3. In the Hierarchy panel, right-click and select **UI (Canvas) > Panel**. Name the new panel `Settings Panel`. This panel will be used to hold the settings UI elements. 

![](<../../resources (ignore)/img/week-03.1-breakout-3-audio-scene-management-player-prefs-coroutines/04.png>)

4. Drag and drop the `SettingsController` script from the Project panel onto the `Settings Panel` GameObject in the Hierarchy panel. Set the field for **Sound XF Slider** to a new slider that you create as a child of the `Settings Panel`.

![](<../../resources (ignore)/img/week-03.1-breakout-3-audio-scene-management-player-prefs-coroutines/05.png>)

5. You will need to write code to open the settings panel when the "Settings" button is clicked in the main menu, and to close the settings panel when the "Back" button is clicked in the settings panel. 

---


## Coroutines

Coroutines are a powerful tool in Unity that allow you to execute code over multiple frames. They are often used for tasks that require waiting, such as animations, timers or sequences of events. Coroutines are implemented using the `IEnumerator` interface and the `yield return` statement.

1. Drag and drop the `SampleScene` from the `Scenes` folder into the Hierarchy panel. 

2. Remove the `MainMenuScene` by right-clicking on it in the Hierarchy panel and selecting **Remove Scene**.

3. In the Canvas GameObject, add a new TextMeshPro text element as a child of the Canvas. This text element will be used to display the countdown before the game starts. Name the TextMeshPro text element `Go Text`. Set the text to "Press Space to Start".

![](<../../resources (ignore)/img/week-03.1-breakout-3-audio-scene-management-player-prefs-coroutines/06.png>)

4. Update the `GameManager` script to include a coroutine that displays a countdown before the game starts. Add the following code to the `GameManager` script:

```csharp
// Omitted for brevity
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Omitted for brevity

    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI goText;

    // Omitted for brevity

    private void Start()
    {
        goText.text = "Press Space to Start";

        // Omitted for brevity
    }

    public void OnStartGame(InputAction.CallbackContext context)
    {
        // Omitted for brevity

        StartCoroutine(GoRoutine());
    }

    // Omitted for brevity

    private IEnumerator GoRoutine()
    {
        gameManagerMap.Disable();
        goText.gameObject.SetActive(true);

        string[] sequence = { "3", "2", "1", "Go!" };

        foreach (string step in sequence)
        {
            goText.text = step;
            yield return new WaitForSeconds(1f);
        }

        goText.gameObject.SetActive(false);
        paddleMap.Enable();
        Instantiate(slowBallPrefab, Vector2.zero, Quaternion.identity);
    }
}
```

5. In the Inspector panel for the `Game Manager` GameObject, set the field for **Go Text** to the `Go Text` TextMeshPro text element that you created in the Canvas.

![](<../../resources (ignore)/img/week-03.1-breakout-3-audio-scene-management-player-prefs-coroutines/07.png>)

6. Click the **Play** button at the top of the Unity Editor to run the game. You should see the "Press Space to Start" text on the screen. When you press the spacebar, a countdown will begin, displaying "3", "2", "1", and then "Go!" before the game starts.

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
