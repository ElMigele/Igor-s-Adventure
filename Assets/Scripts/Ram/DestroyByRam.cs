using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByRam : MonoBehaviour {
    [Range(1, 10)]
    public int Health = 5;          // Запас прочности
    [Range(0.1f, 1)]
    public float Weakness = 0.5f;   // Хрупкость

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag("Ram"))
        {
            float Velocity = col.transform.GetComponent<DestroyDistanceJoint>().Velocity[0];
            int Damage = Mathf.RoundToInt(Velocity * Weakness);
            Health -= Damage;
            Debug.Log("Скорость: " + Velocity + ", Урон: " + Damage + ", ХП: " + Health);
            if (Health <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
