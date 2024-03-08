using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RopeCtrl : MonoBehaviour, IController, ICanSendEvent
{
    public Transform target;

    public bool isCheck = false;
    public virtual IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }



    public virtual void ShrinkRope()
    {
        if(isCheck)
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
