using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class WeaponCtrl: MonoBehaviour, IController
{
    public LayerMask layerMask;

    bool _hitPlayer;
    [SerializeField]
    bool changeTrigger, hasTimes, isAnimalAttack;
    [SerializeField]
    int times = 1, checkTimes = 0, killAnimalLevel;

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

    public void Start()
    {
    }

    public virtual bool CheckCollider(Transform player)
    {
        //if (((1 << player.gameObject.layer) & layerMask) != 0)
        if(checkTimes > 0)
        {
            {
                PlayerCtrl playerCtrl = player.GetComponent<PlayerCtrl>();
                if (playerCtrl != null)
                {
                    gameObject.transform.parent = playerCtrl.weaponNode;
                    gameObject.transform.localPosition = Vector3.zero;
                    playerCtrl.isWeapon = true;
                    checkTimes = 0;
                    return true;
                }
            }
        }
        
        return false;
    }

    public virtual void OnTriggerEnter(Collider collision)
    {
        if (collision != null)
        {
            Transform player = collision.transform;

            if(CheckCollider(player))
            {

            }
        }
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            Transform player = collision.transform;

            if (!hasTimes || (hasTimes && checkTimes < times))
            {
                CheckCollider(player);
            }
        }
    }
}
