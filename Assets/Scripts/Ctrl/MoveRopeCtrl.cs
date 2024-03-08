using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoveRopeCtrl: RopeCtrl
{
    public bool isMove;

    public LayerMask layerMask;

    public Vector3 moveVec;

    public Rigidbody FixedRigid;
}
