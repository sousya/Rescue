using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowderShooterCtrl: MonoBehaviour, IController
{
    public LayerMask layerMask;

    [SerializeField]
    public GameObject boob;
    [SerializeField]
    Rigidbody m_rigid;
    bool waitDeath = false;
    public List<CharacterJoint> targetList;
    public Transform moveTarget, upTarget, shootPoint;
    public float attackCoolDown, time;
    CharacterJoint[] tempList;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Awake()
    { 
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

    void Update()
    {
        time += Time.deltaTime;

        if(time > attackCoolDown)
        {
            Attack();
        }
    }

    private void Attack()
    {
        time = 0;

        GameObject pushObject = Instantiate(boob, shootPoint);
        pushObject.transform.localPosition = Vector3.zero;

        pushObject.transform.DOKill();
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
