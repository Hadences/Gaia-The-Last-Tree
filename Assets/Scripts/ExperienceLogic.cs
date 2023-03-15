using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceLogic : MonoBehaviour
{
    private GameObject Tree;
    private GameManagerScript GM;
    [SerializeField] private int expAmount = 3;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float randomMoveTimer = 2;
    private bool move = false;
    private float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        GM = GameManagerScript.Instance;
        Tree = GM.getTree();
        randomMoveTimer = Random.Range(0, randomMoveTimer);
    }

    // Update is called once per frame
    void Update()
    {
        time+= Time.deltaTime;
        if(time >= randomMoveTimer)
        {
            move = true;
        }
        if (move)
        {
            transform.position = Vector2.MoveTowards(transform.position, Tree.transform.position, movementSpeed * Time.deltaTime);
            Vector2 dist = transform.position - Tree.transform.position;
            if (dist.magnitude < 0.5f)
            {
                GM.increaseEXP(expAmount);
                move = false;
                Destroy(gameObject);
            }
        }
    }
}
