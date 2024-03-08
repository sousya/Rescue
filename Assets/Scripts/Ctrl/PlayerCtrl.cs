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
    bool isLock = true;
    public bool isDeath = false, isCarryingBox = false, isWind = false, isWeapon = false;
    public Transform boxPoint, weaponNode;
    public PushBoxCtrl carryBox;
    public GameObject bulletPrefab;
    public float bulletSpeed = 1f, attackCooldown = 1f, time = 0f;
    public LayerMask weaponShootLayer;
    public bool hasTarget = false, hasTargetRight = false, hasTargetLeft = false;
    public Transform shootLeftPoint, shootRightPoint;

    CharacterJoint[] tempList;

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
    public virtual void FixedUpdate()
    {

        hasTargetRight = Physics.Raycast(shootRightPoint.position, Vector3.right, 1000, weaponShootLayer);
        hasTargetLeft = Physics.Raycast(shootLeftPoint.position, Vector3.left, 1000, weaponShootLayer);

        hasTarget = hasTargetRight || hasTargetLeft;


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
                    break;
                }
            }
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

    public void OnDeath()
    {
        if(isDeath)
        {
            return;
        }
        isDeath = true;
        transform.DOKill();
        m_rigid.velocity = Vector3.zero;

        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90)) ;
        Debug.Log("死亡");
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
            }
            else if (hasTargetRight)
            {
                GameObject go = GameObject.Instantiate(bulletPrefab, shootRightPoint);

                BulletCtrl bulletCtrl = go.GetComponent<BulletCtrl>();
                bulletCtrl.moveVec = Vector3.right * bulletSpeed;
            }
            time = 0;
        }
    }
}
