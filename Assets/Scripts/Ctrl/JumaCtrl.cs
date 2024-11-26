using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JumaCtrl : MonoBehaviour, IController
{
    [SerializeField]
    Transform deathTrans, normalTrans;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    void CheckDestroy(Transform trans)
    {
        AnimalCtrl animalCtrl = trans.GetComponent<AnimalCtrl>(); 
        if(animalCtrl != null)
        {
            if(animalCtrl.canJuma)
            {
                OnDeath();
            }
        }
    }

    public void OnDeath()
    {
        deathTrans.gameObject.SetActive(true);
        normalTrans.gameObject.SetActive(false);
        gameObject.layer = 17;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform != null)
        {
            CheckDestroy(collision.transform);
        }
    }
}
