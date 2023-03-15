using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCard : MonoBehaviour
{
    [SerializeField] private GameObject AbilityToActivate;
    [SerializeField] private bool ACTIVE = false;
    public void Activate()
    {
        SoundManager.Instance.SELECT.Play();
        AbilityToActivate.SetActive(true);
        ACTIVE = true;
    }

    public bool isActive()
    {
        return ACTIVE;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
