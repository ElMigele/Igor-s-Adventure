using UnityEngine;
using System.Collections;
using System.Linq;

public class MoveableMonster : Monster
{
    [Header("Основные параметры")]
    public float speed = 5;                     // Скорость
    public int LivesMonstr = 3;             // Количество жизней
    private Vector3 direction;              // Вектор направления
    private bool isFacingRight = true;      // Проверка на смотр вправо
    private float dieCooldown;              // 
    private float dieRate = 0.9f;           //
    private SpriteRenderer sprite;          // 
    private Rigidbody2D rb2d;               // Твердое тело
    public float dazedTime;                 // Оглушение
    public float startDazedTime;            // 
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
    public enum Patrol
    {
        Cycle,                              // Цикличный обход
        ForwBack                            // Перебор вперед-назад
    }

    [Header("Атака")]
    public GameObject Arrow;                // Стрела
    private bool InVisibilityZone = false;  // Проверка на нахождение в зоне видимости
    private bool InImpactArea = false;      // Проверка на нахождение в зоне поражения
    public float ArrowVel = 50;             // Начальная скорость арбалетного болта
    public float AttackDelay = 5;           // Задержка по атаке
    public float AttackTimer = 5;           // Таймер до атаки
    private Vector3 PlayerPos;              // Позиция игрока, необходима для расчета угла стрельбы
    [Header("Патрулирование")]
    public Patrol PatrolType = Patrol.Cycle;// Тип патрулирования
    public GameObject PointMassive;         // Предок массива точек обхода
    public Transform[] PatrolPoints;        // Массив точек обхода для режима патруль
    public int PointID = 1;                 // № точки, к которой идет враг
    public float MinDist = 0.5f;            // Допустимое расстояние, при котором враг переключается на следующую точку
    public bool MoveUpList = true;
    [Header("Под-объекты")]
    public GameObject Body;
    public GameObject ImpactZone;
    public GameObject VisibilityZone;

    private EnemyVisibility EnemyVisibility;// скрипт видимости (Исус прости)

    protected override void Start()
    {
        dieCooldown = 0f;
        direction = transform.right;
        if (BehaivourType == Behaivour.Patroling)
        {
            int i = PointMassive.GetComponentInChildren<Transform>().childCount;
            PatrolPoints = new Transform[i];
            for (int j = 0; j < i; j++)
            {
                PatrolPoints[j] = PointMassive.GetComponentInChildren<Transform>().GetChild(j);
            }
            Debug.Log(PatrolPoints.Length);
        }
    }

    protected override void Update()
    {
        EnemyVisibility = VisibilityZone.GetComponentInChildren<EnemyVisibility>();
        InVisibilityZone = EnemyVisibility.InVisibilityZone;
        PlayerPos = EnemyVisibility.PlayerPos;

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
    }

    private void Move()
    {
        if (BehaivourType == Behaivour.Patroling)
        {
            if ((PatrolPoints.Length - 1) >= 1)
            {
                if ((PatrolPoints.Length - 1) >= 1)
                {
                    if (Mathf.Sign(Body.transform.localScale.x) == Mathf.Sign(Body.transform.position.x - PatrolPoints[PointID].transform.position.x))
                    {
                        Body.transform.localScale = new Vector3(-Body.transform.localScale.x, Body.transform.localScale.y, Body.transform.localScale.z);
                    }
                    if (Vector3.Distance(PatrolPoints[PointID].transform.position, Body.transform.position) > MinDist)
                    {
                        Body.transform.position = Vector3.MoveTowards(Body.transform.position, PatrolPoints[PointID].transform.position, speed * Time.deltaTime);
                    }
                    else
                    {
                        if (PatrolType == Patrol.ForwBack)
                        {
                            if ((PointID == PatrolPoints.Length - 1) || (PointID == 0))
                            {
                                MoveUpList = !MoveUpList;
                            }
                            if (MoveUpList == true)
                            {
                                PointID++;
                            }
                            else
                            {
                                PointID--;
                            }
                        }
                        if (PatrolType == Patrol.Cycle)
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
        if (EnemyVisibility.InVisibilityZone)
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
                    float angle = Vector3.Angle(Vector3.right, (-transform.position + EnemyVisibility.PlayerPos));
                    if ((EnemyVisibility.PlayerPos.y - transform.position.y) < 0)
                    {
                        angle = -angle;
                    }
                    Instantiate(Arrow, transform.position, Quaternion.Euler(0, 0, angle));
                    AttackTimer = 0;
                }
            }
        }
        AttackTimer += Time.deltaTime;
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