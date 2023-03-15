using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float MaxHealth = 100.0f;
    [SerializeField] private float Health = 100.0f;

    [Header("Animations")]
    [SerializeField] private Animator animator;
    private string currentState;
    const string PLAYER_IDLE = "Idle";
    const string PLAYER_RUN = "Run";
    const string PLAYER_HURT = "Hurt";

    [Header("UI")]
    //[SerializeField] private TMP_Text HealthText;
    [SerializeField] private Slider HealthSlider;
    [SerializeField] private TextMeshProUGUI HealthText;

    [Header("Attack")]
    [SerializeField] private float playerReachDist = 1.0f; // How far the player can attack/interact
    public GameObject DefaultProjectilePrefab;
    private bool canAttack = true;

    [Header("Etc")]
    [SerializeField] private Transform centerPoint;

    [Header("Particle")]
    [SerializeField] private GameObject HitParticle;

    [Header("Dash Variables")]
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;
    private bool canDash = true;
    private bool isDashing;
    private bool dashStart = false;
    [SerializeField] private GameObject DashParticle;
    [SerializeField] private GameObject DIL;

    private InputManager inputManager;
    private Transform aimTransform;
    private Vector3 aimDirection;
    private Vector3 aimPosition;
    private Rigidbody2D rb;

    //hit particle
    private GameObject HitP;

    private GameManagerScript GM;
    public enum PlayerState
    {
        IDLE,
        MOVING,
        HURT
    }
    public PlayerState playerState = PlayerState.IDLE;

    // Start is called before the first frame update
    void Start()
    {
        Health = MaxHealth;
        inputManager = InputManager.Instance;
        GM = GameManagerScript.Instance;
        aimTransform = transform.Find("Aim");
        rb = gameObject.GetComponent<Rigidbody2D>();
        HealthSlider.maxValue = MaxHealth;
        HitP = Instantiate(HitParticle, transform.position, Quaternion.identity,transform);
        HitP.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        updateUI();
   
        updateDirection();
        checkInputs();
        updateAnimations();
    }

    public void setMaxHealth(float num)
    {
        MaxHealth = num;
    }

    public float getMaxHealth()
    {
        return MaxHealth;
    }

    //
    public void onPlayerMove()
    {
        //player is moving
        playerState = PlayerState.MOVING;
    }

    public void onPlayerDamage()
    {
        playerState = PlayerState.HURT;
    }

    public void updateAnimations()
    {
        if(rb.velocity.magnitude <= 0.5f)
            playerState = PlayerState.IDLE;

        if (playerState == PlayerState.MOVING)
        {
            ChangeAnimationState(PLAYER_RUN);
        }
        else if (playerState == PlayerState.HURT)
        {
            ChangeAnimationState(PLAYER_HURT);
        }
        else
        {
            playerState = PlayerState.IDLE;
            ChangeAnimationState(PLAYER_IDLE);
        }
    }

    void checkInputs()
    {
        if(inputManager.getLeftClickStatus())
        {
            if (canAttack)
            {
                Attack();
                canAttack = false;
                Invoke("resetAttack", DefaultProjectilePrefab.GetComponent<Projectile>().projectileData.fireRateCooldown);
            }
        }

        if (inputManager.DashButtonPressed())
        {
            if (canDash)
            {
                StartCoroutine(Dash());
            }
        }

        
    }

    void resetAttack()
    {
        canAttack = true;
    }

    void updateUI()
    {
        if (HealthSlider != null) { HealthSlider.value = Health; }
        HealthText.text = Health + "/" + MaxHealth;
    }

    void Attack()
    {
        SoundManager.Instance.BLIP.Play();
        GameManagerScript.Instance.PlayerShootEvent.Invoke();

        GameObject projectile = Instantiate(DefaultProjectilePrefab, aimTransform.position, aimTransform.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        Projectile projectileScript = DefaultProjectilePrefab.GetComponent<Projectile>();
     
        rb.AddForce((aimTransform.position-centerPoint.transform.position).normalized * projectileScript.projectileData.projectileForce, ForceMode2D.Impulse);
    }

    public void setProjectilePierced(bool value)
    {
        DefaultProjectilePrefab.GetComponent<Projectile>().setPierced(value);
    }

    public GameObject getProjectile()
    {
        return DefaultProjectilePrefab;
    }

    public Transform getCenterPoint()
    {
        return centerPoint;
    }

    public Transform getAimTransform()
    {
        return aimTransform;
    }

    void updateDirection()
    {
        if (Time.timeScale != 1) return;
        Vector3 cameraPos = Camera.main.ScreenToWorldPoint(inputManager.GetMousePos());
        aimDirection = (cameraPos - centerPoint.transform.position).normalized;

        aimPosition = new Vector2(aimDirection.x, aimDirection.y).normalized;
        aimTransform.position = centerPoint.transform.position + aimPosition * playerReachDist;

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        
        if(aimPosition.x >= 0)
        {
            //right
            //gameObject.transform.localScale = new Vector3(1, 1, 1);
            gameObject.GetComponent<SpriteRenderer>().flipX= false;
        }else if(aimPosition.x < 0)
        {
            //left
            //gameObject.transform.localScale = new Vector3(-1, 1, 1);
            gameObject.GetComponent<SpriteRenderer>().flipX = true;


        }
    }

    public void damage(float hitPoint)
    {
        SoundManager.Instance.EXPLOSION.Play();
        //monster hit him
        GM.PlayerOnDamageEvent.Invoke();

        bool critical = Random.Range(0, 100) < 10; //30% to crit
        DamagePopUp.Create(transform.position, (int) hitPoint, critical);
        HitP.SetActive(true);
        hitPoint = (critical) ? 2*hitPoint : hitPoint;
        onPlayerDamage();
        //Shake Camera
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>().start = true;
        if(Health - hitPoint > 0)
        {
            Health -= hitPoint;
        }
        else
        {
            //dead
            Health = 0;
            GM.GameOver();
            
        }
    }

    public void increaseHealth(float point)
    {
        if(Health + point >= MaxHealth)
        {
            Health = MaxHealth;
        }
        else
        {
            Health += point;
        }
    }

    private IEnumerator Dash()
    {
        SoundManager.Instance.DASH.Play();
        //dash particle
        DashParticle.transform.position = (aimTransform.position-aimPosition);

        float angle = Mathf.Atan2(gameObject.GetComponent<PlayerMovement>().getMovementDirection().y, gameObject.GetComponent<PlayerMovement>().getMovementDirection().x) * Mathf.Rad2Deg;
        DashParticle.transform.eulerAngles = new Vector3(0, 0, angle);
        DashParticle.SetActive(true);


        dashStart = true;
        //logic
        canDash = false;
        isDashing = true;

        rb.velocity = Vector2.zero;
        //Debug.Log("called");
        Vector2 dir = gameObject.GetComponent<PlayerMovement>().getMovementDirection().normalized;
        rb.velocity = new Vector2(dir.x*dashingPower, dir.y*dashingPower);
        //Debug.Log(dir);
        
        //rb.velocity = new Vector2((transform.localScale.x/Mathf.Abs(transform.localScale.x)) * dashingPower, 0);

        dashStart = false;
        yield return new WaitForSeconds(dashingTime);

        rb.velocity = Vector2.zero;
        isDashing = false;
        DIL.SetActive(true);
        DIL.GetComponent<DashIndicatorLogic>().setIndicatorCD(dashingCooldown);
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;

    }

    public bool playerIsDashing()
    {
        return isDashing;
    }

    public bool playerDashStarted()
    {
        return dashStart;
    }


    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

  

    /*GETTER FUNCTIONS*/
    public float getHealth() { return Health; }

    

}
