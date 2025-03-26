## Week 06

## Previous Class

Link to the previous class: [Week 05](https://github.com/otago-polytechnic-bit-courses/ID623001-introductory-game-development/blob/main-s1-25/lecture-notes/05-input-manager-clamping-prefab-variants.md)

---

## Space Invaders Game

In this module, you will continue to develop **Space Invaders** using Unity.

> **Note:** The following content does not take into account last week's formative assessment. Please ensure you have completed the formative assessment before continuing with this week's content. 

---

## Game Manager

In the **Hierarchy**, create an empty game object and name it `Game`. Create a new script called `GameController` and attach it to the `Game` object. In the `GameController` script, add the following code:

```csharp
// Omitted for brevity

public class GameController : MonoBehaviour
{
    public void WinGame()
    {
        Debug.Log("You win!");
    }
}
```

> **Note:** This is for debugging purposes only.

In the `EnemyController` script, update the `OnTriggerEnter2D` method to the following:

```csharp
// Omitted for brevity

public class EnemyController : MonoBehaviour
{
    // Omitted for brevity
    [SerializeField] GameController gameController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Laser"))
        {
            if (transform.parent.childCount <= 1)
            {
                gameController.WinGame();
            }

            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        // Omitted for brevity
    }
}
```

When the last enemy is destroyed, the `WinGame` method will be called. For each `Enemy` and `Enemy2` **Prefab**, drag the `Game` object into the `Game Controller` field.

Click on the **Play** button to test the game. When the last enemy is destroyed, you should see the message "You win!" in the console.

---

## Enemy Laser

Create a **Prefab** for the enemy laser. This **Prefab** will be similar to the player's laser, but with a different sprite. Create a new script called `EnemyLaserController` and attach it to the **Prefab**. In the `EnemyLaserController` script, add the following code:

```csharp
// Omitted for brevity

public class EnemyLaserController : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
}
```

**Task:**

1. The code is similar to the `LaserController` script, but the laser moves downwards. Think about how you can improve the code by reusing the `LaserController` script.

---

## Invoke Repeating

The `InvokeRepeating` method is used to call a method repeatedly after a specified delay. In the `EnemyController` script, add the following code:

```csharp
// Omitted for brevity

public class EnemyController : MonoBehaviour
{
    // Omitted for brevity
    [SerializeField] GameObject laserPrefab;

    void Start()
    {
        InvokeRepeating(nameof(Fire), 1f, 1f);
    }

    void Fire()
    {
        if (Random.value < (1f / transform.parent.childCount))
        {
            Instantiate(laserPrefab, transform.position, Quaternion.identity);           
        }        
    }

    // Omitted for brevity
}
```

What does this code do?

- `InvokeRepeating(nameof(Fire), 1f, 1f)` calls the `Fire` method every second.
- `Random.value < (1f / transform.parent.childCount)` is used to determine the probability of firing a laser. The probability is inversely proportional to the number of enemies left.
- `Instantiate(laserPrefab, transform.position, Quaternion.identity)` creates a new enemy laser at the enemy's position.

**Tasks:**

1. Click on the `Enemy` and `Enemy2` **Prefab** and drag the enemy laser **Prefab** into the `Laser Prefab` field.
2. In the `KillZone` script, destroy the enemy laser when it collides with the kill zone.
2. Add a **Box Collider 2D** component to the player **Prefab**.
3. In the `PlayerController` script destroy the enemy or enemy laser and the player when they collide.

Click on the **Play** button to test the game. The enemies should now fire lasers at the player.

---

## API Integration

You are going to learn how to consume an API using Unity. In this example, you will use the **Random User Generator** API to get a random user's information.

In the **Assets > Scripts** folder, create a new folder called `API`. Inside the `API` folder, create a new script called `APIController`. In the `APIController` script, add the following code:

```csharp
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class ApiResponse
{
    public List<User> results;
}

[Serializable]
public class Login
{
    public string uuid;
    public string username;
}

[Serializable]
public class User
{
    public Login login;
}

public class APIController : MonoBehaviour
{
    private const string API_URL = "https://randomuser.me/api/"; 

    public string uuid;
    public string username;

    void Start()
    {
        StartCoroutine(FetchUserData());
    }

    IEnumerator FetchUserData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(API_URL))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                ApiResponse apiResponse = JsonUtility.FromJson<ApiResponse>(request.downloadHandler.text);
                if (apiResponse.results.Count > 0)
                {
                    User user = apiResponse.results[0];
                    uuid = user.login.uuid;
                    username = user.login.username;

                    Debug.Log($"Login UUID: {uuid}");
                    Debug.Log($"Username: {username}");
                }
            }
            else
            {
                Debug.LogError("Failed to fetch user data: " + request.error);
            }
        }
    }
}
```

What does this code do?

- The `ApiResponse` class is used to deserialize the JSON response from the API.
- The `Login` class is used to deserialize the `login` object in the JSON response.
- The `User` class is used to deserialize the `results` object in the JSON response.
- The `APIController` class is used to fetch user data from the API.
- The `API_URL` constant is the URL of the API.
- The `Start` method calls the `FetchUserData` coroutine when the game starts. A coroutine is a function that can pause its execution and return control to Unity but then continue where it left off on the following frame.
- The `FetchUserData` coroutine sends a GET request to the API and deserializes the JSON response. The user's login UUID and username are logged to the console.

Attach the `APIController` script to the `Game` **GameObject**. Click on the **Play** button to test the game. The user's login UUID and username should be logged to the console.

---

## Saving to File

In the `GameController` script, add the following code:

```csharp
// Omitted for brevity
using System.IO;

public class GameController : MonoBehaviour
{
    public void WinGame(string uuid, string username)
    {
        Debug.Log("You win!");
        SaveDataToFile(uuid, username);
    }

    private void SaveDataToFile(string uuid, string username)
    {
        string path = $"{Application.dataPath}/data.txt";
        string data = $"UUID: {uuid}\nUsername: {username}";
        File.WriteAllText(path, data);
        Debug.Log($"Data saved to file: {path}");
    }
}
```

What does this code do?

- The `WinGame` method calls the `SaveDataToFile` method when the player wins the game.
- The `SaveDataToFile` method saves the user's login UUID and username to a text file. The file is saved in the `Assets` folder.

In the `EnemyController` script, update the `WinGame` method to the following:

```csharp
// Omitted for brevity

public class EnemyController : MonoBehaviour
{
    // Omitted for brevity
    [SerializeField] APIController apiController;

    // Omitted for brevity

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Laser"))
        {
            if (transform.parent.childCount <= 1)
            {
                gameController.WinGame(apiController.uuid, apiController.username);
            }

            // Omitted for brevity
        }

        // Omitted for brevity
    }
}
```

Click on the **Play** button to test the game. When the player wins the game, the user's login UUID and username should be saved to a text file.

---

## Next Class

Link to the next class: [Week 07]()