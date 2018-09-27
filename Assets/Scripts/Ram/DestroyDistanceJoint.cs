using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDistanceJoint : MonoBehaviour {
    public DistanceJoint2D HoldingRope;
    public bool IsControl = false;
    public bool IsActiveControl = false;
    public GameObject Button4Control;
    private Button Button4ControlSrcipt;
    [Range(0.1f, 5)]
    public float DistanceSpeed = 1;
    private Rigidbody2D rigidbody;
    [HideInInspector]
    public float[] Velocity = new float[2];
    
    private void Start()
    {
        Button4ControlSrcipt = Button4Control.GetComponent<Button>();
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (IsControl)
        {
            IsActiveControl = Button4ControlSrcipt.Active;
            if (IsActiveControl)
            {
                if (!HoldingRope.enabled)
                {
                    HoldingRope.enabled = true;
                }
                RamMotion();
            }
            else
            {
                if (HoldingRope.enabled)
                {
                    HoldingRope.enabled = false;
                }
            }
            Velocity[0] = Velocity[1];
            Velocity[1] = Mathf.Sqrt(rigidbody.velocity.x * rigidbody.velocity.x + rigidbody.velocity.y * rigidbody.velocity.y);             
        }
    }

    private void RamMotion()
    {
        HoldingRope.distance -= DistanceSpeed * Time.deltaTime;
    }

    public void Destroy()
    {
        HoldingRope.enabled = false;
    }
}
