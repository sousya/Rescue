using GameDefine;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractCtrl : MonoBehaviour, IController
{
    public AttractType type;

    public bool cantAttack = false;
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }
}
