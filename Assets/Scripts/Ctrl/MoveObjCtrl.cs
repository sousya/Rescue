using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MoveObjCtrl: MonoBehaviour, IController
{
    [SerializeField]
    float moveSpeed = 1f;
    [SerializeField]
    float rotateSpeed = 0.5f;
    [SerializeField]
    List<Transform> movePoints = new List<Transform>();
    [SerializeField]
    bool startMove = true, needRotate = false;
    int nowMoveTo = 0;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Start()
    {
        foreach (Transform t in movePoints)
        {
            t.transform.position = new Vector3 (t.transform.position.x, t.transform.position.y, transform.position.z);
            t.gameObject.SetActive (false);
        }

        if(startMove)
        {
            SetMove();
        }
    }

    void SetMove()
    {
        transform.DOKill();
        float distance = Vector3.Distance(transform.position, movePoints[nowMoveTo].position);
        Transform target = movePoints[nowMoveTo];
        Tweener tweener = transform.DOMove(target.position, distance / moveSpeed);
        tweener.SetEase(Ease.Linear);
        tweener.OnComplete(() =>
        {
            nowMoveTo++;
            if (nowMoveTo == movePoints.Count)
            {
                nowMoveTo = 0;
            }
            if(needRotate)
            {
                float angle = target.rotation.eulerAngles.z - transform.rotation.eulerAngles.z;
                Debug.Log("angle " + angle + " euler_1 " + target.rotation.eulerAngles + "euler_2 " + transform.rotation.eulerAngles);
               Tweener rotateTwenner = transform.DORotateQuaternion(target.rotation, angle / rotateSpeed);
                rotateTwenner.SetEase(Ease.Linear);
                rotateTwenner.OnComplete(() =>
                {
                    SetMove();
                });
            }
            else
            {
                SetMove();
            }
        });
    }

   
}
