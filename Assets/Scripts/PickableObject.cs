 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickableObject : MonoBehaviour
{
    public enum Object
    {
        Gold,           // Золото
        HealthBonus,    // Здоровье
    }
    public Object ObjectType;   // Тип объекта
    public int Count;           // Количество восстанавливаемого здоровья / получаемого золота 
    public  GameObject[] UIKey;

    private float PickUpTimer;                      // Таймер подбора
    [Range(0.1f, 2)] public float PickUpDelay = 1;  // Время между подборами
    void Start()
    {
        PickUpTimer = PickUpDelay;
    }
    void Update()
    {
        if (PickUpTimer < PickUpDelay)
        {
            PickUpTimer += Time.deltaTime;
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag("Player"))
        {
            if (ObjectType == Object.Gold)
            {
                col.transform.GetComponent<Player>().count += Count;
                col.transform.GetComponent<Player>().SetCountText();
                Destroy(gameObject);
            }

           
        }
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if ((Input.GetKeyDown(KeyCode.E)) && (PickUpTimer >= PickUpDelay))
        {
            if (ObjectType == Object.HealthBonus)
            {
                Player player = col.transform.GetComponent<Player>();
                if (player.Lives < player.MaxLives)
                {
                    int iLives = player.Lives + Count;
                    if (iLives > player.MaxLives)
                    {
                        player.Lives = player.MaxLives;
                    }
                    else
                    {
                        player.Lives = iLives;
                    }
                    player.SetLives(player.Lives);
                    Destroy(gameObject);
                }
            }
        }
    }
    }
