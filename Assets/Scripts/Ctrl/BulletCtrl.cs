using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BulletCtrl : SpikeCtrl
{
    public Transform from;
    public Vector3 moveVec
    {
        get
        {
            return _moveVec;
        }
        set
        {
            _moveVec = value;
            Vector3 rotateVec = new Vector3( 0, 0, Vector3.Angle(Vector3.up, _moveVec));
            transform.rotation = Quaternion.Euler(rotateVec);
        }
    }

    public Vector3 _moveVec;
    [SerializeField]
    float existTime = 5f;
    private void Start()
    {
        StartCoroutine(AutoDestroy());
        transform.parent = null;
    }

    private void FixedUpdate()
    {
        transform.position += moveVec * Time.fixedDeltaTime;
    }
    public override bool CheckCollider(Transform player)
    {
        if(from == player)
        {
            return false;
        }
        if (((1 << player.gameObject.layer) & layerMask) != 0)
        {
            Destroy(gameObject);

            PlayerCtrl playerCtrl = player.GetComponent<PlayerCtrl>();
            if (playerCtrl != null)
            {

                if (belongAnimal != null && !playerCtrl.isDeath)
                {
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
    public override void OnTriggerEnter(Collider collision)
    {
        if (collision != null)
        {
            Transform player = collision.transform;

            if(CheckCollider(player))
            {
                Debug.Log("destroy1");

            }


        }
    }

    IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(existTime);
        Debug.Log("destroy2");
        Destroy(gameObject);
    }

}
