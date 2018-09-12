using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class DepObj
{
    public GameObject Object;
    public enum Activation
    {
        Activation,     // Активация SetActive
        SetVelocity     // Задание скорости (как в лифте)
    }
    public Activation ActivationType;
}
