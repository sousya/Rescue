using DG.Tweening;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArrowTriggerCtrl: MonoBehaviour, IController
{
    public Transform shootLeftPoint, shootRightPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 1f;
    public LayerMask layerMask, checkLayer;
    public bool hasTarget = false, hasTargetRight = false, hasTargetLeft = false, isDeath = false;
    [HideInInspector]
    public Rigidbody m_rigid;
    [HideInInspector]
    public Collider m_collider;
    public ArrowCheckCtrl checkLeft, checkRight;
    public Animator anim;
    public int killanimal;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    public virtual void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();

        //bulletPrefab.GetComponent<BulletCtrl>().layerMask = layerMask;
        //checkLeft.layerMask = layerMask;
        //checkRight.layerMask = layerMask;

    }

    public virtual void Update()
    {
            Attack();
    }

    public virtual void FixedUpdate()
    {

        //hasTargetRight = Physics.Raycast(shootRightPoint.position, Vector3.right, 1000, layerMask);
        //hasTargetLeft = Physics.Raycast(shootLeftPoint.position, Vector3.left, 1000, layerMask);

        //hasTarget = hasTargetRight || hasTargetLeft;

        RaycastHit hitAttack = new RaycastHit();
        RaycastHit checkFloor = new RaycastHit();

        RaycastHit[] hitAttacks = Physics.RaycastAll(shootLeftPoint.position, transform.up, 10000f, layerMask);
        RaycastHit[] checkFloors = Physics.RaycastAll(shootLeftPoint.position, transform.up, 10000f, checkLayer);

        Array.Sort(checkFloors, (left, right) => { return left.distance.CompareTo(right.distance); });
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

        if (checkFloor.transform != null)
        {
            if (checkFloor.transform.gameObject.layer != 7 && checkFloor.transform.gameObject.layer != 11)
            {
                var attackTarget = hitAttack.transform;
                hasTargetLeft = true;
            }
        }

    }

    public virtual void Attack()
    {
        if (hasTargetLeft && !isDeath)
        {
            GameObject go = GameObject.Instantiate(bulletPrefab, shootLeftPoint);

            BulletCtrl bulletCtrl = go.GetComponent<BulletCtrl>();
            bulletCtrl.moveVec = transform.up * bulletSpeed;
            bulletCtrl.transform.localRotation = Quaternion.Euler(90 * transform.up.y, 0, (-90 * transform.up.x + 90) + transform.up.y * 90);
            bulletCtrl.killAnimalLevel = killanimal;
            bulletCtrl.layerMask = layerMask;
            OnDeath();
        }
    }

    public void OnDeath()
    {
        if (isDeath)
        {
            return;
        }

        anim.Play("Death");
        isDeath = true;
    }

}
