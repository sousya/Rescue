using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDefine;
using LitJson;
using System.Runtime.CompilerServices;
using QFramework.Example;
using QAssetBundle;
using TMPro;
using System.IO;
using Material = UnityEngine.Material;

[MonoSingletonPath("[Level]/LevelManager")]
public class LevelManager : MonoBehaviour, ICanSendEvent, ICanGetUtility, ICanRegisterEvent
{
    private ResLoader mResLoader = ResLoader.Allocate();
    public RenderTexture shouUI;
    int maxNum = 26, nowShowUIId;
    public static LevelManager Instance;
    public int levelNow = 1;
    public Transform levelNode, showUINode;
    public GameObject nowLStage;
    public List<Transform> transDic = new List<Transform>();
    public bool NoCostVitality = false;

    GameObject nowShowUI;
    [SerializeField]
    Light showLight;
    [SerializeField]
    Color darkColor, lightColor;
    [SerializeField]
    List<int> ADList = new List<int>()
    {
        5, 8, 10, 12, 13
    };

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Awake()
    {
        Instance = this;
        levelNode = GameObject.Find("LevelNode").transform;

        this.RegisterEvent<AddVitality>((e) =>
        {
            this.GetUtility<SaveDataUtility>().AddVitalityNum();
            UIKit.ClosePanel("UILevelAddHeart");
        });

        this.RegisterEvent<SkipLevel>((e) =>
        {
            LevelManager.Instance.SkipLevel();
        });
    }

    private void Start()
    {
   

        ReadAllCfg();
        ResetShader();
        var unlock = this.GetUtility<SaveDataUtility>().GetSaveUnlock();
        NoCostVitality = false;
        if (unlock == 1)
        {
            NoCostVitality = true;
        }
        //UIKit.OpenPanel("UILevelMain", UILevel.Common, "uilevelmain_prefab");
    }

    public void ReadAllCfg()
    {
        ReadCfg();
    }

    public void ReadCfg()
    {
 
    }

    public void Restart()
    {
        int vitality = this.GetUtility<SaveDataUtility>().GetVitalityNum();
        if (vitality > 0)
        {
            this.GetUtility<SaveDataUtility>().CostVitality();
            StartGame(levelNow);
        }
        else
        {
            UIKit.OpenPanel("UILevelAddHeart", UILevel.Common, "uileveladdheart_prefab");
        }
        
    }

    public void SkipLevel()
    {
        int vitality = this.GetUtility<SaveDataUtility>().GetVitalityNum();
        if(vitality > 0)
        {
            //this.GetUtility<SaveDataUtility>().CostVitality();
            this.GetUtility<SaveDataUtility>().SaveLevel(levelNow + 1);
            StartGame(levelNow + 1);
        }
        else
        {
            UIKit.OpenPanel("UILevelAddHeart", UILevel.Common, "uileveladdheart_prefab");
        }
    }

    public void StartGame(int level)
    {
        if(nowLStage != null)
        {
            Destroy(nowLStage);
        }

        if(levelNow >= 135)
        {
            return;
        }
        LevelManager.Instance.SetDarkLight(true);

        levelNow = level;
        LoadShowItem();
        levelNode.DestroyChildren();
        //GameObject go = Instantiate(Resources.Load<GameObject>("Prefab/Stages/Stage" + levelNow), levelNode);
        var g = mResLoader.LoadSync<GameObject>("Prefab", "Stage" + levelNow);
        var go = Instantiate(g, levelNode);
        go.transform.localPosition = Vector3.zero;
        nowLStage = go;
        //LevelStartEvent e = new LevelStartEvent();
        //e.Level = level;
        this.SendEvent<LevelStartEvent>();
        this.SendEvent<LoadRawImage>();
    }

    public void CheckAD()
    {
        if(levelNow < 14 && ADList.Contains(levelNow))
        {
            Debug.Log("判断广告");
            TopOnADManager.Instance.ShowInterstitialAd();
        }
        else if(((levelNow - 13) % 2) == 0 && levelNow > 13)
        {
            Debug.Log("判断广告");
            TopOnADManager.Instance.ShowInterstitialAd();
        }
    }

    public void BeginGame()
    {
    
        levelNow = this.GetUtility<SaveDataUtility>().GetLevelClear();
        StartGame(levelNow);
        TenjinManager.Instance.Init();
    }

