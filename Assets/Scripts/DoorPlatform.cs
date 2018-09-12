using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPlatform : MonoBehaviour {

    public GameObject ButtonControl;
    public GameObject PointParent;
    public Transform[] PointMassive;
    public GameObject Door;
    public enum MoveType
    {
        OneMove,
        CycleMove
    }
    public MoveType Motion;
    public enum PrerollType
    {
        Yes,
        No
    }
    public PrerollType Preroll;
    
    public bool Active;
    public Button Owner;
    public int ForwardPointID = 1;
    public int BackwardPointID = 0;
    [Range(0.1f, 100)]
    public float speed = 1;
    [Range(0, 100)]
    public float MinDist = 0;

	// Use this for initialization
	void Start ()
    {
        int i = PointParent.GetComponentInChildren<Transform>().childCount;
        PointMassive = new Transform[i];
        for (int j = 0; j < i; j++)
        {
            PointMassive[j] = PointParent.GetComponentInChildren<Transform>().GetChild(j);
        } 
	}
	
	// Update is called once per frame
	void Update ()
    {
        Owner = ButtonControl.GetComponent<Button>();
        if (Owner.Active)
        {
            Active = true;
        }
        else
        {
            Active = false;
        }

        if (Active)
        {
            if (Motion == MoveType.OneMove)
            {
                if ((PointMassive.Length - 1) >= 1)
                {
                    if (Vector3.Distance(PointMassive[ForwardPointID].position, Door.transform.position) > MinDist)
                    {
                        Door.transform.position = Vector3.MoveTowards(Door.transform.position, PointMassive[ForwardPointID].position, speed * Time.deltaTime);
                    }
                    else
                    {
                        if (ForwardPointID < (PointMassive.Length - 1))
                        {
                            ForwardPointID++; BackwardPointID++;
                        }
                    }
                }
            }

            if (Motion == MoveType.CycleMove)
            {
                if ((PointMassive.Length - 1) >= 1)
                {
                    if (Vector3.Distance(PointMassive[ForwardPointID].position, Door.transform.position) > MinDist)
                    {
                        Door.transform.position = Vector3.MoveTowards(Door.transform.position, PointMassive[ForwardPointID].position, speed * Time.deltaTime);
                    }
                    else
                    {
                        if (ForwardPointID < (PointMassive.Length - 1))
                        {
                            ForwardPointID++; BackwardPointID++;
                        }
                        else
                        {
                            ForwardPointID = 0; BackwardPointID = PointMassive.Length - 1;
                        }
                    }
                }
            }
        }
        else
        {
            if ((Preroll == PrerollType.Yes) && (Motion == MoveType.OneMove))
            {
                if ((PointMassive.Length - 1) >= 1)
                {
                    if (Vector3.Distance(PointMassive[BackwardPointID].position, Door.transform.position) > MinDist)
                    {
                        Door.transform.position = Vector3.MoveTowards(Door.transform.position, PointMassive[BackwardPointID].position, speed * Time.deltaTime);
                    }
                    else
                    {
                        if (BackwardPointID > 0)
                        {
                            ForwardPointID--; BackwardPointID--;
                        }
                    }
                }
            }
        }
	}
}
