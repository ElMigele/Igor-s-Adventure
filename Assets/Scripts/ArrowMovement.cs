using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour {
    public  float InitVelocity;         // Начальная скорость стрелы
    public  float curVelocity;          // Текущая скорость
    private float startAngle;           // Начальный угол стрельбы
    private float timer = 0;            // счетчик времени
    private Vector2[] positionsMassive; //
    public float destroyDelay = 5.0f;   // задержка перед удалением стрелы после попадание в объект
    private bool isCollision;           //
    private float destroyTime;          // Время уничтожения стрелы
    private float colAngle;             // Угловое положение стрелы в момент удара
    private Rigidbody2D m_Rigidbody;
    public float changAngle;           //
    public Collider2D OurCollider;

    public enum OwnerType
    {
        Enemy,
        Player
    }
    public OwnerType Owner = OwnerType.Player;
    // Use this for initialization
    void Start () {
        if (Owner == OwnerType.Player)
        {
            ArcherControl archer = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ArcherControl>();
            InitVelocity = archer.Velocity;
            archer.Velocity = archer.velDiap.x;
        }
        if (Owner == OwnerType.Enemy)
        {
            EnemyBehaivor enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponentInChildren<EnemyBehaivor>();
            InitVelocity = enemy.ArrowVel;
        }
        startAngle = Mathf.Deg2Rad*transform.eulerAngles.z;
        if (transform.eulerAngles.z > 90)
            changAngle = 90 - (transform.eulerAngles.z - 270);
        else
            changAngle = transform.eulerAngles.z;
        positionsMassive = new Vector2[2];
        isCollision = false;
        OurCollider = gameObject.GetComponentInChildren<Collider2D>();
        OurCollider.isTrigger = true;
	}

    private void OnTriggerExit2D(Collider2D collider)
    {
        Debug.Log("Я тут!");
        if (collider.transform.CompareTag("Player"))
        {
            OurCollider = gameObject.GetComponentInChildren<Collider2D>();
            OurCollider.isTrigger = false;
        }

    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (isCollision == false)
        {
            ArrowMove();
            ArrowRotate();
        }
        else
        {
            if (timer > destroyTime)
                Destroy(gameObject);
            else
            {
                transform.position = positionsMassive[0] + (positionsMassive[1] - positionsMassive[0]);
                transform.eulerAngles = new Vector3(0, 0, colAngle);
            }
        }
        //Debug.Log("Положение стрелы: " + positionsMassive[0].ToString("0.00"));
    }

    public void ArrowRotate()
    {// Стрела всегда смотрит в направление вектора скорости
        float angle = Vector3.Angle(Vector3.right, positionsMassive[1] - positionsMassive[0]);
        if (positionsMassive[1].y < positionsMassive[0].y)
        {
            angle = 360 - angle;
        }
        transform.eulerAngles = new Vector3(0, 0, angle);
        changAngle = angle;
        colAngle = transform.eulerAngles.z;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //if (isCollision == false)
        //{
        //    if (collision.collider.name.CompareTo("Target") == 0)
        //    {// Попадание в мишень
        //        Debug.Log("В яблочко!");
        //    }
        //    else if (collision.collider.name.CompareTo("Ground") == 0)
        //    {
        //        Debug.Log("Мимо!");
        //    }
        //    isCollision = true;
        //    destroyTime = timer + destroyDelay;
        //    if (collision.collider.tag.CompareTo("Player") == 0)
        //    {// Отключено
        //        Debug.Log("Самоубийство!");
        //        isCollision = false;
        //        destroyTime = 0;
        //    }
        //}
    }

    private void ArrowMove()
    {// для полета стрелы используется уравнение равнопеременного движения        
        positionsMassive[0] = transform.position;
        transform.position += new Vector3((InitVelocity * (float)Math.Cos(startAngle) * timer) * Time.deltaTime,
                                          (InitVelocity * (float)Math.Sin(startAngle) * timer - 9.8f * timer * timer / 2) * Time.deltaTime);
        float Vx = (InitVelocity * (float)Math.Cos(startAngle));
        float Vy = (InitVelocity * (float)Math.Sin(startAngle) - 9.8f * timer);
        curVelocity = Mathf.Sqrt(Vx * Vx + Vy * Vy);
        positionsMassive[1] = transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();

        if (unit && (unit is MoveableMonster || unit is Vaza))
        {
            unit.ReceiveDamage();
        }
    }
}
