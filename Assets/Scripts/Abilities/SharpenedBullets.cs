using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpenedBullets : MonoBehaviour
{
    private float originalDmg;
    // Start is called before the first frame update
    void OnEnable()
    {
        originalDmg = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().getProjectile().GetComponent<Projectile>().projectileData.attackDamage;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().getProjectile().GetComponent<Projectile>().projectileData.attackDamage = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().getProjectile().GetComponent<Projectile>().projectileData.attackDamage + 2;
        
    }

    private void OnDisable()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().getProjectile().GetComponent<Projectile>().projectileData.attackDamage = originalDmg;

    }


}
