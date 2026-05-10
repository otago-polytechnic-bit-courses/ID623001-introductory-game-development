# Week 09.1 - Rogue-Like: Object Pooling

**Object Pooling** is a performance technique where a fixed set of GameObjects is created once at the start of the game and reused, rather than instantiated and destroyed repeatedly during gameplay.

It is especially important for bullets, enemies, and particle effects - objects that are created and destroyed at high frequency.

---

## 1. How This Fits

Two separate pools manage bullets and enemies. Instead of calling `Instantiate` and `Destroy` in `PlayerController`, `BulletController`, or `EnemyController`, you ask the pool for an available object and return it when you are done.

| Script             | Where it goes                           | What it does                                                         |
| ------------------ | --------------------------------------- | -------------------------------------------------------------------- |
| `ObjectPool`       | An empty GameObject called `BulletPool` | Creates and manages a collection of reusable bullets                 |
| `EnemyPool`        | An empty GameObject called `EnemyPool`  | Creates and manages a collection of reusable enemies                 |
| `BulletController` | The bullet prefab                       | Calls `EnemyController.TakeDamage()` on hit, then returns to pool    |
| `EnemyController`  | The enemy prefab                        | Returns itself to `EnemyPool` on death instead of destroying itself  |
| `PlayerController` | The player GameObject                   | Requests a bullet from `ObjectPool` instead of calling `Instantiate` |

---

## 2. The Problem

Every time `Instantiate` is called, Unity allocates memory for a new object. Every time `Destroy` is called, that memory is marked for cleanup by the **garbage collector** - a process that can cause small but noticeable freezes during gameplay. In a roguelike where the player fires constantly and many enemies are active at once, this adds up quickly.

Object pooling solves this by recycling objects. Instead of creating and destroying, objects are simply **activated** and **deactivated**.

---

## 3. Creating a Bullet Pool

**Step 1** - In the Hierarchy, create a new empty GameObject called `BulletPool`.

**Step 2** - In the `Scripts` folder, create a new script called `ObjectPool` and attach it to `BulletPool`.

**Step 3** - Add the following code:

```csharp
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize = 20;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject Get()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        // Pool is empty - create a new one as a fallback
        Debug.LogWarning("ObjectPool: pool exhausted, consider increasing pool size.");
        GameObject newObj = Instantiate(prefab, transform);
        return newObj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
```

What is happening in the code above?

| Element                | Purpose                                                      |
| ---------------------- | ------------------------------------------------------------ |
| `Queue<GameObject>`    | Stores available (inactive) objects in order                 |
| `Awake` loop           | Pre-creates all pooled objects and deactivates them          |
| `Get()`                | Takes an object from the queue, activates it, and returns it |
| `ReturnToPool()`       | Deactivates the object and puts it back in the queue         |
| Fallback `Instantiate` | Handles the rare case where all pooled objects are in use    |

**Step 4** - Select `BulletPool` in the Hierarchy. In the Inspector, drag the `PlayerBullet` prefab into the **Prefab** field and set **Pool Size** to `20`.

---

## 4. Creating an Enemy Pool

Enemies need their own separate pool because they use a different prefab and require state to be reset on reuse (health, navigation path).

**Step 1** - In the Hierarchy, create a new empty GameObject called `EnemyPool`.

**Step 2** - In the `Scripts` folder, create a new script called `EnemyPool` and attach it to the `EnemyPool` GameObject.

```csharp
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance;

    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject Get(Vector3 position)
    {
        GameObject obj = pool.Count > 0
            ? pool.Dequeue()
            : Instantiate(prefab, transform);

        obj.transform.position = position;
        obj.SetActive(true);
        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    // Call this when the player moves to a new room to clear all active enemies
    public void ReturnAll()
    {
        foreach (EnemyController enemy in FindObjectsByType<EnemyController>(FindObjectsSortMode.None))
            ReturnToPool(enemy.gameObject);
    }
}
```

**Step 3** - Select `EnemyPool` in the Hierarchy. Drag the enemy prefab into the **Prefab** field and set **Pool Size** to `10`.

---

## 5. Using the Bullet Pool

Update `PlayerController` to request a bullet from the pool instead of instantiating one:

```csharp
void Update()
{
    // Before - creates a new object every shot:
    // Instantiate(bulletPrefab, transform.position, gunTransform.rotation);

    // After - retrieves a pooled object:
    if (Input.GetMouseButtonDown(0))
    {
        GameObject bullet = ObjectPool.Instance.Get();
        bullet.transform.position = transform.position;
        bullet.transform.rotation = gunTransform.rotation;
    }
}
```

---

## 6. Returning Bullets to the Pool

`BulletController` now calls `EnemyController.TakeDamage()` on collision (rather than destroying the enemy directly), then returns itself to the pool. `OnEnable` is used instead of `Start` because pooled objects are reused - `Start` only runs once when the object is first created, but `OnEnable` runs every time it is activated.

```csharp
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float damage = 10f;

    private Rigidbody2D rb;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;

        // Return to pool after a set time even if no collision occurs
        Invoke(nameof(ReturnToPool), lifetime);
    }

    void OnDisable()
    {
        CancelInvoke();
        // Clear velocity so the bullet does not carry momentum when reused
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Notify the enemy rather than destroying it directly
        EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
        if (enemy != null)
            enemy.TakeDamage(damage);

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        ObjectPool.Instance.ReturnToPool(gameObject);
    }
}
```

---

## 7. Returning Enemies to the Pool

`EnemyController` must reset its state when activated and return itself to `EnemyPool` on death. Replace `Start` with `OnEnable` so health and timers reset each time the enemy is retrieved from the pool.

```csharp
// In EnemyController:

[SerializeField] private float maxHealth = 100f;
private float health;

// OnEnable runs every time a pooled enemy is activated - resets its state
private void OnEnable()
{
    health = maxHealth;
    attackTimer = 0f;
}

public void TakeDamage(float amount)
{
    health -= amount;
    if (health <= 0f)
        Die();
}

private void Die()
{
    // Return to pool instead of Destroy(gameObject)
    EnemyPool.Instance.ReturnToPool(gameObject);
}
```

> **Note:** `EnemyNavigator` also resets its path in `OnEnable` - see the `EnemyNavigator` script for details.

---

## 8. Checking It Works

Click **Play** and fire several bullets. In the Hierarchy, you should see the bullet objects being activated and deactivated rather than appearing and disappearing from the list entirely. Kill an enemy - it should deactivate and return to the pool rather than being removed from the Hierarchy.

---

## Exercises

---

### Task 1 - Spawn Enemies from the Pool

Use a `Coroutine` in a `EnemySpawner` script to retrieve enemies from `EnemyPool` at random positions around the edge of the current room every few seconds.

> **Hint:** Use `WaitForSeconds` in the coroutine and `EnemyPool.Instance.Get(position)` to retrieve an enemy. The position must be set before `SetActive(true)` is called - the pool's `Get` method handles this automatically.

---

### Task 2 - Pool Size Warning

A `Debug.LogWarning` is already included in `ObjectPool.Get()` when the pool runs out. Enter Play mode, fire rapidly, and watch the Console. Use this to tune your pool size during testing.

---

### Task 3 - Return on Room Exit

When the player moves to a new room, return all active enemies to the pool so the old room is cleared.

> **Hint:** Call `EnemyPool.Instance.ReturnAll()` from the `Room` script when `OnTriggerEnter2D` fires.
