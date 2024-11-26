using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoorCtrl: MonoBehaviour, IController, ICanSendEvent
{
    [SerializeField] float enterTime = 1f;
    [SerializeField] bool isCheck = false;
    [SerializeField] LayerMask layerMask;
    [SerializeField] GameObject CloseBox, OpenBox;
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Start()
    {
    }

    void CheckDeath(Transform player)
    {
        if (((1 << player.gameObject.layer) & layerMask) != 0)
        {
            PlayerCtrl playerCtrl = player.GetComponent<PlayerCtrl>();
            if (playerCtrl != null)
            {
                if (playerCtrl.targetList.Count == 0 && !playerCtrl.isDeath)
                {
                    playerCtrl.SetUseGravity(false);
                    player.DOKill();

                    playerCtrl.PlayAnim("kaibaoxiang");
                    CloseBox.SetActive(false);
                    OpenBox.SetActive(true);
                    this.SendEvent<LevelClearEvent>();

                    StartCoroutine(WaitWin(playerCtrl));
                    //Tweener tweener = player.DOMove(new Vector3(player.position.x, player.position.y, player.position.z + 1), enterTime);
                    //tweener.OnComplete(() =>
                    //{
                    //    Debug.Log("Í¨¹Ø");
                    //});
                    //tweener.SetEase(Ease.Linear);

                }
            }

        }
    }

    IEnumerator WaitWin(PlayerCtrl playerCtrl)
    {
        yield return new WaitForSeconds(1.2f);
        if(!playerCtrl.isDeath)
        {
           LevelManager.Instance.LevelClear();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (isCheck)
        {
            return;
        }

        if (collision != null)
        {
            Transform player = collision.transform;
            CheckDeath(player);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isCheck)
        {
            return;
        }

        if (collision != null)
        {
            Transform player = collision.transform;
            CheckDeath(player);
        }
    }



}
