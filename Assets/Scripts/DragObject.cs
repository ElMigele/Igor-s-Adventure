using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class DragObject : Unit
{
    bool MouseOn = false;
    private Transform curObj;
    // Use this for initialization
    float mass;
    public int itemID;
    void Start()
    {

    }
	void OnMouseDown()
    {
       
        MouseOn = true;
    }
    void OnMouseUp()
    {
        
        MouseOn = false;
    }
    // Update is called once per frame
    void Update() {


        if (Input.GetKey(KeyCode.Q) && (MouseOn))
        {
            Debug.Log("prees Q");
            this.transform.rotation *= Quaternion.Euler( 0f , 0f, 100f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E) && (MouseOn))
        {
            Debug.Log("prees Q");
            this.transform.rotation *= Quaternion.Euler(0f, 0f, -100f * Time.deltaTime);
        }
        Vector3 Cursor = Input.mousePosition;
        Cursor = Camera.main.ScreenToWorldPoint(Cursor);
        Cursor.z = 0;
        if (MouseOn)
        {
            this.transform.position = Cursor;
            curObj = transform;
            mass = curObj.GetComponent<Rigidbody2D>().mass; // запоминаем массу объекта
            curObj.GetComponent<Rigidbody2D>().mass = 0.0001f; // убираем массу, чтобы не сбивать другие объекты
            curObj.GetComponent<Rigidbody2D>().gravityScale = 0; // убираем гравитацию
            curObj.GetComponent<Rigidbody2D>().freezeRotation = true; // заморозка вращения
            curObj.position += new Vector3(0, 0.1f, 0); // немного приподымаем выбранный объект
        }
        if (!MouseOn)
        {
            curObj = transform;
            curObj.GetComponent<Rigidbody2D>().freezeRotation = false;
            curObj.GetComponent<Rigidbody2D>().mass = 5;
            curObj.GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }




}
