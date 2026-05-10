using UnityEngine;

public enum PlayerType { Warrior, Rogue, Mage }

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Identity")]
    public Sprite sprite;
    public new string name;
    public PlayerType playerType;

    [Header("Base Stats")]
    public float health = 100f;
    public float damage = 10f;
    public float moveSpeed = 5f;
    public float defense = 0f;

    [Header("Crit")]
    [Range(0f, 1f)] public float critChance = 0f;
    public float critMultiplier = 1.5f;
}