using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyBlack : Enemy
{
    #region 生命周期
    protected override void Start()
    {
        base.Start();
        _Agent.isStopped = true;
        var root = new SelectNode().AddChild(new SequenceNode().AddChild(new ConditionNode(() => isDeath))
                                                                .AddChild(new ActionNode(Death)))
                                    .AddChild(new SequenceNode().AddChild(new ConditionNode(() => isHurt))
                                                                .AddChild(new ActionNode(Hurt)))
                                    .AddChild(new SequenceNode().AddChild(new ConditionNode(() => isAttack))
                                                                .AddChild(new ActionNode(Attack)))
                                    .AddChild(new SequenceNode().AddChild(new ConditionNode(() => isChase))
                                                                .AddChild(new ActionNode(Chase)))
                                    .AddChild(new SequenceNode().AddChild(new ConditionNode(() => isPatrol))
                                                                .AddChild(new ActionNode(Patrol)))
                                    .AddChild(new ActionNode(Idle));
        bt = new BehaviorTree(root);

    }
    void Update()
    {
        bt.Update();
        if (FoundPlayer() && !isChase)
        {
            isChase = true;
            chase = Time.time + chaseTime;
        }
        else if (!isPatrol)
        {
            isPatrol = true;
            GetRandomPoint();
            patrol = Time.time + patrolTime;
        }
        if (Vector3.Distance(transform.position, TargetPos) <= attackRange && isChase && !isAttack && attack < Time.time)
        {
            isAttack = true;
            attack = Time.time + attackTime;
        }
        //模拟重力
        if (characterController.isGrounded)
        {
            isFall = false;
            velocity.y = 0;
        }
        else
        {
            isFall = true;
            velocity.y += gravity * Time.deltaTime;
            move = transform.up * velocity.y;
            characterController.Move(move);
        }
    }
    #endregion

    #region 行为
    protected override void Idle()
    {
        _Agent.isStopped = true;
        TargetPos = originBornPos;
        _Agent.SetDestination(TargetPos);
        anim.SetInteger("EnemyState", (int)EnemyState.Idle);
    }
    private void Patrol()
    {
        if (patrol < Time.time)
        {
            isPatrol = false;
        }
        _Agent.isStopped = false;
        _Agent.SetDestination(TargetPos);
        anim.SetInteger("EnemyState", (int)EnemyState.Patrol);
    }
    private void Chase()
    {
        _Agent.isStopped = false;
        _Agent.SetDestination(TargetPos);
        if (chase < Time.time) isChase = false;
        anim.SetInteger("EnemyState", (int)EnemyState.Chase);
    }
    protected void Attack()
    {
        _Agent.isStopped = true;
        anim.SetInteger("EnemyState", (int)EnemyState.Attack);
        if (attack < Time.time)
        {
            isAttack = false; anim.SetInteger("EnemyState", (int)EnemyState.Patrol);
        }
    }
    protected void Hurt()
    {
        if (hurt < Time.time)
        {
            isHurt = false;
        }
        anim.SetInteger("EnemyState", (int)EnemyState.Hurt);
    }
    protected override void Death()
    {
        _Agent.isStopped = true;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        Destroy(this.gameObject, 1.5f);
        anim.SetInteger("EnemyState", (int)EnemyState.Death);
    }
    #endregion

    public override void initState()
    {
        isAttack = false;
        isHurt = false;
        isJump = false;
        isMoving = false;
    }
}
