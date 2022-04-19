using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// UI的单例管理类
/// </summary>
public class UIManager : Singleton<UIManager>
{
    /// <summary>
    /// 主摄像机
    /// </summary>
    private Transform mainCanvas;
    /// <summary>
    /// 注册的UI面板
    /// </summary>
    private Dictionary<string, GameObject> allPanel;
    /// <summary>
    /// 注册的UI组件
    /// </summary>
    private Dictionary<string, Dictionary<string, GameObject>> allWebgate;

    protected override void Awake()
    {
        base.Awake();
        allPanel = new Dictionary<string, GameObject>();
        allWebgate = new Dictionary<string, Dictionary<string, GameObject>>();
        mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").transform;
    }

    /// <summary>
    /// 注册UIBase
    /// </summary>
    /// <param name="panelName">名字</param>
    /// <param name="obj">组件</param>
    public void RegisPanel(string panelName, GameObject obj)
    {
        if (!allPanel.ContainsKey(panelName))
        {
            allPanel.Add(panelName, obj);
        }
    }

    /// <summary>
    /// 注册UIBehaviour
    /// </summary>
    /// <param name="panelName">名字</param>
    /// <param name="obj">组件</param>
    public void RegisGameObject(string panelName, string wedageName, GameObject obj)
    {
        if (!allWebgate.ContainsKey(panelName))
        {
            allWebgate[panelName] = new Dictionary<string, GameObject>();
        }
        allWebgate[panelName].Add(wedageName, obj);
    }

    /// <summary>
    /// 获取UIBase
    /// </summary>
    /// <param name="panelName">名字</param>
    public GameObject GetPanel(string panelName)
    {
        if (allPanel.ContainsKey(panelName))
        {
            return allPanel[panelName];
        }
        return null;
    }

    /// <summary>
    /// 获取UIBehavours
    /// </summary>
    /// <param name="panelName">UIBase名字</param>
    /// <param name="wedageName">组件名</param>
    public GameObject GetGameObject(string panelName, string wedageName)
    {
        if (allWebgate.ContainsKey(panelName))
        {
            return allWebgate[panelName][wedageName];
        }
        return null;
    }

    /// <summary>
    /// 加载结束界面
    /// </summary>
    public void OpenDefeatPanel()
    {

        Object obj = Resources.Load("Fighting/UI/DefeatPanel");
        GameObject defeatPanel = GameObject.Instantiate(obj) as GameObject;
        defeatPanel.name = defeatPanel.name.Replace("(Clone)", "");
        defeatPanel.AddComponent<UIDefeatPanel>();
        defeatPanel.transform.SetParent(UIManager.Instance.GetMainCanvas(), false);
    }

    /// <summary>
    /// 获取摄像机
    /// </summary>
    public Transform GetMainCanvas()
    {
        return mainCanvas;
    }

}
