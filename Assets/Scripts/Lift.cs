using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour {
    public Transform TopPosition;
    public Transform DownPosition;
    public GameObject DependObject;
    public float speedUp;
    public float speedDown;
    public float speed;
    public bool LiftIsActive;
    bool i, backMotion;// Motion;
    public enum Motion
    {
        Up,                 // Вверх
        UpBack,             // Вверх и вниз с разными скоростями
    }
    public Motion MotionType;   // Тип перемещения
    public bool MoveUp;
    /*
    public bool ParentLift;
    public bool Motion = true;
    public bool backMotion = true;*/

    private void Start()
    {

    }/*
    // Update is called once per frame
    void Update()
    {
        if (LiftIsActive == true)
        {
            if ((MotionType == Motion.Up) && (DependObject.transform.position != TopPosition.position))
            {
                DependObject.transform.position = Vector3.MoveTowards(DependObject.transform.position, TopPosition.position, speedUp * Time.deltaTime);
            }
            if (MotionType == Motion.UpBack)
            {
                if (MoveUp)
                {
                    if (DependObject.transform.position != TopPosition.position)
                    {
                        DependObject.transform.position = Vector3.MoveTowards(DependObject.transform.position, TopPosition.position, speedUp * Time.deltaTime);
                    }
                    else
                    {
                        MoveUp = !MoveUp;
                    }
                }
                else
                {

                }
            }
            transform.localPosition = Vector3.MoveTowards(transform.position, TopPosition.position, speed * Time.deltaTime);
            if (transform.position == TopPosition.position)
            {
                if (backMotion)
                {
                    i = false;
                }
            }
        }

        if (i == false)
        {
            transform.localPosition = Vector3.MoveTowards(transform.position, DownPosition.position, speed * Time.deltaTime);
            if (transform.position == DownPosition.position)
            {
                if (Motion)
                {
                    i = true;
                }
            }
        }
    }

        void OnCollisionEnter2D(Collision2D coll)
        {
        if (ParentLift)
            coll.transform.parent = transform;
        }

        void OnCollisionExit2D(Collision2D coll)
        {
            coll.transform.parent = null;
        }*/
    }
