using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float timeAttack;
    public float startTimeAttack;

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRange;
    public int damage = 1;
    void Update()
    {
        if (timeAttack <= 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    Sword WeaponScript = gameObject.GetComponentInParent<Player>().Sword.GetComponent<Sword>();
                    enemiesToDamage[i].GetComponent<MoveableMonster>().ReceiveDamage(damage); 
                }
            timeAttack = startTimeAttack;
            }
        }
        else
        {
            timeAttack -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
