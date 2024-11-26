using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElectricCtrl: SpikeCtrl
{
    private ResLoader mResLoader = ResLoader.Allocate();
    [SerializeField]
    GameObject fx;

    public override bool CheckCollider(Transform transform)
    {
        if (isDeath || (hasTimes && checkTimes < times))
        {
            return false;
        }

        PlayerCtrl playerCtrl = transform.GetComponent<PlayerCtrl>();
        if (playerCtrl != null)
        {

            if (belongAnimal != null && !playerCtrl.isDeath)
            {
                belongAnimal.transform.DOKill();
                belongAnimal.OnAttack();
            }

            playerCtrl.OnDeath();
            var fx = mResLoader.LoadSync<GameObject>("fx", "lighting");
            Instantiate(fx, playerCtrl.transform);

            _hitPlayer = true;
            return true;
        }
        else
        {
            ShooterCtrl shooterCtrl = transform.GetComponent<ShooterCtrl>();
            if (shooterCtrl != null)
            {
                if (belongAnimal != null && !shooterCtrl.isDeath)
                {
                    belongAnimal.transform.DOKill();
                    belongAnimal.OnAttack();
                }

                shooterCtrl.OnDeath();
                var fx = mResLoader.LoadSync<GameObject>("fx", "lighting");
                Instantiate(fx, shooterCtrl.transform);
                checkTimes++;


                return false;

            }
            else
            {
                AnimalCtrl animalCtrl = transform.GetComponent<AnimalCtrl>();
                if (animalCtrl != null)
                {
                    if (killAnimalLevel >= animalCtrl.animalLevel)
                    {
                        if (belongAnimal != null && !animalCtrl.isDeath)
                        {
                            belongAnimal.transform.DOKill();
                            belongAnimal.OnAttack();
                        }

                        animalCtrl.OnDeath();
                        var fx = mResLoader.LoadSync<GameObject>("fx", "lighting");
                        Instantiate(fx, animalCtrl.transform);
                        checkTimes++;


                        return false;
                    }
                    else
                    {
                        if (isAnimalAttack)
                        {
                            animalCtrl.Defend(base.transform.parent);

                        }
                        checkTimes++;
                        return false;
                    }
                }
                else
                {
                    KnightCtrl knightCtrl = transform.GetComponent<KnightCtrl>();

                    if (knightCtrl != null)
                    {
                        if (isAnimalAttack)
                        {
                            base.transform.parent.GetComponent<AnimalCtrl>().OnDeath();
                        }

                        knightCtrl.OnDeath();
                        var fx = mResLoader.LoadSync<GameObject>("fx", "lighting");
                        Instantiate(fx, knightCtrl.transform);
                        checkTimes++;
                        return false;
                    }
                }
            }



        }
        return false;
    }
}
