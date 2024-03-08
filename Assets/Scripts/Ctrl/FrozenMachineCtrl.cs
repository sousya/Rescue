using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class FrozenMachineCtrl: MonoBehaviour, IController
{
    [SerializeField] LayerMask layerMask;

    [SerializeField]
    bool changeTrigger, hasTimes, hasTarget, isLeft;
    [SerializeField]
    int times = 1, checkTimes = 0;
    public Transform shootPoint, shootCheck;


    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }
    public virtual void FixedUpdate()
    {
        Vector3 checkVec = Vector3.right;
        if(isLeft)
        {
            checkVec = -Vector3.right;
        }
        hasTarget = Physics.Raycast(shootPoint.position, Vector3.right, 1000, layerMask);
        Debug.DrawRay(shootPoint.position, Vector3.right, Color.green,1000);
    }

    public virtual void Update()
    {
            Attack();
    }

    public virtual void Attack()
    {
        if (hasTarget)
        {
           shootCheck.gameObject.SetActive(true);
        }
        else
        {
            shootCheck.gameObject.SetActive(false);
        }
    }

   
}
