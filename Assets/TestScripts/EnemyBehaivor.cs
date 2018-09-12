using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaivor : MonoBehaviour {
    // Скрипт по поведению врагов
    //
    // В скрипте прописаны варианты поведения врагов - их передвижение и реакция на игрока
    public enum Attack
    {
        Infighting,                         // Ближний бой
        Outfighting                         // Дальний бой
    }
    public enum Behaivour
    {
        Following,                          // Преследование
        Patroling,                          // Патрулирование
        Idle                                // Покой
    }
    public Attack AttackType = Attack.Infighting;
    public Behaivour BehaivourType = Behaivour.Idle;

    public GameObject Arrow;                // Стрела

    private bool InVisibilityZone = false;  // Проверка на нахождение в зоне видимости
    private bool InImpactArea = false;      // Проверка на нахождение в зоне поражения
    public float ArrowVel = 50;             // Начальная скорость арбалетного болта
    public float AttackDelay = 5;           // Задержка по атаке
    public float AttackTimer = 5;           // Таймер до атаки
    private Vector3 PlayerPos;              // Позиция игрока, необходима для расчета угла стрельбы
    public Transform[] PatrolPoints;          // Точки обхода для режима патруль
    public int PointID = 1;
    public float MinDist = 0.5f;
    public float objVel = 1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Move();
        Assault();        
	}
    private void Move()
    {
        if (BehaivourType == Behaivour.Patroling)
        {
            if ((PatrolPoints.Length - 1) >= 1)
            {
                if (Mathf.Sign(transform.localScale.x) == Mathf.Sign(transform.position.x - PatrolPoints[PointID].position.x))
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
                if (Vector3.Distance(PatrolPoints[PointID].position, transform.position) > MinDist)
                {
                    transform.position = Vector3.MoveTowards(transform.position, PatrolPoints[PointID].position, objVel * Time.deltaTime);
                }
                else
                {
                    if (PointID < (PatrolPoints.Length - 1))
                    {
                        PointID++;
                    }
                    else
                    {
                        PointID = 0;
                    }
                }
            }
        }
    }



    private void Assault()
    {
        if (InVisibilityZone)
        {
            // Рукопашный бой с игроком
            if (AttackType == Attack.Infighting)
            {
                
            }
            // Стрельба по игроку
            if (AttackType == Attack.Outfighting)
            {
                if (AttackTimer >= AttackDelay)
                {
                    float angle = Vector3.Angle(Vector3.right, (-transform.position + PlayerPos));
                    if ((PlayerPos.y - transform.position.y) < 0)
                    {
                        angle = -angle;
                    }
                    Instantiate(Arrow, transform.position, Quaternion.Euler(0, 0, angle));
                    // Debug.Log("Кватернион: " + Quaternion.Euler(0, 0, angle) + ", Угол стельбы: " + angle.ToString("0.0"));
                    AttackTimer = 0;
                }
            }
        }
        AttackTimer += Time.deltaTime;
    }
    
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Игрок в зоне видимости!");
            InVisibilityZone = true;
            PlayerPos = collision.transform.position;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Игрок в зоне видимости!");
            PlayerPos = collision.transform.position;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Игрок вне зоне видимости!");
            InVisibilityZone = false;
            PlayerPos = Vector3.zero;
        }
    }
}/*
public abstract class UnityBase : GameObjectBase
{
    public List<transform> VisiblePoints;
    public static bool IsVisibleUnit<T>(T unit, Transform from, float angle, float distance, Latermask mask)
    {
        bool result = false;
        if (unit != null)
        {
            foreach (Transform visibleOint in unit.VisiblePoints)
            {
                if (IsVisibleObject(from, VisiblePoints.position, unit.gameObject, angle, distance, mask))
                {
                    result = true;
                    break;
                }
            }
        }
        return result;
    }
    public static bool IsVisibleObject(transform from, Vector3 point, GameObject target, float angle, float distance, Layermask mask)
    {
        bool result = false;
        if (IsAvaiblePoint(from, poin, angle, distance))
        {
            Vector3 direction = point - from.position;
            Ray ray = new Ray(from.poition, direction);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance, mask.value))
            {
                if (hit.collider.gameObject == target)
                {
                    result = true;
                }
            }
        }
        return result;
    }
}*/