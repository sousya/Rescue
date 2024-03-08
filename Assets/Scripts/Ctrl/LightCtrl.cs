using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class LightCtrl: MonoBehaviour, IController
{
    [SerializeField] LayerMask layerMask;

    bool _hitPlayer;
    [SerializeField]
    bool isLight = false;
    [SerializeField]
    int times = 1, checkTimes = 0, killAnimalLevel;

    public bool hitPlayer
    {
        get
        {
            return _hitPlayer;
        }
    }

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    public virtual bool CheckCollider(Transform player)
    {
        if (((1 << player.gameObject.layer) & layerMask) != 0)
        {
            isLight = !isLight;
            this.GetModel<StageModel>().nowStage.ChangeLight(isLight); 
        }
        return false;
    }

    public virtual void OnTriggerEnter(Collider collision)
    {
        if (collision != null)
        {
            Transform player = collision.transform;

            CheckCollider(player);
        }
    }
}
