using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RopeButtonCtrl: MonoBehaviour, IController
{
    public RopeCtrl ropeCtrl;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Start()
    {
    }

   
}
