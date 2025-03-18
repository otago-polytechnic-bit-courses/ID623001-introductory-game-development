## Week 06

## Previous Class

Link to the previous class: [Week 05]()

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

## Next Class

Link to the next class: [Week 07]()