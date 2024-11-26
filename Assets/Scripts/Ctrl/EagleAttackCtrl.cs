using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class EagleAttackCtrl: MonoBehaviour, IController
{
    public LayerMask layerMask;
    [SerializeField]
    EagleCtrl eagle;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            PlayerCtrl playerCtrl = other.GetComponent<PlayerCtrl>();
            EagleCtrl eagleCtrl = other.GetComponent<EagleCtrl>();
            AnimalCtrl animalCtrl = other.GetComponent<AnimalCtrl>();
            ShooterCtrl shootCtrl = other.GetComponent<ShooterCtrl>();

            if (playerCtrl != null)
            {
                if(!playerCtrl.isDeath)
                {
                    eagle.targets.Add(other.transform);
                }
            }
            else if(eagleCtrl != null)
            {

            }
            else if(animalCtrl != null)
            {
                if (!animalCtrl.isDeath)
                {
                    eagle.targets.Add(other.transform);
                }
            }
            else if(shootCtrl != null)
            {
                if (!shootCtrl.isDeath)
                {
                    eagle.targets.Add(other.transform);
                }
            }


        }
    }
}
