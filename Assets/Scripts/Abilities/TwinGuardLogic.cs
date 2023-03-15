using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinGuardLogic : MonoBehaviour
{
    public LayerMask enemyLayer;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if ((enemyLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            Destroy(collision.gameObject);

        }
    }
}
