using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeTypeList", menuName = "Scriptable Objects/Dungeon/Room Node Type List")]
public class RoomNodeTypeList : ScriptableObject
{
    public List<RoomNodeType> list;
}