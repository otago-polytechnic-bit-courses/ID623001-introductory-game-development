using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    public float CurrentHealth { get; private set; }

    [Header("Data")]
    [SerializeField] private PlayerData playerData;
    [SerializeField] private ProjectileData projectileData;

    [Header("Input")]
    [SerializeField] private InputActionAsset inputActions;

    private SpriteRenderer spriteRenderer;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection = Vector2.right;
    private bool isDead;

    private InputAction moveAction;
    private InputAction projectAction;

    private bool canAttack = true;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        Rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        InputActionMap map = inputActions.FindActionMap("Player");
        moveAction = map.FindAction("Move");
        projectAction = map.FindAction("Project");

        CurrentHealth = playerData.health;

        if (spriteRenderer != null)
            spriteRenderer.sprite = playerData.sprite;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        projectAction.Enable();
        projectAction.performed += OnProject;
    }

    private void OnDisable()
    {
        moveAction.Disable();
        projectAction.Disable();
        projectAction.performed -= OnProject;
    }

    private void Update()
    {
        if (isDead) return;

        moveInput = moveAction.ReadValue<Vector2>();

        if (moveInput != Vector2.zero)
            lastMoveDirection = moveInput.normalized;

        if (moveInput.x != 0)
            spriteRenderer.flipX = moveInput.x < 0;
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        Rb.MovePosition(Rb.position + playerData.moveSpeed * Time.fixedDeltaTime * moveInput);
    }

    private void OnProject(InputAction.CallbackContext ctx)
    {
        if (isDead || !canAttack || playerData.playerType != PlayerType.Mage) return;
        StartCoroutine(SpawnProjectile());
    }

    private IEnumerator SpawnProjectile()
    {
        canAttack = false;

        float angle = Mathf.Atan2(lastMoveDirection.y, lastMoveDirection.x) * Mathf.Rad2Deg;

        GameObject projectile = ObjectPool.Instance.Get();
        projectile.transform.position = transform.position;
        projectile.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        projectile.GetComponent<Projectile>().Init(
            projectileData.damage + playerData.damage,
            projectileData.speed,
            projectileData.range
        );

        projectile.SetActive(true);

        yield return new WaitForSeconds(projectileData.cooldown);
        canAttack = true;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        float mitigated = Mathf.Max(0f, amount - playerData.defense);
        CurrentHealth -= mitigated;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, playerData.health);

        StartCoroutine(FlashDamage());

        if (CurrentHealth <= 0f) Die();
    }

    private void Die()
    {
        isDead = true;
        spriteRenderer.color = Color.gray;
    }

    private IEnumerator FlashDamage()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
}