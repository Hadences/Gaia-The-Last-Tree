using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private float Health = 10.0f;
    [SerializeField] private EnemyScriptableObject EnemyData;
    [SerializeField] private GameObject Exp;
    [SerializeField] private int maxExp = 5;

    [Header("Animations")]
    public Animator animator;
    private string currentState;
    const string PLAYER_IDLE = "Idle";
    const string PLAYER_RUN = "Run";
    const string PLAYER_ATTACK = "Attack";

    [Header("Particles")]
    [SerializeField] private GameObject HitParticle;

    [SerializeField] private int hitCD = 2;
    private int hitCount = 0;

    //hit Particle
    private GameObject HitP;

    public enum EnemyStates
    {
        IDLE,
        MOVING,
        ATTACK
    }

    public EnemyStates enemyState = EnemyStates.IDLE;

    // Start is called before the first frame update
    void Start()
    {
        hitCount = hitCD - 1;
        Health = EnemyData.Health;
        HitP = Instantiate(HitParticle, transform.position, Quaternion.identity,transform);
        HitP.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Health);
        updateAnimations();
    }

    public void updateAnimations()
    {
        if (enemyState == EnemyStates.IDLE)
        {
            ChangeAnimationState(PLAYER_IDLE);
        }
        else if (enemyState == EnemyStates.MOVING)
        {
            ChangeAnimationState(PLAYER_RUN);
        }else if(enemyState == EnemyStates.ATTACK)
        {
            ChangeAnimationState(PLAYER_ATTACK);
        }
    }

    public void damage(float hitPoint)
    {
        if(hitPoint > 1)
        {
            SoundManager.Instance.EXPLOSION.Play();
        }

        //player deals dmg
        GameManagerScript.Instance.PlayerDealDamageEvent.Invoke();

        bool critical = Random.Range(0, 100) < 10; //30% to crit
        hitPoint = (critical) ? 2 * hitPoint : hitPoint;

        DamagePopUp.Create(transform.position, (int)hitPoint, critical);
        if (hitCount >= hitCD) {
            HitP.SetActive(true);
            hitCount = 0;
        }

        if (Health - hitPoint > 0)
        {
            Health -= hitPoint;
        }
        else
        {
            //dead
            dropExp();
            Instantiate(GameManagerScript.Instance.DeathParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);
            SoundManager.Instance.HIT.Play();

        }
        hitCount++;
    }

    public void dropExp()
    {
        int drops = Random.Range(1, maxExp);

        for(int i = 0; i < drops; i++) {
            Vector2 randomRelativePos = new Vector2(transform.position.x + Random.Range(-1.0f, 1.0f), transform.position.y + Random.Range(-1.0f, 1.0f));
            Instantiate(Exp, randomRelativePos, Quaternion.identity);
        }
    }

    void ChangeAnimationState(string newState)
    {
        if (animator == null) return;
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

    /*GETTER FUNCTIONS*/
    public float getHealth() { return Health; }
}
