using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{

    public GameObject Gold;
    public virtual void ReceiveDamage()
    {
        Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
