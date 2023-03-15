using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : MonoBehaviour
{
    //Vampire Ability: % chance to gain 1 health
    [SerializeField] private int chance = 10; //percent
    [SerializeField] private int HealthIncrement = 1;
    public void VampireSkill()
    {
        if (!gameObject.activeInHierarchy) return;

        if (Random.Range(0,100) < chance)
        {
            TextPopUp.Create(transform.position, "+" + HealthIncrement, "FD3333");

            //heal
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().increaseHealth(HealthIncrement);
        }
    }
}
