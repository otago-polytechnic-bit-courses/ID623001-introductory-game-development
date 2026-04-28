# Week 08.2 - Rogue-Like: State Machines

A **State Machine** is a way of organising a game object's behaviour into a set of distinct states - such as Idle, Chase, and Attack - where only one state is active at a time and rules control when the object transitions between them.

State machines are used extensively in games: enemy AI, animation controllers, game flow management, and UI menus all commonly use this pattern.

---

## 1. How This Fits

The state machine lives entirely inside `EnemyController`, which already sits on the enemy GameObject from previous weeks. There is nothing new to add to the Hierarchy or drag into the Inspector for the basic enum version - you are replacing the logic already inside `Update()`.

For the class-based version in section 4, each state becomes its own script file in your `Scripts` folder. They do not get attached to any GameObject directly - instead `EnemyController` creates and manages them in code.

Here is how the pieces relate:

| Thing                              | Where it lives           | Purpose                                     |
| ---------------------------------- | ------------------------ | ------------------------------------------- |
| `EnemyState` enum                  | Inside `EnemyController` | Lists all possible states                   |
| `currentState` variable            | Inside `EnemyController` | Tracks which state is active right now      |
| `switch` block in `Update()`       | Inside `EnemyController` | Runs the correct logic each frame           |
| `State`, `IdleState`, `ChaseState` | Separate script files    | Class-based alternative for larger projects |

---

## 2. The Problem

Without a state machine, AI logic quickly becomes a tangle of nested `if` statements. Adding a new behaviour (e.g. a Flee state) means editing conditions scattered across `Update()`. A state machine keeps each behaviour isolated and makes transitions explicit.

---

## 3. States as an Enum

The simplest state machine uses an `enum` to define all possible states and a `switch` statement to run the correct logic each frame.

**Step 1** - Define an `EnemyState` enum and add it to `EnemyController`:

```csharp
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private enum EnemyState { Idle, Chase, Attack }

    [SerializeField] private float chaseRange = 6f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private Rigidbody2D rb;

    private EnemyState currentState = EnemyState.Idle;

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);

        switch (currentState)
        {
            case EnemyState.Idle:
                rb.linearVelocity = Vector2.zero;
                if (distanceToPlayer < chaseRange)
                    currentState = EnemyState.Chase;
                break;

            case EnemyState.Chase:
                ChasePlayer();
                if (distanceToPlayer < attackRange)
                    currentState = EnemyState.Attack;
                else if (distanceToPlayer > chaseRange)
                    currentState = EnemyState.Idle;
                break;

            case EnemyState.Attack:
                rb.linearVelocity = Vector2.zero;
                // Attack logic goes here
                if (distanceToPlayer > attackRange)
                    currentState = EnemyState.Chase;
                break;
        }
    }

    private void ChasePlayer()
    {
        Vector2 direction = (PlayerController.Instance.transform.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }
}
```

What is happening in the code above?

| Element                 | Purpose                                                       |
| ----------------------- | ------------------------------------------------------------- |
| `EnemyState` enum       | Defines all possible states in one place                      |
| `currentState`          | Tracks which state is currently active                        |
| `switch (currentState)` | Runs only the logic for the active state each frame           |
| Transition conditions   | Distance checks inside each `case` that switch to a new state |

Click **Play**. The enemy should stand still until the player enters `chaseRange`, then chase. When close enough, it stops and enters the attack state.

---

## 4. Entry and Exit Logic

Sometimes a state needs to run setup code once when it becomes active (e.g. play an animation, reset a timer) rather than every frame. A simple way to handle this is to track whether the state has just changed.

```csharp
private EnemyState currentState = EnemyState.Idle;
private EnemyState previousState;

void Update()
{
    bool stateChanged = currentState != previousState;
    previousState = currentState;

    switch (currentState)
    {
        case EnemyState.Idle:
            if (stateChanged)
            {
                Debug.Log("Entered Idle - reset patrol timer");
                // Run any one-time setup here
            }
            // Per-frame idle logic
            break;

        case EnemyState.Chase:
            ChasePlayer();
            break;

        case EnemyState.Attack:
            if (stateChanged)
            {
                Debug.Log("Entered Attack - start attack animation");
            }
            // Per-frame attack logic
            break;
    }
}
```

