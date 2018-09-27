using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour {
    public enum Object
    {
        Gold,           // Золото
        HealthBonus     // Здоровье
    }
    public Object ObjectType;   // Тип объекта
    public int Count;           // Количество восстанавливаемого здоровья / получаемого золота

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
