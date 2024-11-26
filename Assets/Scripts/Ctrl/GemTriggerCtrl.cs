using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GemTriggerCtrl: MonoBehaviour, IController
{
    public float angle, speed, angleNow;
    public bool isStart = false, isRotate = true, isChange, isMove, isMoveCheck;
    public Transform movingTarget, changeTarget;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }
    private void Update()
    {
        if (isStart)
        {
            if(isRotate)
            {
                float rotatescale = 1;

                // 根据时间timeslice旋转物体
                angleNow += speed * rotatescale;
                Debug.Log("angleNow" + angleNow);
                transform.Rotate(new Vector3(0, speed * rotatescale, 0));

                if (angleNow >= angle)
                {
                    isStart = false;
                }
            }
            else if(isMove && !isMoveCheck)
            {
                transform.DOMove(movingTarget.position, 1);

                isMoveCheck = true;
            }
            else if(isChange)
            {
                changeTarget.gameObject.SetActive(true);
                transform.gameObject.SetActive(false);
            }
           
        }

    }
}
