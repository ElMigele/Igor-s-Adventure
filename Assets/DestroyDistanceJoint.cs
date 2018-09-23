using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDistanceJoint : MonoBehaviour {
    public DistanceJoint2D HoldingRope;

    public void Destroy()
    {
        HoldingRope.enabled = false;
    }
}
