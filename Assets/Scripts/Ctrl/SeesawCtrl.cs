using DG.Tweening;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SeesawCtrl : MonoBehaviour, IController
{
    public LayerMask layerMask;

    [SerializeField]
    Transform pushObject, moveTarget, upTarget;
    [SerializeField]
    Rigidbody m_rigid;
    bool waitDeath = false;
    public List<CharacterJoint> targetList;
    CharacterJoint[] tempList;


    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Awake()
    {
        //m_rigid = GetComponent<Rigidbody>();
        //m_collider = GetComponent<Collider>();
        //tempList = GetComponents<CharacterJoint>();
        //foreach (CharacterJoint joint in tempList)
        //{
        //    targetList.Add(joint);
        //}
        //tempList = null;

        ////SetUseGravity(false);
        //RegisterEvent();
        pushObject.GetComponent<Rigidbody>().useGravity = false;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            Transform player = collision.transform;

            if (((1 << player.gameObject.layer) & layerMask) != 0)
            {
                Debug.Log("Seasaw");
                //playerCtrl.OnDeath();
                Sequence s = DOTween.Sequence();
                //����һ��2��� x ���ƶ�
                s.Append(pushObject.transform.DOMoveX(moveTarget.position.x, 2).SetEase(Ease.Linear));
                //����0-1��� y ���ƶ�
                s.Insert(0, pushObject.transform.DOMoveY(upTarget.position.y, 1F).SetEase(Ease.OutCirc));
                //���� 1-2��� y ���ƶ�
                s.Insert(1f, pushObject.transform.DOMoveY(moveTarget.position.y, 1F).SetEase(Ease.InCirc));

                //����
                s.Play();
                //�ö�����Ļص�����
                s.OnComplete(() =>
                {
                    //eff.SetActive(false);
                    pushObject.GetComponent<Rigidbody>().useGravity = true;
                });
            }
        }
    }


}
