using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class FrozenCtrl: MonoBehaviour, IController
{
    [SerializeField] LayerMask layerMask;
    [SerializeField]
    bool _hitPlayer;
    [SerializeField]
    bool changeTrigger, hasTimes;
    [SerializeField]
    int times = 1, checkTimes = 0;

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
        if (((1 << player.gameObject.layer) & layerMask) != 0)
        {
            PlayerCtrl playerCtrl = player.GetComponent<PlayerCtrl>();
            if (playerCtrl != null)
            {
                playerCtrl.OnDeath();
                _hitPlayer = true;
                return true;
            }
            else
            {
                ShooterCtrl shooterCtrl = player.GetComponent<ShooterCtrl>();
                if (shooterCtrl != null)
                {
                    shooterCtrl.OnDeath();
                    checkTimes++;
                    return false;

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

            CheckCollider(player);
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