    public void LevelClear()
    {
        this.GetUtility<SaveDataUtility>().SaveLevel(levelNow + 1);
        AnalyticsManager.Instance.SendCompleteEvent(levelNow + 1);
        StartGame(levelNow + 1);
        //UIKit.OpenPanel("UILevelClear");
        UIKit.OpenPanel("UILevelClear", UILevel.Common, "uilevelclear_prefab");
    }

    public void LoadShowItem()
    {
        //int checkNum = (levelNow - 1) / 5 + 1;
        //if(nowShowUIId != checkNum)
        //{
        //    nowShowUIId = checkNum;
        //    if(nowShowUI != null)
        //    {
        //        Destroy(nowShowUI);
        //    }
        //    if (checkNum <= maxNum)
        //    {
        //        var g = mResLoader.LoadSync<GameObject>("Prefab", "Show" + checkNum);
        //        nowShowUI = Instantiate(g, showUINode);
        //        //nowShowUI = Instantiate(Resources.Load<GameObject>("Prefab/ShowPrefab/Show" + checkNum), showUINode);
        //        nowShowUI.transform.localPosition = Vector3.zero;
        //    }
        //}
        
           
    }

    public void SetDarkLight(bool isLight)
    {
        if(isLight)
        {
            SetLight(lightColor);
        }
        else
        {
            SetLight(darkColor);
        }
    }

    public void SetLight(Color color)
    {
        showLight.color = color;
    }

    void ResetShader()
    {
        //var a = mResLoader.LoadSync<TMP_FontAsset>("font", "fontall");
        //a.material.shader = Shader.Find(a.material.shader.name);

        //a = mResLoader.LoadSync<TMP_FontAsset>("font", "fontextra");
        //a.material.shader = Shader.Find(a.material.shader.name);

        var b = mResLoader.LoadSync<Material>("material", "background");
        b.shader = Shader.Find(b.shader.name);

        b = mResLoader.LoadSync<Material>("material", "background1");
        b.shader = Shader.Find(b.shader.name);

        b = mResLoader.LoadSync<Material>("material", "background2");
        b.shader = Shader.Find(b.shader.name);

        b = mResLoader.LoadSync<Material>("material", "background3");
        b.shader = Shader.Find(b.shader.name);

        b = mResLoader.LoadSync<Material>("material", "cubecolor");
        b.shader = Shader.Find(b.shader.name);

        b = mResLoader.LoadSync<Material>("material", "BgPlane");
        b.shader = Shader.Find(b.shader.name);

        b = mResLoader.LoadSync<Material>("material", "bianfu");
        b.shader = Shader.Find(b.shader.name);

        b = mResLoader.LoadSync<Material>("material", "bianfu");
        b.shader = Shader.Find(b.shader.name);

        b = mResLoader.LoadSync<Material>("material", "Button");
        b.shader = Shader.Find(b.shader.name);

        b = mResLoader.LoadSync<Material>("material", "caizh_11");
        b.shader = Shader.Find(b.shader.name);

        b = mResLoader.LoadSync<Material>("material", "color");
        b.shader = Shader.Find(b.shader.name);

        b = mResLoader.LoadSync<Material>("material", "lang");
        b.shader = Shader.Find(b.shader.name);

        b = mResLoader.LoadSync<Material>("material", "teleport");
        b.shader = Shader.Find(b.shader.name);

        b = mResLoader.LoadSync<Material>("material", "Wood");
        b.shader = Shader.Find(b.shader.name);

        b = mResLoader.LoadSync<Material>("material", "youling");
        b.shader = Shader.Find(b.shader.name);

        b = mResLoader.LoadSync<Material>("material", "zhuazi");
        b.shader = Shader.Find(b.shader.name);

        for (int i = 1; i <= 9; i++)
        {
            b = mResLoader.LoadSync<Material>("material", "caizhi_0" + i);
            b.shader = Shader.Find(b.shader.name);
        }

        b = mResLoader.LoadSync<Material>("material", "caizhi_10");
        b.shader = Shader.Find(b.shader.name);

        for (int i = 1; i <= 9; i++)
        {
            b = mResLoader.LoadSync<Material>("material", "caizhi_5" + i);
            b.shader = Shader.Find(b.shader.name);
        }

    }
}
