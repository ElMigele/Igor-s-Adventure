using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour
{
    private GameObject parent;
    public GameObject Parent { set { parent = value; } get { return parent; } }

    private float speed = 5.0F;
    private Vector3 direction;
    public Vector3 Direction { set { direction = value; } }

    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        Destroy(gameObject, 2F);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();

        if (unit && unit.gameObject != parent)
        {
            Destroy(gameObject);
        }
    }
    }