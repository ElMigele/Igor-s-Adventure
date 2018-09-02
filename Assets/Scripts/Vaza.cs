using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vaza : Unit
{
    public GameObject Gold;
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
    }
    public override void ReceiveDamage()
    {
        //Arrow arrow = collider.gameObject.GetComponent<Arrow>();

        //if (arrow)
        //{
        //    ReceiveDamage();
        //}
        var p = transform.position;
        int count = Random.Range(0, 2);
        for (int i = 0; i < count; i++)
        { 
         Instantiate(Gold, new Vector3(p.x, p.y, p.z), Quaternion.identity);

        }
        Destroy(gameObject);
    }
}
