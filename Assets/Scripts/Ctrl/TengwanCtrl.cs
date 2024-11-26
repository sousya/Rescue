using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TengwanCtrl: MonoBehaviour, IController
{
    public Animator anim;
    public Transform target;
    public bool isGrow, isCheck, isVertical;
    public float moveTime;
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    public void Grow()
    {
        anim.Play("shengzhang");
        isGrow = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other != null && isGrow && !isCheck)
        {
            PlayerCtrl playerCtrl = other.transform.GetComponent<PlayerCtrl>();
            if (playerCtrl != null)
            {
                isCheck = true;
                if (isVertical)
                {
                    playerCtrl.SetUseGravity(false);
                    other.transform.DOMoveY(target.transform.position.y, moveTime).OnComplete(() =>
                    {
                        playerCtrl.SetUseGravity(true);
                    }
                        );
                }
                else
                {
                    float yPos = other.transform.position.y;
                    other.transform.DOMoveX(target.transform.position.x, moveTime)
                        .OnUpdate(() =>
                        {
                        other.transform.position = new Vector3(other.transform.position.x, yPos, other.transform.position.z);
                    });
                }
            }
        }
    }

}
