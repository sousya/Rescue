using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpringCtrl : MonoBehaviour, IController
{
    public LayerMask layerMask;

    [SerializeField]
    Transform checkBomb;
    [SerializeField]
    Rigidbody m_rigid;
    bool waitDeath = false;
    public List<CharacterJoint> targetList;
    public Transform moveTarget, upTarget;
    CharacterJoint[] tempList;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Awake()
    {
        //m_rigid = GetComponent<Rigidbody>();
        //m_collider = GetComponent<Collider>();
        tempList = GetComponents<CharacterJoint>();
        foreach (CharacterJoint joint in tempList)
        {
            targetList.Add(joint);
        }
        tempList = null;

        //SetUseGravity(false);
        RegisterEvent();
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


    private void OnTriggerEnter(Collider collision)
    {
        if (collision != null)
        {
            Debug.Log("������ײ");

            Transform pushObject = collision.transform;

            if (((1 << pushObject.gameObject.layer) & layerMask) != 0)
            {
                pushObject.DOKill();
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

    IEnumerator WaitDeath()
    {    

        if (waitDeath)
        {
        }
        else
        {
            waitDeath = true;
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }
    }


}
