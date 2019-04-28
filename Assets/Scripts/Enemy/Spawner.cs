using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Unit
{
        public float Lives = 3;
        public GameObject Item;
        public int costExperience = 20;         // Дает опыта за свое уничтожение
        [SerializeField]
        private GameObject enemyPrefab; 
        private GameObject _enemy; // Закрытая переменная для слежения за экземпляром врага в сцене.
        void Start()
        {
        }
    // Update is called once per frame
    void Update()
    {
        if (_enemy == null)
        {
            var p = transform.position;
            _enemy = Instantiate(enemyPrefab, new Vector3(p.x, p.y, p.z), Quaternion.identity) as GameObject; // Метод, копирующий объект-шаблон.
        }
    }

        public override void ReceiveDamage(int damage)
        {
        Lives -= damage;
        if (Lives <= 0)
        {
            var p = transform.position;
                Instantiate(Item, new Vector3(p.x, p.y, p.z), Quaternion.Euler(180, 0, 0));
            PlayerPrefs.SetInt("exp", PlayerPrefs.GetInt("exp") + costExperience / 2);
            PlayerPrefs.Save();
            Destroy(gameObject);
        }
        }
    }
