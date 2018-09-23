using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalParameters : MonoBehaviour {
    [HideInInspector] public int PortalID = 0;
    public bool Active = true;
    public enum PortalActivation
    {
        Constant,
        Control
    }
    public PortalActivation portalActivationType;
    public GameObject[] Buttons4Active;
    private Button[] Buttons4ActiveScripts;
    private bool IsEnterEnabled;
    public bool EnterEnabled = false;
    public bool ExitEnabled = false;

    private void Start()
    {
        CallControlObjects();
        IsEnterEnabled = EnterEnabled;
    }

    private void CallControlObjects()
    {
        if (portalActivationType == PortalActivation.Control)
        {
            Buttons4ActiveScripts = new Button[Buttons4Active.Length];
            for (int i = 0; i < Buttons4ActiveScripts.Length; i++)
            {
                Buttons4ActiveScripts[i] = Buttons4Active[i].GetComponent<Button>();
            }
        }
    }

    private void Update()
    {
        if (portalActivationType == PortalActivation.Control)
        {
            Active = CheckButtons(Buttons4Active, Buttons4ActiveScripts);
        }
    }

    private bool CheckButtons(GameObject[] Buttons, Button[] ButtonsScripts)
    {
        bool Active = false;
        for (int i = 0; i < Buttons.Length; i++)
        {
            if (ButtonsScripts[i].Active == false)
            {
                Active = false;
                break;
            }
            else
            {
                Active = true;
            }
        }
        return Active;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (Active)
        {
            if (col.CompareTag("Player"))
            {
                if (EnterEnabled)
                {
                    gameObject.GetComponentInParent<PortalGroup>().SendObject(col.gameObject, PortalID);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (Active)
        {
            if (col.CompareTag("Player"))
            {
                if (IsEnterEnabled)
                {
                    EnterEnabled = true;
                }
            }
        }
    }
}
