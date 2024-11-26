using DG.Tweening;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AnimalCtrl: MonoBehaviour, IController
{
    public bool isAttract, canPatrol = false, checkFront = true, checkGround = true, faceRight = true, rushKnight = false, canJuma = false, dontAttack = false;
    public bool isAttacking = false, isRotating = false, isDeath = false, isStop = false, isBounce = false, BounceAttack = false, isWaitingPatrol = false;
    public float speed = 1f, rotateTime = 1f, attackRushSpeed = 3f, attackRange = 1f, checkFrontRange = 0.5f, groundCheckRange = 0.4f;
    public float xMoveScale = 1f, yMoveScale = 0f, zMoveScale = 0f, waitingPartrolTime = 2;
    public Rigidbody m_rigid;
    public Transform right, left, groundCheck, attackCheck, model, front;
    public LayerMask patrolLayer, attackLayer, checkLayer;
    public SpikeCtrl spike;
    public int animalLevel = 0, life = 1;
    public Collider m_collider;
    public Animator anim;
    public string lastAnim;
    public float originZ;

    public Transform attackTarget;
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    public virtual void Start()
    {
        originZ = transform.position.z;
        spike.isAnimalAttack = true;
        //spike.killAnimalLevel = animalLevel;
        spike.layerMask = attackLayer;
        m_collider = GetComponent<Collider>();
        m_rigid = GetComponent<Rigidbody>();
    }

    public virtual void FixedUpdate()
    {
        CheckHit();
        CheckAttack();
        CheckDown();

        if (attackTarget != null)
        {
            Attack();
        }
        else
        {
            if(!isRotating ||(isAttacking && attackTarget == null))
            {
                transform.DOKill();
                isAttacking = false;
                isRotating = false;
                if (faceRight)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }

                //attackCheck.gameObject.SetActive(true);
                if (CheckGround())
                {
                    m_rigid.velocity = Vector3.zero;
                }
            }
        }

        if (!CheckStop())
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
            bool hasTarget = Physics.Raycast(front.position, transform.right, checkFrontRange, patrolLayer);
                //|| Physics.Raycast(left.position, -transform.right, checkFrontRange, patrolLayer);

            return hasTarget;
        }
        else
        {
            return false;
        }
    }

    public virtual void CheckAttack()
    {
        attackTarget = null;
        if (CheckStop() || isDeath)
        {
            
            return;
        }
        RaycastHit hitAttack = new RaycastHit();
        RaycastHit checkFloor = new RaycastHit();

        RaycastHit[] hitAttacks = Physics.RaycastAll(right.position, -transform.right * transform.localScale.x, 10000f, attackLayer);
        RaycastHit[] checkFloors = Physics.RaycastAll(right.position, -transform.right * transform.localScale.x, 10000f, checkLayer);

        Array.Sort(checkFloors, (x, y) => { return x.distance.CompareTo(y.distance); });
        if (hitAttacks.Length > 0)
        {
            if (hitAttacks[0].transform != transform)
            {
                hitAttack = hitAttacks[0];
            }
            else
            {
                if(hitAttacks.Length > 1 && hitAttacks[0].transform == transform)
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
            Array.Sort(checkFloors, (x, y) => { return x.distance.CompareTo(y.distance); });
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
    /// <summary>
    /// 判断脚下是否有路
    /// </summary>
    /// <returns></returns>

    public virtual bool CheckGround()
    {
        if (checkGround)
        {
            RaycastHit hitInfo;
            var hit = Physics.Raycast(groundCheck.position, -transform.up, out hitInfo, groundCheckRange, patrolLayer);
            if(hit)
            {
                BounceAttack = false;
            }
            return hit;
        }
        else
        {
            return true;
        }
    }

    public virtual void CheckDown()
    {
        if(CheckGround())
        {
            if (checkGround)
            {
                //m_rigid.useGravity = false;
            }
        }
        else
        {
            if (checkGround && !isBounce)
            {
                m_rigid.useGravity = true;
            }
        }
    }

    public virtual void Attack()
    {
        if (attackTarget != null && (CheckGround() || BounceAttack) && !dontAttack)
        {
            float moveVec = attackTarget.position.x - transform.position.x;
            if (((faceRight && !(moveVec > 0)) || (!faceRight && moveVec > 0)) && isAttacking)
            {
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 180, 0);
                faceRight = !faceRight;
            }

            isAttacking = true;

            float distanceScale = moveVec > 0 ? 1 : -1;
            Vector3 movePos = new Vector3(attackTarget.position.x + (transform.localScale.x * attackRange * distanceScale), transform.position.y, transform.position.z);
            //Vector3 movePos = new Vector3(attackTarget.position.x, transform.position.y, transform.position.z);
            //Vector3 movePos = new Vector3(attackTarget.position.x - (attackRange / 4), transform.position.y, transform.position.z);
            float distance = Mathf.Abs(attackTarget.position.x - transform.position.x);

            if((distanceScale > 0 && !faceRight) || (distanceScale < 0 && faceRight))
            {
                faceRight = !faceRight;
                if (faceRight)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
            }

            if (attackTarget.gameObject.layer != 15)
            {
                PlayAnim("gongji");

                if (gameObject.layer == 15 && attackTarget.gameObject.layer == 9)
                {
                    attackCheck.gameObject.SetActive(false);
                }
                else
                {
                    attackCheck.gameObject.SetActive(true);
                }


                transform.DOMove(movePos, distance / attackRushSpeed)
                    .OnComplete(() => {
                        isAttacking = false;
                        isRotating = false;
                        if (faceRight)
                        {
                            transform.rotation = Quaternion.Euler(0, 180, 0);
                        }
                        else
                        {
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                    })
                    .SetEase(Ease.Linear);
            }
            else
            {
                if (distance < 1.5f || rushKnight)
                {
                    PlayAnim("gongji");
                    if(gameObject.layer == 15 && attackTarget.gameObject.layer == 9)
                    {
                        attackCheck.gameObject.SetActive(false);
                    }
                    else
                    {
                        attackCheck.gameObject.SetActive(true);
                    }
                }

            }

        }
    }

    public virtual void Patrol()
    {

        if(!isRotating && !isAttacking && !CheckStop() && canPatrol && !isWaitingPatrol)
        {
            bool onGround = CheckGround();
            if (CheckFront() || !onGround)
            {
                isRotating = true;
                isWaitingPatrol = true;
                StartCoroutine(WaitingPatrol());

                transform.DORotateQuaternion(Quaternion.Euler(0, transform.rotation.eulerAngles.y + 180, 0), rotateTime)
                    .OnComplete(() => { 
                        StartCoroutine(WaitRotateEnd());
                        faceRight = !faceRight;
                        if(onGround)
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

    public virtual IEnumerator WaitRotateEnd()
    {
        yield return new WaitForSeconds(0.1f);
        isRotating = false;
    }

    public virtual IEnumerator WaitingPatrol()
    {
        yield return new WaitForSeconds(waitingPartrolTime);
        isWaitingPatrol = false;
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
            gameObject.layer = 18;

            isDeath = true;
            transform.DOKill();
            attackCheck.gameObject.SetActive(false);
            isAttacking = false;
            isRotating = false;
            if (faceRight)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            PlayAnim("siwang");
            
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
        return isDeath || isStop || isBounce;
    }

    public virtual void Defend(Transform source)
    {

    }

    public virtual void OnAttack()
    {
       
    }
    public void PlayAnim(string name)
    {
        if(name == lastAnim && name != "gongji")
        {
            return;
        }
        if (isDeath && !name.Contains("siwang"))
        {
            return;
        }
        anim.enabled = false;
        StartCoroutine(BeginPlay(name));
    }
    IEnumerator BeginPlay(string name)
    {
        yield return new WaitForEndOfFrame();

        anim.enabled = true;
        lastAnim = name;
        anim.Play(name);
    }
}
