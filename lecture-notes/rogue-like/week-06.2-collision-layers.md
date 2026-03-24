## 1. Singleton Pattern

A **Singleton** ensures that only one instance of a class exists at a time, and provides global access to it. Add the following to `PlayerController`:

```csharp
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
```

This allows other scripts — such as `EnemyController` — to access the player via `PlayerController.Instance` without needing a serialised reference.

---

## 2. Enemy

**Step 1** - Create a new GameObject for an enemy. The setup should mirror the player: a sprite, a `CircleCollider2D`, and a `Rigidbody2D`.

![](../../resources%20(ignore)/img/09-images/09-image-1.png)

**Step 2** - In the `Scripts` folder, create a new script called `EnemyController` and attach it to the enemy GameObject. Add the following code:

```csharp
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float rangeToPlayer;

    private Vector3 direction;

    void Update()
    {
        if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) < rangeToPlayer)
        {
            direction = PlayerController.Instance.transform.position - transform.position;
        }
        else
        {
            direction = Vector3.zero;
        }

        direction.Normalize();
        rb.linearVelocity = direction * speed;
    }
}
```

What is happening in `Update()`?

- `Vector3.Distance` checks whether the player is within `rangeToPlayer` units.
- If the player is in range, `direction` is set toward the player's position.
- If not, `direction` is zeroed so the enemy stops moving.
- `direction.Normalize()` ensures the enemy moves at a constant speed regardless of distance.
- `rb.linearVelocity` applies the movement. Note that Unity 6 uses `linearVelocity` rather than the legacy `velocity`.

![](../../resources%20(ignore)/img/09-images/09-image-2.png)

---

## 3. Bullet

**Step 1** - Create a new GameObject for a bullet. Add a `BoxCollider2D` and a `Rigidbody2D` component to it.

**Step 2** - In the `Scripts` folder, create a new script called `BulletController` and attach it to the bullet GameObject.

![](../../resources%20(ignore)/img/09-images/09-image-3.png)

---

## 4. Camera Controller

The camera should smoothly follow the player as they move between rooms.

**Step 1** - In the `Scripts` folder, create a new script called `CameraController` and attach it to the **Main Camera**. Add the following code:

```csharp
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    [SerializeField] private float speed = 30f;
    [SerializeField] private Transform target;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(target.position.x, target.position.y, transform.position.z),
                speed * Time.deltaTime
            );
        }
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
```

What is happening in this script?

| Element | Purpose |
| --- | --- |
| `Instance` | Singleton — allows `Room` scripts to call `CameraController.Instance.ChangeTarget()` |
| `Vector3.MoveTowards` | Smoothly moves the camera toward the target at a fixed units-per-second rate |
| `ChangeTarget` | Allows other scripts to redirect the camera to a new target (e.g. a room centre) |

![](../../resources%20(ignore)/img/09-images/09-image-5.png)

---

## 5. Room Trigger

**Step 1** - In the `Scripts` folder, create a new script called `Room` and attach it to the `BasicRoom` prefab. Add the following code:

```csharp
using UnityEngine;

public class Room : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CameraController.Instance.ChangeTarget(transform);
        }
    }
}
```

When the player enters the trigger collider of a room, the camera's target is updated to that room's transform — snapping focus to the new room.

> **Note:** Create a **Tag** called `Player` and assign it to the `Player` GameObject. Tags are case-sensitive.

**Step 2** - Create a new **Empty GameObject** called `RoomTrigger`. Add a `BoxCollider2D` component and configure it as follows:

| Property    | Value  |
| ----------- | ------ |
| `Is Trigger`| `true` |
| `Size X`    | `16`   |
| `Size Y`    | `8`    |

![](../../resources%20(ignore)/img/09-images/09-image-7.png)

---

## 6. Collision Layers

**Collision Layers** control which GameObjects can physically collide with each other, preventing unwanted interactions.

**Step 1** - In the Inspector, click the **Layer** dropdown on any GameObject and select **Add Layer...**. Add the following layers:

- `Player`
- `PlayerBullet`
- `IgnoreBullet`

**Step 2** - Go to **Edit > Project Settings > Physics 2D**. In the **Layer Collision Matrix**, uncheck the following pairs:

| Layer A       | Layer B       | Reason                                           |
| ------------- | ------------- | ------------------------------------------------ |
| `Player`      | `PlayerBullet`| Prevents the player's own bullets hitting themselves |
| `PlayerBullet`| `PlayerBullet`| Prevents bullets colliding with each other       |

![](../../resources%20(ignore)/img/09-images/09-image-8.png)

**Step 3** - Assign layers to the following GameObjects and prefabs:

| GameObject / Prefab | Layer          |
| ------------------- | -------------- |
| `Player`            | `Player`       |
| `PlayerBullet`      | `PlayerBullet` |
| `RoomTrigger`       | `IgnoreBullet` |

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

### Task 1 — Enemy Facing Direction

Write the code so that the enemy flips horizontally to face the direction it is currently moving.

> **Hint:** Compare `rb.linearVelocity.x` to zero and set `transform.localScale` accordingly, similar to how the player handles facing direction.

---

### Task 2 — Bullet Movement and Destruction

Write the code in `BulletController` to move the bullet in the direction it is facing. The bullet should be destroyed when it collides with an enemy.

> **Hint:** Set `rb.linearVelocity` in `Start()` using `transform.right * speed`. Use `OnTriggerEnter2D` or `OnCollisionEnter2D` and call `Destroy(gameObject)` on collision.

---

### Task 3 — Bullet Prefab

Drag the bullet GameObject into the **Prefabs** folder in the Project panel to create a prefab. Delete the original from the Hierarchy.

---

### Task 4 — Shooting

Write the code to make the player shoot a bullet when the **left mouse button** is pressed. The bullet should be instantiated at the player's position, facing the direction the gun is pointing.

> **Hint:** Use `Instantiate(bulletPrefab, transform.position, gunTransform.rotation)` inside `Update()` when `Input.GetMouseButtonDown(0)` is true.

📖 Reference: [Unity — Object.Instantiate](https://docs.unity3d.com/ScriptReference/Object.Instantiate.html)

---

### Task 5 — Basic Room Prefab

Create a new GameObject called `BasicRoom`. Move the `Grid` and `Tilemap` GameObjects inside it. Drag the `BasicRoom` GameObject into the **Prefabs** folder. Add a `BasicRoom` prefab instance to the scene and confirm the player can move between rooms.

![](../../resources%20(ignore)/img/09-images/09-image-4.png)

![](../../resources%20(ignore)/img/09-images/09-image-6.png)