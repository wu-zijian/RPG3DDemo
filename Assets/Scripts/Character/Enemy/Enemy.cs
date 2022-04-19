using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum EnemyState
{
    Idle,
    Patrol,
    Chase,
    Attack,
    Hurt,
    Death,
}

public class Enemy : Character
{
    protected NavMeshAgent _Agent;
    private Transform HealthBarCanvas;
    private Image currentHealth;
    public EnemyState enemyState;                   //敌人状态
    public float viewRange;                         //可见距离
    public float patrolRange;                       //巡逻距离
    public bool isPatrol = false;
    public bool isChase = false;
    public float patrol;
    public float chase;
    public float patrolTime = 5f;  
    public float chaseTime = 3f;
    public float attackRange;                       //攻击距离
    protected BehaviorTree bt;                      //行为树
    public Vector3 TargetPos;                       //目标点
    public Vector3 originBornPos;                   //出生点

    protected override void Start()
    {
        base.Start();
        _Agent = this.GetComponent<NavMeshAgent>();
        HealthBarCanvas = transform.Find("HealthBarCanvas");
        currentHealth = transform.Find("HealthBarCanvas/MaxHealth/CurrentHealth").GetComponent<Image>();
        TargetPos = transform.position;
        originBornPos = transform.position;
        enemyState = EnemyState.Idle;
    }

    private void LateUpdate()
    {
        if (FightSceneCtrl.Instance.cam != null)
        {
            //血条方向
            HealthBarCanvas.forward = FightSceneCtrl.Instance.cam.transform.forward;
        }
    }

    ///<summary>
    ///检测玩家是否被发现
    ///</summary>
    protected virtual bool FoundPlayer()
    {
        var player = Physics.OverlapSphere(transform.position, viewRange, LayerMask.GetMask("PlayerLayer"));
        if (player.Length != 0)
        {
            TargetPos = player[0].transform.position;
            return true;
        }
        else
        {
            return false;
        }
    }

    ///<summary>
    ///生成随机点
    ///</summary>
    protected virtual void GetRandomPoint()
    {
        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);
        Vector3 randomPoint = new Vector3(originBornPos.x + randomX, originBornPos.y, originBornPos.z + randomZ);
        TargetPos = randomPoint;
    }

    ///<summary>
    ///画图，黄色为发现玩家距离，绿色为攻击距离，蓝色是巡逻位置
    ///</summary>
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(originBornPos, patrolRange);
    }

    ///<summary>
    ///更新血条
    ///</summary>
    public override void UpdateHealthBar()
    {
        currentHealth.fillAmount = (float)characterStats.CurrentHealth / characterStats.MaxHealth;
    }

}
