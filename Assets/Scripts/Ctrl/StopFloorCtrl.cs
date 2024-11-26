using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StopFloorCtrl : MonoBehaviour, IController
{
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision != null)
        {
            Transform player = collision.transform;
            PlayerCtrl playerCtrl = player.GetComponent<PlayerCtrl>();
            if(playerCtrl != null )
            {
                playerCtrl.m_rigid.velocity = new Vector3(0, playerCtrl.m_rigid.velocity.y, 0);
            }
        }
    }


}
