using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class Player : Unit
{
    [Header("Ссылки на интерфейс")]
    public Text coinText;                          // Количество золота, имеющееся у игрока
    public Text expText;                          // Количество опыта
    public Text levelText;                          // уровень
    public Text LivesText;                          // Количество жизней, оставшихся у игрока
    [HideInInspector]
    public HealthEnergyBar HE;                      // Полосы здороья и силы натяжения лука у героя
    [Header("Параметры игрока")]
    [Range(0.1f, 5)]
    public float maxSpeed = 1f;                     // Максимальная скорость игрока
    public float hightJump = 920;                   // Высота прыжка
    [Range(1, 100)]
    public int Lives;                               // Количество жизней    
    [HideInInspector]
    public int MaxLives;                           // Максимальное количество жизней игрока
    private int Level = 1;                           // Уровень персонажа
    [NonSerialized]
    public int count;                               // Счетчик золота
    private float BarrierNear = -0.085f;            // Растояние на котором можно таскать ящик
    [Header("Анимация")]
    private Animator anim;                          // Анимация
    private Rigidbody2D rb2d;                       // Твердое тело игрока
    private SpriteRenderer sprite;                  // Спрайт игрока
    public double vSpeed;                           // Устанавливаемая скорость взлета и падения для анимации
    //ссылка на компонент анимаций
    private float moveHorizontal;
    private float moveVertical;
    [Header("Оружия")]
    public float TimeOff = 3;                       // Время атаки
    public GameObject WeaponPoint;                  // Точка, в которой располагается оружие
    public GameObject Sword;                        // Объект меча
    public GameObject Bow;                          // Объект лука
    public enum ActiveWeapon
    {
        Лук,          // Лук 
        Меч,          // Меч
        Гарпун        // Гарпун
    }
    public ActiveWeapon Активное_Оружие;            // Активное оружие
    private PlayerAttack attack;                    // Скрипт на владение мечем
    private ArcherControl archer;                   // Скрипт на владение луком
    private RopeSystem rope;                        // Скрипт на владение гарпуном
    public Interface _interface;

    private float dieCooldown;
    private float dieRate = 0.3f;

    [Header("Проверки на состояние")]
    public bool isGrounded = false;                 // Стоит на земле
    public LayerMask whatIsGround;                  // Cсылка на слой, представляющий землю
    //public LayerMask whatIsBox;
    public LayerMask whatIsBarrier;
    private bool isPress = false;                   // 
    //public bool isWater = false;                  // В воде

    private float groundAngle = 0;
    public Transform ground;

    private BoxCollider2D box;

    public Transform RespawnPoint;                  // Точка респауна
    [Header("Подключаемые эффекты")]
    public GameObject HPRestore;
    public GameObject BloodsEffect;
    [Header("Переменые для гарпуна")]
    public Vector2 ropeHook;
    public float swingForce = 6f;
    public bool isSwinging;
    private float jumpInput;
    public float jumpSpeed = 3f;
    private bool isJumping;
    public bool groundCheck;
    [Header("Скрипты на восстановление положения объектов")]
    public ResetState RSVaz;
    public ResetState RSBox;
    [Header("Таскание ящиков")]
    public float distance = 1f;
    public LayerMask boxMask;
    public bool isBoxPushig;
    GameObject Box;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        dieCooldown = 0f;
        SetCountText(); 
        HealthEnergyBar.use.AdjustCurrentEnergy(-150);
        Lives = MaxLives;
        LivesText.text = "Lives: " + Lives.ToString();
        attack = gameObject.GetComponent<PlayerAttack>();
        archer = gameObject.GetComponent<ArcherControl>();
        rope = gameObject.GetComponent<RopeSystem>();
        Bow.transform.position = WeaponPoint.transform.position;
        Sword.transform.position = WeaponPoint.transform.position;
        ChangeWeapon(Активное_Оружие);

        if (!PlayerPrefs.HasKey("exp"))
        {
            PlayerPrefs.SetInt("exp", 0);
            PlayerPrefs.Save();
        }
        expText.text = "exp " + PlayerPrefs.GetInt("exp").ToString() + "/" + (100 * Level + Level * 20);
    }

    private void ChangeWeapon(ActiveWeapon активноeOружие)
    {
        if (Активное_Оружие == ActiveWeapon.Лук)
        {
            attack.enabled = false;
            Sword.SetActive(false);
            archer.enabled = true;
            Bow.SetActive(true);
            archer.AimLine.SetActive(true);
            rope.enabled = false;
            rope.ResetRope();
            rope.crosshairSprite.enabled = false;
        }
        if (Активное_Оружие == ActiveWeapon.Меч)
        {
            attack.enabled = true;
            Sword.SetActive(true);
            archer.enabled = false;
            Bow.SetActive(false);
            archer.AimLine.SetActive(false);
            rope.enabled = false;
            rope.ResetRope();
            rope.crosshairSprite.enabled = false;
        }
        if (Активное_Оружие == ActiveWeapon.Гарпун)
        {
            attack.enabled = false;
            archer.enabled = false;
            Bow.SetActive(false);
            Sword.SetActive(false);
            archer.AimLine.SetActive(false);
            rope.enabled = true;
            rope.crosshairSprite.enabled = true;
        }
    }

    /// <summary>
    /// Выполняем действия в методе FixedUpdate, т. к. в компоненте Animator персонажа
    /// выставлено значение Animate Physics = true и анимация синхрflipонизируется с расчетами физики
    /// </summary>
    private void FixedUpdate()
    {
        HitDetected(); // Лучи на нахождение стены
        //определяем, на земле ли персонаж
        isGrounded = Physics2D.OverlapBox(ground.position, new Vector2(System.Convert.ToSingle(0.02), System.Convert.ToSingle(0.1)), groundAngle, whatIsGround);
        //isWallLeft = Physics2D.OverlapBox(wallLeftCheck.position, new Vector2(System.Convert.ToSingle(1.6), System.Convert.ToSingle(0.05)), groundAngle, whatIsGround);
        //isWallRight = Physics2D.OverlapBox(wallRightCheck.position, new Vector2(System.Convert.ToSingle(1.6), System.Convert.ToSingle(0.05)), groundAngle, whatIsGround);
        isPress = Input.GetKey(KeyCode.S);
        //устанавливаем соответствующую переменную в аниматоре
        anim.SetBool("Ground", isGrounded);
        //устанавливаем в аниматоре значение скорости взлета/падения
        anim.SetFloat("vSpeed", rb2d.velocity.y);
        anim.SetBool("Press", isPress && !isSwinging && isGrounded && !isBoxPushig);
        //используем Input.GetAxis для оси Х. метод возвращает значение оси в пределах от -1 до 1.



        //в компоненте анимаций изменяем значение параметра Speed на значение оси Х.
        //приэтом нам нужен модуль значения

        //обращаемся к компоненту персонажа RigidBody2D. задаем ему скорость по оси Х, 
        //равную значению оси Х умноженное на значение макс. скорости
        //Debug.Log(rb2d.velocity.x);
        //если нажали клавишу для перемещения вправо, а персонаж направлен влево
        if ((moveHorizontal != 0) && (Mathf.Sign(moveHorizontal) != Mathf.Sign(transform.localScale.x)))
            //отражаем персонажа вправо
            Flip();
        //приседание

        //uгарпун
        if (!isSwinging)
        {
            rb2d.velocity = new Vector2(moveHorizontal * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
            if (isGrounded && !isBoxPushig)
            {

                if (isGrounded && Input.GetKey(KeyCode.S) && !isSwinging)
                {
                    box = GetComponent<BoxCollider2D>();
                    box.size = new Vector2(System.Convert.ToSingle(0.1409988), System.Convert.ToSingle(0.27));
                    GetComponent<Rigidbody2D>().velocity = new Vector2(moveHorizontal * maxSpeed / 2, GetComponent<Rigidbody2D>().velocity.y);
                }
                else
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(moveHorizontal * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
                    box = this.GetComponent<BoxCollider2D>();
                    box.size = new Vector2(System.Convert.ToSingle(0.14099885), System.Convert.ToSingle(0.4085822));
                }
            }
        }
        if (moveHorizontal < 0f || moveHorizontal > 0f)
        {
            anim.SetFloat("Speed", Mathf.Abs(moveHorizontal));
            if (isSwinging)
            {
                anim.SetBool("IsSwinging", true);
                // 1 - получаем нормализованный вектор направления от игрока к точке крюка
                var playerToHookDirection = (ropeHook - (Vector2)transform.position).normalized;

                // 2 - Инвертируем направление, чтобы получить перпендикулярное направление
                Vector2 perpendicularDirection;
                if (moveHorizontal < 0)
                {
                    perpendicularDirection = new Vector2(-playerToHookDirection.y, playerToHookDirection.x);
                    var leftPerpPos = (Vector2)transform.position - perpendicularDirection * -2f;
                    Debug.DrawLine(transform.position, leftPerpPos, Color.green, 0f);
                }
                else
                {
                    perpendicularDirection = new Vector2(playerToHookDirection.y, -playerToHookDirection.x);
                    var rightPerpPos = (Vector2)transform.position + perpendicularDirection * 2f;
                    Debug.DrawLine(transform.position, rightPerpPos, Color.green, 0f);
                }

                var force = perpendicularDirection * swingForce;
                rb2d.AddForce(force, ForceMode2D.Force);
            }
        }

        if (!isSwinging)
        {
            if (!groundCheck) return;

            isJumping = jumpInput > 0f;
            if (isJumping)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
            }
        }
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("exp") >= 100 * Level + Level * 20)               // увеличение уровня персонажа/ прокачка 
           {
            int x = PlayerPrefs.GetInt("exp") - (100 * Level + Level * 20);
            PlayerPrefs.SetInt("exp", x);
            PlayerPrefs.Save();
            Level++;
            SetCountText();
            if (PlayerPrefs.GetInt("h") == 0) // Swordmen
            {
                MaxLives = MaxLives + Level * 10;
                Lives = MaxLives;
                HealthEnergyBar.use.AdjustCurrentHealth(Lives);
                LivesText.text = "Lives: " + Lives.ToString();
            }
            if (PlayerPrefs.GetInt("h") == 1) // Archer 
            {
                maxSpeed = maxSpeed + 1.1f;
            }

        }

        SetCountText();

        SelectWeapon();
        if (dieCooldown > 0)
        {
            dieCooldown -= Time.deltaTime;
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space) && !isBoxPushig)
        {
            Jump();
        }
        // jumpInput = Input.GetAxis("Jump");
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distance, boxMask);

        if (hit.collider != null && hit.collider.gameObject.tag == "Box" && Input.GetKeyDown(KeyCode.E) && !Input.GetKey(KeyCode.S))
        {
            Box = hit.collider.gameObject;
            Box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
            Box.GetComponent<FixedJoint2D>().enabled = true;
            //box.GetComponent<boxpull>().beingPushed = true;
            isBoxPushig = true;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            Box.GetComponent<FixedJoint2D>().enabled = false;
            isBoxPushig = false;
            //box.GetComponent<boxpull>().beingPushed = false;
        }

        moveHorizontal = Input.GetAxis("Horizontal");
        var halfHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
        groundCheck = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - halfHeight - 0.1f), Vector2.down, 0.025f);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - halfHeight - 0.1f), Vector2.down);
        Restart();
    }
    public void Restart()
    {
        if (Input.GetKeyDown(KeyCode.R)) // рестарт 
        {
            PlayerPrefs.DeleteAll(); // сброс PLayers.Prefas уровня и опыта
            MaxLives = 100;
            Lives = MaxLives;
            HealthEnergyBar.use.AdjustCurrentHealth(Lives);
            HealthEnergyBar.use.AdjustCurrentEnergy(-150f);
            LivesText.text = "Lives: " + Lives.ToString();
            rb2d.position = RespawnPoint.position;
            RSBox.ResetPosition();
            Level = 1;
            PlayerPrefs.SetInt("exp", 0);
            PlayerPrefs.Save();
        }
    }
    private void SelectWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            switch (Активное_Оружие)
            {
                case ActiveWeapon.Меч:
                    Активное_Оружие = ActiveWeapon.Лук;
                    break;
                case ActiveWeapon.Лук:
                    Активное_Оружие = ActiveWeapon.Гарпун;
                    break;
                case ActiveWeapon.Гарпун:
                    Активное_Оружие = ActiveWeapon.Меч;
                    break;
            }
            ChangeWeapon(Активное_Оружие);
        }
    }

    void Awake()
    {
        Активное_Оружие = ActiveWeapon.Меч;
        sprite = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    /// <summary>
    /// Метод для смены направления движения персонажа и его зеркального отражения
    /// </summary>
    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private bool CanDie
    {
        get
        {
            return dieCooldown <= 0f;
        }
    }

    private void Jump()
    {
        //устанавливаем в аниматоре переменную в false
        anim.SetBool("Ground", false);
        //прикладываем силу вверх, чтобы персонаж подпрыгнул
        rb2d.AddForce(new Vector2(0, hightJump));
    }

    public override void ReceiveDamage(int damage)
    {
        if (CanDie)
        {
            dieCooldown = dieRate;
            HealthEnergyBar.use.AdjustCurrentHealth(-damage);
            Lives = Lives - damage;

            LivesText.text = "Lives: " + Lives.ToString();
            rb2d.velocity = Vector3.zero;
            rb2d.AddForce(transform.up * 3.5F, ForceMode2D.Impulse);
            var p = transform.position;
            Instantiate(BloodsEffect, new Vector3(p.x, p.y, p.z), Quaternion.identity);
        }
        if (Lives < 1)
        {
            Lives = MaxLives;
            HealthEnergyBar.use.AdjustCurrentEnergy(1);
            HealthEnergyBar.use.AdjustCurrentHealth(100);
            LivesText.text = "Lives: " + Lives.ToString();
            rb2d.position = RespawnPoint.position;
            RSBox.ResetPosition();
            //RSVaz.ResetPosition();
            //
        }
    }

    public void SetLives(int Count)
    {
        Instantiate(HPRestore, transform.position, Quaternion.identity);
        LivesText.text = "Lives: " + Lives.ToString();
        HealthEnergyBar.use.AdjustCurrentHealth(Lives);
    }

    public void SetCountText()
    {
        coinText.text = "Gold: " + count.ToString();
        expText.text = "exp " + PlayerPrefs.GetInt("exp").ToString() + "/" + (100 * Level + Level * 20);
        levelText.text = "Level " + Level.ToString();

    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;

    //    Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * transform.localScale.x * distance);
    //}

    public void HitDetected()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, BarrierNear, whatIsBarrier);
        RaycastHit2D hitLeft2 = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - 0.23f, transform.position.z), Vector2.left, BarrierNear, whatIsBarrier);
        RaycastHit2D hitLeft3 = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + 0.18f, transform.position.z), Vector2.left, BarrierNear, whatIsBarrier);
        RaycastHit2D hitLeft4 = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - 0.11f, transform.position.z), Vector2.left, BarrierNear, whatIsBarrier);
        RaycastHit2D hitLeft5 = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + 0.09f, transform.position.z), Vector2.left, BarrierNear, whatIsBarrier);
        //RaycastHit2D hitLeftBox = Physics2D.Raycast(transform.position, Vector2.left, -0.12f, whatIsBox);
        Debug.DrawRay(transform.position, BarrierNear * Vector2.left);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y - 0.23f, transform.position.z), BarrierNear * Vector2.left);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 0.18f, transform.position.z), BarrierNear * Vector2.left);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y - 0.11f, transform.position.z), BarrierNear * Vector2.left);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 0.09f, transform.position.z), BarrierNear * Vector2.left);
        if (hitLeft.collider != null)
        {
            if (!groundCheck && moveHorizontal > 0f)
            {
                moveHorizontal = 0;
            }
        }
        if (hitLeft2.collider != null)
        {
            if (!groundCheck && moveHorizontal > 0f)
            {
                moveHorizontal = 0;
            }
        }
        if (hitLeft3.collider != null)
        {
            if (!groundCheck && moveHorizontal > 0f)
            {
                moveHorizontal = 0;
            }
        }
        if (hitLeft4.collider != null)
        {
            if (!groundCheck && moveHorizontal > 0f)
            {
                moveHorizontal = 0;
            }
        }
        if (hitLeft5.collider != null)
        {
            if (!groundCheck && moveHorizontal > 0f)
            {
                moveHorizontal = 0;
            }
        }

        //if (hitLeft3.collider != null)
        //{
        //    if (!groundCheck && moveHorizontal > 0f && GetComponent<Rigidbody2D>().velocity.y < 0)
        //    {
        //        GetComponent<Rigidbody2D>().velocity = new Vector2(maxSpeed * hitLeft3.normal.x, -maxSpeed);
        //    }
        //}

        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, BarrierNear, whatIsBarrier);
        RaycastHit2D hitRight2 = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - 0.23f, transform.position.z), Vector2.right, BarrierNear, whatIsBarrier);
        RaycastHit2D hitRight3 = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + 0.18f, transform.position.z), Vector2.right, BarrierNear, whatIsBarrier);
        RaycastHit2D hitRight4 = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - 0.11f, transform.position.z), Vector2.right, BarrierNear, whatIsBarrier);
        RaycastHit2D hitRight5 = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + 0.09f, transform.position.z), Vector2.right, BarrierNear, whatIsBarrier);
        //RaycastHit2D hitRightBox = Physics2D.Raycast(transform.position, Vector2.right, -0.12f, whatIsBox);
        Debug.DrawRay(transform.position, BarrierNear * Vector2.right);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y - 0.23f, transform.position.z), BarrierNear * Vector2.right);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 0.18f, transform.position.z), BarrierNear * Vector2.right);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y - 0.11f, transform.position.z), BarrierNear * Vector2.right);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 0.09f, transform.position.z), BarrierNear * Vector2.right);

        if (hitRight.collider != null)
        {
            if (!groundCheck && moveHorizontal < 0f)
            {
                moveHorizontal = 0;
            }
        }
        if (hitRight2.collider != null)
        {
            if (!groundCheck && moveHorizontal < 0f)
            {
                moveHorizontal = 0;
            }
        }
        if (hitRight3.collider != null)
        {
            if (!groundCheck && moveHorizontal < 0f)
            {
                moveHorizontal = 0;
            }
        }
        if (hitRight4.collider != null)
        {
            if (!groundCheck && moveHorizontal < 0f)
            {
                moveHorizontal = 0;
            }
        }
        if (hitRight5.collider != null)
        {
            if (!groundCheck && moveHorizontal < 0f)
            {
                moveHorizontal = 0;
            }
        }

    }
}
