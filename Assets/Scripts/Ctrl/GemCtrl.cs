using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GemCtrl : MonoBehaviour, IController
{
    [SerializeField]
    public GemTriggerCtrl gemTriggerCtrl;
    [SerializeField]
    Rigidbody m_rigid;
    [SerializeField]
    Transform fixPoint;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 21)
        {
            gemTriggerCtrl.isStart = true;

            m_rigid.isKinematic = true;

            transform.position = fixPoint.position;
        }
    }

}
