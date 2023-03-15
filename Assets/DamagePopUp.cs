using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Diagnostics;

public class DamagePopUp : MonoBehaviour
{
    

    public static DamagePopUp Create(Vector3 position, int damageAmount, bool isCriticalHit)
    {
        Vector3 offset = new Vector3(Random.Range(-1f, 1f),0,0);
        Transform damagePopup = Instantiate(GameManagerScript.Instance.DamagePopUp, position+offset, Quaternion.identity);
        DamagePopUp damagepu = damagePopup.GetComponent<DamagePopUp>();
        damagepu.SetUp(damageAmount,isCriticalHit);

        return damagepu;
    }


    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void SetUp(int amount, bool isCriticalHit)
    {
        textMesh.SetText(amount.ToString());
        if(isCriticalHit)
        {
            
            textMesh.color = UtilsClass.GetColorFromString("FF0053");
        }
        else
        {
            textMesh.color = UtilsClass.GetColorFromString("FF7100");
            
        }
        textColor = textMesh.color;
        disappearTimer = 0.4f;
    }

    private void Update()
    {
        float moveYSpeed = 4f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        disappearTimer -= Time.deltaTime;
        if(disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if(textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
