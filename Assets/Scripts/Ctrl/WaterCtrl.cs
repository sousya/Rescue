using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class WaterCtrl: MonoBehaviour, IController
{

    bool _hitPlayer;
    [SerializeField]
    bool changeTrigger, hasTimes, isAnimalAttack, isCheck = false;
    [SerializeField]
    int times = 1, checkTimes = 0, killAnimalLevel;
    [SerializeField]
    GameObject fx, model, fx2;
    [SerializeField]
    Rigidbody m_rigid;
    [SerializeField]
    LayerMask floorLayer;

    public bool hitPlayer
    {
        get
        {
            return _hitPlayer;
        }
    }

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    public virtual bool CheckCollider(Transform player)
    {
        if(isCheck)
        {
            return false;
        }
        bool isEffect = false;
        FireCtrl fire = player.GetComponent<FireCtrl>();
        if(fire != null)
        {
            fire.Death();
            isEffect = true;
        }
        else
        {
            TengwanCtrl tengwan = player.GetComponent<TengwanCtrl>();
            if(tengwan != null && !tengwan.isCheck)
            {
                tengwan.Grow(); 
                isEffect = true;
                StartCoroutine(ShowFx2());
            }
            else
            {
                int objLayerMask = 1 << player.gameObject.layer;
                if ((floorLayer.value & objLayerMask) > 0)
                {
                    isEffect = true;
                }
            }
        }


        return isEffect;
    }


    public virtual void OnTriggerEnter(Collider collision)
    {
        if (collision != null)
        {
            Transform player = collision.transform;

            if(CheckCollider(player))
            {
                //StartCoroutine(ShowFx());
                StartCoroutine(ShowFx2(true));

            }

        }
    }

    IEnumerator ShowFx()
    {
        isCheck = true;
        fx.SetActive(true);
        model.SetActive(false);
        m_rigid.useGravity = false;
        m_rigid.isKinematic = true;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    IEnumerator ShowFx2(bool destroy = true)
    {
        isCheck = true;
        fx2.SetActive(true);
        model.SetActive(false);
        m_rigid.useGravity = false;
        m_rigid.isKinematic = true;
        yield return new WaitForSeconds(0.6f);
        if(destroy)
        {
            Destroy(gameObject);
        }
    }

}
