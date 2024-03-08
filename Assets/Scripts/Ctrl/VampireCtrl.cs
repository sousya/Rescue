using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class VampireCtrl : GhostCtrl
{

    public override void Start()
    {
        base.Start();
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
    public override void CheckAttack()
    {
        if (CheckStop())
        {
            attackTarget = null;
            return;
        }
        RaycastHit hitAttack;
        Physics.Raycast(right.position, transform.right * transform.localScale.x, out hitAttack, 10000f, attackLayer);

        attackTarget = hitAttack.transform;
    }

    public override bool CheckStop()
    {
        return isDeath || isStop || noAttack;
    }

    public override void Defend(Transform source)
    {
        base.Defend(source);
    }
}
