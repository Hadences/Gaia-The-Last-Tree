using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidBullets : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().getProjectile().GetComponent<Projectile>().projectileData.fireRateCooldown = 0.15f;
    }

    private void OnDisable()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().getProjectile().GetComponent<Projectile>().projectileData.fireRateCooldown = 0.2f;
    }

}
