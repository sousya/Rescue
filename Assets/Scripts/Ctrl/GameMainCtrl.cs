using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameDefine;
using System;

public class GameMainCtrl : MonoBehaviour, IController
{

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Start()
    {

    }

    IEnumerator CheckFirstEnter()
    {
        yield return new WaitForEndOfFrame();

    }
}
