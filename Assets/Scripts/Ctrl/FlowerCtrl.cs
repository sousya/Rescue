using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class FlowerCtrl : MonoBehaviour, IController
{
    [SerializeField] LayerMask layerMask;

    bool _hitPlayer;
    [SerializeField]
    bool hasTimes, isDeath = false;
    [SerializeField]
    int times = 1, checkTimes = 0, killAnimalLevel;
    [SerializeField]
    SpikeCtrl attackArea;
    [SerializeField]
    FlowerCheckCtrl checkCtrl;
    [SerializeField]
    Animator anim;

    public bool hitPlayer
    {
        get
        {
            return _hitPlayer;
        }
    }

    private void Start()
    {
        checkCtrl.layerMask = layerMask;
        attackArea.layerMask = layerMask;
    }

    public void Attack()
    {
        if(isDeath || (hasTimes && times < checkTimes))
        {
            return;
        }
        PlayAnim("chi");
        attackArea.gameObject.SetActive(true);
        checkTimes++;
    }

    public void PlayAnim(string name)
    {
        anim.Play(name);
    }

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    public void Death()
    {
        anim.Play("siwang");
        isDeath = true;
    }
}
