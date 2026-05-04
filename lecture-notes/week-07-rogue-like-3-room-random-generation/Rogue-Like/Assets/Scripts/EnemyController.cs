using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 1f;

    private float attackTimer;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (PlayerController.Instance == null) return;

        attackTimer -= Time.deltaTime;

        Vector2 toPlayer = (Vector2)(PlayerController.Instance.transform.position - transform.position);
        spriteRenderer.flipX = toPlayer.x < 0f;

        if (toPlayer.sqrMagnitude <= attackRange * attackRange && attackTimer <= 0f)
        {
            Attack();
            attackTimer = attackCooldown;
        }
    }

    private void Attack()
    {
        PlayerController.Instance.TakeDamage(damage);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}