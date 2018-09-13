using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Скрипт по работе рычага (lever'а)

// Принцип работы:
// void Update
// Если рычаг сработал, ему нужно время на восстановление. Это восттановление происходит здесь.

// void OnTriggerStay2D 
// Идут проверки:
//      - На нахождение Игрока в объекте lever (Рычаг должен быть триггерным)
//      - На наличие зависимых объектов
//      - На допуступность рычага
// Если все условия выполнены, то ожидается пока Игрок не нажмет на клавишу действия (клавиша "E").
// В зависимости от типа работы рычага, нужно чтобы Игрок нажал на клавишу действия или удерживал ее какое-то время (условие срабатывания).
// В случае выполенения условия срабатывания, инициализируется void ChangeGOStat

// void ChangeGOStat
// Происходит изменение статуса состояния (active) объектов на противоположный

// Скрипт готов к работе


public class Lever : MonoBehaviour {
    public enum LeverType                       // Перечень типов работы рычага
    {
        Moment,                                 // Моментальное срабатываение
        Prolong                                 // Продолжительное действие
    }
    public LeverType Type;                      // Выбранный тип работы рычага
    public GameObject[] DependGO;               // Массив зависимых объектов
    public float Timer = 5;                     // Таймер на перезагрузку срабатывания рычага
    public float EnanbleTime = 5;               // Время, через которое рычаг сможет снова сработать
    [Header("Для продолжительного действия")]
    public float ProlongTime = 2.5f;               // Время, которое нужно взаимодействовать с рычагом
    public float ProlongTimer = 0;              // Таймер на взаимодействие с рычагом
    public Lift Lift;
    private void Update()
    {
        if (Timer < EnanbleTime)
        {
            Timer += Time.deltaTime;
        }

    }

    public void OnTriggerStay2D(Collider2D coll)
    {
        if ((coll.gameObject.tag == "Player") && (DependGO != null) && (Timer >= EnanbleTime))
        {
            if (Type == LeverType.Moment)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {                    
                    ChangeGOStat(DependGO);
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    Timer = 0;
                }
            }
            if (Type == LeverType.Prolong)
            {
                if (Input.GetKey(KeyCode.E))
                {                    
                    if (ProlongTimer < ProlongTime)
                    {
                        ProlongTimer += Time.deltaTime;            
                    }
                    else
                    {
                        ChangeGOStat(DependGO);
                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                        ProlongTimer = 0;
                        Timer = 0;
                    }                    
                }
                else
                {
                    ProlongTimer = 0;
                    //Lift.GetComponent<Rigidbody2D>().gravityScale = 0;
                }
            }
        }
    }

    public void ChangeGOStat(GameObject[] GO)
    {
        //var time = 5.0f;
        /* while (time > 0)
         {
             time -= Time.deltaTime;           
         }*/
        //Debug.Log("Дверь!");
        for (int i = 0; i <= GO.Length - 1; i++)
        {
            GO[i].gameObject.SetActive(!(GO[i].gameObject.activeSelf));
        }
    }
}