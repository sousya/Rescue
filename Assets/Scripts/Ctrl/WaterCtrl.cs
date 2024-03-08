using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class WaterCtrl: MonoBehaviour, IController
{

    bool _hitPlayer;
    [SerializeField]
    bool changeTrigger, hasTimes, isAnimalAttack, isFire = true;
    [SerializeField]
    int times = 1, checkTimes = 0, killAnimalLevel;
    FireCtrl hitFire;

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
        
        FireCtrl fire = player.GetComponent<FireCtrl>();


        return fire != null;
    }

    public virtual void OnTriggerEnter(Collider collision)
    {
        if (collision != null)
        {
            Transform player = collision.transform;

            if(CheckCollider(player))
            {
                FireCtrl fire = player.GetComponent<FireCtrl>();
                fire.Death();
                Destroy(gameObject);
            }

        }
    }

}
