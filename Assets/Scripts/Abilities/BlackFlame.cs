using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackFlame : MonoBehaviour
{
    [SerializeField] LayerMask TargetLayer;
    [SerializeField] private GameObject BF;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, 4, TargetLayer);
        if (targets.Length > 0)
        {
            foreach (Collider2D tar in targets)
            {

                if(tar.gameObject.GetComponentInChildren<BlackFlameLogic>() == null)
                {
                    Instantiate(BF, tar.gameObject.transform.position, Quaternion.identity, tar.gameObject.transform);
                }
               
            }
        }
    }
}
