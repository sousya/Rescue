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
    public Transform target, upPos, downPos, ropeNode, rotateNode, rotateFollow;
    public Animator anim;
    public GameObject zhuazi1, zhuazi2, zhuazi3;
    public DoubleRopeCtrl doubleRope;

    public bool isCheck = false;
    public virtual IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Awake()
    {
    }

    private void Update()
    {
        rotateNode.rotation = rotateFollow.rotation;
    }

    public void PlayAnim(string animName)
    {
        anim.Play(animName);
    }

    public virtual void ShrinkRope()
    {
        if(isCheck)
        {
            return;
        }

        isCheck = true;
        PlayAnim("Release");

        zhuazi1.SetActive(true);
        zhuazi2.SetActive(true);
        zhuazi3.SetActive(true);

        ropeNode.DOMove(upPos.position, 0.3f).SetEase(Ease.Linear);
        //cursor.ChangeLength(0.5f);
        UnRopeEvent sendEvent = new UnRopeEvent();
        sendEvent.rope = target;
        this.SendEvent<UnRopeEvent>(sendEvent);

        if(doubleRope != null )
        {
            doubleRope.UnRopeOther(this);
        }
    }
   


}
