using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Scriptable Objects/ProjectileData")]
public class ProjectileData : ScriptableObject
{
    [Header("Identity")]
    public new string name;
    public GameObject projectilePrefab;

    [Header("Stats")]
    public float damage = 10f;
    public float speed = 10f;
    public float range = 5f;
    public float cooldown = 0.3f;
    public int poolSize = 10;
}