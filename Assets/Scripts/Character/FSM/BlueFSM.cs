using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BlueIdle : FSMBase
{
    Animator animator;
    public BlueIdle(Animator animator)
    {
        this.animator = animator;
    }
    public override void OnEnter()
    {
        animator.SetInteger("playState", (int)PlayState.Idle);
    }
}
public class BlueMove : FSMBase
{
    Animator animator;
    public BlueMove(Animator animator)
    {
        this.animator = animator;
    }
    public override void OnEnter()
    {
        animator.SetInteger("playState", (int)PlayState.Move);
    }
}
public class BlueJump : FSMBase
{
    Animator animator;
    public BlueJump(Animator animator)
    {
        this.animator = animator;
    }
    public override void OnEnter()
    {
        animator.SetInteger("playState", (int)PlayState.Jump);
    }
}
public class BlueAttack : FSMBase
{
    //判断该动画是否播放完毕
    Animator animator;
    GameObject go;
    int attackCount;
    float attackTime;
    public BlueAttack(Animator animator)
    {
        this.animator = animator;
        go = animator.gameObject;
    }
    public override void OnEnter()
    {
        attackCount = 0;
        attackTime = Time.time + 0.6f;
        animator.SetInteger("playState", (int)PlayState.Attack);

        float targetAngle = go.GetComponent<PlayerBlue>().cam.eulerAngles.y;
        go.transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
    }
    //扩展
    //像攻击这样的特殊动作 可以在这里写攻击判定
    public override void OnStay()
    {
        base.OnStay();
        if (attackTime < Time.time && Input.GetButton("Fire1") && attackCount < 3)
        {
            attackTime = Time.time + 0.4f;
            animator.SetInteger("attackState", ++attackCount);
        }
        if (attackTime <= Time.time - 0.4f)
        {
            go.GetComponent<PlayerBlue>().initState();
        }
    }
    public override void OnExit()
    {
        base.OnExit();
        animator.SetInteger("attackState", 0);
    }
}
public class BlueHurt : FSMBase
{
    Animator animator;
    public BlueHurt(Animator animator)
    {
        this.animator = animator;
    }
    public override void OnEnter()
    {
        animator.SetInteger("playState", (int)PlayState.Hurt);
    }
}
public class BlueDeath : FSMBase
{
    Animator animator;
    public BlueDeath(Animator animator)
    {
        this.animator = animator;
    }
    public override void OnEnter()
    {
        animator.SetInteger("playState", (int)PlayState.Death);
    }
}
