using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float movementAcceleration = 50.0f;
    //[SerializeField] private float movementSpeed = 1.0f;
    [SerializeField] private float maxMovementSpeed = 10.0f;
    private Vector2 dir = Vector2.zero;
    //[SerializeField] private float playerReachDist = 1.0f; // How far the player can attack/interact
    public UnityEvent OnMove;

    //private Transform aimTransform;
    private InputManager inputManager;


    // Start is called before the first frame update
    void Start()
    {
        inputManager = InputManager.Instance;
        rb = GetComponent<Rigidbody2D>();
        //aimTransform = transform.Find("Aim");
       
    }

    // Update is called once per frame
    void Update()
    {
        movePlayer();
        //updateDirection();
    }

    void movePlayer()
    {
        if (gameObject.GetComponent<PlayerManager>().playerIsDashing() || gameObject.GetComponent<PlayerManager>().playerDashStarted()) return;
        //Debug.Log(new Vector2(inputManager.GetPlayerMovement().x, inputManager.GetPlayerMovement().y));
        dir = new Vector2(inputManager.GetPlayerMovement().x, inputManager.GetPlayerMovement().y);
        
        rb.AddForce(dir*movementAcceleration,ForceMode2D.Force);
        if(Mathf.Abs(rb.velocity.magnitude) > maxMovementSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.normalized.x * maxMovementSpeed, rb.velocity.normalized.y * maxMovementSpeed);
        }

        if(dir == Vector2.zero)
        {
            rb.velocity = new Vector2(rb.velocity.x , rb.velocity.y) * 0.8f;
            //rb.AddForce(dir*-movementDeacceleration, ForceMode2D.Force);
            if(Mathf.Abs(rb.velocity.magnitude) <= 0)
            {
                rb.velocity = Vector2.zero;
            }
        }

        if (rb.velocity.magnitude > 0.5f)
        {
            OnMove.Invoke();
        }
    }

    /*void updateDirection()
    {
        Vector3 cameraPos = Camera.main.ScreenToWorldPoint(inputManager.GetMousePos());
        Vector3 aimDirection = (cameraPos - transform.position).normalized;

        Vector3 aimPosition = new Vector2(aimDirection.x, aimDirection.y).normalized;
        aimTransform.position = transform.position + aimPosition * playerReachDist;
    }*/

    public Vector2 getMovementDirection()
    {
        return dir;
    }

    
}
