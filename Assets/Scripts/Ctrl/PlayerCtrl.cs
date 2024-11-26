using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour, IController
{
    public float moveSpeed = 1f;
    [HideInInspector]
    public Rigidbody m_rigid;
    [HideInInspector]
    public Collider m_collider;
    public List<CharacterJoint> targetList;

    [SerializeField]
    bool isLock = true, lastMoving = false;
    public Animator anim;
    public bool isDeath = false, isCarryingBox = false, isWind = false, isWeapon = false, isMoving = false, isWin = false, isGrounded = false, inRotation = false, inSlope = false;
    public bool isBoucing;
    public bool inRopeMove = false;
    public Transform boxPoint, weaponNode, groundCheck, model, movingTarget;
    public PushBoxCtrl carryBox;
    public GameObject bulletPrefab;
    public float bulletSpeed = 1f, attackCooldown = 1f, time = 0f;
    public LayerMask weaponShootLayer, patrolLayer;
    public bool hasTarget = false, hasTargetRight = false, hasTargetLeft = false;
    public Transform shootLeftPoint, shootRightPoint, zhuazi;
    public GravityModifier gravity;

    string lastAnim;

    CharacterJoint[] tempList;
    List<RopeCtrl> ropeList = new List<RopeCtrl>();

    [SerializeField]
    LayerMask groundLayer;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();
        tempList = GetComponents<CharacterJoint>();
        foreach (CharacterJoint joint in tempList)
        {
            targetList.Add(joint);
        }
        tempList = null;

        //SetUseGravity(false);
        RegisterEvent();
    }


    private void Start()
    {
        time = attackCooldown;
    }

    private void Update()
    {
        CarryBox();
        time += Time.deltaTime;
        if (time >= attackCooldown)
        {
            Attack();
        }
    }

    public virtual bool CheckCanMove()
    {
        return !isDeath && !isWind;
    }

    public virtual void FixedUpdate()
    {

        hasTargetRight = Physics.Raycast(shootRightPoint.position, Vector3.right, 1000, weaponShootLayer);
        hasTargetLeft = Physics.Raycast(shootLeftPoint.position, Vector3.left, 1000, weaponShootLayer);

        hasTarget = hasTargetRight || hasTargetLeft;

        bool nowGrounded = CheckGround();
        if(!isGrounded && nowGrounded && !isDeath)
        {
            PlayAnim("luodi");
        }
        if(movingTarget != null && CheckCanMove())
        {
            var currentPosition = transform.position;
            Vector3 movePos = new Vector3(movingTarget.position.x, transform.position.y, transform.position.z);
            Vector3 directionToTarget = movePos - transform.position;
            transform.position = Vector3.MoveTowards(currentPosition, movePos, 1 * Time.deltaTime * moveSpeed);

            // 检查是否接近目标位置，如果足够接近则停止移动
            if (Vector3.Distance(transform.position, movePos) < 0.1f) // 你可以根据需要调整这个阈值
            {
                movingTarget = null;
                isMoving = false; // 设置标志为false表示停止移动
            }
        }

        isGrounded = nowGrounded;

        CheckDown();       
    }
    public virtual bool CheckGround()
    {
        bool onGround = Physics.Raycast(groundCheck.position, -transform.up, 0.11f, patrolLayer);

        //Debug.Log(Physics.Raycast(groundCheck.position, -transform.up, 0.5f, patrolLayer));
        return onGround;
    }
    public virtual void CheckDown()
    {
        if(inSlope)
        {
            SetUseGravity(inSlope);
            return;
        }

        if(inRopeMove || isBoucing)
        {
            SetUseGravity(false);
            return;
        }

        if (CheckGround() && !inRotation && !isWind)
        {
            SetUseGravity(true);
            m_rigid.velocity = Vector3.zero;
            SetIsTrigger(false);
        }
        else
        {
            SetUseGravity(true);

            if (!isGrounded)
            {
                anim.SetBool("CanRun", isGrounded);
            }
            //SetIsTrigger(true);
        }

    }

    public void SetUseGravity(bool isOn)
    {
        m_rigid.useGravity = isOn;
    }

    public void SetIsTrigger(bool isOn)
    {
        m_collider.isTrigger = isOn;
    }

    public void SetVelocity(Vector3 velocity)
    {
        m_rigid.velocity = velocity;
    }

    public void RemoveList(Transform target)
    {
        Rigidbody targetRigid = target.GetComponent<Rigidbody>();

        if (targetList.Count > 0)
        {
            foreach (CharacterJoint joint in targetList)
            {
                if (joint.connectedBody == targetRigid)
                {
                    targetList.Remove(joint);
                    Destroy(joint);
                   target.gameObject.SetActive(false);
                    SetIsTrigger(false);
                    break;
                }
            }
        }

        if(targetList.Count == 0)
        {
            PlayAnim("tiaoyue");
            zhuazi.gameObject.SetActive(false);
        }
    }

    void RegisterEvent()
    {
        this.RegisterEvent<UnRopeEvent>(e =>
        {
            OnUnRope(e);
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    void OnUnRope(UnRopeEvent e)
    {
        RemoveList(e.rope);
    }

    public void OnDeath(int useDeath = 1)
    {
        if(isDeath)
        {
            return;
        }

        foreach(var rope in ropeList)
        {
            rope.ShrinkRope();
        }
        isDeath = true;
        transform.DOKill();
        m_rigid.velocity = Vector3.zero;
        gameObject.layer = 18;
        PlayAnim("siwang" + useDeath);
    }

    public void StartCarryBox(PushBoxCtrl pushBoxCtrl)
    {
        isCarryingBox = true;
        carryBox = pushBoxCtrl;
        carryBox.transform.position = boxPoint.position;
    }

    public void CarryBox()
    {
        if(carryBox != null)
        {
            carryBox.transform.position = boxPoint.position;            
        }
    }
    public virtual void Attack()
    {
        if (hasTarget && isWeapon)
        {

            if (hasTargetLeft)
            {
                GameObject go = GameObject.Instantiate(bulletPrefab, shootLeftPoint);

                BulletCtrl bulletCtrl = go.GetComponent<BulletCtrl>();
                bulletCtrl.moveVec = Vector3.left * bulletSpeed;
                model.rotation = Quaternion.Euler(0, -90, 0);
            }
            else if (hasTargetRight)
            {
                GameObject go = GameObject.Instantiate(bulletPrefab, shootRightPoint);

                BulletCtrl bulletCtrl = go.GetComponent<BulletCtrl>();
                bulletCtrl.moveVec = Vector3.right * bulletSpeed;
                model.rotation = Quaternion.Euler(0, 90, 0);
            }
            time = 0;
            PlayAnim("kaiqiang");

        }
    }

    public void PlayAnim(string name)
    {
        if(lastAnim == "kaibaoxiang" && name != "siwang")
        {
            return;
        }

        if(isDeath && !name.Contains("siwang"))
        { 
            return;
        }

        if(name != "kaiqiang" && name != "pao")
        {
            model.rotation = Quaternion.Euler(0, 180, 0);
        }

        if(anim.GetBool("CanRun") && name == "luodi")
        {
            float moveVec = transform.position.x - movingTarget.position.x > 0 ? -1 : 1;
            transform.DOKill();
            model.rotation = Quaternion.Euler(0, 90 * moveVec, 0);
        }
        //Debug.Log(name);

        lastAnim = name;

        anim.enabled =false;
        StartCoroutine(BeginPlay(name));
    }
    IEnumerator BeginPlay(string name)
    {
        yield return new WaitForEndOfFrame();

        anim.enabled = true;
        anim.Play(name);
    }

}
