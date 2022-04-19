using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RedIdle : FSMBase
{
    Animator animator;
    public RedIdle(Animator animator)
    {
        this.animator = animator;
    }
    public override void OnEnter()
    {
        animator.SetInteger("playState", (int)PlayState.Idle);
    }
}
public class RedMove : FSMBase
{
    Animator animator;
    public RedMove(Animator animator)
    {
        this.animator = animator;
    }
    public override void OnEnter()
    {
        animator.SetInteger("playState", (int)PlayState.Move);
    }
}
public class RedJump : FSMBase
{
    Animator animator;
    public RedJump(Animator animator)
    {
        this.animator = animator;
    }
    public override void OnEnter()
    {
        animator.SetInteger("playState", (int)PlayState.Jump);
    }
}
public class RedAttack : FSMBase
{
    //判断该动画是否播放完毕
    Animator animator;
    float attackTime;
    public RedAttack(Animator animator)
    {
        this.animator = animator;
    }
    public override void OnEnter()
    {
        attackTime = Time.time + 0.1f;
        animator.SetInteger("playState", (int)PlayState.Attack);
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;//碰撞信息
        if (Physics.Raycast(ray, out hit))
        {
            SoundsManager.Instance.ShootAudio();
            if (hit.transform.tag == "Enemy")
            {
                hit.transform.GetComponent<CharacterStats>().TakeDamage(animator.gameObject.GetComponent<CharacterStats>());
                Character character = hit.transform.gameObject.GetComponent<Character>();
                character.DeathCheck();
                character.UpdateHealthBar();
            }
        }
    }
    public override void OnStay()
    {
        base.OnStay();
        if (attackTime < Time.time)
        {
            animator.gameObject.GetComponent<PlayerRed>().StopAttack();
        }
    }
    public override void OnExit()
    {
        base.OnExit();
    }
}
public class RedHurt : FSMBase
{
    Animator animator;
    public RedHurt(Animator animator)
    {
        this.animator = animator;
    }
    public override void OnEnter()
    {
        animator.SetInteger("playState", (int)PlayState.Hurt);
    }
}
public class RedDeath : FSMBase
{
    Animator animator;
    public RedDeath(Animator animator)
    {
        this.animator = animator;
    }
    public override void OnEnter()
    {
        animator.SetInteger("playState", (int)PlayState.Death);
    }
}
