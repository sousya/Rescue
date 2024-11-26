using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PushBoxCtrl : MonoBehaviour, IController
{
    public LayerMask playerLayer, groundLayer;

    public bool beginCheck, ischecked, isOnce = true;
    public Rigidbody m_rigid;
    bool waitDeath = false;
    public List<CharacterJoint> targetList;
    CharacterJoint[] tempList;
    PlayerCtrl playerctrl;
    [SerializeField]
    PushBoxCheck pushBoxCheck;
    //[HideInInspector]
    public Collider m_collider;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Awake()
    {
        //m_rigid = GetComponent<Rigidbody>();
        //m_collider = GetComponent<Collider>();
        tempList = GetComponents<CharacterJoint>();
        pushBoxCheck.pushBoxCtrl = this;

        foreach (CharacterJoint joint in tempList)
        {
            targetList.Add(joint);
        }
        tempList = null;
    }

     void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            Transform trans = collision.transform;

            if (((1 << trans.gameObject.layer) & groundLayer) != 0)
            {
                beginCheck = true;
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision != null && (!ischecked || !isOnce))
        {
            Transform player = collision.transform;
            if ((((1 << player.gameObject.layer) & playerLayer) != 0) && beginCheck)
            {
                PlayerCtrl playerCtrl = player.GetComponent<PlayerCtrl>();
                if (playerCtrl != null)
                {
                    playerCtrl.StartCarryBox(this);
                    playerctrl = playerCtrl;

                    m_rigid.useGravity = false;
                    beginCheck = false;
                    ischecked = true;
                }
            }
               
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision != null)
        {
            Transform trans = collision.transform;

            if (((1 << trans.gameObject.layer) & groundLayer) != 0)
            {
                if (playerctrl != null)
                {
                    playerctrl.carryBox = null;
                    m_rigid.useGravity = true;
                }
            }
        }
    }

}
