using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomPrefabs
{
    public GameObject roomDown, roomLeft, roomLeftDown, roomLeftRight,
        roomLeftRightDown, roomRight, roomRightDown, roomUp, roomUpDown,
        roomUpLeft, roomUpLeftDown, roomUpLeftRight, roomUpLeftRightDown,
        roomUpRight, roomUpRightDown;
}

[RequireComponent(typeof(Transform))]
public class LevelGenerator : MonoBehaviour
{
    private enum Direction { Up, Down, Left, Right }

    [SerializeField] private RoomPrefabs roomPrefabs;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private int distanceToEnd;
    [SerializeField] private Transform generationPoint;
    [SerializeField] private Direction direction;
    [SerializeField] private float xOffset = 18f;
    [SerializeField] private float yOffset = 10f;
    [SerializeField] private LayerMask roomLayerMask;

    private Vector3 startRoomPosition;
    private Vector3 endRoomPosition;
    private List<Vector3> roomPositions = new List<Vector3>();
    private List<GameObject> markers = new List<GameObject>();

    private Transform roomParent;

    void Start()
    {
        roomParent = new GameObject("Rooms").transform;

        startRoomPosition = generationPoint.position;
        PlaceMarker(startRoomPosition);

        direction = (Direction)Random.Range(0, 4);
        MoveGenerationPoint();

        for (int i = 0; i < distanceToEnd; i++)
        {
            Vector3 pos = generationPoint.position;
            PlaceMarker(pos);

            if (i == distanceToEnd - 1)
                endRoomPosition = pos;
            else
                roomPositions.Add(pos);

            Vector3 lastValidPosition = generationPoint.position;
            direction = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();

            int safetyLimit = 100;
            while (Physics2D.OverlapCircle(generationPoint.position, 0.2f, roomLayerMask))
            {
                generationPoint.position = lastValidPosition;
                direction = (Direction)Random.Range(0, 4);
                MoveGenerationPoint();

                if (--safetyLimit <= 0)
                {
                    Debug.LogWarning("LevelGenerator: could not find a free position, breaking.");
                    break;
                }
            }
        }

        CreateRoomOutline(startRoomPosition);
        foreach (Vector3 pos in roomPositions)
            CreateRoomOutline(pos);
        CreateRoomOutline(endRoomPosition);

        if (playerPrefab != null)
        {
            GameObject player = Instantiate(playerPrefab, startRoomPosition, Quaternion.identity);
            CameraController.Instance.ChangeTarget(player.transform);
        }

        foreach (GameObject marker in markers)
            Destroy(marker);

        GridManager.Instance.BakeWalls();
    }

    private void PlaceMarker(Vector3 position)
    {
        GameObject marker = new GameObject("RoomMarker");
        marker.transform.position = position;
        marker.layer = GetLayerFromMask(roomLayerMask);

        CircleCollider2D col = marker.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = 0.1f;

        markers.Add(marker);
    }

    private int GetLayerFromMask(LayerMask mask)
    {
        int value = mask.value;
        int layer = 0;
        while (value > 1)
        {
            value >>= 1;
            layer++;
        }
        return layer;
    }

    private void MoveGenerationPoint()
    {
        switch (direction)
        {
            case Direction.Up:    generationPoint.position += new Vector3(0,  yOffset, 0); break;
            case Direction.Down:  generationPoint.position += new Vector3(0, -yOffset, 0); break;
            case Direction.Left:  generationPoint.position += new Vector3(-xOffset, 0, 0); break;
            case Direction.Right: generationPoint.position += new Vector3( xOffset, 0, 0); break;
        }
    }

    public void CreateRoomOutline(Vector3 roomPosition)
    {
        bool isRoomAbove = Physics2D.OverlapCircle(roomPosition + new Vector3(0,  yOffset, 0), 0.2f, roomLayerMask);
        bool isRoomBelow = Physics2D.OverlapCircle(roomPosition + new Vector3(0, -yOffset, 0), 0.2f, roomLayerMask);
        bool isRoomLeft  = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0, 0), 0.2f, roomLayerMask);
        bool isRoomRight = Physics2D.OverlapCircle(roomPosition + new Vector3( xOffset, 0, 0), 0.2f, roomLayerMask);

        GameObject prefabToSpawn = GetRoomPrefab(isRoomAbove, isRoomBelow, isRoomLeft, isRoomRight);

        if (prefabToSpawn != null)
            Instantiate(prefabToSpawn, roomPosition, Quaternion.identity, roomParent);
        else
            Debug.LogWarning($"No prefab found at {roomPosition} — above:{isRoomAbove} below:{isRoomBelow} left:{isRoomLeft} right:{isRoomRight}");
    }

    private GameObject GetRoomPrefab(bool up, bool down, bool left, bool right)
    {
        if ( up && !down && !left && !right) return roomPrefabs.roomUp;
        if (!up &&  down && !left && !right) return roomPrefabs.roomDown;
        if (!up && !down &&  left && !right) return roomPrefabs.roomLeft;
        if (!up && !down && !left &&  right) return roomPrefabs.roomRight;

        if ( up &&  down && !left && !right) return roomPrefabs.roomUpDown;
        if ( up && !down &&  left && !right) return roomPrefabs.roomUpLeft;
        if ( up && !down && !left &&  right) return roomPrefabs.roomUpRight;
        if (!up &&  down &&  left && !right) return roomPrefabs.roomLeftDown;
        if (!up &&  down && !left &&  right) return roomPrefabs.roomRightDown;
        if (!up && !down &&  left &&  right) return roomPrefabs.roomLeftRight;

        if ( up &&  down &&  left && !right) return roomPrefabs.roomUpLeftDown;
        if ( up &&  down && !left &&  right) return roomPrefabs.roomUpRightDown;
        if ( up && !down &&  left &&  right) return roomPrefabs.roomUpLeftRight;
        if (!up &&  down &&  left &&  right) return roomPrefabs.roomLeftRightDown;

        if ( up &&  down &&  left &&  right) return roomPrefabs.roomUpLeftRightDown;

        return null;
    }
}