---

## 5. State Machine with Classes

For more complex AI, each state can be its own class. This is cleaner when states have a lot of logic and makes it easy to add new states without changing existing ones.

**Step 1** - Create a base `State` class and concrete state classes:

```csharp
public abstract class State
{
    protected EnemyController enemy;

    public State(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public virtual void OnEnter() { }
    public virtual void OnUpdate() { }
    public virtual void OnExit() { }
}

public class IdleState : State
{
    public IdleState(EnemyController enemy) : base(enemy) { }

    public override void OnEnter()
    {
        enemy.rb.linearVelocity = Vector2.zero;
    }

    public override void OnUpdate()
    {
        float distance = Vector2.Distance(enemy.transform.position, PlayerController.Instance.transform.position);
        if (distance < enemy.chaseRange)
            enemy.ChangeState(new ChaseState(enemy));
    }
}

public class ChaseState : State
{
    public ChaseState(EnemyController enemy) : base(enemy) { }

    public override void OnUpdate()
    {
        Vector2 direction = (PlayerController.Instance.transform.position - enemy.transform.position).normalized;
        enemy.rb.linearVelocity = direction * enemy.speed;

        float distance = Vector2.Distance(enemy.transform.position, PlayerController.Instance.transform.position);
        if (distance > enemy.chaseRange)
            enemy.ChangeState(new IdleState(enemy));
    }
}
```

**Step 2** - Update `EnemyController` to manage the current state object:

```csharp
public class EnemyController : MonoBehaviour
{
    public float chaseRange = 6f;
    public float speed = 2f;
    public Rigidbody2D rb;

    private State currentState;
    {
        ChangeState(new IdleState(this));
    }

    void Update()
    {
        currentState?.OnUpdate();
    }

    public void ChangeState(State newState)
    {
        currentState?.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }
}
```

What changed compared to the enum approach?

| Aspect                     | Enum approach           | Class approach               |
| -------------------------- | ----------------------- | ---------------------------- |
| Adding a new state         | Edit the `switch` block | Create a new class           |
| Per-state entry/exit logic | Manual flag tracking    | `OnEnter` / `OnExit` methods |
| Complexity                 | Low                     | Higher, but more scalable    |

---

## 6. Unity's Animator as a State Machine

Unity's built-in **Animator** is itself a visual state machine. Each state plays an animation clip, and **Transitions** between states are triggered by parameters you set from code.

**Step 1** - In the Animator window, create states for `Idle`, `Walk`, and `Attack`. Connect them with transitions.

**Step 2** - Add parameters in the Animator: a `float` called `Speed` and a `trigger` called `Attack`.

**Step 3** - Set the parameters from `EnemyController`:

```csharp
[SerializeField] private Animator animator;

void Update()
{
    float speed = rb.linearVelocity.magnitude;
    animator.SetFloat("Speed", speed);
}

public void TriggerAttack()
{
    animator.SetTrigger("Attack");
}
```

The Animator handles transitions automatically once the parameters are set. This keeps animation logic separate from gameplay logic.

---

## Exercises

---

### Task 1 - Add a Flee State

Add a `Flee` state to the enum state machine. When the enemy's health drops below 25%, it should move directly away from the player instead of toward it.

> **Hint:** Reverse the direction vector: `direction = (transform.position - PlayerController.Instance.transform.position).normalized`.

---

### Task 2 - Add a Patrol State

Add a `Patrol` state where the enemy moves between two waypoints when the player is out of range. The enemy should switch to `Chase` when the player comes close.

> **Hint:** Serialise two `Transform` waypoints. Move between them using `Vector2.MoveTowards` and toggle the target when within a small distance.

---

### Task 3 - Class-Based Attack State

Using the class-based approach, create an `AttackState` that fires a bullet at the player every 2 seconds. The state should transition back to `ChaseState` if the player moves out of attack range.

> **Hint:** Use a `Coroutine` with `WaitForSeconds(2f)` in a loop to fire repeatedly. Start it in `OnEnter` and stop it in `OnExit` using `StopAllCoroutines()`.
