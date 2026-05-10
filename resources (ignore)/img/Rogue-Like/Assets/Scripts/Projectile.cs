using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float damage;
    private float speed;
    private float lifetime;
    private Rigidbody2D rb;

    public void Init(float damage, float speed, float range)
    {
        this.damage = damage;
        this.speed = speed;
        lifetime = range / speed;
    }

    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;
        Invoke(nameof(ReturnToPool), lifetime);
    }

    void OnDisable()
    {
        CancelInvoke();
        
        if (rb != null) 
            rb.linearVelocity = Vector2.zero;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
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