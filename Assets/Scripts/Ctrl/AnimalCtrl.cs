using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AnimalCtrl: MonoBehaviour, IController
{
    public bool canPatrol = false, checkFront = true, checkGround = true;
    public bool isAttacking = false, isRotating = false, isDeath = false, isStop = false;
    public float speed = 1f, rotateTime = 1f, attackRushSpeed = 3f, attackRange = 1f, checkFrontRange = 0.5f;
    public float xMoveScale = 1f, yMoveScale = 0f, zMoveScale = 0f;
    public Rigidbody m_rigidBody;
    public Transform right, left, groundCheck, attackCheck;
    public LayerMask patrolLayer, attackLayer;
    public SpikeCtrl spike;
    public int animalLevel = 0, life = 1;

    public Transform attackTarget;
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    public virtual void Start()
    {
        spike.isAnimalAttack = true;
        spike.killAnimalLevel = animalLevel - 1;
        spike.layerMask = attackLayer;
    }

    public virtual void FixedUpdate()
    {
        CheckHit();
        CheckAttack();

        if (attackTarget != null)
        {
            Attack();
        }
        else
        {
            if(!isRotating)
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
    /// <summary>
    /// 判断前方是否有障碍
    /// </summary>
    /// <returns></returns>
    public virtual bool CheckFront()
    {
        if(checkFront)
        {
            bool hasTarget = Physics.Raycast(right.position, transform.right, checkFrontRange, patrolLayer) ||
                Physics.Raycast(left.position, -transform.right, checkFrontRange, patrolLayer);

            return hasTarget;
        }
        else
        {
            return false;
        }
    }

    public virtual void CheckAttack()
    {
        if(CheckStop())
        {
            attackTarget = null;
            return;
        }
        RaycastHit hitAttack;
        RaycastHit checkFloor;
        Physics.Raycast(right.position, transform.right * transform.localScale.x, out hitAttack, 10000f, attackLayer);
        Physics.Raycast(right.position, transform.right * transform.localScale.x, out checkFloor, 10000f);

        if(checkFloor.transform!= null)
        {
            if (checkFloor.transform.gameObject.layer != 7 || checkFloor.transform.gameObject.layer != 11)
            {
                attackTarget = hitAttack.transform;
            }
        }
        
    }
    /// <summary>
    /// 判断脚下是否有路
    /// </summary>
    /// <returns></returns>

    public virtual bool CheckGround()
    {
        if (checkGround)
        {
            m_rigidBody.useGravity = false;
            return Physics.Raycast(groundCheck.position, -transform.up, 1f, patrolLayer);
        }
        else
        {
            m_rigidBody.useGravity = true;
            return true;
        }
    }

    public virtual void Attack()
    {
        if (attackTarget != null)
        {
            isAttacking = true;
            float moveVec = attackTarget.position.x - transform.position.x;
            Vector3 movePos = new Vector3(attackTarget.position.x - (attackRange / 4), transform.position.y, transform.position.z);
            float distance = Mathf.Abs(attackTarget.position.x - transform.position.x);
            transform.DOMove(movePos, attackRushSpeed)
                .OnComplete(() => {
                    attackCheck.gameObject.SetActive(true);
                    isAttacking = false;
                })
                .SetEase(Ease.Linear);
        }
    }

    public virtual void Patrol()
    {

        if(!isRotating && !isAttacking && !CheckStop())
        {
            if (CheckFront() || !CheckGround())
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

    public virtual void EditorCheck()
    {
#if UNITY_EDITOR
        Debug.DrawRay(transform.position, transform.right, Color.blue);
#endif

    }

    public virtual void OnDeath()
    {
        life--;
        if(life == 0)
        {
            isDeath = true;
            transform.DOKill();

            Destroy(gameObject);
        }
        

    }

    public virtual void CheckHit()
    {
        if(spike.hitPlayer)
        {
            isStop = true;
            //transform.DOKill();
        }
    }

    public virtual bool CheckStop()
    {
        return isDeath || isStop;
    }

    public virtual void Defend(Transform source)
    {

    }

    public virtual void OnAttack()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("碰撞 " + collision.gameObject.name);
    }
}
