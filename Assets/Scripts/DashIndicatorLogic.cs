using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashIndicatorLogic : MonoBehaviour
{
    private bool start = false;
    private float t;
    private float cooldown;
    [SerializeField] private SpriteRenderer sr;

    public void setIndicatorCD(float cd)
    {
        cooldown = cd;
        start = true;

    }

    private void OnEnable()
    {
        sr.color = UtilsClass.GetColorFromString("FF426A");

    }

    // Update is called once per frame
    void Update()
    {
        if (!start) return;
        t += Time.deltaTime;
        
        //set the scale
        //max scale vaue is 1
        float scaleX = t / cooldown;
        if(scaleX > 1) scaleX= 1;
        transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
        if(t >= cooldown)
        {
            sr.color = UtilsClass.GetColorFromString("CAFF42");
            StartCoroutine(hideIndicator());
            
        }
    }

    IEnumerator hideIndicator()
    {
        yield return new WaitForSeconds(0.5f);
        start = false;
        t = 0;
        gameObject.SetActive(false);
    }
}
