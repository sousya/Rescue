using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StopWallCtrl: MonoBehaviour, IController
{
    public LayerMask layerMask;
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision != null)
        {
            Transform player = collision.transform;

            CheckCollider(player);
        }
    }

    public virtual bool CheckCollider(Transform player)
    {
        if (((1 << player.gameObject.layer) & layerMask) != 0)
        {


            PlayerCtrl playerCtrl = player.GetComponent<PlayerCtrl>();
            if (playerCtrl != null)
            {
                if(!playerCtrl.isWind)
                {
                    playerCtrl.m_rigid.velocity = Vector3.zero;
                }
                return true;
            }
            else
            {
                ShooterCtrl shooterCtrl = player.GetComponent<ShooterCtrl>();
                if (shooterCtrl != null)
                {

                    shooterCtrl.m_rigid.velocity = Vector3.zero;
                    return false;

                }
                else
                {
                    AnimalCtrl animalCtrl = player.GetComponent<AnimalCtrl>();
                    if (animalCtrl != null)
                    {
                            animalCtrl.m_rigid.velocity = Vector3.zero;                       
                            return false;
                    }
                    else
                    {
                        KnightCtrl knightCtrl = player.GetComponent<KnightCtrl>();

                        if (knightCtrl != null)
                        {
                            knightCtrl.m_rigid.velocity = Vector3.zero;
                            return false;
                        }
                        else
                        {
                            BombCtrl bombCtrl = player.GetComponent<BombCtrl>();

                            if (bombCtrl != null)
                            {
                                bombCtrl.m_rigid.velocity = Vector3.zero;
                                return false;
                            }
                        }
                    }
                }
            }



        }
        return false;
    }

}
