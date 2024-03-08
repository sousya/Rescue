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
    [SerializeField] bool isCheck = false;
    [SerializeField] LayerMask playerMask;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Start()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(isCheck)
        {
            return;
        }

        if(collision != null)
        {
            Transform player = collision.transform;
            if (((1 << player.gameObject.layer) & playerMask) != 0)
            {
                PlayerCtrl playerCtrl = player.GetComponent<PlayerCtrl>();
                if(playerCtrl != null)
                {
                    if(playerCtrl.targetList.Count == 0 && !playerCtrl.isDeath && !playerCtrl.isWind)
                    {
                        float distance = Mathf.Abs(player.position.x - movePoint.position.x);

                        playerCtrl.SetUseGravity(false);
                        player.DOKill();

                        Tweener tweener = player.DOMove(new Vector3(movePoint.position.x, player.transform.position.y, player.transform.position.z), distance / playerCtrl.moveSpeed);
                        tweener.OnComplete(() =>
                        {
                            playerCtrl.SetUseGravity(true);
                        });
                        tweener.SetEase(Ease.Linear);

                        isCheck = true;
                    }
                }
                
            }
        }
    }


}
