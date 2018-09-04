using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Player : Unit
{

    public Text countText;          //Store a reference to the UI Text component which will display the number of pickups collected.
    public Text LivesText;          // Колинчество жизней, оставшихся у Игоря
    private int count;

    //переменная для установки макс. скорости персонажа
    public float maxSpeed = 2f;
    public int Lives = 100;
    private float shootCooldown;
    private float dieCooldown;
    private float shotingRate = 0.45f;
    private float dieRate = 0.3f;
    //ссылка на компонент Transform объекта
    //для определения соприкосновения с землей
    public Transform groundCheck;
    public Transform wallLeftCheck;
    public Transform wallRightCheck;
    //переменная для определения направления персонажа вправо/влево
    private bool isFacingRight = true;
    //ссылка на компонент анимаций
    private Animator anim;
    private Rigidbody2D rb2d;
    private SpriteRenderer sprite;
    private float moveHorizontal;
    private bool isGrounded = false;
    private bool isPress = false;
    public bool isWater = false;
    private bool isWallLeft = false;
    private bool isWallRight = false;
    private float groundAngle = 0;
    //радиус определения соприкосновения с землей
    public double vSpeed;
    private BoxCollider2D box;
    private Arrow arrow;
    public GameObject Sword;
    //ссылка на слой, представляющий землю
    public LayerMask whatIsGround;
    public Transform RespawnPoint;

    public ResetState RSVaz;
    public ResetState RSBox;
    public HealthEnergyBar HE;
    public GameObject[] searchScript;
    public float TimeOff = 3F; // время атаки
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        arrow = Resources.Load<Arrow>("Arrow");
        shootCooldown = 0f;
        dieCooldown = 0f;
        SetCountText();
        LivesText.text = "Lives: " + Lives.ToString();
    }

    /// <summary>
    /// Выполняем действия в методе FixedUpdate, т. к. в компоненте Animator персонажа
    /// выставлено значение Animate Physics = true и анимация синхрflipонизируется с расчетами физики
    /// </summary>
    private void FixedUpdate()
    {
        //определяем, на земле ли персонаж
        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(System.Convert.ToSingle(0.02), System.Convert.ToSingle(0.25)), groundAngle, whatIsGround);
        //isWallLeft = Physics2D.OverlapBox(wallLeftCheck.position, new Vector2(System.Convert.ToSingle(1.6), System.Convert.ToSingle(0.05)), groundAngle, whatIsGround);
        //isWallRight = Physics2D.OverlapBox(wallRightCheck.position, new Vector2(System.Convert.ToSingle(1.6), System.Convert.ToSingle(0.05)), groundAngle, whatIsGround);
        isPress = Input.GetKey(KeyCode.S);
        //устанавливаем соответствующую переменную в аниматоре
        anim.SetBool("Ground", isGrounded);
        //устанавливаем в аниматоре значение скорости взлета/падения
        anim.SetFloat("vSpeed", rb2d.velocity.y);
        anim.SetBool("Press", isPress);
        //используем Input.GetAxis для оси Х. метод возвращает значение оси в пределах от -1 до 1.


        float moveHorizontal = Input.GetAxis("Horizontal");


        //в компоненте анимаций изменяем значение параметра Speed на значение оси Х.
        //приэтом нам нужен модуль значения
        anim.SetFloat("Speed", Mathf.Abs(moveHorizontal));
        //обращаемся к компоненту персонажа RigidBody2D. задаем ему скорость по оси Х, 
        //равную значению оси Х умноженное на значение макс. скорости
        if (isWater == false)
        {
            rb2d.velocity = new Vector2(moveHorizontal * maxSpeed, rb2d.velocity.y);

        }
        else
        {
            float moveVertical = Input.GetAxis("Vertical");
            
            GetComponent<Rigidbody2D>().velocity = new Vector2(moveHorizontal * maxSpeed / 2, moveVertical * maxSpeed/4);
           
        }
        //Debug.Log(rb2d.velocity.x);
        //если нажали клавишу для перемещения вправо, а персонаж направлен влево
        if (moveHorizontal > 0 && !isFacingRight)
            //отражаем персонажа вправо
            Flip();
        //обратная ситуация. отражаем персонажа влево
        else if (moveHorizontal < 0 && isFacingRight)
            Flip();

        //приседание
        if (isGrounded && Input.GetKey(KeyCode.S))
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
        if (Input.GetButtonDown("Fire1")) Shoot();
        if (Input.GetButtonUp ("Fire1")) DontAttack();
        
    }


    private void Update()
    {
        if (shootCooldown > 0)
        {
            shootCooldown -= Time.deltaTime;
        }
        if (dieCooldown > 0)
        {
            dieCooldown -= Time.deltaTime;
        }
        if (isGrounded && Input.GetKeyDown(KeyCode.Space)) Jump();
        if (Input.GetKeyDown(KeyCode.R))
        {
            Lives = 100;
            HE.HPrestart();
            LivesText.text = "Lives: " + Lives.ToString();
            rb2d.position = RespawnPoint.position;
            RSBox.ResetPosition();
        }
    }

    /// <summary>
    /// Метод для смены направления движения персонажа и его зеркального отражения
    /// </summary>
    private void Flip()
    {
        //меняем направление движения персонажа
        isFacingRight = !isFacingRight;
        //получаем размеры персонажа
        Vector3 theScale = transform.localScale;
        //зеркально отражаем персонажа по оси Х
        theScale.x *= -1;
        //задаем новый размер персонажа, равный старому, но зеркально отраженный
        transform.localScale = theScale;
        Sword.transform.localScale = new Vector3(-Sword.transform.localScale.x, -Sword.transform.localScale.y, Sword.transform.localScale.z);
    }
    private void Shoot()
    {

            if (isFacingRight)
            {
                Vector3 position = transform.position; position.y += 0.1F; position.x += 0.2F;
                Arrow newArrow = Instantiate(arrow, position, arrow.transform.rotation) as Arrow;
                newArrow.Direction = newArrow.transform.right;
                Sword.transform.eulerAngles = new Vector3(0, 0, 180);
                //Sword.transform.position = position;
            }
            if (!isFacingRight)
            {
                Vector3 position = transform.position; position.y += 0.1F; position.x -= 0.2F;
                Arrow newArrow = Instantiate(arrow, position, arrow.transform.rotation) as Arrow;
                newArrow.Direction = newArrow.transform.right * (sprite.flipX ? 1 : -1);
                Sword.transform.eulerAngles = new Vector3(0, 0, 0);
                //Sword.transform.position = position;

                Vector3 theScaleArrow = newArrow.transform.localScale;
                theScaleArrow.x *= -1;
                newArrow.transform.localScale = theScaleArrow;
            }
        
    }
    public void DontAttack() // отмена стрельбы 
    {
        Sword.transform.eulerAngles = new Vector3(0, 0, 270);
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
    public override void ReceiveDamage()
    {
        if (CanDie)
        {
            dieCooldown = dieRate;
            HE.HPdown();
            Lives= Lives + HE.RandomR;
            LivesText.text = "Lives: " + Lives.ToString();
            rb2d.velocity = Vector3.zero;
            rb2d.AddForce(transform.up * 3.5F, ForceMode2D.Impulse);
        }
        if (Lives < 0)
        {
            Lives = 100;
            HE.HPrestart();
            LivesText.text = "Lives: " + Lives.ToString();
            rb2d.position = RespawnPoint.position;
            RSBox.ResetPosition();
            //RSVaz.ResetPosition();
            //
        }
    }
    // чистка консоли
    public static void ClearLog()
    {
        var assembly = System.Reflection.Assembly.GetAssembly(typeof(UnityEditor.ActiveEditorTracker));
        var type = assembly.GetType("UnityEditorInternal.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
    void OnTriggerStay2D(Collider2D Player)
    {
        //Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
        if (Player.gameObject.CompareTag("Gold"))
        {
            Player.gameObject.SetActive(false);
            //Add one to the current value of our count variable.
            count = count + Random.Range(40, 100); ;
            //Update the currently displayed count by calling the SetCountText function.
            SetCountText();
        }
    }
    // Вход в воду
    // Если Игорь заходит в воду то активируется движение в воде
    void OnTriggerEnter2D(Collider2D Player)
    {
        if (Player.gameObject.tag == "Water")
        {
            isWater = true;
            print("Igor' come in water");
        }        
    }
    // Выход из воды
    // Если Игорь выходит из воды то движение в воде отключается
    // для того чтобы не было проблем с выходом Игоря из воды был добавлен импульс
    /*void OnTriggerExit2D(Collider2D Player)
    {
        if (Player.gameObject.CompareTag("Water"))
        {
            isWater = false;
            //rb2d.velocity = Vector3.zero;
            rb2d.AddForce(transform.up * 0.8F, ForceMode2D.Impulse);
            print("Igor' leave water");
        }        
    }*/


    //This function updates the text displaying the number of objects we've collected and displays our victory message if we've collected all of them.
    void SetCountText()
    {
        //Set the text property of our our countText object to "Count: " followed by the number stored in our count variable.
        countText.text = "Gold: " + count.ToString();
        //LivesText.text = "Lives: " + Lives.ToString();
    }
}