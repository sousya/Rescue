using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BearCtrl: AnimalCtrl
{
    public bool isAttract, noAttack, isDefending;

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

        if (canPatrol)
        {
            Patrol();
        }

        EditorCheck();
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
                    transform.DOMoveX(attractPos.x - (moveLength * vec), 1f)
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
        return isDeath || isStop || noAttack;
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
}
