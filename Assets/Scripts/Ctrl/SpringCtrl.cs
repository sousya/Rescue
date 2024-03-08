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
            Debug.Log("弹簧碰撞");

            Transform pushObject = collision.transform;

            if (((1 << pushObject.gameObject.layer) & layerMask) != 0)
            {
                pushObject.DOKill();
                Sequence s = DOTween.Sequence();
                //定义一共2秒的 x 轴移动
                s.Append(pushObject.transform.DOMoveX(moveTarget.position.x, 2).SetEase(Ease.Linear));
                //定义0-1秒的 y 轴移动
                s.Insert(0, pushObject.transform.DOMoveY(upTarget.position.y, 1F).SetEase(Ease.OutCirc));
                //下落 1-2秒的 y 轴移动
                s.Insert(1f, pushObject.transform.DOMoveY(moveTarget.position.y, 1F).SetEase(Ease.InCirc));

                //播放
                s.Play();
                //该动画组的回调方法
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
