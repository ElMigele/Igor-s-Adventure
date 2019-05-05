using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CodeMonkey.Utils;

public class DamagePopUp : MonoBehaviour {

    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;

    public void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }
    public void Setup(int damagePopUp, bool isCriticalHit) {
        textMesh.SetText(damagePopUp.ToString());
        if (!isCriticalHit) { 
        // normal hit
        textMesh.fontSize = 2;
        textColor = UtilsClass.GetColorFromString("D9D9D9");
 
        } else { 
            // critical hit
            textMesh.fontSize = 2.3f;
        textColor = UtilsClass.GetColorFromString("FF0000");
    
        }
         textMesh.color = textColor;
        disappearTimer = 9f;

        moveVector = new Vector3(.7f , 1) * 1.2f;
    }

    public void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 1f * Time.deltaTime;
        disappearTimer = -Time.deltaTime;

        if (disappearTimer < 0)
            {
            float disappearSpeed = 2f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;

            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }

            }

    }
	
}
