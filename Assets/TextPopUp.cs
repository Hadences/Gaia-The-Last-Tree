using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Diagnostics;

public class TextPopUp : MonoBehaviour
{
    

    public static TextPopUp Create(Vector3 position, string text, string colorHex)
    {
        Vector3 offset = new Vector3(Random.Range(-1f, 1f),0,0);
        Transform damagePopup = Instantiate(GameManagerScript.Instance.TextPopup, position+offset, Quaternion.identity);
        TextPopUp damagepu = damagePopup.GetComponent<TextPopUp>();
        damagepu.SetUp(text,colorHex);

        return damagepu;
    }


    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void SetUp(string text, string colorHex)
    {
        textMesh.SetText(text);
        textMesh.color = UtilsClass.GetColorFromString(colorHex);  
        
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
