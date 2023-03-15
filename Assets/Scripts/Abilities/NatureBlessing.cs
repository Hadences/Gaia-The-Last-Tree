using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureBlessing : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        //increases Health by 20 and fully heals the player
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().setMaxHealth(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().getMaxHealth() + 20);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().increaseHealth(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().getMaxHealth() + 20);

    }

}
