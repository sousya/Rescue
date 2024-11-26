using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LightCtrl: MonoBehaviour, IController
{
    [SerializeField] LayerMask layerMask;

    bool _hitPlayer;
    [SerializeField]
    bool isLight = false, checkGround = false, canDown = false;
    [SerializeField]
    int times = 1, checkTimes = 0, killAnimalLevel;
    [SerializeField]
    float groundCheckRange;
    [SerializeField]
    GameObject lightFx;
    [SerializeField]
    Collider m_Collider;
    [SerializeField]
    Rigidbody m_Rigidbody;
    [SerializeField]
    LayerMask patrolLayer;
    [SerializeField]
    Color darkColor, lightColor;

    public bool hitPlayer
    {
        get
        {
            return _hitPlayer;
        }
    }

    private void Update()
    {
        CheckGround();
    }

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Start()
    {
        LevelManager.Instance.SetDarkLight(false);

        //StartCoroutine(SetDarkLight());
    }
    public virtual bool CheckCollider(Transform player)
    {
        if (((1 << player.gameObject.layer) & layerMask) != 0)
        {
            isLight = !isLight;
            lightFx.SetActive(isLight);
            LevelManager.Instance.SetDarkLight(true);


            this.GetModel<StageModel>().nowStage.ChangeLight(isLight); 
        }
        return false;
    }

    public virtual void OnTriggerEnter(Collider collision)
    {
        if (collision != null)
        {
            Transform player = collision.transform;

            CheckCollider(player);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 7 || collision.gameObject.layer == 11)
        {
            m_Collider.isTrigger = true;
            m_Rigidbody.isKinematic = true;
        }
    }
    public virtual void CheckGround()
    {
        if (checkGround)
        {
            RaycastHit hitInfo;
            var hit = Physics.Raycast(transform.position, -transform.forward, out hitInfo, groundCheckRange, patrolLayer);
            
            if(!hit)
            {
                m_Collider.isTrigger = false;
                m_Rigidbody.isKinematic = false;
            }
            else
            {
                m_Collider.isTrigger = true;
                m_Rigidbody.isKinematic = true;
            }
        }
        else
        {
            m_Collider.isTrigger = true;
            m_Rigidbody.isKinematic = true;
        }
    }

    public void OnDeath()
    {
        Destroy(gameObject);
    }
 
}
