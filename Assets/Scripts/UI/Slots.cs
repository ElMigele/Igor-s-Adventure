using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slots : MonoBehaviour
{

    private Inventory inventory;
    public int i;

    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    public void Update()
    {
        if (transform.childCount <= 0)
        {
            inventory.isFull[i] = false;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropItem();
        }
    }

    public void DropItem()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Spawn>().SpawnDroppedItem();
            GameObject.Destroy(child.gameObject);
        }
    }
    //public void SetCross()
    //{
    //    foreach (Transform child in transform)
    //    {
    //        gameObject.SetActive(false);
    //    }
    //}
}