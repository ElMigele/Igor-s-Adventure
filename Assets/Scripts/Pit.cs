using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Pit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();

        if (unit && unit is Player)
        {
            unit.ReceiveDamage();
        }
    }
}
