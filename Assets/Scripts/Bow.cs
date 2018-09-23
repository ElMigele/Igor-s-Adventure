using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour {
    public bool InHand = false;
    [Range(1, 20)] public float minVel = 1;
    [Range(1, 20)] public float maxVel = 20;
    [Range(5, 15)] public float delVel = 10;
    private Collider2D ourCollider;
    private float timer;
    [Range(0.1f, 2)] public float delayTimer = 1;

    // Use this for initialization
    void Start ()
    {
        ourCollider = transform.GetComponent<Collider2D>();
        timer = delayTimer;
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
        if (timer < delayTimer)
        {
            timer += Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if ((Input.GetKeyDown(KeyCode.E)) && (timer >= delayTimer))
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
                timer = 0;
            }
        }
    }
}
