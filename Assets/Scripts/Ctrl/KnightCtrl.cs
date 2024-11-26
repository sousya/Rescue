using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class KnightCtrl : AnimalCtrl
{ 
    public bool isAttract, noAttack, isDefending, isRebirth, checkGroundDown = false;

    public LayerMask attractLayer;

    public Vector3 attractPos;

    public float attractRange = 10, moveLength = 0.6f;

    public Transform attractCheck;
    public override void FixedUpdate()
    {

        CheckHit();
        CheckAttack();
        Attract();
        if (attackTarget != null)
        {
            Attack();
        }

        if (!CheckStop())
        {
            Patrol();
        }

        EditorCheck();
    }
    public override void Patrol()
    {

        if (!isRotating && !isAttacking && !CheckStop())
        {
            bool onGround = CheckGround();
            if (CheckFront() || (!onGround && !checkGroundDown))
            {
                isRotating = true;
                transform.DORotateQuaternion(Quaternion.Euler(0, transform.rotation.eulerAngles.y + 180,  0), rotateTime)
                    .OnComplete(() => {
                        StartCoroutine(WaitRotateEnd());
                        faceRight = !faceRight;
                        if (onGround)
                        {
                            transform.position = new Vector3(transform.position.x, transform.position.y, originZ);
                        }
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


    public void Attract()
    {
        if (CheckAttract() && !isDefending)
        {
            if(!isAttract)
            {
              
                 noAttack = CheckAttract();
                if(Mathf.Abs(attractPos.x - transform.position.x)  > moveLength + 0.1f)
                {
                    isAttract = true;
                    transform.DOKill();
                    float vec = attractPos.x - transform.position.x;
                    if(vec > 0)
                    {
                        vec = 1;
                        transform.localScale = new Vector3(1, 1, 1);
                    }
                    else
                    {
                        vec = -1;
                        transform.localScale = new Vector3(-1, 1, 1);
                    }
                    transform.DOMoveX(attractPos.x - (moveLength * vec), 1f / speed)
                      .OnComplete(() => {
                          isAttract = false;
                      })
                      .SetEase(Ease.Linear);
                }
                    
                
                
            }
            
        }
        else
        {
            if (isAttract)
            {
                transform.DOKill();
            }
        }


    }

    public override bool CheckStop()
    {
        return isDeath || isStop || noAttack || isRebirth;
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
                    }
                    else
                    {
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
                        return false;
                    }
                }
            }
        }
        //Debug.DrawRay(right.position, transform.right, Color.blue, checkFrontRange);
        return hasTarget;

    }

    public bool CheckAttractType(GameDefine.AttractType type)
    {
        if (type == GameDefine.AttractType.Honey || type == GameDefine.AttractType.Honey)
        {
            return true;
        }

        return false;

    }

    public override void OnDeath()
    {
        base.OnDeath();
        if(life == 1)
        {
            //TODO 变模型
            PlayAnim("fuhuo");
            isRebirth = true;
            StartCoroutine(WaitRebirth());
        }
    }

    IEnumerator WaitRebirth()
    {
        yield return new WaitForSeconds(2f);

        isRebirth = false;
    }
}
