using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class StageCtrl: MonoBehaviour, IController, ICanSendEvent
{
    [SerializeField] LayerMask buttonLayer;

    private bool _islight;

    public bool isLight
    {
        get
        {
            return _islight;
        }
        set
        {
            ChangeLight(_islight);
        }
    }
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    public void Awake()
    {
        this.GetModel<StageModel>().nowStage = this;
    }


    public void ChangeLight(bool light)
    {
        _islight = light;
        SwitchLightEvent e = new SwitchLightEvent();
        e.isOpen = _islight;        
        this.SendEvent<SwitchLightEvent>(e);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 99999, buttonLayer))
            {
                Transform hitTrans = hit.transform;
                if(hitTrans.tag == "RopeButton")
                {
                    RopeButtonCtrl ropeButtonCtrl = hitTrans.GetComponent<RopeButtonCtrl>();
                    if(ropeButtonCtrl.ropeCtrl != null)
                    {
                        ropeButtonCtrl.ropeCtrl.ShrinkRope();
                    }
                }
            }
        }
    }

   
}
