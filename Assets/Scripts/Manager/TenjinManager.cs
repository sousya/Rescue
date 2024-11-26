using AnyThinkAds.Api;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TenjinManager: MonoBehaviour
{
   static public TenjinManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    public void Init()
    {
        Debug.Log("Tenjin StartConnect-11");
        //DontDestroyOnLoad(this.gameObject);
        //TenjinConnect();

        Debug.Log("Tenjin StartConnect0");

        BaseTenjin instance = Tenjin.getInstance("PHJEXJSG8IZZWBNEWUOJPNEYG7SACOYN");
        Debug.Log("Tenjin StartConnect1");

#if UNITY_ANDROID
        Debug.Log("Tenjin StartConnect2");

        instance.SetAppStoreType(AppStoreType.googleplay);
        // Sends install/open event to Tenjin
        instance.Connect();
#endif
    }

    void OnApplicationPause(bool pauseStatus)
    {
        //if (!pauseStatus)
        //{
        //    TenjinConnect();
        //}
    }

    public void TenjinConnect()
    {

    }

}
