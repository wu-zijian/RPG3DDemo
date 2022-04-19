using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float speed = 5f;                                                //移动速度
    protected Vector3 move;                                                 //移动参数
    public float jumpSpeed = 10f;                                           //跳跃速度
    public float gravity = -0.5f;                                           //重力
    protected Vector3 velocity = Vector3.zero;                              //重力影响的速度
    protected CharacterController characterController;                      //控制器
    protected Animator anim;                                                //动画
    [HideInInspector] public CharacterStats characterStats;                 //角色数据
    public bool isMoving = false;
    public bool isJump = false;
    public bool isFall = false;
    public bool isAttack = false;
    public bool isHurt = false;
    public bool isDeath = false;

    public float attack;
    public float attackTime;
    public float hurt;
    public float hurtTime = 0.5f;
    protected virtual void Start()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
    }

    public virtual bool DeathCheck()
    {
        if (characterStats != null && characterStats.CurrentHealth <= 0)
        {
            Death();
            isDeath = true;
            return true;
        }
        return false;
    }

    protected virtual void Idle()
    { }
    protected virtual void Death()
    { }
    public virtual void initState()
    { }
    public virtual void UpdateHealthBar()
    { }

    public void BeHurt()
    {
        isHurt = true;
        isAttack = false;
        attack = Time.time + attackTime;
    }

    public virtual void SetMove(Vector3 move)
    {
        this.move = move;
    }
}