using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// 所有UI的组件类
/// </summary>
public class UIBehaviour : MonoBehaviour
{
    private void Awake()
    {
        UIBase tmpBase = transform.GetComponentInParent<UIBase>();
        UIManager.Instance.RegisGameObject(tmpBase.name, transform.name, gameObject);
    }

    /// <summary>
    /// 添加按钮回调函数
    /// </summary>
    /// <param name="action">回调函数</param>
    public void AddButtonListen(UnityAction action)
    {
        Button tmpButton = transform.GetComponent<Button>();
        if (tmpButton != null)
        {
            tmpButton.onClick.AddListener(action);
        }
    }

    /// <summary>
    /// 添加滑动条回调函数
    /// </summary>
    /// <param name="action">回调方法</param>
    public void AddSliderListen(UnityAction<float> action)
    {
        Slider tmpSlider = transform.GetComponent<Slider>();
        if (tmpSlider != null)
        {
            tmpSlider.onValueChanged.AddListener(action);
        }
    }

    /// <summary>
    /// 添加滑动条回调函数
    /// </summary>
    /// <param name="action">回调方法</param>
    public void AddToggleListen(UnityAction<bool> action)
    {
        Toggle tmpToggle = transform.GetComponent<Toggle>();
        if (tmpToggle != null)
        {
            tmpToggle.onValueChanged.AddListener(action);
        }
    }

    /// <summary>
    /// 添加输入框回调函数
    /// </summary>
    /// <param name="action">回调方法</param>
    public void AddInputFiledListen(UnityAction<string> action)
    {
        InputField tmpInputField = transform.GetComponent<InputField>();
        if (tmpInputField != null)
        {
            tmpInputField.onEndEdit.AddListener(action);
        }
    }

    /// <summary>
    /// 修改文字
    /// </summary>
    /// <param name="content">修改内容</param>
    public void ChangeTextContent(string content)
    {
        Text tmpText = transform.GetComponent<Text>();
        if (tmpText != null)
        {
            tmpText.text = content;
        }
    }

    /// <summary>
    /// 修改图片
    /// </summary>
    /// <param name="content">修改内容</param>
    public void ChangeImage(Sprite content)
    {
        Image tmpImage = transform.GetComponent<Image>();
        if (tmpImage != null)
        {
            tmpImage.sprite = content;
        }
    }
}
