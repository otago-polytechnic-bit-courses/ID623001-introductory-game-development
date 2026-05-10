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

    void FixedUpdate()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(target.position.x, target.position.y, transform.position.z),
                speed * Time.fixedDeltaTime
            );
        }
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }
}