using DG.Tweening;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class FrozenCtrl: MonoBehaviour, IController
{
    private ResLoader mResLoader = ResLoader.Allocate();
    public LayerMask layerMask, wallLayer;
    [SerializeField]
    bool _hitPlayer;
    [SerializeField]
    bool changeTrigger, hasTimes;
    [SerializeField]
    int times = 1, checkTimes = 0;
    [SerializeField]
    GameObject frozenFx;
    public bool faceRight = true, hasTargetRight;
    public Transform checkTrans;

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

    private void Update()
    {
        Vector3 vec = Vector3.right;
        if (!faceRight)
        {
            vec = Vector3.left;
        }
        RaycastHit hitAttack = new RaycastHit();
        RaycastHit checkFloor = new RaycastHit();

        RaycastHit[] hitAttacks = Physics.RaycastAll(checkTrans.position, vec, 10000f, layerMask);
        RaycastHit[] checkFloors = Physics.RaycastAll(checkTrans.position, vec, 10000f, wallLayer);
        Array.Sort(checkFloors, (x, y) => { return x.distance.CompareTo(y.distance); });
        if (hitAttacks.Length > 0)
        {
            if (hitAttacks[0].transform != transform)
            {
                hitAttack = hitAttacks[0];
            }
            else
            {
                if (hitAttacks.Length > 1 && hitAttacks[0].transform == transform)
                {
                    hitAttack = hitAttacks[1];
                }
            }
        }

        if (checkFloors.Length > 0)
        {
            if (checkFloors[0].transform != transform)
            {
                checkFloor = checkFloors[0];
            }
            else
            {
                if (hitAttacks.Length > 1 && checkFloors[0].transform == transform)
                {
                    checkFloor = checkFloors[1];
                }
            }
        }
        hasTargetRight = false;
        if (checkFloor.transform != null)
        {
            if (checkFloor.transform.gameObject.layer != 7 && checkFloor.transform.gameObject.layer != 11 && checkFloor.transform == hitAttack.transform)
            {
                hasTargetRight = true;
            }
        }
    }
    public virtual bool CheckCollider(Transform player)
    {
        if (((1 << player.gameObject.layer) & layerMask) != 0 && hasTargetRight)
        {
            PlayerCtrl playerCtrl = player.GetComponent<PlayerCtrl>();
            if (playerCtrl != null)
            {
                playerCtrl.OnDeath();
                var frozenFx = mResLoader.LoadSync<GameObject>("fx", "ice");
                Instantiate(frozenFx, playerCtrl.transform);
                _hitPlayer = true;
                return true;
            }
            else
            {
                ShooterCtrl shooterCtrl = player.GetComponent<ShooterCtrl>();
                if (shooterCtrl != null)
                {
                    shooterCtrl.OnDeath();
                    var frozenFx = mResLoader.LoadSync<GameObject>("fx", "ice");
                    Instantiate(frozenFx, shooterCtrl.transform);

                    checkTimes++;
                    return false;

                }
                else
                {
                    AnimalCtrl animalCtrl = player.GetComponent<AnimalCtrl>();
                    if (animalCtrl != null)
                    {
                        animalCtrl.OnDeath();
                        var frozenFx = mResLoader.LoadSync<GameObject>("fx", "ice");
                        Instantiate(frozenFx, animalCtrl.transform);

                        checkTimes++;
                        return false;

                    }
                }

            }



        }
        return false;
    }

    public virtual void OnTriggerStay(Collider collision)
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
