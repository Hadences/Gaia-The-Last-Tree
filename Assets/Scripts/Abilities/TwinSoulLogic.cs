using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinSoulLogic : MonoBehaviour
{
    public LayerMask enemyLayer;
    public float damagePoint = 1f;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if ((enemyLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            //if its an enemy
            if (collision.gameObject.GetComponent<EnemyManager>() != null)
            {
                EnemyManager enemyManager = collision.gameObject.GetComponent<EnemyManager>();
                enemyManager.damage(damagePoint);
            }
            else if (collision.gameObject.GetComponent<PlayerManager>() != null)
            {
                PlayerManager playerManager = collision.gameObject.GetComponent<PlayerManager>();
                playerManager.damage(damagePoint);
            }
            
        }
    }
}
