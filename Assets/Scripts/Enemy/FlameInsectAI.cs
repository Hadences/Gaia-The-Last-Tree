using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameInsectAI : MonoBehaviour
{
    [SerializeField] private GameObject Tree;
    [SerializeField] private EnemyScriptableObject EnemyData;
    private GameManagerScript GM;
    private EnemyManager EM;

    // Start is called before the first frame update
    void Start()
    {
        EM = gameObject.GetComponent<EnemyManager>();
        EM.enemyState = EnemyManager.EnemyStates.MOVING;

        GM = GameManagerScript.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        EM.enemyState = EnemyManager.EnemyStates.MOVING;
        transform.position = Vector2.MoveTowards(transform.position, Tree.transform.position, EnemyData.movementSpeed * Time.deltaTime);
        Vector2 dist = transform.position - Tree.transform.position;
        if(dist.magnitude < 0.5f)
        {
            //Damage the Tree
            GM.damageTree((int) EnemyData.attackDamage);
            Destruct();
        }
        if((Tree.transform.position - transform.position).x >= 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    void Destruct()
    {
        //Particle Effect
        //TODO


        Destroy(gameObject);
    }
}
