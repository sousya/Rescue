using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RotaionFloorCtrl : MonoBehaviour, IController
{
    public float speed = 1, rotationAngle;
    public bool rotateRight = false, updateRotate = true;
    public RopeHangCtrl hangCtrl;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Start()
    {
        if(hangCtrl != null)
        {
            //hangCtrl.onUnRopeCall += OnUnRope;
        }
    }

    private void Update()
    {
        if(updateRotate)
        {
            // 根据时间timeslice旋转物体
            float angle = speed * Time.deltaTime;

            float rotatescale = 1;
            if (rotateRight)
            {
                rotatescale = -1;
            }
            transform.Rotate(new Vector3(0, 0, speed * rotatescale));
        }

    }

    public virtual void OnTriggerEnter(Collider collision)
    {
        if (collision != null)
        {
            Transform player = collision.transform;

            PlayerCtrl playerCtrl = player.GetComponent<PlayerCtrl>();
            if (playerCtrl != null)
            {
                playerCtrl.m_collider.isTrigger = false;
            }
            else
            {
                ShooterCtrl shooterCtrl = player.GetComponent<ShooterCtrl>();
                if (shooterCtrl != null)
                {
                    shooterCtrl.m_collider.isTrigger = false;
                }
                else
                {
                    AnimalCtrl animalCtrl = player.GetComponent<AnimalCtrl>();
                    if (animalCtrl != null)
                    {
                        animalCtrl.m_collider.isTrigger = false;
                    }
                }
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision != null)
        {
            Transform player = collision.transform;

            PlayerCtrl playerCtrl = player.GetComponent<PlayerCtrl>();
            if (playerCtrl != null)
            {
                playerCtrl.inRotation = true;
            }
            else
            {
                ShooterCtrl shooterCtrl = player.GetComponent<ShooterCtrl>();
                if (shooterCtrl != null)
                {
                    shooterCtrl.m_rigid.useGravity = true;
                }
                else
                {
                    AnimalCtrl animalCtrl = player.GetComponent<AnimalCtrl>();
                    if (animalCtrl != null)
                    {
                        animalCtrl.m_rigid.useGravity = true;
                    }
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision != null)
        {
            Transform player = collision.transform;

            PlayerCtrl playerCtrl = player.GetComponent<PlayerCtrl>();
            if (playerCtrl != null)
            {
                playerCtrl.m_collider.isTrigger = true;
                playerCtrl.inRotation = false;
            }
            else
            {
                ShooterCtrl shooterCtrl = player.GetComponent<ShooterCtrl>();
                if (shooterCtrl != null)
                {
                    //shooterCtrl.m_collider.isTrigger = true;
                }
                else
                {
                    AnimalCtrl animalCtrl = player.GetComponent<AnimalCtrl>();
                    if (animalCtrl != null)
                    {
                        animalCtrl.m_collider.isTrigger = true;
                    }
                }
            }
        }
    }
}
