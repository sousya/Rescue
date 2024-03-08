using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MoveRopePointCtrl : MonoBehaviour, IController
{
    [SerializeField]
    MoveRopeCtrl moveRopeCtrl;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!moveRopeCtrl.isCheck && collision != null)
        {
            Transform player = collision.transform;

            if (((1 << player.gameObject.layer) & moveRopeCtrl.layerMask) != 0)
            {
                PlayerCtrl playerCtrl = player.GetComponent<PlayerCtrl>();
                if (playerCtrl != null)
                {
                    Debug.Log("…Ë÷√£°£°£°");
                    moveRopeCtrl.FixedRigid.isKinematic = false;

                    StartCoroutine(Push(playerCtrl));

                }

            }
        }
    }

    IEnumerator Push(PlayerCtrl playerCtrl)
    {
        yield return new WaitForEndOfFrame();

        Rigidbody targetRigid = moveRopeCtrl.target.GetComponent<Rigidbody>();

        playerCtrl.transform.DOKill();
        //playerCtrl.targetList.Add(hangCtrl.target);
        moveRopeCtrl.FixedRigid.AddForce(moveRopeCtrl.moveVec);

        playerCtrl.transform.position = moveRopeCtrl.target.position;
        CharacterJoint joint = playerCtrl.AddComponent<CharacterJoint>();
        joint.connectedBody = targetRigid;
        playerCtrl.targetList.Add(joint);
    }

}
