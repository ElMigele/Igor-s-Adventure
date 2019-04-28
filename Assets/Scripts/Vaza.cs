using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vaza : Unit
{
    public float Lives = 1; 
    public GameObject Gold;
    public GameObject FragmentEffect;
    public int costExperience = 4;         // Дает опыта за свое уничтожение
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
    }

    public override void ReceiveDamage(int damage)
    {
        Lives -= damage;
        if (Lives <= 0)
        {
            var p = transform.position;
            Instantiate(FragmentEffect, new Vector3(p.x, p.y, p.z), Quaternion.identity);
            int count = Random.Range(0, 2);
            for (int i = 0; i < count; i++)
            {
                Instantiate(Gold, new Vector3(p.x, p.y, p.z), Quaternion.identity);
            }
            PlayerPrefs.SetInt("exp", PlayerPrefs.GetInt("exp") + costExperience/2);
            PlayerPrefs.Save();
            Destroy(gameObject);
        }
    }
}
