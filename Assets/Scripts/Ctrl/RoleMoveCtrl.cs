using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoleMoveCtrl: MonoBehaviour, IController
{
    [SerializeField]
    float moveRotation;
    [SerializeField]
    Transform moveTrans, catchPoint;
    bool isCheck = false;
    PlayerCtrl playerCtrl;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Update()
    {
        if(moveTrans != null)
        {
            moveTrans.position = catchPoint.position;
        }
    }

    void BeginRotation()
    {
        //Quaternion rotation = Quaternion.identity;
        //rotation.eulerAngles = new Vector3(0, 0, moveRotation);

        transform.DORotate(new Vector3(0, 0, moveRotation), 1f)
            .SetEase(Ease.Linear).OnComplete(() =>
            {
                moveTrans = null;
                playerCtrl.m_rigid.velocity = Vector3.zero;
                playerCtrl.inRopeMove = false;
                playerCtrl.PlayAnim("tiaoyue");
            });
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(isCheck)
        {
            return;
        }

        playerCtrl = collision.gameObject.GetComponent<PlayerCtrl>();
        if(playerCtrl != null)
        {
            playerCtrl.inRopeMove = true;
            playerCtrl.movingTarget = null;
            isCheck = true;
            moveTrans = playerCtrl.transform;
            BeginRotation();
        }
    }


}
