using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField] private float speed = 5f;
    [SerializeField] private InputActionAsset inputActions;

    public Rigidbody2D rb { get; private set; }
    private Vector2 moveInput;
    private InputAction moveAction;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        moveAction = inputActions.FindActionMap("Player").FindAction("Move");
    }

    private void OnEnable()
    {
        moveAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }

    private void FixedUpdate()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * moveInput);
    }

    public void TakeDamage(float amount)
    {
        // hook up to your health system here
        Debug.Log($"Player took {amount} damage");
    }
}