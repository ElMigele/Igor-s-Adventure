using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour{

    public GameObject item;
    private Transform player;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;  
        //player = heroes[PlayerPrefs.GetInt("h")].transform;
    }
    public void SpawnDroppedItem()
    {
        Vector2 playerPos = new Vector2(player.position.x  , player.position.y + 0.4f);
        Instantiate(item, playerPos, Quaternion.identity);
    }
}
    