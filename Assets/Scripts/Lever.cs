using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Скрипт по работе рычага (lever'а)
//
// Принцип работы:
// void OnTriggerStay2D 
// Идет проверка на нахождение Игрока в объекте lever (Рычаг должен быть триггерным)
// Если Игрок находится в объекте, то ожидается пока Игрок не нажмет на клавишу действия (клавиша "E")
// В случа нажатия кнопки, инициализируется void ChangeGOStat
// void ChangeGOStat
// Происходит изменение статуса состояния (active) объекта на противоположный
//
// Скрипт готов к работе
// Пробема в одноразовости скрипта:
// При нахождении Игрока и большом количестве нажатии клавиши действия, происходит срабатываение только на первое нажатие
// Чтобы изменить состояние управляемого объекта еще раз, необходимо выйти из объекта рычага, войти в него снова и нажать клавишу действия


public class Lever : MonoBehaviour {
    public GameObject GO;

    public void OnTriggerStay2D(Collider2D coll)
    {

        if (coll.gameObject.tag == "Player")
        {
            //Debug.Log("На рычаге!");
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Debug.Log("Нажал Е!");
                // Проверка на наличие Объекта
                if (GO != null) ChangeGOStat(GO);
            }
        } 
        
    }
    // Функция по изменению статуса объекта на противоположный
    public void ChangeGOStat(GameObject GO)
    {       
        //var time = 5.0f;
        /* while (time > 0)
         {
             time -= Time.deltaTime;           
         }*/
        //Debug.Log("Дверь!");
        GO.gameObject.SetActive(!(GO.gameObject.activeSelf));
    }
}