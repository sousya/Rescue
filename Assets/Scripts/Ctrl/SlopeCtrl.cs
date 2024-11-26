using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeCtrl : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            PlayerCtrl playerCtrl = collision.transform.GetComponent<PlayerCtrl>();
            if(playerCtrl != null)
            {
                playerCtrl.inSlope = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider != null)
        {
            PlayerCtrl playerCtrl = collision.transform.GetComponent<PlayerCtrl>();
            if (playerCtrl != null)
            {
                playerCtrl.inSlope = false;
            }
        }
    }
}
