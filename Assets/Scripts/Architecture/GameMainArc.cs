using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainArc : Architecture<GameMainArc>
{
    protected override void Init()
    {
        RegisterModels();
        RegisterUtilitys();
        RegisterSystems();
        CreateInstance();
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

        LevelManager levelManager = LevelManager.Instance;
        levelManager.ReadAllCfg();

        ADManager aDManager = ADManager.Instance;
        aDManager.InitializeAds();

        ShareManager shareManager = ShareManager.Instance;
        AnalyticsManager analyticsManager = AnalyticsManager.Instance;
    }


}
