using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public static SoundManager Instance
    {
        get { return instance; }
    }    // Start is called before the first frame update

    public AudioSource DASH;
    public AudioSource BLIP;
    public AudioSource HIT;
    public AudioSource EXPLOSION;
    public AudioSource SELECT;
    public AudioSource POWERUP;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
