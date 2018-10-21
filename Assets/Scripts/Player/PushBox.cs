using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBox : MonoBehaviour
{

    public float distance = 1f;
    public LayerMask boxMask;
    public Player player;
    public bool IsBoxPushig;
    GameObject box;
    // Use this for initialization
    void Start()
    {
        IsBoxPushig = false;
    }

    // Update is called once per frame
    void Update()
    {

        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distance, boxMask);

        if (hit.collider != null && hit.collider.gameObject.tag == "Box" && Input.GetKeyDown(KeyCode.E))
        {
            box = hit.collider.gameObject;
            box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
            box.GetComponent<FixedJoint2D>().enabled = true;
            //box.GetComponent<boxpull>().beingPushed = true;
            IsBoxPushig = true;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            box.GetComponent<FixedJoint2D>().enabled = false;
            IsBoxPushig = false;
            //box.GetComponent<boxpull>().beingPushed = false;
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * transform.localScale.x * distance);
    }
}