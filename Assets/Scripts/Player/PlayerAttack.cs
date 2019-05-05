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
    private int normalDamage;
    public int damage = 1;
    [Range(1, 100)]
    public int chanceOfCritical = 30;
    [Range(1, 10)]
    public int criticalDamageMultiplication = 2;
    private int criticalDamage;
    public Player player;
    public Sword Sword;
    private bool damageShow = false;
    public bool isCriticalHit;
    [SerializeField]
    public Transform PopUpDamage;          // Всплывающий текст с уроном по монстру

    public void Start()
    {
        criticalDamage = damage * criticalDamageMultiplication;
        normalDamage = damage;
    }
    public void Update()
    {
        if (timeAttack <= 0)
        {
           

            if (Input.GetButtonDown("Fire1"))
            {
                isCriticalHit = Random.Range(0, 100) < chanceOfCritical;
                Sword.anim.Play("SwordAtack");
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    Sword WeaponScript = gameObject.GetComponentInParent<Player>().Sword.GetComponent<Sword>();
                    var p = attackPos.transform.position;
                    

                    if (!isCriticalHit)
                    {
                        damage = normalDamage;
                        enemiesToDamage[i].GetComponent<Unit>().ReceiveDamage(damage);
                        if (!damageShow)
                        {
                            Transform damagePopupTransform = Instantiate(PopUpDamage, new Vector3(p.x, p.y, p.z), Quaternion.identity);
                            DamagePopUp damagePopUp = damagePopupTransform.GetComponent<DamagePopUp>();
                            damagePopUp.Setup(damage, isCriticalHit);
                            damageShow = true;
                        }
                    }
                    else
                    {
                        damage = criticalDamage;
                        enemiesToDamage[i].GetComponent<Unit>().ReceiveDamage(damage);
                        if (!damageShow)
                        {
                            Transform damagePopupTransform = Instantiate(PopUpDamage, new Vector3(p.x, p.y, p.z), Quaternion.identity);
                            DamagePopUp damagePopUp = damagePopupTransform.GetComponent<DamagePopUp>();
                            damagePopUp.Setup(damage, isCriticalHit);
                            damageShow = true;
                        }
                    }
                }
                timeAttack = startTimeAttack;
                damageShow = false;
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
