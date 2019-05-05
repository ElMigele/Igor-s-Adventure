using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Inventory inventory;
    public GameObject itemButton;


    private void Start()
    {
        //inventory = heroes[PlayerPrefs.GetInt("h")].GetComponent<Inventory>(); 
        //inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }
    
    void OnTriggerEnter2D(Collider2D other){
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        if (other.CompareTag("Player"))
        {
         for (int i = 0; i < inventory.slots.Length; i++)   
          {
             if (inventory.isFull[i] == false)
              {
             inventory.isFull[i] = true;
             Instantiate(itemButton, inventory.slots[i].transform, false);
    
                    Destroy(gameObject);
             break;
              }
          }
        }
        
    }
}