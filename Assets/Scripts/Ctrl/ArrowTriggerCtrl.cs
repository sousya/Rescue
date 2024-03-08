using DG.Tweening;
using QFramework;
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
    public LayerMask layerMask;
    public bool hasTarget = false, hasTargetRight = false, hasTargetLeft = false, isDeath = false;
    [HideInInspector]
    public Rigidbody m_rigid;
    [HideInInspector]
    public Collider m_collider;
    public ArrowCheckCtrl checkLeft, checkRight;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    public virtual void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();

        bulletPrefab.GetComponent<BulletCtrl>().layerMask = layerMask;
        checkLeft.layerMask = layerMask;
        checkRight.layerMask = layerMask;

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
        

    }

    public virtual void Attack()
    {
        if (hasTargetLeft && !isDeath)
        {
            GameObject go = GameObject.Instantiate(bulletPrefab, shootLeftPoint);

            BulletCtrl bulletCtrl = go.GetComponent<BulletCtrl>();
            bulletCtrl.moveVec = Vector3.left * bulletSpeed;
            isDeath = true;
        }

        if (hasTargetRight && !isDeath)
        {
            GameObject go = GameObject.Instantiate(bulletPrefab, shootRightPoint);

            BulletCtrl bulletCtrl = go.GetComponent<BulletCtrl>();
            bulletCtrl.moveVec = Vector3.right * bulletSpeed;
            isDeath = true;
        }
    }

    public void OnDeath()
    {
        if (isDeath)
        {
            return;
        }
        isDeath = true;
        transform.DOKill();
        m_rigid.velocity = Vector3.zero;
        m_collider.isTrigger = true;

        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        Debug.Log("shooterÀ¿Õˆ");
    }

}
