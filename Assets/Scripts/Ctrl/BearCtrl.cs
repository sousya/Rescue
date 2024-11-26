using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BearCtrl: AnimalCtrl
{
    public bool noAttack, isDefending;

    public LayerMask attractLayer;

    public Vector3 attractPos;

    public float attractRange = 10, moveLength = 0.6f, attractMoveRange = 0.5f;

    public Transform attractCheck;
    public override void FixedUpdate()
    {

        CheckHit();
        CheckAttack();
        Attract();
        CheckDown();

        if (attackTarget != null)
        {
            Attack();
        }
        else
        {
            if (!isRotating && !isDeath && !isAttract)
            {
                transform.DOKill();
                isAttacking = false;
                attackCheck.gameObject.SetActive(true);
                //if (CheckGround())
                //{
                //    m_rigid.velocity = Vector3.zero;
                //}
            }
        }

        if (canPatrol)
        {
            Patrol();
        }

        if(dontAttack)
        {
            attackCheck.gameObject.SetActive(false);
        }

        EditorCheck();
    }

    public void Attract()
    {
        if (CheckAttract() && !isDefending && !isAttacking && !isAttract)
        {
            //if(!isAttract)
            //{

            //noAttack = CheckAttract();
            if (Mathf.Abs(attractPos.x - transform.position.x) > moveLength + 0.6f)
            {
                isAttract = true;
                transform.DOKill();
                float moveVec = attractPos.x - transform.position.x;
                if ((faceRight && !(moveVec > 0)) || (!faceRight && moveVec > 0))
                {
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 180, 0);
                    faceRight = !faceRight;
                }

                float distanceScale = moveVec > 0 ? 1 : 1;
       
                //Vector3 movePos = new Vector3(attractPos.x + (transform.localScale.x * attackRange * distanceScale), transform.position.y, transform.position.z);
                Vector3 movePos = new Vector3(attractPos.x - distanceScale * attractMoveRange * transform.localScale.x, transform.position.y, transform.position.z);
                float distance = Mathf.Abs(attractPos.x - transform.position.x);
                float lastY = transform.position.y;
                transform.DOMoveX(movePos.x, distance / attackRushSpeed)
                   .OnComplete(() => {
                       isAttract = false;
                       PlayAnim("chi");
                   })
                   .OnUpdate(() => {
                       if(transform.position.y != lastY)
                       {
                           transform.DOKill();
                           isAttract = false;
                       }
                   })
                   .SetEase(Ease.Linear);
                //transform.position += 
                //transform.DOMoveX(attractPos.x - (moveLength * vec), 1f)
                //  .OnComplete(() => {
                //      isAttract = false;
                //  })
                //  .SetEase(Ease.Linear);
            }
                    
                
                
            //}
            
        }
        else
        {
            //if (isAttract)
            //{
            //    transform.DOKill();
            //}
        }


    }

    public override bool CheckStop()
    {
        return isDeath || isStop || noAttack;
    }
    public override void CheckAttack()
    {
        if (CheckStop() || isDeath || dontAttack)
        {
            attackTarget = null;
            return;
        }
        base.CheckAttack();

    }
    public override void Defend(Transform source)
    {
        base.Defend(source);
    }

    public bool CheckAttract()
    {
        RaycastHit hit1 = new RaycastHit();
        RaycastHit hit2 = new RaycastHit();
        bool hasTarget = Physics.Raycast(attractCheck.position, transform.right, out hit1, attractRange, attractLayer) ||
               Physics.Raycast(attractCheck.position, -transform.right, out hit2, attractRange, attractLayer);
        if(hasTarget)
        {
            if(hit1.transform != null)
            {
                AttractCtrl attract = hit1.transform.GetComponent<AttractCtrl>();
                if(attract != null ) 
                {
                    if(CheckAttractType(attract.type))
                    {
                        attractPos = hit1.point;
                        dontAttack = attract.cantAttack;
                    }
                    else
                    {
                        dontAttack = false;
                        return false;
                    }
                }

            }
            else if(hit2.transform != null)
            {
                AttractCtrl attract = hit2.transform.GetComponent<AttractCtrl>();
                if (attract != null)
                {
                    if (CheckAttractType(attract.type))
                    {
                        attractPos = hit2.point;
                    }
                    else
                    {
                        dontAttack = false;
                        return false;
                    }
                }
            }
        }
        //Debug.DrawRay(right.position, transform.right, Color.blue, checkFrontRange);
        if(!hasTarget)
        {
            dontAttack = false;
        }
        return hasTarget;

    }

    public bool CheckAttractType(GameDefine.AttractType type)
    {
        if (type == GameDefine.AttractType.Honey || type == GameDefine.AttractType.Ball)
        {
            return true;
        }

        return false;

    }
}
