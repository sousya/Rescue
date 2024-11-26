using DG.Tweening;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class WindMachineCtrl: MonoBehaviour, IController
{
    [SerializeField] LayerMask layerMask, wallLayer;

    [SerializeField]
    bool changeTrigger, hasTimes, hasTarget, isLeft;
    [SerializeField]
    int times = 1, checkTimes = 0, continueTime;
    public Transform shootPoint, shootCheck, checkTrans;

    public float windTime = 1f, forceScale = 30;
    public bool isVertical;
    public List<Rigidbody> frigs = new List<Rigidbody>();
    public bool faceRight = true, hasTargetRight, faceUp;


    List<Transform> movingTarget = new List<Transform>();
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    public void Update()
    {
        Vector3 vec = Vector3.right;
        if(faceUp)
        {
            vec = Vector3.up;
        }
        else
        {
            if (!faceRight)
            {
                vec = Vector3.left;
            }
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
        //hasTargetRight = (hitInfoRight.transform == checkRightHit.transform) && hasTargetRight;

    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision != null && hasTargetRight)
        {
            if (((1 << collision.gameObject.layer) & layerMask) != 0)
            {
                //collision.transform.DOKill();
                //movingTarget.Add(collision.transform);
                PlayerCtrl playerCtrl = collision.transform.GetComponent<PlayerCtrl>();
                if(playerCtrl != null && !playerCtrl.isDeath)
                {
                    if (playerCtrl.isMoving)
                    {
                        playerCtrl.transform.DOKill();
                        playerCtrl.isMoving = false;
                    }
                    Vector3 force;
                    if (isVertical)
                    {
                        force = new Vector3(0, shootPoint.position.y - playerCtrl.transform.position.y, 0).normalized;
                    }
                    else
                    {
                        force = new Vector3(shootPoint.position.x - playerCtrl.transform.position.x, 0, 0).normalized;
                    }
                    playerCtrl.m_rigid.AddForce(force * forceScale);
                    if (playerCtrl != null)
                    {
                        playerCtrl.isWind = true;
                    }
                }
                else
                {
                    ShooterCtrl shooterCtrl = collision.transform.GetComponent<ShooterCtrl>();
                    if(shooterCtrl != null && !shooterCtrl.isDeath)
                    {
                        Vector3 force;
                        if (isVertical)
                        {
                            force = new Vector3(0, shootPoint.position.y - shooterCtrl.transform.position.y, 0).normalized;
                        }
                        else
                        {
                            force = new Vector3(shootPoint.position.x - shooterCtrl.transform.position.x, 0, 0).normalized;
                        }
                        shooterCtrl.m_rigid.AddForce(force * forceScale);
                        //shooterCtrl.m_rigid.velocity = force * forceScale;
                        if (shooterCtrl != null)
                        {
                            shooterCtrl.isWind = true;
                        }
                    }
                }
               
            }
               
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision != null)
        {
            if (((1 << collision.gameObject.layer) & layerMask) != 0)
            {
                PlayerCtrl playerCtrl = collision.transform.GetComponent<PlayerCtrl>();

                if (playerCtrl != null)
                {
                    playerCtrl.isWind = false;
                }
            }
            else
            {
                ShooterCtrl shooterCtrl = collision.transform.GetComponent<ShooterCtrl>();
                if (shooterCtrl != null)
                {
                    shooterCtrl.isWind = false;
                }
            }

        }
    }

    public virtual void OnDeath()
    {
        foreach (var frig in frigs)
        {
            frig.isKinematic = false;
            frig.useGravity = true;
        }
    }
}
