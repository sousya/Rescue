using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameDefine;
using System;
using Facebook.Unity;
using GoogleMobileAds.Ump.Api;

public class GameMainCtrl : MonoBehaviour, IController, ICanSendEvent
{
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Start()
    {
        //LevelManager.Instance.ReadAllCfg();
        GetArchitecture();
        //只调整小于16:9的分辨率

        if (Camera.main != null)
        {
            const float radio16X9 = 1.7777f; // =>(float)1960/1080
            var curRadio = (float)1920 / 1080;
            var main = Camera.main;
            var fov = main.fieldOfView;
            var radio = radio16X9 / curRadio;
            main.fieldOfView = radio * fov;
        }

        //ActivityFaceBook();
        ActivityUMP();
    }

    public void ActivityFaceBook()
    {
        FB.Init();
        // priceCurrency is a string containing the 3-letter ISO code for the currency
        // that the user spent, and priceAmount is a float containing the quantity spent;
        // packageName is a string containing your SKU code for the thing they bought

        // numGold is the number of in-app currency the user spent in this purchase,
        // storeItem is a string naming the item the user bought
        var softPurchaseParameters = new Dictionary<string, object>();
        softPurchaseParameters["Success"] = true;
        FB.LogAppEvent(Facebook.Unity.AppEventName.ActivatedApp, (float)1, softPurchaseParameters);
    }

    public void ActivityUMP()
    {
        UMPManager.Instance.Init();
       
    }

    

    IEnumerator CheckFirstEnter()
    {
        yield return new WaitForEndOfFrame();

    }
}
