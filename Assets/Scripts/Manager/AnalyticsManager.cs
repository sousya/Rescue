using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using Unity.Services.Analytics;
using Unity.Services.Analytics.Internal;
using Unity.Services.Core;
using UnityEngine;
using Event = Unity.Services.Analytics.Event;

[MonoSingletonPath("[Analytics]/AnalyticsManager")]
public class AnalyticsManager : MonoSingleton<AnalyticsManager>, ICanGetUtility, ICanSendEvent
{
    public class CompleteLevel : Event
    {
        public CompleteLevel() : base("completeLevel")
        {
        }

        public int level { set { SetParameter("level", value); } }
    }
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        ConsentGiven();
    }
    void ConsentGiven()
    {
        AnalyticsService.Instance.StartDataCollection();
    }

    public void SendCompleteEvent(int level)
    {
        CompleteLevel e = new CompleteLevel
        { 
            
            level = level 
        };
        AnalyticsService.Instance.RecordEvent(e);
    }

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

}
