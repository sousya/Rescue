using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloorCtrl: MonoBehaviour, IController
{
    [SerializeField] Transform movePoint;
    [SerializeField] bool isCheck = false, isSlide = false, unRopeCheck = true, onceCheck = true;
    [SerializeField] LayerMask playerMask;
    [SerializeField] RopeHangCtrl rope;
    public Collider m_collider; 
    public BrokenFloorCtrl brokenFloor;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Update()
    {
        if (rope != null && rope.targetList.Count == 0 && !unRopeCheck && !m_collider.isTrigger)
        {
            gameObject.layer = 17;
        }
    }

    private void CheckCollider(Transform transform)
    {
        if(rope != null && rope.targetList.Count == 0)
        {
            return;
        }
        if (((1 << transform.gameObject.layer) & playerMask) != 0)
        {
            PlayerCtrl playerCtrl = transform.GetComponent<PlayerCtrl>();
            if (playerCtrl != null)
            {
                if (playerCtrl.targetList.Count == 0 && !playerCtrl.isDeath && !playerCtrl.isWind && playerCtrl.CheckGround())
                {
                    float moveVec = transform.position.x - movePoint.position.x > 0 ? -1 : 1;

                    playerCtrl.SetUseGravity(false);
                    transform.DOKill();
                    playerCtrl.isMoving = true;
                    playerCtrl.anim.SetBool("CanRun", true);
                    playerCtrl.model.rotation = Quaternion.Euler(0, 90 * moveVec, 0);
                    playerCtrl.movingTarget = movePoint;

                    isCheck = true;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (isCheck && onceCheck)
        {
            return;
        }

        if (collision != null)
        {
            CheckCollider(collision.transform);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isCheck && onceCheck)
        {
            return;
        }

        if(collision != null)
        {
            CheckCollider(collision.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (((1 << transform.gameObject.layer) & playerMask) != 0)
        {
            isCheck = false;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (((1 << transform.gameObject.layer) & playerMask) != 0)
        {
            isCheck = false;
        }
    }

    public void OnDeath()
    {
        brokenFloor.OnDeath();
        StartCoroutine(WaitDeath());
    }

    IEnumerator WaitDeath()
    {
        yield return new WaitForSeconds(0.6f);

        Destroy(gameObject);
    }
}
