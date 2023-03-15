using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    public float Health = 5.0f;
    public float movementSpeed = 1.0f;

    public float attackDamage = 1.0f;
    public float attackRange = 0.5f;
    public float attackCooldown = 1.0f;
    public float sightRange = 3.0f;
}
