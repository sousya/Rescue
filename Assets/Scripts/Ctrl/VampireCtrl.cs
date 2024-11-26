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
    public GameObject bianfu;
    public Animator bianfuAnim;
    public bool lastLight = false;

    public override void Start()
    {
        base.Start();
    }
    public override void FixedUpdate()
    {
        CheckGround();
        CheckHit();
        CheckAttack();
        CheckLight();
        if (attackTarget != null)
        {
            Attack();
        }

        if(stageModel.nowStage.isLight || attackTarget == null)
        {
            attackCheck.gameObject.SetActive(false);
        }

        if (canPatrol)
        {
            Patrol();
        }

        if(!lastLight && stageModel.nowStage.isLight)
        {
            Sleep();
        }
        lastLight = stageModel.nowStage.isLight;

        EditorCheck();
    }
    public override void CheckAttack()
    {
        if (stageModel.nowStage.isLight)
            return;

        attackTarget = null;
        if (CheckStop() || isDeath)
        {
            attackTarget = null;
            return;
        }
        RaycastHit hitAttack = new RaycastHit();
        RaycastHit checkFloor = new RaycastHit();

        RaycastHit[] hitAttacks = Physics.RaycastAll(right.position, -transform.right * transform.localScale.x, 10000f, attackLayer);
        RaycastHit[] checkFloors = Physics.RaycastAll(right.position, -transform.right * transform.localScale.x, 10000f, checkLayer);
        if (hitAttacks.Length > 0)
        {
            if (hitAttacks[0].transform != transform)
            {
                hitAttack = hitAttacks[0];
            }
            else
            {
                if (hitAttacks.Length > 1 && hitAttacks[0].transform == transform)
                {
                    hitAttack = hitAttacks[1];
                }
            }
        }

        if (checkFloors.Length > 0)
        {
            if (checkFloors[0].transform != transform)
            {
                checkFloor = checkFloors[0];
            }
            else
            {
                if (checkFloors.Length > 1 && checkFloors[0].transform == transform)
                {
                    checkFloor = checkFloors[1];
                }
            }
        }

        if (checkFloor.transform != null)
        {
            if (checkFloor.transform.gameObject.layer != 7 && checkFloor.transform.gameObject.layer != 11)
            {

                attackTarget = hitAttack.transform;
                if (hitAttack.transform != null && hitAttack.transform.gameObject.layer == 9)
                {
                    AnimalCtrl animalCtrl = transform.GetComponent<AnimalCtrl>();
                    if (animalCtrl != null)
                    {
                        if (animalCtrl.animalLevel == animalLevel)
                        {
                            attackTarget = null;
                        }
                    }
                }
            }
            else
            {
                attackTarget = null;
            }
        }

        if (attackTarget == null)
        {

            hitAttacks = Physics.RaycastAll(right.position, transform.right * transform.localScale.x, 10000f, attackLayer);
            checkFloors = Physics.RaycastAll(right.position, transform.right * transform.localScale.x, 10000f, checkLayer);

            if (hitAttacks.Length > 0)
            {
                if (hitAttacks[0].transform != transform)
                {
                    hitAttack = hitAttacks[0];
                }
                else
                {
                    if (hitAttacks.Length > 1 && hitAttacks[0].transform == transform)
                    {
                        hitAttack = hitAttacks[1];
                    }
                }
            }

            if (checkFloors.Length > 0)
            {
                if (checkFloors[0].transform != transform)
                {
                    checkFloor = checkFloors[0];
                }
                else
                {
                    if (hitAttacks.Length > 1 && checkFloors[0].transform == transform)
                    {
                        checkFloor = checkFloors[1];
                    }
                }
            }
            if (checkFloor.transform != null)
            {
                if (checkFloor.transform.gameObject.layer != 7 && checkFloor.transform.gameObject.layer != 11)
                {
                    attackTarget = hitAttack.transform;
                    if (hitAttack.transform != null && hitAttack.transform.gameObject.layer == 9)
                    {
                        AnimalCtrl animalCtrl = hitAttack.transform.GetComponent<AnimalCtrl>();
                        if (animalCtrl != null)
                        {
                            if (animalCtrl.animalLevel == animalLevel)
                            {
                                attackTarget = null;
                            }
                        }
                    }
                }
                else
                {
                    attackTarget = null;
                }
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

    public void Change(bool show)
    {
        model.gameObject.SetActive(show);
        bianfu.SetActive(!show);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (stageModel.nowStage.isLight)
            return;
        if (collision.gameObject.layer == 7 && isAttacking)
        {
            Change(false);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (stageModel.nowStage.isLight)
            return;
        if (collision.gameObject.layer == 7)
        {
            Change(true);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (stageModel.nowStage.isLight)
            return;
        if (collision.gameObject.layer == 7 && isAttacking)
        {
            Change(false);
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (stageModel.nowStage.isLight)
            return;
        if (collision.gameObject.layer == 7)
        {
            Change(true);
        }
    }

    public void Sleep()
    {

        PlayAnim("siwang");
        StartCoroutine(AfterShowDeath());
    }

    public override void OnDeath()
    {
    }

    IEnumerator AfterShowDeath()
    {
        yield return new WaitForSeconds(3f);
        Change(false);
        bianfuAnim.Play("siwang");
    }

    public override bool CheckGround()
    {
        if (checkGround)
        {
            RaycastHit hitInfo;
            var hit = Physics.Raycast(transform.position, -transform.up, out hitInfo, groundCheckRange, patrolLayer);

            if (!hit)
            {
                m_collider.isTrigger = false;
                m_rigid.isKinematic = false;
            }
            else
            {
                m_collider.isTrigger = true;
                m_rigid.isKinematic = true;
            }
            return hit;
        }
        else
        {
            m_collider.isTrigger = true;
            m_rigid.isKinematic = true;
            return true;
        }
        return false;
    }
}
