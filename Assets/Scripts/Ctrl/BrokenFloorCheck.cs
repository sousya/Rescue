using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BrokenFloorCheck: MonoBehaviour, IController
{
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    public virtual bool CheckCollider(Transform trans)
    {
        FloorCtrl floorCtrl = trans.GetComponent<FloorCtrl>();
        
        if (floorCtrl != null)
        {
            if(floorCtrl.brokenFloor != null)
            {
                floorCtrl.OnDeath();
            }
        }
        else
        {
            FrozenMachineCtrl frozenMachineCtrl = trans.GetComponent<FrozenMachineCtrl>();
            if (frozenMachineCtrl != null)
            {
                frozenMachineCtrl.OnDeath();
            }
            else
            {
                WindMachineCtrl windMachineCtrl = trans.GetComponent<WindMachineCtrl>();
                if (windMachineCtrl != null)
                {
                    windMachineCtrl.OnDeath();
                }
                else
                {
                    LightCtrl lightCtrl = trans.GetComponent<LightCtrl>();
                    if(lightCtrl != null)
                    {
                        lightCtrl.OnDeath();
                    }
                    else
                    {
                        JumaCtrl jumaCtrl = trans.GetComponent<JumaCtrl>();
                        if(jumaCtrl != null)
                        {
                            jumaCtrl.OnDeath();
                        }
                        else
                        {
                            FlowerCtrl flowerCtrl = trans.GetComponent<FlowerCtrl>();
                            if(flowerCtrl != null)
                            {
                                flowerCtrl.Death();
                            }
                        }
                    }
                }
            }    
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

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            Transform player = collision.transform;
            CheckCollider(player);
        }
    }

}
