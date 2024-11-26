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
    public bool isDown, isDowning;
    public Transform grondPos;
    public List<Transform> targets = new List<Transform>();
    public EagleAttackCtrl eagleAttack;

    public override void Start()
    {
        base.Start();

        eagleAttack.layerMask = attackLayer;
    }

    public override void FixedUpdate()
    {
        CheckHit();
        CheckAttack();
        CheckDeath();

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

    public override void CheckAttack()
    {
        if(targets.Count > 0)
        {
            RaycastHit checkFloorLeft;
            RaycastHit checkFloorRight;


            Physics.Raycast(right.position, transform.right * transform.localScale.x, out checkFloorLeft, 10000f, checkLayer);
            Physics.Raycast(right.position, -transform.right * transform.localScale.x, out checkFloorRight, 10000f, checkLayer);

            float distance;
            if ((checkFloorLeft.transform != null || checkFloorRight.transform != null) && isDown)
            {

                if(checkFloorLeft.transform != null)
                {
                    distance = Vector3.Distance(checkFloorLeft.transform.position, transform.position);

                    foreach (Transform t in targets)
                    {
                        var newDis = Vector3.Distance(checkFloorLeft.transform.position, transform.position);

                        if (newDis < distance)
                        {
                            attackTarget = t;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (Transform t in targets)
                    {
                        distance = Vector3.Distance(checkFloorRight.transform.position, transform.position);

                        var newDis = Vector3.Distance(checkFloorRight.transform.position, transform.position);

                        if (newDis < distance)
                        {
                            attackTarget = t;
                            break;
                        }
                    }
                }
                
            }
            else
            {
                attackTarget = targets[0];
            }

        }
    }

    public void CheckDeath()
    {
        if(targets.Count > 0)
        {
            var temp = new List<Transform>(targets);
            foreach (var other in temp)
            {
                PlayerCtrl playerCtrl = other.GetComponent<PlayerCtrl>();
                AnimalCtrl animalCtrl = other.GetComponent<AnimalCtrl>();
                ShooterCtrl shootCtrl = other.GetComponent<ShooterCtrl>();

                if (playerCtrl != null)
                {
                    if (playerCtrl.isDeath)
                    {
                        targets.Remove(other.transform);
                    }
                }
                else if (animalCtrl != null)
                {
                    if (!animalCtrl.isDeath)
                    {
                        targets.Remove(other.transform);
                    }
                }
                else if (shootCtrl != null)
                {
                    if (!shootCtrl.isDeath)
                    {
                        targets.Remove(other.transform);
                    }
                }
            }
        }
        
    }

    public override void Attack()
    {
        if (attackTarget != null && !isAttacking && !isDowning && !CheckStop())
        {
            transform.DOKill();
            isAttacking = true;
            float moveVec = attackTarget.position.x - transform.position.x;

            if (((faceRight && !(moveVec > 0)) || (!faceRight && moveVec > 0)) && isAttacking)
            {
                faceRight = !faceRight;
            }

            if(faceRight)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            Vector3 movePos = new Vector3(attackTarget.position.x, attackTarget.position.y, transform.position.z);
            float distance = Mathf.Abs(attackTarget.position.x - transform.position.x);
            attackCheck.gameObject.SetActive(true);
            transform.DOMove(movePos, distance / attackRushSpeed)
                  .OnComplete(() => {
                      PlayAnim("gongji");
                      isAttacking = false;
                      targets.Remove(attackTarget);
                  })
                  .SetEase(Ease.Linear);
        }
    }

    public override void Patrol()
    {

        if(!isRotating && !isAttacking && !CheckStop() && !isDowning && !isDown)
        {
            if (CheckFront())
            {
                isRotating = true;
                transform.DORotateQuaternion(Quaternion.Euler(0, transform.rotation.eulerAngles.y + 180, 0), rotateTime)
                    .OnComplete(() => {
                        StartCoroutine(WaitRotateEnd());
                        faceRight = !faceRight;
                    })
                    .SetEase(Ease.Linear);
                //transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 180, 0);
            }
            else
            {
                transform.position += transform.right * Time.fixedDeltaTime * speed;
                PlayAnim("zoulu");
            }
        }
        
    }
    public override void OnAttack()
    {
        if(isDown)
        {
            return;
        }
        //transform.DOKill();
        isDown = true;
        isAttacking = false;
        attackTarget = null;

        //Vector3 downPos = transform.position;
        //downPos.y = grondPos.position.y;
        //isDowning = true;
        //transform.DOMove(downPos, 1).OnComplete(() =>
        //{
        //    isDowning = false;
        //    isAttacking = false;
        //});
    }

    public override void OnDeath()
    {
        base.OnDeath();
        m_rigid.useGravity = true;
    }
}
