# Week 09.1 - Rogue-Like: Object Pooling

**Object Pooling** is a performance technique where a fixed set of GameObjects is created once at the start of the game and reused, rather than instantiated and destroyed repeatedly during gameplay.

It is especially important for bullets, enemies, and particle effects - objects that are created and destroyed at high frequency.

---

## 1. How This Fits

The pool is managed by a single `ObjectPool` script on an empty GameObject in the scene. Instead of calling `Instantiate` and `Destroy` in `PlayerController` or `BulletController`, you ask the pool for an available object and return it when you are done.

| Script             | Where it goes                           | What it does                                                         |
| ------------------ | --------------------------------------- | -------------------------------------------------------------------- |
| `ObjectPool`       | An empty GameObject called `BulletPool` | Creates and manages a collection of reusable bullets                 |
| `BulletController` | The bullet prefab                       | Returns itself to the pool on collision instead of destroying itself |
| `PlayerController` | The player GameObject                   | Requests a bullet from the pool instead of calling `Instantiate`     |

---

## 2. The Problem

Every time `Instantiate` is called, Unity allocates memory for a new object. Every time `Destroy` is called, that memory is marked for cleanup by the **garbage collector** - a process that can cause small but noticeable freezes during gameplay. In a roguelike where the player fires constantly and many enemies are active at once, this adds up quickly.

Object pooling solves this by recycling objects. Instead of creating and destroying, objects are simply **activated** and **deactivated**.

---

## 3. Creating a Pool

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

## 4. Using the Pool

Update `PlayerController` to request a bullet from the pool instead of instantiating one:

```csharp
// Before - creates a new object every shot
// Instantiate(bulletPrefab, transform.position, gunTransform.rotation);

// After - retrieves a pooled object
void Update()
{
    if (Input.GetMouseButtonDown(0))
    {
        GameObject bullet = ObjectPool.Instance.Get();
        bullet.transform.position = transform.position;
        bullet.transform.rotation = gunTransform.rotation;
    }
}
```

---

## 5. Returning Bullets to the Pool

Update `BulletController` to return itself to the pool instead of destroying itself:

```csharp
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;
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
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        ObjectPool.Instance.ReturnToPool(gameObject);
    }
}
```

> **Note:** `OnEnable` is used here instead of `Start`. Because pooled objects are reused, `Start` only runs once when the object is first created. `OnEnable` runs every time the object is activated, which is what we want.

Click **Play** and fire several bullets. In the Hierarchy, you should see the bullet objects being activated and deactivated rather than appearing and disappearing from the list entirely.

---

## 6. Multiple Pools

For a roguelike with both bullets and enemies, a separate pool is needed for each type. The simplest approach is to create multiple `ObjectPool` instances with different prefabs.

A more scalable approach is a generic pool manager that handles multiple types by key:

```csharp
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [System.Serializable]
    public class Pool
    {
        public string key;
        public GameObject prefab;
        public int size;
    }

    [SerializeField] private List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    void Awake()
    {
        Instance = this;
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }
            poolDictionary[pool.key] = queue;
        }
    }

    public GameObject Get(string key)
    {
        if (!poolDictionary.ContainsKey(key)) return null;

        GameObject obj = poolDictionary[key].Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void Return(string key, GameObject obj)
    {
        obj.SetActive(false);
        poolDictionary[key].Enqueue(obj);
    }
}
```

You would then request objects by key:

```csharp
GameObject bullet = PoolManager.Instance.Get("PlayerBullet");
GameObject enemy  = PoolManager.Instance.Get("Enemy");
```

---

## Exercises

---

### Task 1 - Enemy Pool

Create a second pool for enemies using the single `ObjectPool` script on a new `EnemyPool` GameObject. Spawn enemies from the pool at random positions around the edge of the room every few seconds using a `Coroutine`.

> **Hint:** Use `WaitForSeconds` in the coroutine and `ObjectPool.Instance.Get()` to retrieve an enemy. Remember to reset its position before activating it.

---

### Task 2 - Pool Size Warning

Add a `Debug.LogWarning` message to the `Get()` fallback branch that alerts you when the pool has run out of objects. Use this to tune your pool size during testing.

> **Hint:** The fallback branch is the `else` case inside `Get()` when `pool.Count == 0`.

---

### Task 3 - Return on Room Exit

When the player moves to a new room, return all active enemies to the pool so the old room is cleared.

> **Hint:** Keep a `List<GameObject>` of active enemies in `PoolManager`. Call `Return` on each one when the room transition triggers.
