using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpikeCtrl: MonoBehaviour, IController,ICanSendEvent
{
    public LayerMask layerMask;

    public bool _hitPlayer;
    public bool changeTrigger, hasTimes, isAnimalAttack, isDeath, oneKillKnight, isKnightAttack;
    public int times = 1, checkTimes = 0, killAnimalLevel, useDeath = 2;
    public AnimalCtrl belongAnimal;
    //public KnightCtrl belongKnight;
    public Animator anim;
    public GameObject NormalNode, BrokenNode;

    public Rigidbody m_rigidBody;
    public List<Rigidbody> rigidBodies;

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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1)) 
        {
            OnDeath();
        }
    }

    public virtual bool CheckCollider(Transform transform)
    {
        if(isDeath || (hasTimes && checkTimes >= times) || transform == this.transform.parent)
        {
            return false;
        }

        PlayerCtrl playerCtrl = transform.GetComponent<PlayerCtrl>();
        if (playerCtrl != null)
        {

            if (belongAnimal != null && !playerCtrl.isDeath)
            {
                //belongAnimal.transform.DOKill();
                belongAnimal.OnAttack();
            }

            playerCtrl.OnDeath(useDeath);

            _hitPlayer = true;
            return true;
        }
        else
        {
            ShooterCtrl shooterCtrl = transform.GetComponent<ShooterCtrl>();
            if (shooterCtrl != null)
            {
                if (belongAnimal != null && !shooterCtrl.isDeath && !isKnightAttack)
                {
                    belongAnimal.transform.DOKill();
                    belongAnimal.OnAttack();
                }

                if(isKnightAttack)
                {
                    belongAnimal.OnDeath();
                }

                shooterCtrl.OnDeath();
                checkTimes++;


                return false;

            }
            else
            {
                KnightCtrl knightCtrl = transform.GetComponent<KnightCtrl>();

                if (knightCtrl != null)
                {
                    if (isAnimalAttack)
                    {
                        this.transform.parent.GetComponent<AnimalCtrl>().OnDeath();
                    }



                    if(oneKillKnight)
                    {
                        knightCtrl.OnDeath();
                    }

                    knightCtrl.OnDeath();
                    checkTimes++;
                    return false;
                }              
                else
                {
                    AnimalCtrl animalCtrl = transform.GetComponent<AnimalCtrl>();
                    if (animalCtrl != null)
                    {
                        if (killAnimalLevel > animalCtrl.animalLevel)
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
                                animalCtrl.Defend(base.transform.parent);

                            }
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
            if(player != transform.parent)
            {
                CheckCollider(player);
            }
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

    public void OnDeath()
    {
        //foreach (var b in rigidBodies)
        //{
        //    b.useGravity = true;
        //    b.isKinematic = false;
        //}

        isDeath = true;
        
    }
}
