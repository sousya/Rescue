using DG.Tweening;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HangCtrl: MonoBehaviour, IController, ICanSendEvent
{
    [SerializeField]
    Transform target;

    bool isCheck = false;
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Start()
    {
    }

    public void ShrinkRope()
    {
        if (isCheck)
        {
            return;
        }

        isCheck = true;
        //cursor.ChangeLength(0.5f);
        UnRopeEvent sendEvent = new UnRopeEvent();
        sendEvent.rope = target;
        this.SendEvent<UnRopeEvent>(sendEvent);
    }



}
