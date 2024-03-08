using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrossbowShooterCrl: ShooterCtrl
{
    public Transform shootUpPoint;

    public bool hasTargetUp;

    public override void FixedUpdate()
    {

        hasTargetRight = Physics.Raycast(shootRightPoint.position, Vector3.right, 1000, layerMask);
        hasTargetLeft = Physics.Raycast(shootLeftPoint.position, Vector3.left, 1000, layerMask);
        hasTargetUp = Physics.Raycast(shootUpPoint.position, Vector3.left, 1000, layerMask);

        hasTarget = hasTargetRight || hasTargetLeft || hasTargetUp;


    }

    public override void Attack()
    {
        if (hasTarget)
        {


            if (hasTargetLeft)
            {
                GameObject go = GameObject.Instantiate(bulletPrefab, shootLeftPoint);

                BulletCtrl bulletCtrl = go.GetComponent<BulletCtrl>();
                bulletCtrl.moveVec = Vector3.left * bulletSpeed;
            }
            else if (hasTargetRight)
            {
                GameObject go = GameObject.Instantiate(bulletPrefab, shootRightPoint);

                BulletCtrl bulletCtrl = go.GetComponent<BulletCtrl>();
                bulletCtrl.moveVec = Vector3.right * bulletSpeed;
            }
            else if (hasTargetUp)
            {
                GameObject go = GameObject.Instantiate(bulletPrefab, shootUpPoint);

                BulletCtrl bulletCtrl = go.GetComponent<BulletCtrl>();
                bulletCtrl.moveVec = Vector3.up * bulletSpeed;
            }


            time = 0;
        }
    }
}
