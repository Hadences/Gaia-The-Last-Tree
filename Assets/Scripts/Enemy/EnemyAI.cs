using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyAI : MonoBehaviour
{   
    public enum aiType
    {
        MELEE,
        RANGED,
        OBJECTIVE
    }

    private EnemyManager EM;
    public aiType AIType = aiType.MELEE;
    public LayerMask playerMask;

    public bool playerInAttackRange;
    public bool playerInSightRange;
    private float attackRange, sightRange;

    //Direction
    private Vector3 direction;
    private Transform aimTransform;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    public bool canPatrol = true;
    public float patrolCooldown = 0.5f;

    private bool attacked = false;
    private bool reset = false;
    public EnemyScriptableObject EnemyData;
    public GameObject ProjectilePrefab;


    private Rigidbody2D rb;

    private void Start()
    {
        EM = gameObject.GetComponent<EnemyManager>();
        rb = GetComponent<Rigidbody2D>();
        attackRange = EnemyData.attackRange;
        sightRange = EnemyData.sightRange;
        aimTransform = transform.Find("Aim");

    }

    private void Update()
    {
        updateDirection();

        Collider2D[] playersInAttackRange = Physics2D.OverlapCircleAll(transform.position, attackRange, playerMask);
        Collider2D[] playersInSightRange = Physics2D.OverlapCircleAll(transform.position, sightRange, playerMask);

        playerInAttackRange = (playersInAttackRange.Length > 0);
        playerInSightRange = (playersInSightRange.Length > 0);
        
        //Debug.Log(playerInAttackRange + "" + playerInSightRange + "");
        if (!playerInAttackRange && !playerInSightRange) mainObjective();
        if (playerInSightRange && !playerInAttackRange) chasePlayer();
        if (playerInAttackRange && playerInSightRange && !attacked) StartCoroutine(attackPlayer());

    }

    void updateDirection()
    {
        if (attacked) return;
        if (direction == null) return;
        Vector3 aimDirection = direction;

        Vector3 aimPosition = new Vector2(aimDirection.x, aimDirection.y).normalized;
        aimTransform.position = transform.position + aimPosition;

        if (aimPosition.x >= 0)
        {
            //right
            //gameObject.transform.localScale = new Vector3(1, 1, 1);
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (aimPosition.x < 0)
        {
            //left
            //gameObject.transform.localScale = new Vector3(-1, 1, 1);
            gameObject.GetComponent<SpriteRenderer>().flipX = true;


        }
    }

    public virtual void mainObjective()
    {
        Patrol();
    }

    void Patrol()
    {
        if (!walkPointSet && canPatrol) SearchWalkPoint();

        if (walkPointSet)
        {
            direction = walkPoint - transform.position;
            transform.position = Vector2.MoveTowards(transform.position, walkPoint, EnemyData.movementSpeed * Time.deltaTime);
            EM.enemyState = EnemyManager.EnemyStates.MOVING;
            if ((transform.position - walkPoint).magnitude < 0.2f)
            {
                walkPointSet = false;
                Invoke("resetPatrol", patrolCooldown);
            }
        }
           
    }

    void resetPatrol()
    {
        canPatrol = true;
    }

    void SearchWalkPoint()
    {
        canPatrol = false;
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomY = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y + randomY);

        walkPointSet = true;
    }

    void chasePlayer()
    {
        if (!attacked)
        {
            EM.enemyState = EnemyManager.EnemyStates.MOVING;
            transform.position = Vector2.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position, EnemyData.movementSpeed * Time.deltaTime);
            direction = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;
        }

    }

    IEnumerator attackPlayer()
    {
        EM.enemyState = EnemyManager.EnemyStates.ATTACK;
        yield return new WaitForSeconds(0.5f);
    

        if (!attacked)
        {
            if(AIType == aiType.MELEE)
            {

                //attack logic
                Collider2D[] targets = Physics2D.OverlapCircleAll(aimTransform.position, 1, playerMask);
                if (targets.Length > 0)
                {
                    foreach (Collider2D tar in targets)
                    {
                        GameObject player = tar.gameObject;
                        PlayerManager playerManager = player.GetComponent<PlayerManager>();
                        playerManager.damage(EnemyData.attackDamage);
                    }
                }


            }else if(AIType == aiType.RANGED)
            {

                if (ProjectilePrefab != null)
                {
                    direction = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;

                    //ranged attack logic
                    GameObject projectile = Instantiate(ProjectilePrefab, aimTransform.position, aimTransform.rotation);
                    Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                    Projectile projectileScript = ProjectilePrefab.GetComponent<Projectile>();

                    rb.AddForce((aimTransform.position - transform.position).normalized * projectileScript.projectileData.projectileForce, ForceMode2D.Impulse);
                }
            }


            attacked = true;
        }

        if (attacked == true && reset == false)
        {
            Invoke("resetAttack", EnemyData.attackCooldown);
            reset = true;
        }
    }

    void resetAttack()
    {
        EM.enemyState = EnemyManager.EnemyStates.IDLE;

        attacked = false;
        reset = false;
    }

    
}
