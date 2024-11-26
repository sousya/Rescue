using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDefine;
using System;
using Unity.VisualScripting;

public class SaveDataUtility : IUtility
{
    public void SaveLevel(int level)
    {
        //Debug.Log("LevelBefore " + Convert.ToString(clearLevel, 2) + " clearNowLevel " + clearNowLevel + " LevelNow " + Convert.ToString((clearLevel | clearNowLevel), 2));
        PlayerPrefs.SetInt("g_ClearLevel", level);

    }
    public int GetLevelClear()
    {
        int clearLevel = PlayerPrefs.GetInt("g_ClearLevel", 1);
       return clearLevel;
    }
    public void SaveUnlock(int unlock)
    {
        //Debug.Log("LevelBefore " + Convert.ToString(clearLevel, 2) + " clearNowLevel " + clearNowLevel + " LevelNow " + Convert.ToString((clearLevel | clearNowLevel), 2));
        PlayerPrefs.SetInt("g_UnlockHeart", unlock);

    }
    public int GetSaveUnlock()
    {
        int clearLevel = PlayerPrefs.GetInt("g_UnlockHeart", 0);
        return clearLevel;
    }


    public void SetVitality(int num)
    {
        SetVitalityTime();
        SetVitalityNum(num);
    }
    
    public void CostVitality()
    {
        if(LevelManager.Instance.NoCostVitality)
        {
            return;
        }
        int lastVitalityNum = GetVitalityNum();
        SetVitality(lastVitalityNum - 1);
    }
    
    public void SetVitality(int num, string time)
    {
        SetVitalityTime(time);
        SetVitalityNum(num);
    }

    public long GetNowTime()
    {
        DateTime now = DateTime.Now;
        long unixTimestamp = now.ToUniversalTime().Ticks - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
        long time = (unixTimestamp / 10000000);

        return time;
    }

    public void UseVitality()
    {
        SetVitality(GetVitalityNum() - 1);
    }

    public void SetVitalityTime(string time)
    {       
        PlayerPrefs.SetString("g_VitalityTime", time);
    }

    public void SetVitalityTime()
    {       
        PlayerPrefs.SetString("g_VitalityTime", GetNowTime() + "");
    }
    public long GetVitalityTime()
    {
        string timeStr = PlayerPrefs.GetString("g_VitalityTime", "0");
        long time = long.Parse(timeStr);
        return time;
    }

    public void SetVitalityNum(int num)
    {
        PlayerPrefs.SetInt("g_VitalityNum", num);
    }

    public void AddVitalityNum()
    {
        var v = PlayerPrefs.GetInt("g_VitalityNum", GameConst.MaxVitality);
        //v += 1;
        //if(v > 5)
        //{
            v = 5;
        //}
        PlayerPrefs.SetInt("g_VitalityNum", v);
    }


    public int GetVitalityNum()
    {
        return PlayerPrefs.GetInt("g_VitalityNum", GameConst.MaxVitality);
    }

    public string GetSelectLanguage()
    {
        string language =  PlayerPrefs.GetString("g_Language", "-1");

        return language;
    }

    public void SaveSelectLanguage(LanguageType languageType)
    {
        string language = languageType.ToString();
     
        PlayerPrefs.SetString("g_Language", language);
    }

    public void SaveSelectLanguage(string language)
    {
        PlayerPrefs.SetString("g_Language", language);
    }

}
