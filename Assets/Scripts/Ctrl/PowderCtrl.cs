using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowderCtrl: MonoBehaviour, IController
{
    public LayerMask layerMask;

    [SerializeField]
    Transform checkBomb;
    [SerializeField]
    Rigidbody m_rigid;
    bool waitDeath = false;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Awake()
    {
        //m_rigid = GetComponent<Rigidbody>();
        //m_collider = GetComponent<Collider>();

        //SetUseGravity(false);
        RegisterEvent();
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

    public void RemoveList(Transform target)
    {
        Rigidbody targetRigid = target.GetComponent<Rigidbody>();


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            Debug.Log("Õ¨µ¯Åö×²");

            Transform player = collision.transform;

            if (((1 << player.gameObject.layer) & layerMask) != 0)
            {
                PlayerCtrl playerCtrl = player.GetComponent<PlayerCtrl>();
                if (playerCtrl != null)
                {
;
                    //playerCtrl.OnDeath();
                    checkBomb.gameObject.SetActive(true);
                    StartCoroutine(WaitDeath());
                    m_rigid.useGravity = false;

                }
                else
                {
                    ShooterCtrl shooterCtrl = player.GetComponent<ShooterCtrl>();
                    if (shooterCtrl != null)
                    {
                        //shooterCtrl.OnDeath();
                        checkBomb.gameObject.SetActive(true);
                        m_rigid.useGravity = false;
                        StartCoroutine(WaitDeath());

                    }
                    else
                    {
                        Debug.Log("Õ¨µ¯Åö×²333");
                        BrokenFloorCtrl brokenFloorCtrl = player.GetComponent<BrokenFloorCtrl>();
                        if(brokenFloorCtrl != null)
                        {
                            Debug.Log("Õ¨µ¯Åö×²2222");
                            checkBomb.gameObject.SetActive(true);
                            m_rigid.useGravity = false;
                            brokenFloorCtrl.OnDeath();
                            StartCoroutine(WaitDeath());
                        }
                    }

                }
            }
            else
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
            waitDeath = true;
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }
    }


}
