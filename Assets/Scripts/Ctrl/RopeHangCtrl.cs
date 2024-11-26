using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RopeHangCtrl: MonoBehaviour, IController
{
    [HideInInspector]
    public Rigidbody m_rigid;
    [HideInInspector]
    public Collider m_collider;
    public List<CharacterJoint> targetList;
    public delegate void OnUnRopeCall();
    public OnUnRopeCall onUnRopeCall;
    public Transform zhuazi;
    public List<Transform> claws = new List<Transform>();
    public Rigidbody unlockRotation;
    public List<RigidbodyConstraints> constraints = new List<RigidbodyConstraints>()
    {
        RigidbodyConstraints.FreezeRotationZ,
        RigidbodyConstraints.FreezePositionZ
    };

    [SerializeField]
    bool isLock = true, stayX = false;
    public bool isDeath = false, isOneDrag;
    CharacterJoint[] tempList;


    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Start()
    {

        m_rigid = GetComponentInChildren<Rigidbody>();
        m_collider = GetComponentInChildren<Collider>();
        tempList = GetComponents<CharacterJoint>();
        foreach (CharacterJoint joint in tempList)
        {
            targetList.Add(joint);
        }
        tempList = null;
        RegisterEvent();
    }

    private void Update()
    {
        if(stayX)
        {
            if(targetList.Count != 0)
            {
                transform.localRotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
            }
        }
    }

    public void SetUseGravity(bool isOn)
    {
        m_rigid.useGravity = isOn;
    }

    public void SetIsTrigger(bool isOn)
    {
        m_collider.isTrigger = isOn;
    }

    public void SetVelocity(Vector3 velocity)
    {
        m_rigid.velocity = velocity;
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
                    Debug.Log("解锁" + joint.gameObject.name);
                    Destroy(joint);
                    //target.gameObject.SetActive(false);
                    break;
                }
            }
            if(!isOneDrag)
            {
                if (targetList.Count == 0)
                {
                    zhuazi.gameObject.SetActive(false);

                }
            }
            else
            {
                claws[0].gameObject.SetActive(false);
                claws.RemoveAt(0);
            }

            if (targetList.Count == 0)
            {
                if (unlockRotation != null)
                {
                    unlockRotation.constraints = constraints[0];
                   for(int i = 1; i < constraints.Count; i++)
                   {
                        unlockRotation.constraints |= constraints[i];
                   }
                    
                }
            }
                
        }
        else
        {
            if(onUnRopeCall != null)
            {
                onUnRopeCall();
            }
        }
    }

    public void OnUnRopeAll()
    {

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

    public void OnDeath()
    {
        if(isDeath)
        {
            return;
        }
        isDeath = true;
        transform.DOKill();
        //m_rigid.velocity = Vector3.zero;

        //transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90)) ;
        Debug.Log("死亡");
        //TODO 播放死亡动画
    }

}
