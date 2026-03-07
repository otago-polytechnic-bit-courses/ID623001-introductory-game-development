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

![](<../../resources (ignore)/img/week-03.1-breakout-3-audio-scene-management-state-machines/00.png>)

---

## Scene Management

Scene management is the process of loading and unloading scenes in a game. In Unity, you can use the `SceneManager` class to manage scenes. You can load a scene by name or by index, and you can also unload scenes when they are no longer needed.

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

---


## Coroutines

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
        goText.text = "";

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
