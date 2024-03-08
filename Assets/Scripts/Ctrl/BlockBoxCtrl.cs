using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBoxCtrl : MonoBehaviour, IController
{
    [HideInInspector]
    public Rigidbody m_rigid;
    [HideInInspector]
    public Collider m_collider;
    public List<Transform> targetList;

    [SerializeField]
    bool isLock = true;
    public bool isDeath = false;


    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();
        SetUseGravity(false);
        RegisterEvent();
    }


    private void Start()
    {
    }

    private void Update()
    {
        if (targetList.Count > 0)
        {
            transform.position = targetList[0].position;
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
        targetList.Remove(target);
        if (targetList.Count <= 0)
        {
            Rigidbody rigidbody = target.parent.GetComponent<Rigidbody>();
            SetUseGravity(true);
            SetVelocity(rigidbody.velocity);
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

}
