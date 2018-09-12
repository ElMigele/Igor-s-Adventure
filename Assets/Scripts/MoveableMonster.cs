using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class MoveableMonster : Unit
{
    [Header("Основные параметры")]
    public float speed = 5;                     // Скорость
    public int currentHP = 3;             // Текущие количество жизней
    public int MaxHP = 3;             // Максимальное количество жизней
    private Vector3 direction;              // Вектор направления
    private bool isFacingRight = true;      // Проверка на смотр вправо
    private float dieCooldown;              // задержка на нанесение урона врагу
    private float dieRate = 0.9f;           // задержка урона
    private SpriteRenderer sprite;          // сокращение загрузкаи спрайта 
    private Rigidbody2D rb2d;               // Твердое тело
    public float dazedTime;                 // Оглушение
    public float startDazedTime;            // 
    // Параметры из EnemyBehaivour
    public Canvas MyGUI; // UI на котором будем отображать  
    public Slider EnemyHP; // полоска здоровья врага на экране
    public Transform metka;
    public Camera Mycamera;

    Slider ShowHP;

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
    [Header("Стрельба")]
    public GameObject Arrow;                // Стрела
    private bool InVisibilityZone = false;  // Проверка на нахождение в зоне видимости
    private bool InImpactArea = false;      // Проверка на нахождение в зоне поражения
    public float ArrowVel = 50;             // Начальная скорость арбалетного болта
    public float AttackDelay = 5;           // Задержка по атаке
    public float AttackTimer = 5;           // Таймер до атаки
    private Vector3 PlayerPos;              // Позиция игрока, необходима для расчета угла стрельбы
    public Transform[] PatrolPoints;        // Массив точек обхода для режима патруль
    public int PointID = 1;                 // № точки, к которой идет враг
    public float MinDist = 0.5f;            // Допустимое расстояние, при котором враг переключается на следующую точку

    public GameObject ImpactZone;
    public GameObject VisibilityZone;

    private EnemyVisibility EnemyVisibility;// скрипт видимости (Исус прости)

    void Start()
    {
        dieCooldown = 0f;
        direction = transform.right;

        // создаем новый слайдер на основе эталона
        ShowHP = (Slider)Instantiate(EnemyHP);
        //Объявляем что он будет расположен в canvas
        ShowHP.transform.SetParent(MyGUI.transform, true);

        currentHP = MaxHP;
        ShowHP.maxValue = MaxHP;
        ShowHP.value = currentHP;

    }

    void Update()
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
    void OnTriggerEnter2D(Collider2D collider)
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
                    transform.position = Vector3.MoveTowards(transform.position, PatrolPoints[PointID].position, speed * Time.deltaTime);
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
        if (currentHP == 0)
        {
            ReceiveDamage();
   
        }
        if (currentHP == 1)
        {
            GetComponent<Renderer>().material.color = Color.red;
            speed = 4.0F;
        }
        currentHP -= damage;
        Debug.Log("take daamge");
    }
}