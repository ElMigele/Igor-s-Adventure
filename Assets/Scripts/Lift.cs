﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour {
    private bool i;
    public Transform TopPosition;
    public Transform DownPosition;
    public float speed;
    public bool LiftIsActive;
    public bool ParentLift;
    public bool Motion = true;
    public bool backMotion = true;

    private void Start()
    {
        i = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (LiftIsActive == true) { }
        if (i == true)
        {
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
        }
    }
