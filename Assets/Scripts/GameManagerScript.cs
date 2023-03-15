using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    private static GameManagerScript instance;
    public static GameManagerScript Instance
    {
        get { return instance; }
    }

    [Header("GameManager")]
    public List<GameObject> Spawnpoints;
    public GameObject FlameInsect;
    public List<GameObject> Mobs;
    [SerializeField] private float spawnRateInterval = 15f; //every 15 seconds spawn
    [SerializeField] private int minSpawnAtInstance = 2; // should at least spawn 2 at each spawn interval
    [SerializeField] private int maxSpawnAtInstance = 8; // should at least spawn 8 at each spawn interval
    [SerializeField] private int levelUpSpawnIncrement = 2; //spawn 2 extra more every level up
    private float spawnTime = 0.0f;
    [SerializeField] private GameObject LevelUpPanel;

    [SerializeField] private float flameInsectSpawnRate = 0.8f;
    private float FIspawnTime = 0.0f;

    [Header("TreeLogic")]
    public List<GameObject> TreeImages;
    [SerializeField] private GameObject Tree;
   // [SerializeField] private int TreeHealth = 50;
    [SerializeField] private int level = 1; //max level is 5
    [SerializeField] private int experience = 0;

    [SerializeField] private int expNeededLevel2 = 100;
    [SerializeField] private int expNeededLevel3 = 120;
    [SerializeField] private int expNeededLevel4 = 140;
    [SerializeField] private int expNeededLevel5 = 150;
    [SerializeField] private int expNeededToWin = 300;

    //[SerializeField] private TMP_Text TreeHealthText; //temporary indicator
    [SerializeField] private TMP_Text TreeLevelText;
    [SerializeField] private TMP_Text TreeExperienceText;
    [SerializeField] private Slider TreeHealthSlider;

    [Header("Game Assets")]
    public Transform DamagePopUp;
    public GameObject DeathParticle;
    public GameObject TreeLevelUpParticle;
    public Transform TextPopup;
    public GameObject PauseMenu;

    [Header("Game Pause/Resume")]
    private bool gamePaused = false;

    [Header("Events")]
    public UnityEvent PlayerOnDamageEvent;
    public UnityEvent PlayerDealDamageEvent;
    public UnityEvent PlayerShootEvent;

    private InputManager inputManager;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }


    void Start()
    {
        inputManager = InputManager.Instance;
        Time.timeScale = 1;

        Application.targetFrameRate = 300;

        //Cursor.visible = false;

        spawnTime = spawnRateInterval;
        updateTree();

    }

    // Update is called once per frame
    void Update()
    {
        if (inputManager.PauseResumeButtonPressed())
        {
            if (gamePaused) { ResumeGame(); } else { PauseGame(); }
        }

        updateTreeHealthUI();
        //TreeHealthText.text = "Tree Health: " + TreeHealth;
        TreeLevelText.text = "Level: " + level;
        TreeExperienceText.text = "" + experience;

        //Mob Spawn interval
        spawnTime += Time.deltaTime;
        if(spawnTime >= spawnRateInterval)
        {
            //spawn
            StartCoroutine(MobSpawnLogic());
            spawnTime = 0;
        }

        FIspawnTime += Time.deltaTime;
        if(FIspawnTime >= flameInsectSpawnRate)
        {
            spawnFlameInsect();
            FIspawnTime = 0;
        }

    }

    

    void updateTreeHealthUI()
    {
        switch (level)
        {
            case 1:
                
                TreeHealthSlider.maxValue = expNeededLevel2;
                TreeHealthSlider.value = experience;
                
                return;
            case 2:
               
                    TreeHealthSlider.maxValue = expNeededLevel3;
                    TreeHealthSlider.value = experience;

                
                return;
            case 3:

                    TreeHealthSlider.maxValue = expNeededLevel4;
                    TreeHealthSlider.value = experience ;
                
                return;
            case 4:
                    TreeHealthSlider.maxValue = expNeededLevel5;
                    TreeHealthSlider.value = experience;
                
                return;
            case 5:
                TreeHealthSlider.maxValue = expNeededToWin;
                TreeHealthSlider.value = experience;

                return;
        }
    }

    void spawnFlameInsect()
    {
        
            Vector3 spawnPos = Spawnpoints[Random.Range(0, Spawnpoints.Count)].transform.position; //random spawn point
            
            Instantiate(FlameInsect, spawnPos, Quaternion.identity);
        
    }

    IEnumerator MobSpawnLogic()
    {
        int mobToSpawn = Random.Range(minSpawnAtInstance, maxSpawnAtInstance + 1);
        for (int i = 0; i < mobToSpawn; i++)
        {
            Vector3 spawnPos = Spawnpoints[Random.Range(0, Spawnpoints.Count)].transform.position; //random spawn point
            GameObject randomMob = Mobs[Random.Range(0, Mobs.Count)].gameObject;
            Instantiate(randomMob, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }
    }

    //Tree Logic
    public GameObject getTree() { return Tree; }
    public void damageTree(int damage)
    {
        if(experience - damage <= 0)
        {
            //Dead
            experience = 0;
            //GameOver(); // Tree has died and its game over
        }
        else { 
        
            experience -= damage;
        }
    }

    public void GameOver()
    {
        //TODO
        SceneManager.LoadScene(3);
        Debug.Log("Game Over");
    }

    public void increaseEXP(int amount)
    {
        experience += amount;
        checkEXPByLevel();
    }

    void updateTree()
    {
        //show panel
        if (level != 1)
        {

            LevelUpPanel.SetActive(true);
            SoundManager.Instance.POWERUP.Play();

            Instantiate(TreeLevelUpParticle, Tree.transform.position, Quaternion.identity);

            TextPopUp.Create(Tree.transform.position, "LEVEL UP", "7AFF5F");
        }
        //turn off all objects
        foreach(GameObject obj in TreeImages){
            obj.SetActive(false);
        }

        TreeImages[level-1].SetActive(true);

        
    }

    public void checkEXPByLevel()
    {
        switch (level)
        {
            case 1:
                if(experience >= expNeededLevel2)
                {
                    //level up
                    level++;
                    experience = 0;
                    minSpawnAtInstance += levelUpSpawnIncrement;
                    maxSpawnAtInstance += levelUpSpawnIncrement;
                    updateTree();
                }
                return;
            case 2:
                if (experience >= expNeededLevel3)
                {
                    //level up
                    level++;
                    experience = 0;
                    minSpawnAtInstance += levelUpSpawnIncrement;
                    maxSpawnAtInstance += levelUpSpawnIncrement;
                    updateTree();
                }
                return;
            case 3:
                if (experience >= expNeededLevel4)
                {
                    //level up
                    level++;
                    experience = 0;
                    minSpawnAtInstance += levelUpSpawnIncrement;
                    maxSpawnAtInstance += levelUpSpawnIncrement;
                    updateTree();
                }
                return;
            case 4:
                if (experience >= expNeededLevel5)
                {
                    //level up
                    level++;
                    experience = 0;
                    minSpawnAtInstance += levelUpSpawnIncrement;
                    maxSpawnAtInstance += levelUpSpawnIncrement;
                    updateTree();
                }
                return;
            case 5:
                if(experience >= expNeededToWin)
                {
                    Win();
                }
                return;
        }
    }

    public void PauseGame()
    {
        SoundManager.Instance.SELECT.Play();

        gamePaused = true;
        PauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        SoundManager.Instance.SELECT.Play();

        gamePaused = false;
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        SoundManager.Instance.SELECT.Play();

        SceneManager.LoadScene(0);
    }

    void Win() {
        SceneManager.LoadScene(4);
    }

    
}
