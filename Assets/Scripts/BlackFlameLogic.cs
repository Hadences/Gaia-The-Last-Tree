using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BlackFlameLogic : MonoBehaviour
{
    [SerializeField] private LayerMask TargetLayer;
    [SerializeField] private GameObject BlackFlame;
    private EnemyManager EM;
    // Start is called before the first frame update
    void Start()
    {
        EM = gameObject.GetComponentInParent<EnemyManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if(EM != null)
        {
            EM.damage(1);
        }

        //spread to a different entity
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, 3, TargetLayer);
        if (targets.Length > 0)
        {
            foreach (Collider2D tar in targets)
            {
                if (tar.gameObject.GetComponentInChildren<BlackFlameLogic>() == null)
                {
                    Instantiate(BlackFlame, tar.gameObject.transform.position, Quaternion.identity, tar.gameObject.transform);
                }
            }
        }
    }
}
