using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TroopStats", menuName = "ScriptableObjects/NewTroopStats")]
public class TroopStats : ScriptableObject
{
    public LayerMask searchMask;

    //base stats
    [SerializeField] float baseSearchRange;
    [SerializeField] float baseSearchInterval;
    [SerializeField] float baseDeAggroRange;

    
    [SerializeField] float baseHealth;
    [SerializeField] int baseAtkDamage;
    [SerializeField] float baseAtkSpeed;
    [SerializeField] float baseAtkRange;
    [SerializeField] float baseMoveSpeed;

    //runtime stats
    float searchRange;
    float searchInterval;
    float deAggroRange;

    float health;
    int atkDamage;
    float atkSpeed;
    float atkRange;
    float moveSpeed;

    //referenced stats
    public float SearchRange { get { return searchRange; } }
    public float SearchInterval { get { return searchInterval; } }
    public float DeAggroRange { get { return deAggroRange; } }

    public float Health { get { return health; } }
    public int AtkDamage { get { return atkDamage; } }
    public float AtkSpeed { get { return atkSpeed; } }
    public float AtkRange { get { return atkRange; } }
    public float MoveSpeed { get { return moveSpeed; } }

    private void OnEnable()
    {
        searchRange = baseSearchRange;
        searchInterval = baseSearchInterval;
        deAggroRange = baseDeAggroRange;

        health = baseHealth;
        atkDamage = baseAtkDamage;
        atkSpeed = baseAtkSpeed;
        atkRange = baseAtkRange;
        moveSpeed = baseMoveSpeed;
    }
}
