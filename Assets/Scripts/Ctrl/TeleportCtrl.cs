using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeleportCtrl: MonoBehaviour, IController
{

    public Transform teleportOut;
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform != null)
        {
            other.transform.DOKill();
            other.transform.position = teleportOut.position;
        }   
    }


}
