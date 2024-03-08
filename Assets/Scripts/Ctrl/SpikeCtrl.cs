using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SpikeCtrl: MonoBehaviour, IController
{
    public LayerMask layerMask;

    bool _hitPlayer;
    public bool changeTrigger, hasTimes, isAnimalAttack;
    public int times = 1, checkTimes = 0, killAnimalLevel;
    public AnimalCtrl belongAnimal;

    public Rigidbody m_rigidBody;

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

    public virtual void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody>();
    }

    public virtual bool CheckCollider(Transform player)
    {
        if (((1 << player.gameObject.layer) & layerMask) != 0)
        {
            

            PlayerCtrl playerCtrl = player.GetComponent<PlayerCtrl>();
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
                ShooterCtrl shooterCtrl = player.GetComponent<ShooterCtrl>();
                if (shooterCtrl != null)
                {
                    if (belongAnimal != null && !shooterCtrl.isDeath)
                    {
                        belongAnimal.transform.DOKill();
                        belongAnimal.OnAttack();
                    }

                    shooterCtrl.OnDeath();
                    checkTimes++;

                    
                    return false;

                }
                else
                {
                    AnimalCtrl animalCtrl = player.GetComponent<AnimalCtrl>();
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
                            checkTimes++;


                            return false;
                        }
                        else
                        {
                            if (isAnimalAttack)
                            {
                                animalCtrl.Defend(transform.parent);

                            }
                            checkTimes++;
                            return false;
                        }
                    }
                    else
                    {
                        KnightCtrl knightCtrl = player.GetComponent<KnightCtrl>();

                        if (knightCtrl != null)
                        {
                            if (isAnimalAttack)
                            {
                                transform.parent.GetComponent<AnimalCtrl>().OnDeath();
                            }

                            knightCtrl.OnDeath();
                            checkTimes++;
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

            if (!hasTimes || (hasTimes && checkTimes < times))
            {
                CheckCollider(player);
            }
        }
    }
}
