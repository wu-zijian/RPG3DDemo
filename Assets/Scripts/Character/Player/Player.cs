using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayState
{
    Idle,
    Move,
    Jump,
    Attack,
    Hurt,
    Death,
    Max,
}
public class Player : Character
{
    [SerializeField] protected PlayState playState = PlayState.Idle;        //角色状态
    protected FSMManager fsmMrg = new FSMManager((int)PlayState.Max);       //状态机

    public override void initState()
    {
        playState = PlayState.Idle;
        fsmMrg.ChangeState((int)PlayState.Idle);
    }

    ///<summary>
    ///死亡检测
    ///</summary>
    public override bool DeathCheck()
    {
        if (characterStats != null && characterStats.CurrentHealth <= 0)
        {
            Death();
            isDeath = true;
            Invoke("OpenDefeatPanel", 1f);
            return true;
        }
        return false;
    }

    ///<summary>
    ///死亡
    ///</summary>
    protected override void Death()
    {
        move = Vector3.zero;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        Destroy(this.gameObject, 1.5f);
        fsmMrg.ChangeState((int)PlayState.Death);
    }


    ///<summary>
    ///启动失败页面
    ///</summary>
    public void OpenDefeatPanel()
    {
        if (FightSceneCtrl.Instance != null)
        {
            FightSceneCtrl.Instance.OpenDefeatPanel();
            Cursor.lockState = CursorLockMode.None;
            this.enabled = false;
        }
    }
}
