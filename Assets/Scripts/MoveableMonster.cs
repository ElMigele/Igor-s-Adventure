using UnityEngine;
using System.Collections;
using System.Linq;

public class MoveableMonster : Monster
{
    [SerializeField]
    private float speed;
    public int LivesMonstr = 3;
    private Vector3 direction;
    private bool isFacingRight = true;
    private float dieCooldown;
    private float dieRate = 0.9f;
    private SpriteRenderer sprite;
    private Rigidbody2D rb2d;
    public float dazedTime;
    public float startDazedTime;

    protected override void Start()
    {
        dieCooldown = 0f;
        direction = transform.right;
    }

    protected override void Update()
    {
        if (dazedTime <= 0)
        {
            speed = 1;
        }
        else
        {
            speed = 0;
            dazedTime -= Time.deltaTime;
        }
        Move();
        if (dieCooldown > 0)
        {
            dieCooldown -= Time.deltaTime;
        }
    }
    private bool CanDie
    {
        get
        {
            return dieCooldown <= 0f;
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        //Sword sword = collider.gameObject.GetComponent<Sword>();
        //if (sword)
        //{
        //    if (CanDie)
        //    {
        //        dieCooldown = dieRate;
        //        LivesMonstr--;
        //        Debug.Log(LivesMonstr);
        //    }
        //        if (LivesMonstr == 0)
        //        {
        //            ReceiveDamage();
        //        }
        //    if (LivesMonstr == 1)
        //    {
        //        speed = 4.0F;
        //    }
        //}
        //Arrow arrow = collider.gameObject.GetComponent<Arrow>();
        //if (arrow)
        //{
        //    LivesMonstr--;
        //    if (LivesMonstr == 0)
        //    {
        //        ReceiveDamage();
        //    }
        //    if (LivesMonstr == 1)
        //    {
        //        GetComponent<Renderer>().material.color = Color.red;
        //        speed = 4.0F;
        //    }
        //}
        Unit unit = collider.GetComponent<Unit>();

        if (unit && unit is Player)
        {
            unit.ReceiveDamage();
        }
    }

    private void Move()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.5F + transform.right * direction.x * 0.5F, 0.1F);

        if (colliders.Length > 0 ) direction *= -1.0F;

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

        if (colliders.Length > 0)
        {
            Vector3 theScaleMonster = transform.localScale;
            theScaleMonster.x *= -1;
            transform.localScale = theScaleMonster;
        }
    }
    public override void TakeDamage(int damage)
    {
        dazedTime = startDazedTime;
        if (LivesMonstr == 0)
        {
            ReceiveDamage();
        }
        if (LivesMonstr == 1)
        {
            GetComponent<Renderer>().material.color = Color.red;
            speed = 4.0F;
        }
        LivesMonstr -= damage;
        Debug.Log("take daamge");
    }
}