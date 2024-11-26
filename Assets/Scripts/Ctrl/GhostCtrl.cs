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

    public StageModel stageModel;

    public override void Start()
    {
        base.Start();
        stageModel = this.GetModel<StageModel>();

        this.RegisterEvent<SwitchLightEvent>(e =>
        {
            OnDeath();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
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

    public override void CheckAttack()
    {
        if (stageModel.nowStage.isLight)
            return;
        base.CheckAttack();
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
        isDeath = true;
        attackCheck.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
