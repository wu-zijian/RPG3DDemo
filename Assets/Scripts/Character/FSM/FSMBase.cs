using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class FSMBase
{
    //进入这个状态
    public abstract void OnEnter();

    //正处在这个状态
    public virtual void OnStay()
    {
    }
    //退出这个状态
    public virtual void OnExit()
    {
    }
}