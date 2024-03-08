using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BulletCtrl : SpikeCtrl
{
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
    }

    private void FixedUpdate()
    {
        transform.position += moveVec * Time.fixedDeltaTime;
    }

    public override void OnTriggerEnter(Collider collision)
    {
        if (collision != null)
        {
            Transform player = collision.transform;

            if(CheckCollider(player))
            {
                Debug.Log("destroy1");
                Destroy(gameObject);
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
