using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeGraph", menuName = "Scriptable Objects/Dungeon/Room Node Graph")]
public class RoomNodeGraph : ScriptableObject
{
    [HideInInspector] public RoomNodeTypeList roomNodeTypeList;
    [HideInInspector] public List<RoomNode> roomNodeList = new List<RoomNode>();
    [HideInInspector] public Dictionary<string, RoomNode> roomNodeDictionary = new Dictionary<string, RoomNode>();

    private void Awake()
    {
        LoadRoomNodeDictionary();
    }

    // This method is called when the scriptable object is created
    private void LoadRoomNodeDictionary()
    {
        roomNodeDictionary.Clear();

        foreach (RoomNode node in roomNodeList)
        {
            roomNodeDictionary[node.id] = node;
        }
    }

    // This method is called to create a new room node
    public RoomNode GetRoomNode(RoomNodeType roomNodeType)
    {
        foreach (RoomNode node in roomNodeList)
        {
            if (node.roomNodeType == roomNodeType)
            {
                return node;
            }
        }
        return null;
    }

    public RoomNode GetRoomNode(string roomNodeID)
    {
        if (roomNodeDictionary.TryGetValue(roomNodeID, out RoomNode roomNode))
        {
            return roomNode;
        }
        return null;
    }

    public IEnumerable<RoomNode> GetChildRoomNodes(RoomNode parentRoomNode)
    {
        foreach (string childNodeID in parentRoomNode.childRoomNodeIDList)
        {
            yield return GetRoomNode(childNodeID);
        }
    }

    #region Editor Code

#if UNITY_EDITOR

    [HideInInspector] public RoomNode roomNodeToDrawLineFrom = null;
    [HideInInspector] public Vector2 linePosition;

    public void OnValidate()
    {
        LoadRoomNodeDictionary();
    }

    public void SetNodeToDrawConnectionLineFrom(RoomNode node, Vector2 position)
    {
        roomNodeToDrawLineFrom = node;
        linePosition = position;
    }

#endif

    #endregion Editor Code
}