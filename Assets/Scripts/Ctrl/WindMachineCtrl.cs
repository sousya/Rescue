using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class WindMachineCtrl: MonoBehaviour, IController
{
    [SerializeField] LayerMask layerMask;

    [SerializeField]
    bool changeTrigger, hasTimes, hasTarget, isLeft;
    [SerializeField]
    int times = 1, checkTimes = 0;
    public Transform shootPoint, shootCheck;

    public float windTime = 1f;

    List<Transform> movingTarget = new List<Transform>();
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision != null)
        {
            if (((1 << collision.gameObject.layer) & layerMask) != 0)
            {
                if(movingTarget.Contains(collision.transform))
                {

                }
                else
                {
                    collision.transform.DOKill();
                    movingTarget.Add(collision.transform);
                    PlayerCtrl playerCtrl = collision.transform.GetComponent<PlayerCtrl>();

                    //collision.transform.DOMoveX(shootPoint.position.x, windTime).SetEase(Ease.Linear)
                    collision.transform.DOMove(shootPoint.position, windTime).SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            movingTarget.Remove(collision.transform);
                            if (playerCtrl != null)
                            {
                                playerCtrl.isWind = false;
                            }
                        });
                    if (playerCtrl != null)
                    {
                        playerCtrl.isWind = true;
                    }
                }

            }
               
        }
    }

}
