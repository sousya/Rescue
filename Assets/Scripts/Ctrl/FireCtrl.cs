using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FireCtrl: MonoBehaviour, IController
{
    private ResLoader mResLoader = ResLoader.Allocate();
    [SerializeField] LayerMask layerMask;

    bool _hitPlayer;
    [SerializeField]
    bool changeTrigger, hasTimes, isAnimalAttack, isFire = true;
    [SerializeField]
    int times = 1, checkTimes = 0, killAnimalLevel;
    [SerializeField]
    GameObject fx, FxDeath, showFx;

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
            PlayerCtrl playerCtrl = player.GetComponent<PlayerCtrl>();
            if (playerCtrl != null)
            {
                playerCtrl.OnDeath();
                var fx = mResLoader.LoadSync<GameObject>("fx", "fire");
                Instantiate(fx, playerCtrl.transform);
                _hitPlayer = true;
                return true;
            }
            else
            {
                ShooterCtrl shooterCtrl = player.GetComponent<ShooterCtrl>();
                if (shooterCtrl != null)
                {
                    shooterCtrl.OnDeath();
                    var fx = mResLoader.LoadSync<GameObject>("fx", "fire");
                    Instantiate(fx, shooterCtrl.transform);
                    checkTimes++;
                    return false;

                }
                else
                {
                    AnimalCtrl animalCtrl = player.GetComponent<AnimalCtrl>();
                    if(animalCtrl != null )
                    {
                        if(killAnimalLevel > animalCtrl.animalLevel)
                        {
                            animalCtrl.OnDeath();
                            var fx = mResLoader.LoadSync<GameObject>("fx", "fire");
                            Instantiate(fx, animalCtrl.transform);
                            checkTimes++;
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }



        }
        return false;
    }

    public virtual void OnTriggerEnter(Collider collision)
    {
        if (collision != null && isFire)
        {
            Transform player = collision.transform;

            CheckCollider(player);
        }
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision != null && isFire)
        {
            Transform player = collision.transform;

            if (!hasTimes || (hasTimes && checkTimes < times))
            {
                CheckCollider(player);
            }
        }
    }

    public void Death()
    {
        gameObject.layer = 18;
        isFire = false;

        showFx.SetActive(false);
        FxDeath.SetActive(true);
    }
}
