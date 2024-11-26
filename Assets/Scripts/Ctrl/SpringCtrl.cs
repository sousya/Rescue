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
    Animator anim;
    [SerializeField]
    Rigidbody m_rigid;
    bool waitDeath = false;
    public List<CharacterJoint> targetList;
    public Transform moveTarget, upTarget;
    public RopeHangCtrl rope;
    public float upTime = 1f, downTime = 1f;
    public bool canAttack;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision != null && collision.transform != transform.parent  && rope.targetList.Count == 0)
        {
            if (LevelManager.Instance.transDic.Contains(collision.transform))
            {
                return;
            }
            Debug.Log("弹簧碰撞");

            Transform pushObject = collision.transform;
            anim.Play("Bounce");
            Rigidbody rb = pushObject.GetComponent<Rigidbody>();
            if(rb == null)
            {
                return;
            }

            rb.useGravity = false;
            PlayerCtrl playerCtrl = collision.transform.GetComponent<PlayerCtrl>();
            AnimalCtrl animalCtrl = collision.transform.GetComponent<AnimalCtrl>();
            ShooterCtrl shooterCtrl = collision.transform.GetComponent<ShooterCtrl>();

            if (playerCtrl != null)
            {
                if(playerCtrl.isDeath || playerCtrl.isBoucing)
                {
                    //playerCtrl.m_rigid.useGravity = true;
                    return;
                }
                playerCtrl.movingTarget = null;
                playerCtrl.isBoucing = true;
                playerCtrl.SetUseGravity(false);
            }

            if(animalCtrl != null)
            {
                if(animalCtrl.isDeath)
                {
                    //animalCtrl.m_rigid.useGravity = true;
                    animalCtrl.attackCheck.gameObject.SetActive(true);
                    return;
                }


                if (animalCtrl.faceRight)
                {
                    animalCtrl.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    animalCtrl.transform.rotation = Quaternion.Euler(0, 180, 0);
                }

                animalCtrl.isBounce = true;
                animalCtrl.BounceAttack = canAttack;
            }

            if(shooterCtrl != null)
            {
                shooterCtrl.isBouncing = true;
            }

            if (((1 << pushObject.gameObject.layer) & layerMask) != 0)
            {
                pushObject.DOKill();
                Sequence s = DOTween.Sequence();
                //定义一共2秒的 x 轴移动
                s.Append(pushObject.transform.DOMoveX(moveTarget.position.x, upTime + downTime).SetEase(Ease.Linear));
                //定义0-1秒的 y 轴移动
                s.Insert(0, pushObject.transform.DOMoveY(upTarget.position.y, upTime).SetEase(Ease.OutCirc));
                //下落 1-2秒的 y 轴移动
                s.Insert(upTime, pushObject.transform.DOMoveY(moveTarget.position.y, downTime).SetEase(Ease.InCirc));

                //播放
                s.Play();

                LevelManager.Instance.transDic.Add(collision.transform);
                StartCoroutine(RemoveDic(collision.transform));
                //该动画组的回调方法
                s.OnComplete(() =>
                {
                    //eff.SetActive(false);
                    if (animalCtrl != null)
                    {
                        animalCtrl.isBounce = false;
                        animalCtrl.attackCheck.gameObject.SetActive(false);
                        animalCtrl.m_rigid.useGravity = false;
                    }
                    if(playerCtrl != null)
                    {
                        playerCtrl.isBoucing = false;
                        playerCtrl.SetUseGravity(true);
                    }
                    if (shooterCtrl != null)
                    {
                        shooterCtrl.isBouncing = false;
                    }

                    var rigid = pushObject.GetComponent<Rigidbody>();
                    if(rigid != null)
                    {
                        rigid.useGravity = true;
                    }
                });
            }
        }
    }

    IEnumerator RemoveDic(Transform trans)
    {
        yield return 0;

        if (trans != null && LevelManager.Instance.transDic.Contains(trans))
        {
            LevelManager.Instance.transDic.Remove(trans);
        }
    }


}
