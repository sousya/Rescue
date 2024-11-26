using DG.Tweening;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SeesawCtrl : MonoBehaviour, IController, ICanSendEvent
{
    public LayerMask layerMask;

    [SerializeField]
    Transform pushObject, moveTarget, upTarget;
    [SerializeField]
    Rigidbody m_rigid;
    bool waitDeath = false;
    public List<CharacterJoint> targetList;
    CharacterJoint[] tempList;


    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Awake()
    {
        pushObject.GetComponent<Rigidbody>().useGravity = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            Transform player = collision.transform;

            if (((1 << player.gameObject.layer) & layerMask) != 0)
            {
                //playerCtrl.OnDeath();
                Sequence s = DOTween.Sequence();
                //定义一共2秒的 x 轴移动
                s.Append(pushObject.transform.DOMoveX(moveTarget.position.x, 2).SetEase(Ease.Linear));
                //定义0-1秒的 y 轴移动
                s.Insert(0, pushObject.transform.DOMoveY(upTarget.position.y, 1F).SetEase(Ease.OutCirc));
                //下落 1-2秒的 y 轴移动
                s.Insert(1f, pushObject.transform.DOMoveY(moveTarget.position.y, 1F).SetEase(Ease.InCirc));

                //播放
                s.Play();
                //该动画组的回调方法
                s.OnComplete(() =>
                {
                    //eff.SetActive(false);
                    pushObject.GetComponent<Rigidbody>().useGravity = true;
                });

                this.SendEvent<SetSequence>(new SetSequence { go = pushObject.gameObject, s = s });

            }
        }
    }


}
