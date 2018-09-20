using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {
    public bool InHand = false;
    [Range(1, 20)]
    public int damage = 1;
    private Collider2D ourCollider;
    private float timer;
    [Range(0.1f, 2)]
    public float delayTimer = 1;

	// Use this for initialization
	void Start ()
    {
        ourCollider = transform.GetComponent<Collider2D>();
        timer = delayTimer;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (timer < delayTimer)
        {
            timer += Time.deltaTime;
        }
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

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if ((Input.GetKeyDown(KeyCode.E)) && (timer >= delayTimer))
            {
                Player player = col.GetComponent<Player>();
                GameObject OldSword = player.Sword;
                Sword OldSwordScript = OldSword.GetComponent<Sword>();
                gameObject.SetActive(OldSword.activeInHierarchy);
                player.Sword = gameObject;

                OldSword.transform.parent = null;
                OldSwordScript.InHand = false;
                OldSwordScript.Activation(OldSwordScript.InHand);
                OldSword.SetActive(true);

                gameObject.transform.parent = col.transform;
                gameObject.transform.position = player.WeaponPoint.transform.position;
                InHand = true;
                Activation(InHand);
                timer = 0;
            }
        }
    }
}
