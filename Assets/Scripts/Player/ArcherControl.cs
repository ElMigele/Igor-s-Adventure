﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherControl : MonoBehaviour {
    [Header("Объекты")]
    public GameObject Arrow;            // Стрела, которую наш герой выпускает
    public GameObject AimLine;          // Линия прицеливания
    [Header("Параметры")]
    public float Velocity;              // Скорость с которой будет лететь стрела (пересылается в ArrowMovement)    
    public float deltaVel = 10.0f;      // Приращение скорости
    public Vector2 mouseMotion;         // Направление мыши
    public float delayTimer = 5.0f;     // Счетчик времени между выстрелами
    public float mouseSens = 5.0f;      // Чувствиельность мыши
    public Vector2 AimAngleDiap =       // Диапазон минимального и максимального 
                new Vector2(55, 90);    //          значения угла прицеливания
    private Vector2[] AimPoints;        // Пара точек для определения угла стрельбы
    public float curAngle;              // Текущий угол
    private Vector3 Forward;            // Вектор, смотрящий вперед (нужен для расчета направления прицела)
    bool ChangeDirection = false;       // Проверка на смену направления
    public Vector2 Len2Vel =
                new Vector2(1, 11);     // значение, при котором скорость стрелы будет максимальной
    public HealthEnergyBar HE;
    [HideInInspector]public Bow BowScript;
    void Start ()
    {

        BowScript = gameObject.GetComponentInChildren<Bow>();
        Velocity = BowScript.minVel;
        Forward = 3*Vector3.right;
    }
	
	// Update is called once per frame
	void Update ()
    {
        CheckScale();
        //transform.localPosition += new Vector3(Input.GetAxis("Horizontal"), 0, 0) * Time.deltaTime;
        AimLine.transform.localPosition = transform.position;
        if (Input.GetMouseButtonDown(1))
        {// Если идет нажатие пр. кнопки мыши, то генерируются точки для определения направления
            AimPoints = new Vector2[2];
            AimPoints[0] = Vector2.zero;
            AimPoints[1] = Forward;
        }
        ChargeAndFire();
        if (Input.GetMouseButtonUp(1))
        {
            CheckScale();
        }
    }

    private void CheckScale()
    {
        if (Mathf.Sign(transform.localScale.x) != Mathf.Sign(Forward.x))
        {
            Forward = -Forward;
        }
    }

    private void ChargeAndFire()
    {
        //AimPosition = new Vector3(transform.position.x, transform.position.y);
        if (Input.GetMouseButton(1))
        {
            AimLine.gameObject.SetActive(true);
            mouseMotion.x += Input.GetAxis("Mouse X") * mouseSens;
            mouseMotion.y += Input.GetAxis("Mouse Y") * mouseSens;
            AimPoints[0] += mouseMotion;
            /*
            // Углы допустимого диапазона
            bool DiapCheckMin = (AimLine.transform.eulerAngles.z < 0   || AimLine.transform.eulerAngles.z > AimAngleDiap.x);
            bool DiapCheckMax = (AimLine.transform.eulerAngles.z > 360 || AimLine.transform.eulerAngles.z < AimAngleDiap.y);
            if (!(!DiapCheckMin || !DiapCheckMax))
            {
                if (mouseMotion < 0)
                {// Движение вниз
                    AimLine.transform.eulerAngles = new Vector3(0, 0, AimAngleDiap.y);
                }
                else
                {// Движение вверх
                    AimLine.transform.eulerAngles = new Vector3(0, 0, AimAngleDiap.x);
                }
                mouseMotion = 0;
            }*/

            /*// Расчет угла прицела и его задание через две точки пространства
            float angle = Vector2.Angle(Vector2.right, AimPoints[1] - AimPoints[0]);
            if (AimPoints[0].y > AimPoints[1].y)
            {// Задание правильного знака угла, он зависит от того, какая точка выше
                angle = -angle;
            }
            // Разворот игрока при стрельбе "за спину"
            bool Flip = ((Mathf.Abs(angle) > 91) && (transform.localScale.x > 0) || (Mathf.Abs(angle) < 89) && (transform.localScale.x < 0));
            if (Flip)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                //Forward = -Forward;
                ChangeDirection = !ChangeDirection;
                //angle = Vector2.Angle(Forward, AimPoints[1] - AimPoints[0]);
            }
            //Debug.Log("1: " + AimPoints[0] + ", 2: " + AimPoints[1] + ", Угол: " + angle.ToString("0.0") + ", Сторона: " + transform.localScale.x);
            AimLine.transform.eulerAngles = new Vector3(0, 0, angle);
            */
            // Расчет угла прицела и его задание через точку положения мыши            
            var facingDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            var aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
            var aimAngle2 = aimAngle * Mathf.Rad2Deg;
            Debug.Log(aimAngle2);
            if (aimAngle < 0f)
            {
                aimAngle = Mathf.PI * 2 + aimAngle;
            }
            bool Flip = ((Mathf.Abs(aimAngle2) > 91) && (transform.localScale.x > 0) || (Mathf.Abs(aimAngle2) < 89) && (transform.localScale.x < 0));
            if (Flip)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);                
            }
            AimLine.transform.rotation = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg);
            var aimAngle3 = aimAngle - 89.6f;
            var aimAngle4 = 180;
            BowScript.transform.rotation = Quaternion.Euler(aimAngle4, aimAngle4, aimAngle3 * Mathf.Rad2Deg);

            /*// Стрельба на отпускаение правой клавиши
            if ((Vector2.Distance(AimPoints[0], AimPoints[1]) >= Len2Vel.y) ||
                (Vector2.Distance(AimPoints[0], AimPoints[1]) <= Len2Vel.x))
            {// Скорость при расстоянии за границами диапазона
                if (Vector2.Distance(AimPoints[0], AimPoints[1]) >= Len2Vel.y)
                    Velocity = velDiap.y;
                else
                    Velocity = velDiap.x;
            }
            else
            {// Расчет скорости в диапазоне
                 Velocity = Vector2.Distance(AimPoints[0], AimPoints[1]) * velDiap.y / Len2Vel.y;
            }
            Debug.Log("Растояние: " + Vector2.Distance(AimPoints[0], AimPoints[1]).ToString("0.0") + ", Скорость: " + Velocity.ToString("0.0"));
            if (Input.GetMouseButtonUp(0))
            {
                Instantiate(Arrow, AimPosition, AimLine.transform.rotation);
                delayTimer = 0;
            }*/
            // Стрельба на левой клавише мыши
            bool prepareFire = Input.GetMouseButton(0);
   
            if (delayTimer >= BowScript.AttackDelay)
            {
                if (prepareFire)
                {
                    if (Velocity < BowScript.maxVel)
                    {

                        Velocity += BowScript.delVel * Time.deltaTime;
                        Debug.Log("Скорость: " + Velocity);
                        HealthEnergyBar.use.AdjustCurrentEnergy(Velocity * Time.deltaTime * 15);
                    }

                    else
                    {
                        Velocity = BowScript.maxVel;
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    Instantiate(Arrow, AimLine.transform.position, AimLine.transform.rotation);
                    delayTimer = 0;
                    HealthEnergyBar.use.AdjustCurrentEnergy(-150);
                }
            }
        }
        else
        {
            /*if (Input.GetMouseButtonUp(0))
            {
                Instantiate(Arrow, AimPosition, AimLine.transform.rotation);
                delayTimer = 0;
            }*/

            if (ChangeDirection)
            {
                //AimLine.transform.localScale = new Vector3(-AimLine.transform.localScale.x, AimLine.transform.localScale.y, AimLine.transform.localScale.z);
                //AimPoints[1] = -AimPoints[1];
                ChangeDirection = !ChangeDirection;
            }            
            AimLine.transform.Rotate(Vector3.forward, -AimLine.transform.localEulerAngles.z);
            BowScript.transform.Rotate(Vector3.forward, -AimLine.transform.localEulerAngles.z);
            AimLine.gameObject.SetActive(false);
        }
        mouseMotion = Vector2.zero;
        delayTimer += Time.deltaTime; 
    }
}
