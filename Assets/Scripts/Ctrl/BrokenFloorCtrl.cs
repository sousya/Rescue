using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BrokenFloorCtrl : MonoBehaviour, IController
{
   
    [HideInInspector]
    public Rigidbody m_rigid;
    [HideInInspector]
    public Collider m_collider;
    public List<Rigidbody> rigidBodies;
    public List<Transform> hideLists;
    public List<Transform> showLists;
    public FloorCtrl floor;
    public SpikeCtrl spike, spike1;


    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    public virtual void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();

    }

    public virtual void Update()
    {
        
    }

    public void OnDeath()
    {
        foreach (var b in rigidBodies)
        {
            if (b != null)
            {
                b.useGravity = true;
                b.isKinematic = false;
                b.AddForce(100, 100, 0);
            }
        }
        if(spike != null)
        {
            spike.OnDeath();
        }
        if(spike1 != null)
        {
            spike1.OnDeath();
        }

        floor.m_collider.enabled = false;

    }

}
