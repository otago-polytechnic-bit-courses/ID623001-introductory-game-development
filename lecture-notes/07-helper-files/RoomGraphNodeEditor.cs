using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class RoomNodeGraphEditor : EditorWindow
{
    private GUIStyle roomNodeStyle;
    private GUIStyle roomNodeSelectedStyle;
    
    private static RoomNodeGraph currentRoomNodeGraph;

    private Vector2 graphOffset;
    private Vector2 graphDrag;

    private RoomNode currentRoomNode = null;
    private RoomNodeTypeList roomNodeTypeList;

    private const float nodeWidth = 160f;
    private const float nodeHeight = 75f;
    private const int nodePadding = 25;
    private const int nodeBorder = 12;

    private const float connectingLineWidth = 3f;
    private const float connectingLineArrowSize = 6f;

    private const float gridLarge = 100f;
    private const float gridSmall = 25f;

    [MenuItem("Room Node Graph Editor", menuItem = "Window/Dungeon Editor/Room Node Graph Editor")]
    private static void OpenWindow()
    {
        GetWindow<RoomNodeGraphEditor>("Room Node Graph Editor");
    }

    // This method is called when the editor window is opened
    private void OnEnable()
    {
        Selection.selectionChanged += InspectorSelectionChanged;

        // Load the styles for the room nodes
        roomNodeStyle = new GUIStyle();
        roomNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
        roomNodeStyle.normal.textColor = Color.white;
        roomNodeStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);
        roomNodeStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);

        // Load the styles for the selected room nodes
        roomNodeSelectedStyle = new GUIStyle();
        roomNodeSelectedStyle.normal.background = EditorGUIUtility.Load("node1 on") as Texture2D;
        roomNodeSelectedStyle.normal.textColor = Color.white;
        roomNodeSelectedStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);
        roomNodeSelectedStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);

        // Load the room node type list from the GameResources
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    // This method is called when the editor window is closed
    private void OnDisable()
    {
        Selection.selectionChanged -= InspectorSelectionChanged;
    }

    // This method is called when the user double-clicks on a RoomNodeGraph asset in the Project window
    [OnOpenAsset(0)]
    public static bool OnDoubleClickAsset(int instanceID, int line)
    {
        RoomNodeGraph roomNodeGraph = EditorUtility.InstanceIDToObject(instanceID) as RoomNodeGraph;

        if (roomNodeGraph != null)
        {
            OpenWindow();

            currentRoomNodeGraph = roomNodeGraph;

            return true;
        }
        return false;
    }

    // This method is called to draw the GUI for the editor window
    private void OnGUI()
    {
        if (currentRoomNodeGraph != null)
        {
            DrawBackgroundGrid(gridSmall, 0.2f, Color.gray);
            DrawBackgroundGrid(gridLarge, 0.3f, Color.gray);

            DrawDraggedLine();

            ProcessEvents(Event.current);

            DrawRoomConnections();

            DrawRoomNodes();
        }

        if (GUI.changed) Repaint();    
    }

    // This method is called to draw the background grid
    private void DrawBackgroundGrid(float gridSize, float gridOpacity, Color gridColor)
    {
        // Get the current position of the editor window
        int verticalLineCount = Mathf.CeilToInt((position.width + gridSize) / gridSize);
        int horizontalLineCount = Mathf.CeilToInt((position.height + gridSize) / gridSize);

        // Draw the grid lines
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        // Calculate the offset for the grid lines
        graphOffset += graphDrag * 0.5f;

        Vector3 gridOffset = new Vector3(graphOffset.x % gridSize, graphOffset.y % gridSize, 0);

        // Draw the vertical and horizontal lines
        for (int i = 0; i < verticalLineCount; i++)
        {
            Handles.DrawLine(new Vector3(gridSize * i, -gridSize, 0) + gridOffset, new Vector3(gridSize * i, position.height + gridSize, 0f) + gridOffset);
        }

        for (int j = 0; j < horizontalLineCount; j++)
        {
            Handles.DrawLine(new Vector3(-gridSize, gridSize * j, 0) + gridOffset, new Vector3(position.width + gridSize, gridSize * j, 0f) + gridOffset);
        }

        // Reset the color of the handles
        Handles.color = Color.white;
    }

    // This method is called to draw the dragged line between room nodes
    private void DrawDraggedLine()
    {
        if (currentRoomNodeGraph.linePosition != Vector2.zero)
        {
            Handles.DrawBezier(currentRoomNodeGraph.roomNodeToDrawLineFrom.rect.center, currentRoomNodeGraph.linePosition, currentRoomNodeGraph.roomNodeToDrawLineFrom.rect.center, currentRoomNodeGraph.linePosition, Color.white, null, connectingLineWidth);
        }
    }

    // This method is called to process the events in the editor window
    private void ProcessEvents(Event currentEvent)
    {
        graphDrag = Vector2.zero;

        // Check if the mouse is over a room node
        if (currentRoomNode == null || currentRoomNode.isLeftClickDragging == false)
        {
            currentRoomNode = IsMouseOverRoomNode(currentEvent);
        }

        // Check if the mouse is over a room node and process the events accordingly
        if (currentRoomNode == null || currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
        {
            ProcessRoomNodeGraphEvents(currentEvent);
        }

        // Process the events for the current room node
        else
        {
            currentRoomNode.ProcessEvents(currentEvent);
        }
    }

    private RoomNode IsMouseOverRoomNode(Event currentEvent)
    {
        for (int i = currentRoomNodeGraph.roomNodeList.Count - 1; i >= 0; i--)
        {
            if (currentRoomNodeGraph.roomNodeList[i].rect.Contains(currentEvent.mousePosition))
            {
                return currentRoomNodeGraph.roomNodeList[i];
            }
        }

        return null;
    }
    
    private void ProcessRoomNodeGraphEvents(Event currentEvent)
    {
        switch (currentEvent.type)
        {
            case EventType.MouseDown:
                ProcessMouseDownEvent(currentEvent);
                break;

            case EventType.MouseUp:
                ProcessMouseUpEvent(currentEvent);
                break;

            case EventType.MouseDrag:
                ProcessMouseDragEvent(currentEvent);
                break;

            default:
                break;
        }
    }

    private void ProcessMouseDownEvent(Event currentEvent)
    {
        if (currentEvent.button == 1)
        {
            ShowContextMenu(currentEvent.mousePosition);
        }
        else if (currentEvent.button == 0)
        {
            ClearLineDrag();
            ClearAllSelectedRoomNodes();
        }
    }

    // This method is called to show the context menu when the user right-clicks
    private void ShowContextMenu(Vector2 mousePosition)
    {
        GenericMenu menu = new GenericMenu();

        menu.AddItem(new GUIContent("Create Room Node"), false, CreateRoomNode, mousePosition);
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("Select All Room Nodes"), false, SelectAllRoomNodes);
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("Delete Selected Room Node Links"), false, DeleteSelectedRoomNodeLinks);
        menu.AddItem(new GUIContent("Delete Selected Room Nodes"), false, DeleteSelectedRoomNodes);

        menu.ShowAsContext();
    }

    // This method is called to create a new room node
    private void CreateRoomNode(object mousePositionObject)
    {
        // Check if the current room node graph is null
        if (currentRoomNodeGraph.roomNodeList.Count == 0)
        {
            // Create the first room node as an entrance
            CreateRoomNode(new Vector2(200f, 200f), roomNodeTypeList.list.Find(x => x.isEntrance));
        }

        // Create a new room node at the mouse position
        CreateRoomNode(mousePositionObject, roomNodeTypeList.list.Find(x => x.isNone));
    }

    private void CreateRoomNode(object mousePositionObject, RoomNodeType roomNodeType)
    {
        Vector2 mousePosition = (Vector2)mousePositionObject;

        RoomNode roomNode = ScriptableObject.CreateInstance<RoomNode>();

        currentRoomNodeGraph.roomNodeList.Add(roomNode);

        roomNode.Initialise(new Rect(mousePosition, new Vector2(nodeWidth, nodeHeight)), currentRoomNodeGraph, roomNodeType);

        AssetDatabase.AddObjectToAsset(roomNode, currentRoomNodeGraph);

        AssetDatabase.SaveAssets();

        currentRoomNodeGraph.OnValidate();
    }

    // This method is called to delete the selected room nodes
    private void DeleteSelectedRoomNodes()
    {
        Queue<RoomNode> roomNodeDeletionQueue = new Queue<RoomNode>();

        foreach (RoomNode roomNode in currentRoomNodeGraph.roomNodeList)
        {
            if (roomNode.isSelected && !roomNode.roomNodeType.isEntrance)
            {
                roomNodeDeletionQueue.Enqueue(roomNode);

                foreach (string childRoomNodeID in roomNode.childRoomNodeIDList)
                {
                    RoomNode childRoomNode = currentRoomNodeGraph.GetRoomNode(childRoomNodeID);

                    if (childRoomNode != null)
                    {
                        childRoomNode.RemoveParentRoomNodeIDFromRoomNode(roomNode.id);
                    }
                }

                foreach (string parentRoomNodeID in roomNode.parentRoomNodeIDList)
                {
                    RoomNode parentRoomNode = currentRoomNodeGraph.GetRoomNode(parentRoomNodeID);

                    if (parentRoomNode != null)
                    {
                        parentRoomNode.RemoveChildRoomNodeIDFromRoomNode(roomNode.id);
                    }
                }
            }
        }

        while (roomNodeDeletionQueue.Count > 0)
        {
            RoomNode roomNodeToDelete = roomNodeDeletionQueue.Dequeue();

            currentRoomNodeGraph.roomNodeDictionary.Remove(roomNodeToDelete.id);

            currentRoomNodeGraph.roomNodeList.Remove(roomNodeToDelete);

            DestroyImmediate(roomNodeToDelete, true);

            AssetDatabase.SaveAssets();
        }
    }

    // This method is called to delete the selected room node links
    private void DeleteSelectedRoomNodeLinks()
    {
        foreach (RoomNode roomNode in currentRoomNodeGraph.roomNodeList)
        {
            if (roomNode.isSelected && roomNode.childRoomNodeIDList.Count > 0)
            {
                for (int i = roomNode.childRoomNodeIDList.Count - 1; i >= 0; i--)
                {
                    RoomNode childRoomNode = currentRoomNodeGraph.GetRoomNode(roomNode.childRoomNodeIDList[i]);

                    if (childRoomNode != null && childRoomNode.isSelected)
                    {
                        roomNode.RemoveChildRoomNodeIDFromRoomNode(childRoomNode.id);

                        childRoomNode.RemoveParentRoomNodeIDFromRoomNode(roomNode.id);
                    }
                }
            }
        }

        ClearAllSelectedRoomNodes();
    }

    // This method is called to clear all selected room nodes
    private void ClearAllSelectedRoomNodes()
    {
        foreach (RoomNode roomNode in currentRoomNodeGraph.roomNodeList)
        {
            if (roomNode.isSelected)
            {
                roomNode.isSelected = false;

                GUI.changed = true;
            }
        }
    }

    // This method is called to select all room nodes
    private void SelectAllRoomNodes()
    {
        foreach (RoomNode roomNode in currentRoomNodeGraph.roomNodeList)
        {
            roomNode.isSelected = true;
        }
        GUI.changed = true;
    }

    private void ProcessMouseUpEvent(Event currentEvent)
    {
        if (currentEvent.button == 1 && currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
        {
            RoomNode roomNode = IsMouseOverRoomNode(currentEvent);

            if (roomNode != null)
            {
                if (currentRoomNodeGraph.roomNodeToDrawLineFrom.AddChildRoomNodeIDToRoomNode(roomNode.id))
                {
                    roomNode.AddParentRoomNodeIDToRoomNode(currentRoomNodeGraph.roomNodeToDrawLineFrom.id);
                }
            }

            ClearLineDrag();
        }
    }

    private void ProcessMouseDragEvent(Event currentEvent)
    {
        if (currentEvent.button == 1)
        {
            ProcessRightMouseDragEvent(currentEvent);
        }
        else if (currentEvent.button == 0)
        {
            ProcessLeftMouseDragEvent(currentEvent.delta);
        }
    }

    private void ProcessRightMouseDragEvent(Event currentEvent)
    {
        if (currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
        {
            DragConnectingLine(currentEvent.delta);
            GUI.changed = true;
        }
    }

    private void ProcessLeftMouseDragEvent(Vector2 dragDelta)
    {
        graphDrag = dragDelta;

        for (int i = 0; i < currentRoomNodeGraph.roomNodeList.Count; i++)
        {
            currentRoomNodeGraph.roomNodeList[i].DragNode(dragDelta);
        }

        GUI.changed = true;
    }

    public void DragConnectingLine(Vector2 delta)
    {
        currentRoomNodeGraph.linePosition += delta;
    }

    private void ClearLineDrag()
    {
        currentRoomNodeGraph.roomNodeToDrawLineFrom = null;
        currentRoomNodeGraph.linePosition = Vector2.zero;
        GUI.changed = true;
    }

    private void DrawRoomConnections()
    {
        foreach (RoomNode roomNode in currentRoomNodeGraph.roomNodeList)
        {
            if (roomNode.childRoomNodeIDList.Count > 0)
            {
                foreach (string childRoomNodeID in roomNode.childRoomNodeIDList)
                {
                    if (currentRoomNodeGraph.roomNodeDictionary.ContainsKey(childRoomNodeID))
                    {
                        DrawConnectionLine(roomNode, currentRoomNodeGraph.roomNodeDictionary[childRoomNodeID]);

                        GUI.changed = true;
                    }
                }
            }
        }
    }

    private void DrawConnectionLine(RoomNode parentRoomNode, RoomNode childRoomNode)
    {
        Vector2 startPosition = parentRoomNode.rect.center;
        Vector2 endPosition = childRoomNode.rect.center;

        Vector2 midPosition = (endPosition + startPosition) / 2f;

        Vector2 direction = endPosition - startPosition;

        Vector2 arrowTailPoint1 = midPosition - new Vector2(-direction.y, direction.x).normalized * connectingLineArrowSize;
        Vector2 arrowTailPoint2 = midPosition + new Vector2(-direction.y, direction.x).normalized * connectingLineArrowSize;

        Vector2 arrowHeadPoint = midPosition + direction.normalized * connectingLineArrowSize;

        Handles.DrawBezier(arrowHeadPoint, arrowTailPoint1, arrowHeadPoint, arrowTailPoint1, Color.white, null, connectingLineWidth);
        Handles.DrawBezier(arrowHeadPoint, arrowTailPoint2, arrowHeadPoint, arrowTailPoint2, Color.white, null, connectingLineWidth);

        Handles.DrawBezier(startPosition, endPosition, startPosition, endPosition, Color.white, null, connectingLineWidth);

        GUI.changed = true;
    }

    private void DrawRoomNodes()
    {
        foreach (RoomNode roomNode in currentRoomNodeGraph.roomNodeList)
        {
            if (roomNode.isSelected)
            {
                roomNode.Draw(roomNodeSelectedStyle);
            }
            else
            {
                roomNode.Draw(roomNodeStyle);
            }
        }

        GUI.changed = true;
    }

    private void InspectorSelectionChanged()
    {
        RoomNodeGraph roomNodeGraph = Selection.activeObject as RoomNodeGraph;

        if (roomNodeGraph != null)
        {
            currentRoomNodeGraph = roomNodeGraph;
            GUI.changed = true;
        }
    }
}