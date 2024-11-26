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
        RaycastHit hitInfoRight, hitInfoLeft, hitInfoUp, checkRightHit, checkLeftHit, checkUpHit;
        hasTargetRight = Physics.Raycast(checkRightPoint.position, Vector3.right, out hitInfoRight, 10000, layerMask);
        hasTargetLeft = Physics.Raycast(checkLeftPoint.position, Vector3.left, out hitInfoLeft, 10000, layerMask);
        hasTargetUp = Physics.Raycast(shootUpPoint.position, Vector3.up, out hitInfoUp, 10000, layerMask);

        Physics.Raycast(checkRightPoint.position, Vector3.right, out checkRightHit, 10000, wallLayer);
        Physics.Raycast(checkLeftPoint.position, Vector3.left, out checkLeftHit, 10000, wallLayer);
        Physics.Raycast(shootUpPoint.position, Vector3.up, out checkUpHit, 10000, wallLayer);
        hasTargetRight = (hitInfoRight.transform == checkRightHit.transform) && hasTargetRight;
        hasTargetLeft = (hitInfoLeft.transform == checkLeftHit.transform) && hasTargetLeft;
        hasTargetUp = (hitInfoUp.transform == checkUpHit.transform) && hasTargetUp;

        hasTarget = hasTargetRight || hasTargetLeft || hasTargetUp;


    }

    public override void Attack()
    {
        if (hasTarget && !isDeath)
        {

            if (hasTargetLeft)
            {
                PlayAnim("kaiqiang");

            }
            else if (hasTargetRight)
            {
                PlayAnim("kaiqiang");

            }
            else if (hasTargetUp)
            {
                PlayAnim("90sheji");
            }
            
            if (hasTargetLeft)
            {
                GameObject go = GameObject.Instantiate(bulletPrefab, shootLeftPoint);

                BulletCtrl bulletCtrl = go.GetComponent<BulletCtrl>();
                bulletCtrl.moveVec = Vector3.left * bulletSpeed;
                bulletCtrl.killAnimalLevel = killAnimalLevel;
                bulletCtrl.layerMask = layerMask;
                bulletCtrl.from = transform;

                go.transform.localRotation = Quaternion.Euler(0, 0, 90);
                //go.transform.localRotation = Quaternion.Euler(90, 90, 0);

                model.rotation = Quaternion.Euler(0, -90, 0);
            }
            else if (hasTargetRight)
            {
                GameObject go = GameObject.Instantiate(bulletPrefab, shootRightPoint);

                BulletCtrl bulletCtrl = go.GetComponent<BulletCtrl>();
                bulletCtrl.moveVec = Vector3.right * bulletSpeed;
                bulletCtrl.killAnimalLevel = killAnimalLevel;
                bulletCtrl.layerMask = layerMask;
                bulletCtrl.from = transform;
                go.transform.localRotation = Quaternion.Euler(0, 0, -90);
                model.rotation = Quaternion.Euler(0, 90, 0);
            }
            else
            {
                GameObject go = GameObject.Instantiate(bulletPrefab, transform);

                BulletCtrl bulletCtrl = go.GetComponent<BulletCtrl>();
                bulletCtrl.moveVec = Vector3.up * bulletSpeed;
                bulletCtrl.killAnimalLevel = killAnimalLevel;
                bulletCtrl.layerMask = layerMask;
                bulletCtrl.from = transform;
                go.transform.localRotation = Quaternion.Euler(0, 0, 0);
                model.rotation = Quaternion.Euler(0, 90, 0);
                model.rotation = Quaternion.Euler(0, 180, 0);
            }

            time = 0;
        }
        else
        {
            model.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
