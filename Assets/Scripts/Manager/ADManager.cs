using GameDefine;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Advertisements;

[MonoSingletonPath("[AD]/ADManager")]
public class ADManager : MonoSingleton<ADManager>, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidGameId = "5534060";
    [SerializeField] string _iOSGameId = "5534061";
    [SerializeField] string _androidAdUnitId = "Answer_ADReward";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    string _adUnitId = null; // 对于不受支持的平台，此值将保持为 null
    [SerializeField] bool _testMode = false;
    private string _gameId;
    public delegate void RewardAction();

    public RewardAction rewardAction;
    /// <summary>
    ///  获取当前平台的 Ad Unit ID（广告单元 ID）：
    /// </summary>
    public void InitializeAds()
    {
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
            _gameId = _iOSGameId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
        _gameId = _androidGameId;
#elif UNITY_EDITOR
            _gameId = _androidGameId; //Only for testing the functionality in the Editor
#endif
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {

            Advertisement.Initialize(_gameId, _testMode, this);
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");


        LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    public void LoadAd()
    {
        // 重要！仅在初始化之后再加载内容（在此示例中，初始化在另一个脚本中处理）。
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
        Debug.Log("Loading Ad: BackFill" );

        Advertisement.Load("BackFill", this);
        Debug.Log("Loading Ad: BackFill_2");

        Advertisement.Load("BackFill_2", this);
    }
    public void ShowAd()
    {
        // 请注意，如果未事先加载广告内容，此方法将失败
        Debug.Log("Showing Ad: " + _adUnitId);
        AudioKit.PauseMusic();
        Advertisement.Show(_adUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Unity Ads OnUnityAdsAdLoaded");
        //（可选）如果广告单元成功加载内容，执行代码。

    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
        //（可选）如果广告单元加载失败，执行代码（例如再次尝试）。
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        //AudioKit.ResumeMusic();
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        //（可选）如果广告单元展示失败，执行代码（例如加载另一个广告）。
        AudioKit.PauseMusic();
        Advertisement.Load(adUnitId, this);

        if (adUnitId == _adUnitId)
        {
            Advertisement.Show("BackFill", this);
        }
        else if(adUnitId == "BackFill")
        {
            Advertisement.Show("BackFill_2", this);
        }
        else if(adUnitId == "BackFill_2")
        {
            rewardAction();
        }
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
    // 实现 Show Listener 的 OnUnityAdsShowComplete 回调方法来判断用户是否获得奖励：
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        AudioKit.ResumeMusic();
        if ((adUnitId.Equals(_adUnitId) || adUnitId.Equals("BackFill") || adUnitId.Equals("BackFill_2")) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            Advertisement.Load(adUnitId, this);

            // 给予奖励。
            if (rewardAction != null)
            {
                rewardAction();
            }
        }
    }
}
