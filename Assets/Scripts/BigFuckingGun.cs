using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFuckingGun : MonoBehaviour
{
    private GameObject RotationPoint;                       // Точка вращения пушки
    private GameObject Cannon;                              // Пушка
    private GameObject VisionZone;                          // Зона видимости
    private EnemyVisibility CannonVision;                   // Скрипт видимости
    public enum Angle
    {
        ConstantAngle,      // Постоянный угол
        AutoChanging,       // Автовращение
        Control             // Управляемый угол
    }
    [Header("Вращение")]
    public Angle AngleChangeType = Angle.ConstantAngle;     // Тип изменения угла
    public float angle = 0;                                 // Начальный угол
    [Range(0.1f, 10)]
    public float AngleChangeSpeed = 10;                     // Скорость изменения угла
    public bool RotationUp = true;                          // Проверка на вращение вверх
    public Vector2 AngleDiap = new Vector2(-45, 45);        // Ограничения вращения угла
    public GameObject[] Buttons4Angle;                      // Массив кнопок на активацию вращения
    private Button[] Buttons4AngleScripts;                  // Массив скриптов на активацию вращения
    public enum Attack
    {
        Constant,           // Постоянная стрельба
        ByTrigger,          // Стрельба при нахождении в зоне видимости
        Control             // Управляемый выстрел
    }
    [Header("Стрельба")]
    public Attack AttackType = Attack.Constant;             // Тип стрельбы
    public GameObject[] Buttons4Attack;                     // Массив кнопок на активацию стрельбы
    private Button[] Buttons4AttackScripts;                 // Массив скриптов на активацию стрельбы
    public GameObject Packet;                               // Снаряд
    [Range(0.1f, 100)]
    public float InitVelocity = 50;                         // Начальная скорость снаряда
    public float AttackTimer;                               // Таймер стрельбы
    [Range(1, 100)]
    public float AttactDelay = 50;                          // Задерка на стрельбу
    
    private GameObject Platform;                            // Платформа
    public enum PlatformMovement
    {
        Idle,           // Покой
        Patrol,         // Автоматическое движение
        Control         // Управлемое движение
    }
    [Header("Перемещение")]
    public PlatformMovement PlatformMovementType = PlatformMovement.Idle;
    public GameObject[] Buttons4Motion;                     // Массив кнопок на активацию перемещения
    private Button[] Buttons4MotionScripts;                 // Массив скриптов на активацию перемещения
    [Range(0.1f, 10)]
    public float PlatformSpeed = 10;                        // Скорость платформы
    public int NextPointID = 1;                             // Номер точки, к которой идет перемещение
    public GameObject Point0;                               // Точка 0
    public GameObject Point1;                               // Точка 1

	// Use this for initialization
	void Start ()
    {
        CallGameObjects();
        AttackTimer = AttactDelay;
        CallControlObjects();
	}

    private void CallGameObjects()
    {
        Platform = GameObject.Find("/BFG/CannonFrame");
        RotationPoint = GameObject.Find("/BFG/CannonFrame/RotationPoint");
        Cannon = GameObject.Find("/BFG/CannonFrame/RotationPoint/Cannon");
        VisionZone = GameObject.Find("/BFG/CannonFrame/RotationPoint/Cannon/VisionZone");
    }

    private void CallControlObjects()
    {
        if (AngleChangeType == Angle.Control)
        {
            Buttons4AngleScripts = new Button[Buttons4Angle.Length];
            for (int i = 0; i < Buttons4AngleScripts.Length; i++)
            {
                Buttons4AngleScripts[i] = Buttons4Angle[i].GetComponent<Button>();
            }
        }

        if (AttackType == Attack.Control)
        {
            Buttons4AttackScripts = new Button[Buttons4Attack.Length];
            for (int i = 0; i < Buttons4AttackScripts.Length; i++)
            {
                Buttons4AttackScripts[i] = Buttons4Attack[i].GetComponent<Button>();
            }
        }

        if (PlatformMovementType == PlatformMovement.Control)
        {
            Buttons4MotionScripts = new Button[Buttons4Motion.Length];
            for (int i = 0; i < Buttons4MotionScripts.Length; i++)
            {
                Buttons4MotionScripts[i] = Buttons4Motion[i].GetComponent<Button>();
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {
        PlatformMove();
        AngleChange();
        CanonFire();
	}
        
    private void PlatformMove()
    {
        if (PlatformMovementType == PlatformMovement.Patrol)
        {
            PlatformMovePlus();            
        }

        if (PlatformMovementType == PlatformMovement.Control)
        {
           if (CheckButtons(Buttons4Motion, Buttons4MotionScripts))
           {
               PlatformMovePlus();
           }
        }

        if (PlatformMovementType == PlatformMovement.Idle)
        {
            // Ничего не делать, состояние покоя
        }
    }

    private bool CheckButtons(GameObject[] Buttons, Button[] ButtonsScripts)
    {
        bool Active = false;
        for (int i = 0; i < Buttons.Length; i++)
        {
            if (ButtonsScripts[i].Active == false)
            {
                Active = false;
                break;
            }
            else
            {
                Active = true;
            }
        }
        return Active;
    }

    private void PlatformMovePlus()
    {
        Transform Point = Point0.transform;
        if (NextPointID == 1)
        {
            Point = Point1.transform;
        }
        if (Vector3.Distance(Platform.transform.position, Point.position) != 0)
        {
            Platform.transform.position = Vector3.MoveTowards(Platform.transform.position, Point.position, PlatformSpeed * Time.deltaTime);
        }
        else
        {
            if (NextPointID == 0)
            {
                NextPointID = 1;
            }
            else
            {
                NextPointID = 0;
            }
        }
    }

    private void AngleChange()
    {
        if (AngleChangeType == Angle.AutoChanging)
        {
            AngleChangePlus();
        }

        if (AngleChangeType == Angle.Control)
        {
            if (CheckButtons(Buttons4Angle, Buttons4AngleScripts))
            {
                AngleChangePlus();
            }
        }

        if (AngleChangeType == Angle.ConstantAngle)
        {
            // Ничего не делать - состояние покоя
        }
    }

    private void AngleChangePlus()
    {
        if (RotationUp)
        {
            angle += AngleChangeSpeed * Time.deltaTime;
        }
        else
        {
            angle -= AngleChangeSpeed * Time.deltaTime;
        }
        RotationPoint.transform.eulerAngles = new Vector3(0, 0, angle);
        if ((RotationUp == true) && (angle >= AngleDiap.y))
        {
            RotationUp = false;
        }
        //Debug.Log((angle <= AngleDiap.x) + ", Угол: " + angle + ", Ограничение: " + (360 + AngleDiap.x));
        if ((RotationUp == false) && (angle <= AngleDiap.x))
        {
            RotationUp = true;
        }

    }

    private void CanonFire()
    {
        if (AttackType == Attack.Constant)
        {
            CannonFirePlus();
        }
        if (AttackType == Attack.ByTrigger)
        {
            CannonVision = VisionZone.GetComponentInChildren<EnemyVisibility>();
            bool InVision = CannonVision.InVisibilityZone;
            if (InVision)
            {
                CannonFirePlus();
            }
        }
        if (AttackType == Attack.Control)
        {
            if (CheckButtons(Buttons4Attack, Buttons4AttackScripts))
            {
                CannonFirePlus();
            }
        }
    }

    private void CannonFirePlus()
    {
        if (AttackTimer >= AttactDelay)
        {
            Instantiate(Packet, Cannon.transform.position, RotationPoint.transform.rotation);
            AttackTimer = 0;
        }
        else
        {
            AttackTimer += Time.deltaTime;
        }
    }
}
