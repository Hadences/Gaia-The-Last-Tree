using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask enemyLayer;
    public ProjectileScript projectileData;
    private float lifeTime = 0;
    [SerializeField] private GameObject particle;
    public bool pierced = false;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(particle, transform.position, Quaternion.identity,transform);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.GetComponent<Rigidbody2D>().velocity.magnitude <= 0) Destroy(gameObject);

        lifeTime += Time.deltaTime;
        if(lifeTime > projectileData.lifeTime)
        {
            Destroy(gameObject);
        }
    }

    public void setPierced(bool p)
    {
        pierced = p;
    }

    private void FixedUpdate()
    {
    }



    public void OnTriggerEnter2D(Collider2D collision)
    {   
        if((enemyLayer.value & (1<< collision.gameObject.layer)) > 0)
        {
            //if its an enemy
            if (collision.gameObject.GetComponent<EnemyManager>() != null)
            {
                EnemyManager enemyManager = collision.gameObject.GetComponent<EnemyManager>();
                enemyManager.damage(projectileData.attackDamage);
            }else if(collision.gameObject.GetComponent<PlayerManager>() != null)
            {
                PlayerManager playerManager = collision.gameObject.GetComponent<PlayerManager>();
                playerManager.damage(projectileData.attackDamage);                
            }
            if(!pierced)
                Destroy(gameObject);
        }
    }
}
