using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///状态机控制
///</summary>
public class FSMManager
{
    //把所有基类存起来
    FSMBase[] allState;
    //记录当前的状态值
    int curIndex;
    //记录当前有多少个状态
    int topIndex;

    public FSMManager(int nub)
    {
        //初始化这个账本
        allState = new FSMBase[nub];
        //-1表示没有前一个状态
        curIndex = -1;
        topIndex = -1;
    }

    //提供一个接口 添加新状态
    public void AddState(FSMBase myState)
    {
        //如果满了 return
        if (topIndex >= allState.Length - 1)
        {
            return;
        }
        topIndex++;
        allState[topIndex] = myState;
    }
    public void ChangeState(int index)
    {
        //如果要切换的状态，和当前状态一致，就没必要切换
        if (curIndex == index)
        {
            return;
        }
        //如果有前一个状态
        if (curIndex != -1)
        {
            //前一个状态退出
            allState[curIndex].OnExit();
        }
        //切换要切换的状态
        curIndex = index;
        //当前状态进入
        allState[curIndex].OnEnter();
    }
    public void Update()
    {
        if (curIndex != -1)
        {
            //一直运行在当前状态
            allState[curIndex].OnStay();
        }
    }
}