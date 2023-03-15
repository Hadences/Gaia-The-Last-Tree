using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercedBullets : MonoBehaviour
{
    private void OnEnable()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().setProjectilePierced(true);
    }

    private void OnDisable()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().setProjectilePierced(false);
    }




}
