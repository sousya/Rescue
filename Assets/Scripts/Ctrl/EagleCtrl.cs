using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EagleCtrl: AnimalCtrl
{
    int attackTime = 0;
    public Transform grondPos;

    public override void FixedUpdate()
    {
        CheckHit();
        CheckAttack();

        if (attackTarget != null)
        {
            Attack();
        }
        else
        {
            if(!isRotating && !isAttacking)
            {
                transform.DOKill();
            }
        }

        if (canPatrol)
        {
            Patrol();
        }

        EditorCheck();
    }

    public override void Attack()
    {
        if (attackTarget != null && !isAttacking)
        {
            isAttacking = true;

            float moveVec = attackTarget.position.x - transform.position.x;
            Vector3 movePos = new Vector3(attackTarget.position.x - (attackRange / 4), transform.position.y, transform.position.z);
            float distance = Mathf.Abs(attackTarget.position.x - transform.position.x);
            transform.DOMove(movePos, attackRushSpeed)
                .OnComplete(() => {
                    attackTime++;

                    attackCheck.gameObject.SetActive(true);

                    if(attackTime == 1)
                    {
                        
                    }
                    else
                    {
                        isAttacking = false;
                    }
                })
                .SetEase(Ease.Linear);
        }
    }

    public override void Patrol()
    {

        if(!isRotating && !isAttacking && !CheckStop())
        {
            if (CheckFront())
            {
                isRotating = true;
                transform.DORotateQuaternion(Quaternion.Euler(0, transform.rotation.eulerAngles.y + 180, 0), rotateTime)
                    .OnComplete(() => { isRotating = false; })
                    .SetEase(Ease.Linear);
                //transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 180, 0);
            }
            else
            {
                transform.position += transform.right * Time.fixedDeltaTime * speed;
            }
        }
        
    }
    public override void OnAttack()
    {
        Vector3 downPos = transform.position;
        downPos.y = grondPos.position.y;
        transform.DOMove(downPos, 1).OnComplete(() =>
        {
            isAttacking = false;
        });
    }
}
