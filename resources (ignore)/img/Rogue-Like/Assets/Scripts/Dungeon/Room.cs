using UnityEngine;

public class Room : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && PlayerController.Instance != null)
        {
            CameraController.Instance.ChangeTarget(PlayerController.Instance.transform);
        }
    }
}