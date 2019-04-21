using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using System;

public class MoveableMonster : Monster
{
    [Header("Основные параметры")]
    public float speed = 5;                 // Скорость
    public int currentHP = 3;               // Количество жизней
    public int MaxHP = 3;                   // Количество жизней
    private Vector3 direction;              // Вектор направления
    private float dieCooldown;              // 
    private float dieRate = 0.3f;           //
    private SpriteRenderer sprite;          // 
    private Rigidbody2D rb2d;               // Твердое тело
    public float dazedTime;                 // Оглушение
    public float startDazedTime;            // 
    public Canvas MyGUI;                    // UI на котором будем отображать  
    public Slider EnemyHP;                  // полоска здоровья врага на экране
    public Transform metka;
    public Camera Mycamera;
    public GameObject BloodsEffect;

    Slider ShowHP;
    // Параметры из EnemyBehaivour
    public enum Attack
    {
        Infighting,                         // Ближний бой
        Outfighting                         // Дальний бой
    }
    public Attack AttackType = Attack.Infighting;
    public enum Behaivour
    {
        Following,                          // Преследование
        Patroling,                          // Патрулирование
        Idle                                // Покой
    }
    public Behaivour BehaivourType = Behaivour.Idle;
    [Header("Стрельба")]
    public float angle;
    public GameObject Arrow;                // Стрела
    private bool InVisibilityZone = false;  // Проверка на нахождение в зоне видимости
    private bool InImpactArea = false;      // Проверка на нахождение в зоне поражения
    public float ArrowVel = 50;             // Начальная скорость арбалетного болта
    public float AttackDelay = 5;           // Задержка по атаке
    public float AttackTimer = 5;           // Таймер до атаки
    private Vector3 PlayerPos;              // Позиция игрока, необходима для расчета угла стрельбы
    public GameObject PointParent;          // Объект предок всех точек патрулирования
    private Transform[] PointMassive;       // Массив точек обхода для режима патруль
    private float[] PointDelayMassive;      // Массив задержек на точке
    public int PointID = 1;                 // № точки, к которой идет враг
    public float MinDist = 0.5f;            // Допустимое расстояние, при котором враг переключается на следующую точку
    
    public GameObject ImpactZone;
    public GameObject VisibilityZone;

    private EnemyVisibility EnemyVisibility;// Скрипт видимости
    private EnemyImpact EnemyImpact;        // Скрипт зоны нападения
    public float PatrolTimer = 0;

    protected override void Start()
    {
        dieCooldown = 0f;
        direction = transform.right;
        SetHPBar();
        SetPatrol();
    }

    private void SetPatrol()
    {
        if (BehaivourType == Behaivour.Patroling)
        {
            if (PointParent != null)
            {
                int i = PointParent.GetComponentInChildren<Transform>().childCount;
                PointMassive = new Transform[i];
                PointDelayMassive = new float[i];
                for (int j = 0; j < i; j++)
                {
                    PointMassive[j] = PointParent.GetComponentInChildren<Transform>().GetChild(j);
                    if (PointMassive[j].GetComponentInChildren<PointParameters>())
                    {
                        PointDelayMassive[j] = PointMassive[j].GetComponentInChildren<PointParameters>().DelayTime;
                    }
                    else
                    {
                        PointDelayMassive[j] = 0;
                    }
                }
            }
            else
            {
                BehaivourType = Behaivour.Idle;
            }
        }
    }

    private void SetHPBar()
    {
        // создаем новый слайдер на основе эталона
        ShowHP = (Slider)Instantiate(EnemyHP);
        //Объявляем что он будет расположен в canvas
        ShowHP.transform.SetParent(MyGUI.transform, true);
        currentHP = MaxHP;
        ShowHP.maxValue = MaxHP;
        ShowHP.value = currentHP;
    }

    protected override void Update()
    {
        EnemyVisibility = VisibilityZone.GetComponentInChildren<EnemyVisibility>();
        InVisibilityZone = EnemyVisibility.InVisibilityZone;
        PlayerPos = EnemyVisibility.PlayerPos;
        EnemyImpact = ImpactZone.GetComponentInChildren<EnemyImpact>();
        InImpactArea = EnemyImpact.InImpactZone;

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
        Assault();
        if (dieCooldown > 0)
        {
            dieCooldown -= Time.deltaTime;
        }
        if (ShowHP != null)
        {
            // получаем экранные координаты расположения врага
            Vector3 screenPos = metka.transform.position;
            // задаем координаты расположения хп
            ShowHP.transform.position = screenPos;
            // показываем текущие здоровье на полосе хп
            ShowHP.value = currentHP;
        }
    }

    private bool CanDie
    {
        get
        {
            return dieCooldown <= 0f;
        }
    }
    
    private void Move()
    {
        if (BehaivourType == Behaivour.Patroling)
        {
            if (!InVisibilityZone)
            {
                if ((PointMassive.Length - 1) >= 1)
                {
                    if (Mathf.Sign(transform.localScale.x) == Mathf.Sign(transform.position.x - PointMassive[PointID].position.x))
                    {
                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    }
                    if (Vector3.Distance(PointMassive[PointID].position, transform.position) > MinDist)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, PointMassive[PointID].position, speed * Time.deltaTime);
                    }
                    else
                    {
                        if (PatrolTimer > PointDelayMassive[PointID])
                        {
                            if (PointID < (PointMassive.Length - 1))
                            {
                                PointID++;
                            }
                            else
                            {
                                PointID = 0;
                            }
                            PatrolTimer = 0;
                        }
                        else
                        {
                            PatrolTimer += Time.deltaTime;
                        }
                    }
                }
            }
        }

        if (BehaivourType == Behaivour.Following)
        {
            if (InVisibilityZone && (Vector3.Distance(PlayerPos, transform.position) > MinDist))
            {
                transform.position = Vector3.MoveTowards(transform.position, PlayerPos, speed * Time.deltaTime);
            }
        }
    }

    private void Assault()
    {
        if ((EnemyImpact.InImpactZone) && (AttackTimer >= AttackDelay))
        {
            // Рукопашный бой с игроком
            if (AttackType == Attack.Infighting)
            {
                Unit unit = EnemyImpact.col.GetComponent<Unit>();

                int damage = UnityEngine.Random.Range(10, 25);
                if (unit && unit is Player)
                {
                    unit.ReceiveDamage(damage);
                }
            }
            // Стрельба по игроку
            if (AttackType == Attack.Outfighting)
            {
                angle = Vector3.Angle(Vector3.right, (-transform.position + EnemyVisibility.PlayerPos));
                if ((EnemyVisibility.PlayerPos.y - transform.position.y) < 0)
                {
                    angle = -angle;
                }
                Instantiate(Arrow, transform.position, Quaternion.Euler(0, 0, angle));
            }
            AttackTimer = 0;
        }
        AttackTimer += Time.deltaTime;
    }
    public override void ReceiveDamage(int damage)
    {
        if (CanDie)
        {
            dieCooldown = dieRate;
            dazedTime = startDazedTime;
            currentHP -= damage;
            var p = transform.position;
            Instantiate(BloodsEffect, new Vector3(p.x, p.y, p.z), Quaternion.identity);
        }
        if (currentHP == 0)
        {
            Destroy(gameObject);
            ShowHP.transform.SetParent(transform, true);
        }
        if (currentHP == 1)
        {
            GetComponent<Renderer>().material.color = Color.red;
            speed = 4.0F;
        }
    }
}