using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameMainArc : Architecture<GameMainArc>
{
    private ResLoader mResLoader = ResLoader.Allocate();

    public RenderTexture renderTexture;
    protected override void Init()
    {
        ResKit.Init();
        RegisterModels();
        RegisterUtilitys();
        RegisterSystems();
        CreateInstance();
        //PlayerPrefs.DeleteKey("g_ClearLevel");



        if (LevelManager.Instance.levelNow > 135)
        {
            UIKit.OpenPanel("UILevelClear", UILevel.Common, "uilevelclear_prefab");

        }
        else
        {

            UIKit.OpenPanel("UILevelMain", UILevel.Common, "uilevelmain_prefab");
        }
        this.SendEvent<LevelStartEvent>();



      
        //AudioKit.PlayMusic("resources://Sound/bgm");
        //GameObject go = Camera.main.gameObject;
        //go.AddComponent<GameMainCtrl>();

    }

    
    void RegisterModels()
    {
        RegisterModel(new StageModel());
    }

    void RegisterUtilitys()
    {
        RegisterUtility(new SaveDataUtility());
        RegisterUtility(new LanguageUtility());
    }

    void RegisterSystems()
    {
    }

    void CreateInstance()
    {
        TextManager textManager = TextManager.Instance;
        string languageStr = GetUtility<SaveDataUtility>().GetSelectLanguage();
        if (languageStr == "-1")
        {
            if(GetUtility<LanguageUtility>() == null)
            {
                Debug.Log("Ã»ÓÐUtility");
            }
            languageStr = GetUtility<LanguageUtility>().GetSystemLanguage();
            GetUtility<SaveDataUtility>().SaveSelectLanguage(languageStr);
        }
        //Debug.Log("languageStr " + languageStr);

        textManager.ReadTextCfg(languageStr);

        //LevelManager levelManager = LevelManager.Instance;
        //levelManager.ReadAllCfg();

        //ADManager aDManager = ADManager.Instance;
        //aDManager.InitializeAds();
        //ResourceManager.Instance.LoadABPackage("uieveladdheart_prefab");
        //ResourceManager.Instance.LoadABPackage("uilevelclear_prefab");
        //ResourceManager.Instance.LoadABPackage("uilevelmain_prefab");

        ShareManager shareManager = ShareManager.Instance;
        AnalyticsManager analyticsManager = AnalyticsManager.Instance;
        LevelManager.Instance.BeginGame();
        TopOnADManager.Instance.LoadAD();
        //ResourceManager.Instance.LoadFont();
    }


}
