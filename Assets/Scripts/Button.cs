using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
    
    public enum ButtonSort
    {
        OnWallButton,   // Настенная (на пейзаже)
        OnBorderButton, // Боковая (на объектах)
        Lever
    }
    [Header("Параметры кнопки")]
    public ButtonSort ButtonType = ButtonSort.OnWallButton;
    public enum Interact
    {
        ByPlayer,       // Только игрок
        ByPlayerAndBox, // Только игрок и ящики
        ByArrow         // Только стрела
    }
    public Interact InteractType = Interact.ByPlayer;
    public enum HowWork
    {
        OnlyPress,      // Одиночное нажатие
        PolyPress,      // Множество нажатий
        Holding         // Удержание
    }
    public HowWork WorkType = HowWork.OnlyPress;
    public float ActiveTime = 5;
    public Sprite[] sprites = new Sprite[2];
    [Header("Управляемые объекты")]
    public GameObject[] DependObjects;
    [Header("Нерегулируемые параметры")]
    public bool Active = false;
    public float Timer;
    // Use this for initialization
    void Start ()
    {
        Timer = ActiveTime;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Timer < ActiveTime)
        {
            Timer += Time.deltaTime;
        }
	}
    
    void OnTriggerStay2D(Collider2D col)
    {
        if (ButtonType == ButtonSort.OnBorderButton)
        {            
            if (WorkType == HowWork.OnlyPress)
            {
                if (InteractType == Interact.ByPlayer)
                {
                    if (col.CompareTag("Player"))
                    {
                        Active = true;
                        Activation(DependObjects, Active);
                    }
                }

                if (InteractType == Interact.ByPlayerAndBox)
                {
                    if (col.CompareTag("Player") || col.CompareTag("Box"))
                    {
                        Active = true;
                        Activation(DependObjects, Active);
                    }
                }
            }
        }

        if (ButtonType == ButtonSort.OnWallButton)
        {
            if (WorkType == HowWork.Holding)
            {
                if (InteractType == Interact.ByPlayer)
                {
                    if ((col.CompareTag("Player")) && Input.GetKey(KeyCode.E))
                    {
                        Active = true;
                        Activation(DependObjects, Active);
                    }
                    else
                    {
                        Active = false;
                        Activation(DependObjects, Active);
                    }
                }
            }
            
            if (WorkType == HowWork.OnlyPress)
            {
                if (InteractType == Interact.ByPlayer)
                {
                    if ((col.CompareTag("Player")) && Input.GetKey(KeyCode.E))
                    {
                        Active = true;
                        Activation(DependObjects, Active);
                    }
                }
            }

            if (WorkType == HowWork.PolyPress)
            {
                if (InteractType == Interact.ByPlayer)
                {
                    if ((Timer >= ActiveTime) && (col.CompareTag("Player")) && Input.GetKeyDown(KeyCode.E))
                    {
                        Active = !Active;
                        Timer = 0;
                        Activation(DependObjects, Active);
                    }
                }
            }
        }

        if (ButtonType == ButtonSort.Lever)
        {
            if (WorkType == HowWork.Holding)
            {
                if (InteractType == Interact.ByPlayer)
                {
                    if ((col.CompareTag("Player")) && (Input.GetKey(KeyCode.E)))
                    {
                        Active = true;
                        Activation(DependObjects, Active);
                    }
                    else
                    {
                        Active = false;
                        Activation(DependObjects, Active);
                    }
                }
            }

            if (WorkType == HowWork.OnlyPress)
            {
                if (InteractType == Interact.ByPlayer)
                {
                    if ((col.CompareTag("Player")) && (Input.GetKeyDown(KeyCode.E)))
                    {
                        Active = true;
                        Activation(DependObjects, Active);
                    }
                }
            }

            if (WorkType == HowWork.PolyPress)
            {
                if (InteractType == Interact.ByPlayer)
                {
                    if ((Timer >= ActiveTime) && (col.CompareTag("Player")) && (Input.GetKeyDown(KeyCode.E)))
                    {
                        Active = !Active;
                        Timer = 0;
                        Activation(DependObjects, Active);
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if ((Active == false) && (WorkType == HowWork.OnlyPress) && (InteractType == Interact.ByArrow) && (col.tag.CompareTo("Arrow") == 0))
        {
            Active = !Active;
            Activation(DependObjects, Active);
        }

        if (ButtonType == ButtonSort.OnBorderButton)
        {
            if (WorkType == HowWork.Holding)
            {
                if (InteractType == Interact.ByPlayer)
                {
                    if (col.CompareTag("Player"))
                    {
                        Active = true;
                        Activation(DependObjects, Active);
                    }
                }

                if (InteractType == Interact.ByPlayerAndBox)
                {
                    if (col.CompareTag("Player") || col.CompareTag("Box"))
                    {
                        Active = true;
                        Activation(DependObjects, Active);
                    }
                }
            }

            if (WorkType == HowWork.OnlyPress)
            {
                if (InteractType == Interact.ByArrow)
                {
                    if (col.CompareTag("Arrow"))
                    {
                        Active = true;
                        Activation(DependObjects, Active);
                    }
                }
            }

            if (WorkType == HowWork.PolyPress)
            {
                if (InteractType == Interact.ByPlayer)
                {
                    if (col.CompareTag("Player"))
                    {
                        Active = !Active;
                        Activation(DependObjects, Active);
                    }
                }

                if (InteractType == Interact.ByPlayerAndBox)
                {
                    if (col.CompareTag("Player") || col.CompareTag("Box"))
                    {
                        Active = !Active;
                        Activation(DependObjects, Active);
                    }
                }

                if (InteractType == Interact.ByArrow)
                {
                    if (col.CompareTag("Arrow"))
                    {
                        Active = !Active;
                        Activation(DependObjects, Active);
                    }
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (ButtonType == ButtonSort.OnBorderButton)
        {
            if (WorkType == HowWork.Holding)
            {
                if (InteractType == Interact.ByPlayer)
                {
                    if (col.CompareTag("Player"))
                    {
                        Active = false;
                        Activation(DependObjects, Active);
                    }
                }

                if (InteractType == Interact.ByPlayerAndBox)
                {
                    if (col.CompareTag("Player") || col.CompareTag("Box"))
                    {
                        Active = false;
                        Activation(DependObjects, Active);
                    }
                }
            }
        }
    }

    public void Activation(GameObject[] GameObjects, bool Active)
    {
        int i;
        DoorPlatform SetWork;
        for (i = 0; i < GameObjects.Length - 1; i++)
        {
            SetWork = GameObjects[i].GetComponentInChildren<DoorPlatform>();
            SetWork.Active = !SetWork.Active;
        }
        ButtonMove(Active);
    }

    public void ButtonMove(bool Active)
    {
        if (Active)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprites[0];
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprites[1];
        }
    }
}