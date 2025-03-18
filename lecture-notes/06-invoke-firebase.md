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

---

## Next Class

Link to the next class: [Week 07]()