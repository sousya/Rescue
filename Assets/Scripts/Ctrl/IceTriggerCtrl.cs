using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IceTriggerCtrl: MonoBehaviour, IController
{
    public Transform shootPoint;
    public SpikeCtrl bulletPrefab;
    public LayerMask layerMask;
    public bool hasTarget = false, isDeath = false;
    [HideInInspector]
    public Rigidbody m_rigid;
    [HideInInspector]
    public Collider m_collider;

    public GravityModifier m_gravityModifier;
    public List<CharacterJoint> targetList;
    public CharacterJoint[] tempList;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    public virtual void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();

        bulletPrefab.layerMask = layerMask;
        tempList = GetComponents<CharacterJoint>();
        foreach (CharacterJoint joint in tempList)
        {
            targetList.Add(joint);
        }
        tempList = null;
    }

    public virtual void Update()
    {
            Attack();
    }

    public virtual void FixedUpdate()
    {

        //hasTargetRight = Physics.Raycast(shootRightPoint.position, Vector3.right, 1000, layerMask);
        //hasTargetLeft = Physics.Raycast(shootLeftPoint.position, Vector3.left, 1000, layerMask);

        hasTarget = Physics.Raycast(shootPoint.position, Vector3.down, 1000, layerMask);
        Debug.DrawRay(shootPoint.position, Vector3.down, Color.green, 1000);

    }

    public virtual void Attack()
    {
        if (hasTarget && !isDeath)
        {
            m_gravityModifier.useGravity = true;
            isDeath = true;
        }
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
