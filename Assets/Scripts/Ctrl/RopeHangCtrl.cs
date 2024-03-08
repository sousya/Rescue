using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RopeHangCtrl: MonoBehaviour, IController
{
    public float moveSpeed = 1f;
    [HideInInspector]
    public Rigidbody m_rigid;
    [HideInInspector]
    public Collider m_collider;
    public List<CharacterJoint> targetList;


    [SerializeField]
    bool isLock = true;
    public bool isDeath = false;
    CharacterJoint[] tempList;


    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Awake()
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


    private void Start()
    {
    }

    private void Update()
    {

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
                    Destroy(joint);
                    //target.gameObject.SetActive(false);
                    break;
                }
            }
        }
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
