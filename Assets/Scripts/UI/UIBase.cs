using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

/// <summary>
/// 所有UI的基类
/// </summary>
public class UIBase : MonoBehaviour
{
    void Awake()
    {
        //向UIManager注册本身
        UIManager.Instance.RegisPanel(transform.name, gameObject);
        //获取子组件
        Transform[] btnArr = GetComponentsInChildren<Transform>();
        for (int i = 0; i < btnArr.Length; i++)
        {
            if (btnArr[i].name.EndsWith("_N"))
                btnArr[i].gameObject.AddComponent<UIBehaviour>();
        }
    }

    /// <summary>
    /// 获取注册在UIManager的组件
    /// </summary>
    /// <param name="widegateName">组件名</param>
    public GameObject GetWedagte(string widegateName)
    {
        return UIManager.Instance.GetGameObject(transform.name, widegateName);
    }

    /// <summary>
    /// 获取注册在UIManager的组件中的脚本
    /// </summary>
    /// <param name="widegateName">组件名</param>
    public UIBehaviour GetBehaviour(string widegateName)
    {
        GameObject tmpObj = GetWedagte(widegateName);
        if (tmpObj != null)
        {
            return tmpObj.GetComponent<UIBehaviour>();
        }
        return null;
    }

    /// <summary>
    /// 添加组件回调函数
    /// </summary>
    /// <param name="widegateName">组件名</param>
    /// <param name="action">回调函数</param>
    public void AddButtonListen(string widegateName, UnityAction action)
    {
        UIBehaviour tmpBehaviour = GetBehaviour(widegateName);
        if (tmpBehaviour != null)
        {
            tmpBehaviour.AddButtonListen(action);
        }
    }

    public void AddSliderListen(string widegateName, UnityAction<float> action)
    {
        UIBehaviour tmpBehaviour = GetBehaviour(widegateName);
        if (tmpBehaviour != null)
        {
            tmpBehaviour.AddSliderListen(action);
        }
    }

    public void AddToggleListen(string widegateName, UnityAction<bool> action)
    {
        UIBehaviour tmpBehaviour = GetBehaviour(widegateName);
        if (tmpBehaviour != null)
        {
            tmpBehaviour.AddToggleListen(action);
        }
    }

}