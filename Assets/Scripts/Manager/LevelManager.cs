using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDefine;
using LitJson;
using System.Runtime.CompilerServices;

[MonoSingletonPath("[Level]/LevelManager")]
public class LevelManager : MonoSingleton<LevelManager>
{
    private void Awake()
    {


    }

    public void ReadAllCfg()
    {
        ReadCfg();
    }

    public void ReadCfg()
    {
 
    }
   
}
