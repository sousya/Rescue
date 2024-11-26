using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBoxCheck : MonoBehaviour
{
    public PushBoxCtrl pushBoxCtrl;
    public virtual bool CheckCollider(Transform transform)
    {
        if(pushBoxCtrl.m_rigid.velocity.y == 0)
        {
            return false;
        }

        ShooterCtrl shooterCtrl = transform.GetComponent<ShooterCtrl>();
        if (shooterCtrl != null)
        {

            shooterCtrl.OnDeath();

        }
        else
        {
            AnimalCtrl animalCtrl = transform.GetComponent<AnimalCtrl>();
            if (animalCtrl != null)
            {
                animalCtrl.OnDeath();
            }
            else
            {
                KnightCtrl knightCtrl = transform.GetComponent<KnightCtrl>();

                if (knightCtrl != null)
                {
                    knightCtrl.OnDeath();
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
