using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class Player : Unit
{
    [Header("Ссылки на интерфейс")]
    public Text countText;                          // Количество золота, имеющееся у игрока
    public Text LivesText;                          // Количество жизней, оставшихся у игрока
    public HealthEnergyBar HE;                      // Полосы здороья и силы натяжения лука у героя
    [Header("Параметры игрока")]
    [Range(0.1f, 5)]
    public float maxSpeed = 1f;     // Максимальная скорость игрока
    [Range(1, 100)]
    public int MaxLives = 100;      // Максимальное количество жизней игрока
    public int Lives;                               // Количество жизней
    [NonSerialized]
    public int count;                               // Счетчик золота
    public int RandomR;                             // Случайное число
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

    private float shootCooldown;
    private float dieCooldown;
    private float shotingRate = 0.45f;
    private float dieRate = 0.3f;

    [Header("Проверки на состояние")]
    public bool isGrounded = false;                 // Стоит на земле
    public LayerMask whatIsGround;                  // Cсылка на слой, представляющий землю
    public LayerMask whatIsLeft;
    public LayerMask whatIsRight;
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

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        shootCooldown = 0f;
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
        }
        if (Активное_Оружие == ActiveWeapon.Меч)
        {
            attack.enabled = true;
            Sword.SetActive(true);
            archer.enabled = false;
            Bow.SetActive(false);
            archer.AimLine.SetActive(false);
            rope.enabled = false;
        }
        if (Активное_Оружие == ActiveWeapon.Гарпун)
        {
            attack.enabled = false;
            archer.enabled = false;
            Bow.SetActive(false);
            Sword.SetActive(false);
            archer.AimLine.SetActive(false);
            rope.enabled = true;
        }
    }

    /// <summary>
    /// Выполняем действия в методе FixedUpdate, т. к. в компоненте Animator персонажа
    /// выставлено значение Animate Physics = true и анимация синхрflipонизируется с расчетами физики
    /// </summary>
    private void FixedUpdate()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, -0.15f, whatIsGround);
        RaycastHit2D hitLeft2 = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - 0.23f, transform.position.z), Vector2.left, -0.15f, whatIsGround);
        RaycastHit2D hitLeft3 = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + 0.23f, transform.position.z), Vector2.left, -0.15f, whatIsGround);
        Debug.DrawRay(transform.position, -0.15f * Vector2.left);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y - 0.23f,  transform.position.z), -0.15f * Vector2.left);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 0.23f, transform.position.z), -0.15f * Vector2.left);
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
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, -0.15f, whatIsGround);
        RaycastHit2D hitRight2 = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - 0.23f, transform.position.z), Vector2.right, -0.15f, whatIsGround);
        RaycastHit2D hitRight3 = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + 0.23f, transform.position.z), Vector2.right, -0.15f, whatIsGround);
        Debug.DrawRay(transform.position, -0.15f * Vector2.right);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y - 0.23f, transform.position.z), -0.15f * Vector2.right);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 0.23f, transform.position.z), -0.15f * Vector2.right);

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
        //определяем, на земле ли персонаж
        isGrounded = Physics2D.OverlapBox(ground.position, new Vector2(System.Convert.ToSingle(0.02), System.Convert.ToSingle(0.1)), groundAngle, whatIsGround);
        //isWallLeft = Physics2D.OverlapBox(wallLeftCheck.position, new Vector2(System.Convert.ToSingle(1.6), System.Convert.ToSingle(0.05)), groundAngle, whatIsGround);
        //isWallRight = Physics2D.OverlapBox(wallRightCheck.position, new Vector2(System.Convert.ToSingle(1.6), System.Convert.ToSingle(0.05)), groundAngle, whatIsGround);
        isPress = Input.GetKey(KeyCode.S);
        //устанавливаем соответствующую переменную в аниматоре
        anim.SetBool("Ground", isGrounded);
        //устанавливаем в аниматоре значение скорости взлета/падения
        anim.SetFloat("vSpeed", rb2d.velocity.y);
        anim.SetBool("Press", isPress && !isSwinging && isGrounded);
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
 
        if (Input.GetButtonDown("Fire1")) Shoot();
        if (Input.GetButtonUp("Fire1")) DontAttack();
        //uгарпун
        if (!isSwinging)
        {
            rb2d.velocity = new Vector2(moveHorizontal * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
            if (isGrounded)
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
        SelectWeapon();

        if (shootCooldown > 0)
        {
            shootCooldown -= Time.deltaTime;
        }
        if (dieCooldown > 0)
        {
            dieCooldown -= Time.deltaTime;
        }
        //if (isGrounded && Input.GetKeyDown(KeyCode.Space)) Jump();
        jumpInput = Input.GetAxis("Jump");
        moveHorizontal = Input.GetAxis("Horizontal");
        var halfHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
        groundCheck = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - halfHeight - 0.1f), Vector2.down, 0.025f);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - halfHeight - 0.1f), Vector2.down);
        if (Input.GetKeyDown(KeyCode.R))
        {
            Lives = MaxLives;
            HealthEnergyBar.use.AdjustCurrentHealth(100);
            HealthEnergyBar.use.AdjustCurrentEnergy(-150);
            LivesText.text = "Lives: " + Lives.ToString();
            rb2d.position = RespawnPoint.position;
            RSBox.ResetPosition();
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

    private void Shoot()
    {
        if (Mathf.Sign(transform.localScale.x) > 0)
        {
            Sword.transform.eulerAngles = new Vector3(0, 0, 180);
        }
        else
        {
            Sword.transform.eulerAngles = new Vector3(0, 0, 90);
        }
    }

    public void DontAttack()
    {
        if (Mathf.Sign(transform.localScale.x) > 0)
            Sword.transform.eulerAngles = new Vector3(0, 0, 270);
        else
            Sword.transform.eulerAngles = new Vector3(0, 0, 180);
    }

    public bool CanAttack
    {
        get
        {
            return shootCooldown <= 0f;
        }
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
        rb2d.AddForce(new Vector2(0, 220));
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
        HealthEnergyBar.use.AdjustCurrentHealth(-Count);
    }

    public void SetCountText()
    {
        countText.text = "Gold: " + count.ToString();
    }
}