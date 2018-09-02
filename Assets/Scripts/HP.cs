using UnityEngine;
using System.Collections;

public class HP: MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Player player = collider.GetComponent<Player>();

        if (player)
        {
            player.Lives++;
            Destroy(gameObject);
        }
    }
}
