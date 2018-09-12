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
    public ButtonSort ButtonType = ButtonSort.OnWallButton;
    public enum Interact
    {
        ByPlayer,       // Только игрок
        ByPlayerBox,    // Только игрок и ящики
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

    //public List<DepObj> DependObjects;// = new List<DepObj>();
    public GameObject[] DependObjects;
    public bool Active = false;
    public float Timer = 0;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnTriggerStay2D(Collider2D col)
    {
        if (ButtonType == ButtonSort.OnBorderButton)
        {
            if (WorkType == HowWork.Holding)
            {
                if (InteractType == Interact.ByPlayerBox)
                {
                    if (col.CompareTag("Player") || col.CompareTag("Box"))
                    {
                        Active = true;
                        Activation(DependObjects);
                    }
                    else
                    {
                        Active = false;
                        Activation(DependObjects);
                    }
                    if (col == null)
                    {
                        Active = false;
                        Activation(DependObjects);
                    }
                }
            }

            if (WorkType == HowWork.OnlyPress)
            {
                if (InteractType == Interact.ByPlayer)
                {
                    if (col.CompareTag("Player"))
                    {
                        Active = true;
                        Activation(DependObjects);
                    }
                }

                if (InteractType == Interact.ByPlayerBox)
                {
                    if (col.CompareTag("Player") || col.CompareTag("Box"))
                    {
                        Active = true;
                    }
                }

                if (InteractType == Interact.ByArrow)
                {
                    if (col.CompareTag("Arrow"))
                    {
                        Active = true;
                    }
                }
            }

            if (WorkType == HowWork.PolyPress)
            {
                if (InteractType == Interact.ByPlayer)
                {
                    if (col.CompareTag("Player"))
                    {
                        Active = true;
                    }
                }

                if (InteractType == Interact.ByPlayerBox)
                {
                    if (col.CompareTag("Player") || col.CompareTag("Box"))
                    {
                        Active = true;
                    }
                }

                if (InteractType == Interact.ByArrow)
                {
                    if (col.CompareTag("Arrow"))
                    {
                        Active = true;
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
                    if (col.CompareTag("Player"))
                    {
                        Active = true;
                    }
                }
            }
            
            if (WorkType == HowWork.OnlyPress)
            {
                if (InteractType == Interact.ByPlayer)
                {
                    if (col.CompareTag("Player"))
                    {
                        Active = true;
                    }
                }
            }

            if (WorkType == HowWork.PolyPress)
            {
                if (InteractType == Interact.ByPlayer)
                {
                    if (col.CompareTag("Player"))
                    {
                        Active = true;
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
                    if (col.CompareTag("Player"))
                    {
                        Active = true;
                    }
                }

                if (InteractType == Interact.ByPlayerBox)
                {
                    if (col.CompareTag("Player") || col.CompareTag("Box"))
                    {
                        Active = true;
                    }
                }

                if (InteractType == Interact.ByArrow)
                {
                    if (col.CompareTag("Arrow"))
                    {
                        Active = true;
                    }
                }
            }

            if (WorkType == HowWork.OnlyPress)
            {
                if (InteractType == Interact.ByPlayer)
                {
                    if (col.CompareTag("Player"))
                    {
                        Active = true;
                    }
                }

                if (InteractType == Interact.ByPlayerBox)
                {
                    if (col.CompareTag("Player") || col.CompareTag("Box"))
                    {
                        Active = true;
                    }
                }

                if (InteractType == Interact.ByArrow)
                {
                    if (col.CompareTag("Arrow"))
                    {
                        Active = true;
                    }
                }
            }

            if (WorkType == HowWork.PolyPress)
            {
                if (InteractType == Interact.ByPlayer)
                {
                    if (col.CompareTag("Player"))
                    {
                        Active = true;
                    }
                }

                if (InteractType == Interact.ByPlayerBox)
                {
                    if (col.CompareTag("Player") || col.CompareTag("Box"))
                    {
                        Active = true;
                    }
                }

                if (InteractType == Interact.ByArrow)
                {
                    if (col.CompareTag("Arrow"))
                    {
                        Active = true;
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if ((Active == false) && (WorkType == HowWork.OnlyPress) && (InteractType == Interact.ByArrow) && (col.tag.CompareTo("Arrow") == 0))
        {
            Active = true;
        }
    }

    public void Activation(GameObject[] GameObjects)
    {
        int i;
        DoorPlatform SetWork;
        for (i = 0; i < GameObjects.Length - 1; i++)
        {
            SetWork = GameObjects[i].GetComponentInChildren<DoorPlatform>();
            SetWork.Active = !SetWork.Active;
        }
    }
}