using DG.Tweening;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FrozenMachineCtrl: MonoBehaviour, IController
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] LayerMask checkLayer;

    [SerializeField]
    bool changeTrigger, hasTimes, hasTarget, isLeft, isDeath;
    [SerializeField]
    int times = 1, checkTimes = 0;
    public Transform shootPoint, shootCheck;
    public FrozenCtrl frozenCtrl;
    public List<Rigidbody> frigs = new List<Rigidbody>();
    public List<Collider> frigsCollider = new List<Collider>();
    Collider m_collision;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }
    public void Start()
    {
        frozenCtrl.layerMask = layerMask;
        m_collision = gameObject.GetComponent<Collider>();
    }

    public virtual void FixedUpdate()
    {
        hasTarget = false;

        RaycastHit hitAttack = new RaycastHit();
        RaycastHit checkFloor = new RaycastHit();

        RaycastHit[] hitAttacks = Physics.RaycastAll(shootPoint.position, transform.right, 10000f, layerMask);
        RaycastHit[] checkFloors = Physics.RaycastAll(shootPoint.position, transform.right, 10000f, checkLayer);

        Array.Sort(checkFloors, (left, right) => { return left.distance.CompareTo(right.distance); });
        if (hitAttacks.Length > 0)
        {
            if (hitAttacks[0].transform != transform)
            {
                hitAttack = hitAttacks[0];
            }
            else
            {
                if (hitAttacks.Length > 1 && hitAttacks[0].transform == transform)
                {
                    hitAttack = hitAttacks[1];
                }
            }
        }
        if (checkFloors.Length > 0)
        {
            if (checkFloors[0].transform != transform)
            {
                checkFloor = checkFloors[0];
            }
            else
            {
                if (hitAttacks.Length > 1 && checkFloors[0].transform == transform)
                {
                    checkFloor = checkFloors[1];
                }
            }
        }

        if (checkFloor.transform != null)
        {
            if (checkFloor.transform.gameObject.layer != 7 && checkFloor.transform.gameObject.layer != 11 && hitAttack.transform != null && hitAttack.transform == checkFloor.transform)
            {
                var attackTarget = hitAttack.transform;
                hasTarget = true;
            }
        }
        Debug.DrawRay(shootPoint.position, transform.right, Color.green,1000);
    }

    public virtual void Update()
    {
            Attack();
    }

    public virtual void Attack()
    {
        if(!isDeath)
        {
            if (hasTarget)
            {
                shootCheck.gameObject.SetActive(true);
            }
            //else
            //{
            //    shootCheck.gameObject.SetActive(false);
            //}
        }
        
    }

    public virtual void OnDeath()
    {
        
        foreach(var frig in frigs) 
        {
            frig.isKinematic = false;
            frig.useGravity = true;
        }
        
        foreach(var frig in frigsCollider) 
        {
            frig.isTrigger = false;
            frig.gameObject.layer = 17;
        }

        isDeath = true;
        shootCheck.gameObject.SetActive(false);
    }


}
