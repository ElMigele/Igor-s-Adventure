using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slots : MonoBehaviour{
    
private Inventory inventory;
public int i;
public GameObject[] heroes;
    public bool dropped = false;
    private void Start()
    {
        //inventory = heroes[PlayerPrefs.GetInt("h")].GetComponent<Inventory>();
        inventory = heroes[PlayerPrefs.GetInt("h")].GetComponent<Inventory>();
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

public void DropItem(){
            foreach (Transform child in transform)
            {
                child.GetComponent<Spawn>().SpawnDroppedItem();
                GameObject.Destroy(child.gameObject);
            }
}
}