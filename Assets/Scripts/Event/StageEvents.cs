using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 切换灯事件
/// </summary>
public struct SwitchLightEvent
{
    public bool isOpen;
}

/// <summary>
/// 播放动画
/// </summary>
public struct PlayAnimation
{
    public string animName;
}

public struct SetSequence
{
    public Sequence s;
    public GameObject go;
}

public struct PauceSequence
{
    public GameObject go;
}
