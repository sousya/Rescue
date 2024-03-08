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

        if(light)
        {
            Debug.Log("开灯");
        }
        else
        {
            Debug.Log("关灯");
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 sendPos = Camera.main.ScreenToWorldPoint(mousePos);

            RaycastHit hit;
            if (Physics.Raycast(sendPos, Vector3.forward, out hit, 99999, buttonLayer))
            {
                Transform hitTrans = hit.transform;
                if (hitTrans != null)
                {
                    Debug.Log("射线击中 " + hitTrans.transform.name);
                }
                if(hitTrans.tag == "RopeButton")
                {
                    RopeCtrl ropeCtrl = hitTrans.parent.GetComponent<RopeCtrl>();
                    if(ropeCtrl != null)
                    {
                        ropeCtrl.ShrinkRope();
                    }
                    else
                    {
                        RopeDoubleCtrl ropedoubleCtrl = hitTrans.parent.GetComponent<RopeDoubleCtrl>();
                        if (ropedoubleCtrl != null)
                        {
                            ropedoubleCtrl.ShrinkRope();
                        }
                        else
                        {

                        }
                    }
                }
            }
        }
    }

   
}
