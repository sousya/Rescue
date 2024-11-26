using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BombCtrl : MonoBehaviour, IController, ICanSendEvent
{
    public LayerMask layerMask;

    [SerializeField]
    Transform checkBomb, model, zhuazi;
    public Rigidbody m_rigid;
    bool waitDeath = false;
    public List<CharacterJoint> targetList;
    CharacterJoint[] tempList;
    [SerializeField]
    RopeCtrl[] ropeList;
    

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
        //m_collider = GetComponent<Collider>();
        //tempList = GetComponents<CharacterJoint>();
        //foreach (CharacterJoint joint in tempList)
        //{
        //    targetList.Add(joint);
        //}
        //tempList = null;

        //SetUseGravity(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            if (((1 << collision.gameObject.layer) & layerMask) != 0)
            {
                StartCoroutine(WaitDeath());
            }
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision != null)
        {
            if (((1 << collision.gameObject.layer) & layerMask) != 0)
            {
                StartCoroutine(WaitDeath());
            }
        }
    }

    IEnumerator WaitDeath()
    {

        if (waitDeath)
        {
        }
        else  
        { 
            checkBomb.gameObject.SetActive(true);
            model.gameObject.SetActive(false);
            zhuazi.gameObject.SetActive(false);
            m_rigid.useGravity = false;
            m_rigid.velocity = Vector3.zero;
            transform.DOKill();
            waitDeath = true;
            
            this.SendEvent<PauceSequence>(new PauceSequence { go = gameObject });

            foreach(var r in ropeList)
            {
                r.ShrinkRope();
            }
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }
    }


}
