using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GhostCtrl : AnimalCtrl
{
    public bool isAttract, noAttack, isDefending;

    public LayerMask attractLayer;

    public Vector3 attractPos;

    public float attractRange = 10, moveLength = 0.6f;

    public Transform attractCheck;

    StageModel stageModel;

    public override void Start()
    {
        base.Start();
        stageModel = this.GetModel<StageModel>();
    }
    public override void FixedUpdate()
    {

        CheckHit();
        CheckAttack();
        CheckLight();
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

    public void CheckLight()
    {
        if(stageModel != null)
        {
            if(stageModel.nowStage.isLight)
            {
                OnDeath();
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

    public override void OnDeath()
    {
       
    }
}
