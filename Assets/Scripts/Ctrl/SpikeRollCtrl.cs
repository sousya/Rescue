using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpikeRollCtrl: SpikeCtrl
{
    public RopeHangCtrl ropeHangCtrl;

    public override bool CheckCollider(Transform transform)
    {
        if (isDeath || (hasTimes && checkTimes >= times))
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

                CheckTime();



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
                        CheckTime();



                        return false;
                    }
                    else
                    {
                        if (isAnimalAttack)
                        {
                            animalCtrl.Defend(base.transform.parent);

                        }

                        CheckTime();

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
                        CheckTime();


                        return false;
                    }
                }
            }



        }
        return false;
    }

    void CheckTime()
    {
        if (ropeHangCtrl.targetList.Count <= 0)
        {
            checkTimes++;
        }

        if(checkTimes >= times)
        {
            Destroy(gameObject); 
        }
    }
}
