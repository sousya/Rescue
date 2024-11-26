using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoubleRopeCtrl : MonoBehaviour, IController, ICanSendEvent
{
    [SerializeField]
    List<RopeCtrl> ropeList;

    public virtual IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    public void Start()
    {
        foreach (var ropeCtrl in ropeList)
        {
            ropeCtrl.doubleRope = this;
        }
    }

    public void UnRopeOther(RopeCtrl rope)
    {
        foreach(var ropeCtrl in ropeList)
        {
            if(ropeCtrl != rope)
            {
                ropeCtrl.ShrinkRope();
            }
        }    
    }




}
