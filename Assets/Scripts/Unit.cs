using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{
    public virtual void ReceiveDamage(int damage)
    {
        Destroy(gameObject);
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
