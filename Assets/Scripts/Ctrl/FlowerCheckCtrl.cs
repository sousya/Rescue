using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class FlowerCheckCtrl: MonoBehaviour, IController
{
    public LayerMask layerMask;

    public FlowerCtrl flower;
    bool _hitPlayer;
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

    public virtual void CheckCollider(Transform player)
    {
        if (((1 << player.gameObject.layer) & layerMask) != 0)
        {
            flower.Attack();
        }
    }

    public virtual void OnTriggerEnter(Collider collision)
    {
        if (collision != null && collision.transform != flower.transform)
        {
            
                Transform player = collision.transform;
                CheckCollider(player);
        }
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision != null && collision.transform != flower.transform)
        {

                Transform player = collision.transform;
                CheckCollider(player);
        }
    }
}
