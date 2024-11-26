using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeleportCtrl: MonoBehaviour, IController
{
    public bool isBouble, isWaiting;
    public Transform teleportOut;
    public TeleportCtrl teleport;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform != null && !isWaiting)
        {
            other.transform.DOKill();
            other.transform.DOMove(transform.position, 0.4f);
            //other.transform.position = teleportOut.position;
            StartCoroutine(PlayTelportAnim(other.transform));
            if(teleport)
            {
                StartCoroutine(WaitCheck());
            }
        }   
    }

    IEnumerator PlayTelportAnim(Transform obj)
    {
        var scale = obj.localScale;
        obj.DOScale(Vector3.zero, 0.5f);
        yield return new WaitForSeconds(0.5f);
        obj.DOKill();
        obj.position = teleportOut.position;
        obj.localScale = scale;
    }

    IEnumerator WaitCheck()
    {
        isWaiting = true;
        teleport.isWaiting = true;

        yield return new WaitForSeconds(1.5f);

        teleport.isWaiting = false;
        isWaiting = false;
    }

}
