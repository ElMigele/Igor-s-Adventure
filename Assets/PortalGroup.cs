using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGroup : MonoBehaviour {
    public bool Active = true;
    public enum PortalActivation
    {
        Constant,
        Control
    }
    public PortalActivation portalActivationType;
    public GameObject[] Buttons4Active;
    private Button[] Buttons4ActiveScripts;
    [HideInInspector] public Transform[] PortalMassive;
    public enum Work
    {
        Random,
        Cycle
    }
    public Work WorkType = Work.Random;

	// Use this for initialization
	void Start ()
    {
        CallControlObjects();
        SetPortalMassive();        
    }

    private void SetPortalMassive()
    {
        int i = gameObject.GetComponentInChildren<Transform>().childCount;
        PortalMassive = new Transform[i];
        for (int j = 0; j < i; j++)
        {
            PortalMassive[j] = gameObject.GetComponentInChildren<Transform>().GetChild(j);
            PortalMassive[j].GetComponent<PortalParameters>().PortalID = j;
        }
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

    public void SendObject(GameObject Object, int PortalID)
    {
        if (Active)
        {
            if (WorkType == Work.Cycle)
            {
                Transform NewPortal = NextExitEnabledPortal(PortalID);
                if (NewPortal.GetComponent<PortalParameters>().EnterEnabled)
                {
                    NewPortal.GetComponent<PortalParameters>().EnterEnabled = false;
                }
                Object.transform.position = NextExitEnabledPortal(PortalID).position;
            }

            if (WorkType == Work.Random)
            {
                Transform NewPortal = RandExitEnabledPortal(PortalID);
                if (NewPortal.GetComponent<PortalParameters>().EnterEnabled)
                {
                    NewPortal.GetComponent<PortalParameters>().EnterEnabled = false;
                }
                Object.transform.position = NewPortal.position;
            }
        }
    }    

    private Transform NextExitEnabledPortal(int curID)
    {
        Transform NextPortal;
        int i;
        if (curID == PortalMassive.Length - 1)
        {
            i = 0;
        }
        else
        {
            i = curID + 1;
        }
        NextPortal = PortalMassive[i];
        while (true)
        {
            PortalParameters PorPar = NextPortal.GetComponent<PortalParameters>();
            if ((PorPar.ExitEnabled == true) && (PorPar.Active == true))
            {
                break;
            }
            else
            {
                if (i == PortalMassive.Length - 1)
                {
                    i = 0;
                }
                else
                {
                    i++;
                }
                NextPortal = PortalMassive[i];
            }
        }
        return NextPortal;
    }

    private Transform RandExitEnabledPortal(int portalID)
    {
        List<Transform> ExitEnabledMassive = new List<Transform>();
        int j = 0;
        for (int i = 0; i < PortalMassive.Length - 1; i++)
        {
            Transform Portal = PortalMassive[i];
            PortalParameters PorPar = Portal.GetComponent<PortalParameters>();
            if ((PorPar.ExitEnabled) && (PorPar.PortalID != portalID) && (PorPar.Active))
            {
                ExitEnabledMassive.Insert(j, PortalMassive[i]);
                j++;
            }
        }        
        return ExitEnabledMassive[UnityEngine.Random.Range(0, j)];
    }
}
