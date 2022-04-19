using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum NodeType
{
    Action, Condition, Wait, Sequence, Select
}

public enum NodeState
{
    Ready, Running, Success, Failed
}


///<summary>
///行为树
///</summary>
public class BehaviorTree
{
    private readonly BtNode _rootNode;//根节点
    private BtNode _runingNode;
    public NodeState State { get; private set; }

    public BehaviorTree(BtNode root)
    {
        _rootNode = root;
        State = NodeState.Ready;
    }

    public NodeState Update()
    {
        State = _rootNode.OnVisit();
        return State;
    }

    public void Reset()
    {
        _rootNode.Reset();
    }
}

#region 节点定义

public abstract class BtNode                        //节点父级
{
    public NodeType Type { get; private set; }
    public NodeState State { get; protected set; }

    protected BtNode(NodeType nodeType)             //构造节点
    {
        State = NodeState.Ready;
        Type = nodeType;
    }

    public abstract NodeState OnVisit();            //经过节点

    public virtual void Reset()                     //初始化节点
    {
        State = NodeState.Ready;
    }
}

public abstract class BtCompostieNode : BtNode
{
    protected List<BtNode> ChildNodes;//子节点集合

    protected BtCompostieNode(List<BtNode> nodes, NodeType nodeType) : base(nodeType)//构造节点
    {
        ChildNodes = nodes ?? new List<BtNode>();
    }

    public virtual BtCompostieNode AddChild(BtNode node)//添加子节点
    {
        ChildNodes.Add(node);
        return this;
    }
}
#endregion

#region 叶子节点

// 动作节点
public class ActionNode : BtNode
{
    private readonly Action _action;

    public ActionNode(Action action) : base(NodeType.Action)
    {
        _action = action;
    }

    public override NodeState OnVisit()
    {
        _action?.Invoke();
        return NodeState.Success;
    }
}

// 条件节点
public class ConditionNode : BtNode
{
    private readonly Func<bool> _checkFunc;

    public ConditionNode(Func<bool> checkFunc) : base(NodeType.Condition)
    {
        _checkFunc = checkFunc;
    }

    public override NodeState OnVisit()
    {
        if (_checkFunc == null)
            return NodeState.Success;

        return _checkFunc.Invoke() ? NodeState.Success : NodeState.Failed;
    }
}

// 等待节点
public class WaitNode : BtNode
{
    private readonly int _waitSec;
    private long _startSec;

    public WaitNode(int waitSec) : base(NodeType.Wait)
    {
        _waitSec = waitSec;
    }

    public override NodeState OnVisit()
    {
        if (_startSec == 0)
        {
            _startSec = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
            State = NodeState.Running;
            Console.WriteLine(DateTime.Now.Ticks / TimeSpan.TicksPerSecond);
        }

        // 还没有到时间返回running
        if (DateTime.Now < new DateTime((_waitSec + _startSec) * TimeSpan.TicksPerSecond))
            return State;

        Console.WriteLine(DateTime.Now.Ticks / TimeSpan.TicksPerSecond);
        State = NodeState.Success;
        return State;
    }

    public override void Reset()
    {
        base.Reset();
        _startSec = 0;
    }
}

#endregion

#region 复合节点

public class SequenceNode : BtCompostieNode
{
    public SequenceNode(List<BtNode> nodes = null) : base(nodes, NodeType.Sequence)
    {
    }

    public override NodeState OnVisit()
    {
        foreach (var node in ChildNodes)
        {
            var result = node.OnVisit();
            if (result != NodeState.Success)
                return result;
        }

        return NodeState.Success;
    }
}

public class SelectNode : BtCompostieNode
{
    public SelectNode(List<BtNode> nodes = null) : base(nodes, NodeType.Select)
    {
    }

    public override NodeState OnVisit()
    {
        foreach (var node in ChildNodes)
        {
            var result = node.OnVisit();
            if (result != NodeState.Failed)
                return result;
        }

        return NodeState.Failed;
    }
}
#endregion