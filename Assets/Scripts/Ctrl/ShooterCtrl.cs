using DG.Tweening;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class ShooterCtrl : MonoBehaviour, IController
{
    public Animator anim;
    public Transform shootLeftPoint, shootRightPoint, groundCheck, model, front, checkRightPoint, checkLeftPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 1f, attackCooldown = 1f, time = 0f, speed = 1f, rotateTime = 1f, checkFrontRange = 0.5f, groundCheckRange = 0.2f;
    public LayerMask layerMask, patrolLayer, wallLayer;
    public bool hasTarget = false, hasTargetRight = false, hasTargetLeft = false, isDeath = false, isGrounded = false, canPatrol = false, isRotating = false, movingRight = false, isWind = false, isAttacking, faceRight = true;
    public bool isPatroling = false, checkFront = true, bouncingAttack = false, isBouncing, unCheckGroundPatrol = false;
    public int killAnimalLevel = 0;
    [HideInInspector]
    public Rigidbody m_rigid;
    [HideInInspector]
    public Collider m_collider;
    public string lastAnim;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    public virtual void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();

        //bulletPrefab.GetComponent<BulletCtrl>().layerMask = layerMask;
        //bulletPrefab.GetComponent<BulletCtrl>().killAnimalLevel = killAnimalLevel;

        time = attackCooldown;

    }

    public virtual void Update()
    {
        time += Time.deltaTime;
        if(time >= attackCooldown)
        {
            Attack();
        }

    }

    public virtual void FixedUpdate()
    {
        if(isWind)
        {
            return;
        }
        RaycastHit hitInfoRight, hitInfoLeft, checkRightHit, checkLeftHit;
        hasTargetRight = Physics.Raycast(checkRightPoint.position, Vector3.right, out hitInfoRight, 10000, layerMask);
        hasTargetLeft = Physics.Raycast(checkLeftPoint.position, Vector3.left, out hitInfoLeft, 10000, layerMask);
        Physics.Raycast(checkRightPoint.position, Vector3.right, out checkRightHit, 10000, wallLayer);
        Physics.Raycast(checkLeftPoint.position, Vector3.left, out checkLeftHit, 10000, wallLayer);

        hasTargetRight = (hitInfoRight.transform == checkRightHit.transform) && hasTargetRight;
        hasTargetLeft = (hitInfoLeft.transform == checkLeftHit.transform) && hasTargetLeft;

        hasTarget = hasTargetRight || hasTargetLeft;

        bool nowGrounded = CheckGround();

       

        isGrounded = nowGrounded;

        CheckDown();

        //bool lastPatrol = isPatroling;
        if (canPatrol && !hasTarget)
        {
            Patrol();
        }

        if (!isDeath && !isRotating)
        {
            //Debug.Log("isGrounded " + isGrounded + "  nowGrounded " + nowGrounded);
            if (!isGrounded && nowGrounded)
            {
                PlayAnim("luodi");
            }
            else if (isGrounded && !nowGrounded)
            {
                PlayAnim("tiaoyue");
            }
        }
    }

    public virtual void Patrol()
    {

         if(!isRotating && !isAttacking && !CheckStop() && !isWind)
        {
            int vecScale = 1;
            if (!faceRight)
            {
                vecScale = -1;
            }

            bool onGround = CheckGround() || unCheckGroundPatrol;
            if (!isRotating)
            {
                if (CheckFront() || !onGround)
                {
                    isRotating = true;
                    model.transform.DORotateQuaternion(Quaternion.Euler(0, model.transform.rotation.eulerAngles.y + 180, 0), rotateTime)
                        .OnComplete(() => {
                            StartCoroutine(WaitRotateEnd());
                            faceRight = !faceRight;
                            isRotating = false;
                            //if (onGround)
                            //{
                            //    transform.position = new Vector3(transform.position.x, transform.position.y, originZ);
                            //}
                        })
                        .SetEase(Ease.Linear);
                    //transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 180, 0);
                }
                else
                {

                    transform.position += transform.right * Time.fixedDeltaTime * speed * vecScale;
                    PlayAnim("pao");
                    if (faceRight)
                    {
                        model.transform.rotation = Quaternion.Euler(0, 90, 0);
                    }
                    else
                    {
                        model.transform.rotation = Quaternion.Euler(0, -90, 0);
                    }
                }
            }
            else
            {
                transform.position += transform.right * Time.fixedDeltaTime * speed;
                PlayAnim("pao");
                if (faceRight)
                {
                    model.transform.rotation = Quaternion.Euler(0, 90, 0);
                }
                else
                {
                    model.transform.rotation = Quaternion.Euler(0, -90, 0);
                }
            }


        }

    }
    public virtual IEnumerator WaitRotateEnd()
    {
        yield return new WaitForSeconds(0.1f);
        isRotating = false;
    }

    public virtual bool CheckFront()
    {
        if (checkFront)
        {
            RaycastHit hitinfo;
            bool hasTarget = Physics.Raycast(front.position, model.transform.forward, out hitinfo, checkFrontRange, patrolLayer);
            //|| Physics.Raycast(left.position, -transform.right, checkFrontRange, patrolLayer);

            return hasTarget;
        }
        else
        {
            return false;
        }
    }

    public virtual bool CheckRight()
    {
        if (canPatrol)
        {
            bool hasTarget = Physics.Raycast(checkRightPoint.position, transform.right, checkFrontRange, patrolLayer);
            //|| Physics.Raycast(left.position, -transform.right, checkFrontRange, patrolLayer);

            return hasTarget;
        }

        return false;
    }

    public virtual bool CheckLeft()
    {
        if (canPatrol)
        {
            bool hasTarget = Physics.Raycast(checkLeftPoint.position, -transform.right, checkFrontRange, patrolLayer);
            //|| Physics.Raycast(left.position, -transform.right, checkFrontRange, patrolLayer);

            return hasTarget;
        }

        return false;
    }

    public virtual bool CheckStop()
    {
        return isDeath ;
    }

    public virtual void CheckDown()
    {
        if (CheckGround())
        {
            //m_rigid.useGravity = false;
            m_rigid.velocity = Vector3.zero;
        }
        else
        {
            m_rigid.useGravity = true;
        }
    }

    public virtual bool CheckGround()
    {

        RaycastHit hitInfo;
        var hit = Physics.Raycast(groundCheck.position, -transform.up, out hitInfo, groundCheckRange, patrolLayer);
        return hit;

        //Debug.Log(Physics.Raycast(groundCheck.position, -transform.up, 0.5f, patrolLayer));
        //return Physics.Raycast(groundCheck.position, -transform.up, 0.2f, patrolLayer);
    }

    public virtual void Attack()
    {
        if (hasTarget && !isDeath)
        {
            if(!bouncingAttack && isBouncing)
            {
                return;
            }
            isPatroling = false;
            isRotating = false;
            model.DOKill();
            model.rotation = Quaternion.identity;
            PlayAnim("kaiqiang");

            if (hasTargetLeft)
            {
                GameObject go = GameObject.Instantiate(bulletPrefab, shootLeftPoint);

                BulletCtrl bulletCtrl = go.GetComponent<BulletCtrl>();
                bulletCtrl.moveVec = Vector3.left * bulletSpeed;
                bulletCtrl.killAnimalLevel = killAnimalLevel;
                bulletCtrl.layerMask = layerMask;
                go.transform.localRotation = Quaternion.Euler(90, 90, 0);

                model.rotation = Quaternion.Euler(0, -90, 0);
            }
            else if(hasTargetRight)
            {
                GameObject go = GameObject.Instantiate(bulletPrefab, shootRightPoint);

                BulletCtrl bulletCtrl = go.GetComponent<BulletCtrl>();
                bulletCtrl.moveVec = Vector3.right * bulletSpeed;
                bulletCtrl.layerMask = layerMask;
                bulletCtrl.killAnimalLevel = killAnimalLevel;

                model.rotation = Quaternion.Euler(0, 90, 0);
            }
            time = 0;

        }
        else
        {
            if(canPatrol)
            {
                if (!isRotating || (isAttacking && (hasTarget)))
                {
                    transform.DOKill();
                    isAttacking = false;
                    //attackCheck.gameObject.SetActive(true);
                    if (CheckGround())
                    {
                        m_rigid.velocity = Vector3.zero;
                    }
                }
            }
            else
            {
                if (!isDeath)
                {
                    PlayAnim("daiji");
                }
            }
          
        }
    }

    public virtual void OnDeath()
    {
        if (isDeath)
        {
            return;
        }
        isDeath = true;
        transform.DOKill();

        gameObject.layer = 18;
        m_rigid.velocity = Vector3.zero;
        

        PlayAnim("siwang");
    }
    //public void PlayAnim(string name)
    //{
    //    //Debug.Log("²¥·Å " + name);
    //    if (name != "kaiqiang" && name != "pao" && name != "zoulu")
    //    {
    //        model.localRotation = Quaternion.Euler(0, 180, 0);
    //    }
    //    else
    //    {
    //        model.localRotation = Quaternion.Euler(0, 90, 0);
    //    }

    //    anim.enabled = false;
    //    anim.enabled = true;
    //    anim.Play(name);
    //}
    public void PlayAnim(string name)
    {
        if (name == lastAnim && name != "kaiqiang")
        {
            return;
        }
        if (isDeath && !name.Contains("siwang"))
        {
            return;
        }
        //if (name != "kaiqiang" && name != "pao")
        //{
        //    model.rotation = Quaternion.Euler(0, 180, 0);
        //}
        //Debug.Log("animal " + name);
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
