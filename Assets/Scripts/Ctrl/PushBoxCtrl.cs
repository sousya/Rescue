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

    public bool beginCheck, ischecked;
    public Rigidbody m_rigid;
    bool waitDeath = false;
    public List<CharacterJoint> targetList;
    public Transform checkGround;
    CharacterJoint[] tempList;
    PlayerCtrl playerctrl;
    [SerializeField]
    SpikeCtrl spikeCtrl;
    [HideInInspector]
    public Collider m_collider;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();
        tempList = GetComponents<CharacterJoint>();
        foreach (CharacterJoint joint in tempList)
        {
            targetList.Add(joint);
        }
        tempList = null;

        //SetUseGravity(false);
        RegisterEvent();
    }

    void RegisterEvent()
    {
        this.RegisterEvent<UnRopeEvent>(e =>
        {
            OnUnRope(e);
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    void OnUnRope(UnRopeEvent e)
    {
        RemoveList(e.rope);
    }

    public void RemoveList(Transform target)
    {
        Rigidbody targetRigid = target.GetComponent<Rigidbody>();

        if (targetList.Count > 0)
        {
            foreach (CharacterJoint joint in targetList)
            {
                if (joint.connectedBody == targetRigid)
                {
                    targetList.Remove(joint);
                    Destroy(joint);
                    target.gameObject.SetActive(false);
                    break;
                }
            }
        }
    }
     void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            Transform trans = collision.transform;

            if (((1 << trans.gameObject.layer) & groundLayer) != 0)
            {
                if (!ischecked)
                {
                    beginCheck = true;
                }
                else
                {
                    m_collider.isTrigger = true;
                    m_rigid.useGravity = false;
                }
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision != null)
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

    IEnumerator WaitDeath()
    {    

        if (waitDeath)
        {
        }
        else
        {
            waitDeath = true;
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }
    }


}
