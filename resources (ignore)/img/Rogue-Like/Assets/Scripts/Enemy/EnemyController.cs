using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;

    private float currentHealth;
    private float attackTimer;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private bool isDead;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb = GetComponent<Rigidbody2D>();

        currentHealth = enemyData.health;

        if (enemyData.sprite != null)
            spriteRenderer.sprite = enemyData.sprite;
    }

    private void Update()
    {
        if (isDead) return;

        if (PlayerController.Instance == null) return;

        attackTimer -= Time.deltaTime;

        Vector2 toPlayer = (Vector2)(PlayerController.Instance.transform.position - transform.position);
        float sqrDist = toPlayer.sqrMagnitude;

        spriteRenderer.flipX = toPlayer.x < 0f;

        float detectionSqr = enemyData.detectionRange * enemyData.detectionRange;
        float attackSqr = enemyData.attackRange * enemyData.attackRange;

        if (sqrDist <= detectionSqr)
        {
            if (enemyData.enemyType == EnemyType.Ranged)
            {
                // TODO: Implement ranged attack logic
                Debug.Log($"Enemy Type: {enemyData.enemyType}, Distance to Player: {Mathf.Sqrt(sqrDist)}");

            }
            else
            {
                if (sqrDist > attackSqr)
                    MoveInDirection(toPlayer.normalized);

                Debug.Log($"Enemy Type: {enemyData.enemyType}, Distance to Player: {Mathf.Sqrt(sqrDist)}");
            }

            if (sqrDist <= attackSqr && attackTimer <= 0f)
            {
                Attack(toPlayer.normalized);
                attackTimer = enemyData.attackCooldown;

                Debug.Log($"Enemy Type: {enemyData.enemyType}, Distance to Player: {Mathf.Sqrt(sqrDist)}");
            }
        }
    }

    private void MoveInDirection(Vector2 direction)
    {
        rb.MovePosition(rb.position + enemyData.moveSpeed * Time.fixedDeltaTime * direction);
    }

    private void Attack(Vector2 directionToPlayer)
    {
        switch (enemyData.enemyType)
        {
            case EnemyType.Melee:
            case EnemyType.Swarm:
                PlayerController.Instance.TakeDamage(enemyData.damage);
                break;
            case EnemyType.Ranged:
                // TODO: Implement projectile logic later
                break;
            case EnemyType.Tank:
                PlayerController.Instance.TakeDamage(enemyData.damage);
                // TODO: Implement knockback logic later
                break;
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        float mitigated = Mathf.Max(0f, amount - enemyData.defense);
        currentHealth -= mitigated;

        StartCoroutine(FlashDamage());

        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        if (isDead) return;

        // TODO: Add splatter logic later

        Destroy(gameObject);
    }

    private IEnumerator FlashDamage()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.08f);
        spriteRenderer.color = Color.white;
    }
}