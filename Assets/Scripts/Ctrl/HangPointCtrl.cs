using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class HangPointCtrl : MonoBehaviour, IController
{
    [SerializeField]
    Collider coll;
    [SerializeField]
    HangCtrl hangCtrl;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Start()
    {
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("设置！！！");
        //if (!hangCtrl.isCheck && collision != null)
        //{
        //    Transform player = collision.transform;

        //    if (((1 << player.gameObject.layer) & hangCtrl.layerMask) != 0)
        //    {
        //        PlayerCtrl playerCtrl = player.GetComponent<PlayerCtrl>();
        //        if (playerCtrl != null)
        //        {
        //            Debug.Log("设置！！！");
        //            //playerCtrl.targetList.Add(hangCtrl.target);
        //            if(playerCtrl.targetList.Count == 1)
        //            {
        //                playerCtrl.transform.position = hangCtrl.target.position;
        //            }

        //            coll.isTrigger = true;

        //            hangCtrl.targetRigid.constraints = RigidbodyConstraints.None;
        //            hangCtrl.targetRigid.velocity = hangCtrl.moveVec;
        //            hangCtrl.isCheck = true;
        //        }

        //    }
        //}

    }

}
