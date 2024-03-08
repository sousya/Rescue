using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArrowCheckCtrl: MonoBehaviour, IController
{
    public LayerMask layerMask;
    [HideInInspector]
    public Rigidbody m_rigid;
    [HideInInspector]
    public Collider m_collider;
    public ArrowTriggerCtrl arrowTrigger;
    public bool isLeft;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    public virtual void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform != null)
        {
            if (((1 << other.gameObject.layer) & layerMask) != 0)
            {
                arrowTrigger.hasTargetLeft = isLeft;
                arrowTrigger.hasTargetRight = !isLeft;
            }
        }
        
    }

}
