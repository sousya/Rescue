using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventReceiver : MonoBehaviour, IController
{
    [SerializeField]
    Animator anim;
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Start()
    {
        this.RegisterEvent<PlayAnimation>(
            e =>
            {
                anim.Play(e.animName);
            });
    }
}
