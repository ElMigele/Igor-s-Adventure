using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour {
    public bool InHand = false;                     // Лук в руках игрока?
    private Collider2D ourCollider;                 // Коллайдер для подбора лука
    [Range(1, 20)] public float minVel = 1;         // Минимальная скорость пуска стрелы
    [Range(1, 20)] public float maxVel = 20;        // Максимальная скорость пуска стрелы
    [Range(5, 15)] public float delVel = 10;        // Скорость изменения скорости пуска стрелы (Скорость натяжения)
    [Range(1, 15)] public float AttackDelay = 5.0f; // Время перезарядки    
    private float PickUpTimer;                      // Таймер подбора
    [Range(0.1f, 2)] public float PickUpDelay = 1;  // Время между подборами

    // Use this for initialization
    void Start ()
    {
        ourCollider = transform.GetComponent<Collider2D>();
        PickUpTimer = PickUpDelay;
    }

    void Activation(bool InHand)
    {
        if (InHand)
        {
            ourCollider.enabled = false;
        }
        else
        {
            ourCollider.enabled = true;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (PickUpTimer < PickUpDelay)
        {
            PickUpTimer += Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if ((Input.GetKeyDown(KeyCode.E)) && (PickUpTimer >= PickUpDelay))
            {
                Player player = col.GetComponent<Player>();
                GameObject OldBow = player.Bow;
                Bow OldBowScript = OldBow.GetComponent<Bow>();
                gameObject.SetActive(OldBow.activeInHierarchy);
                player.Bow = gameObject;

                OldBow.transform.parent = null;
                OldBowScript.InHand = false;
                OldBowScript.Activation(OldBowScript.InHand);
                OldBow.SetActive(true);

                gameObject.transform.parent = col.transform;
                gameObject.transform.position = player.WeaponPoint.transform.position;
                ArcherControl archer = col.GetComponent<ArcherControl>();
                archer.BowScript = gameObject.GetComponent<Bow>();
                InHand = true;
                Activation(InHand);
                PickUpTimer = 0;
                archer.delayTimer = AttackDelay;
            }
        }
    }
}
