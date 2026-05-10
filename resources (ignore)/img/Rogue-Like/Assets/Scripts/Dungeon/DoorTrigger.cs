using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private Vector3 endRoomPosition;
    private GameObject enemyPrefab;
    private int enemyCount;
    private Transform enemyParent;
    private bool hasTriggered = false;

    public void Initialize(Vector3 endPos, GameObject prefab, int count, Transform parent)
    {
        endRoomPosition = endPos;
        enemyPrefab = prefab;
        enemyCount = count;
        enemyParent = parent;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;

        hasTriggered = true;

        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-4f, 4f), Random.Range(-2f, 2f), 0);
            Instantiate(enemyPrefab, endRoomPosition + offset, Quaternion.identity, enemyParent);
        }

        gameObject.SetActive(false);
    }
}