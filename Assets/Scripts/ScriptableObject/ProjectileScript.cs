using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileScriptableObject", menuName = "ScriptableObjects/Projectile")]
public class ProjectileScript : ScriptableObject
{
    public float attackDamage = 1.0f;
    public float projectileForce = 10f;
    public float fireRateCooldown = 0.25f;
    public float lifeTime = 10f;

}
