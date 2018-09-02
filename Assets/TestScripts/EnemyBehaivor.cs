using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaivor : MonoBehaviour {
    // Скрипт по поведению врагов
    //
    // В скрипте прописаны варианты поведения врагов - их передвижение и реакция на игрока
    public GameObject Arrow;    // Стрела

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            Debug.Log("Игрок в зоне видимости!");
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            Debug.Log("Игрок в зоне видимости!");
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            Debug.Log("Игрок в зоне видимости!");
    }
}/*
public abstract class UnityBase : GameObjectBase
{
    public List<transform> VisiblePoints;
    public static bool IsVisibleUnit<T>(T unit, Transform from, float angle, float distance, Latermask mask)
    {
        bool result = false;
        if (unit != null)
        {
            foreach (Transform visibleOint in unit.VisiblePoints)
            {
                if (IsVisibleObject(from, VisiblePoints.position, unit.gameObject, angle, distance, mask))
                {
                    result = true;
                    break;
                }
            }
        }
        return result;
    }
    public static bool IsVisibleObject(transform from, Vector3 point, GameObject target, float angle, float distance, Layermask mask)
    {
        bool result = false;
        if (IsAvaiblePoint(from, poin, angle, distance))
        {
            Vector3 direction = point - from.position;
            Ray ray = new Ray(from.poition, direction);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance, mask.value))
            {
                if (hit.collider.gameObject == target)
                {
                    result = true;
                }
            }
        }
        return result;
    }
}*/