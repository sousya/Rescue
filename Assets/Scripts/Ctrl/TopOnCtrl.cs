using AnyThinkAds.Api;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopOnCtrl: MonoBehaviour, IController
{
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    void Start()
    {
        ATSDKAPI.initSDK("a667e2369f1df6", "a5e2ae5036721db4f108aef055cbe44c3");
        ATSDKAPI.setLogDebug(false);
        TopOnADManager.Instance.LoadAD();
        DontDestroyOnLoad(gameObject);
    }


}
