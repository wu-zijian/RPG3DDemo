using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 角色信息窗口控制器
/// </summary>
public class UIRoleCreateCtrl : UIBase
{
    private string currentPlayer = "Red";
    void Start()
    {
        AddButtonListen("Right_N", Right);
        AddButtonListen("Left_N", Left);
        AddButtonListen("Comfirm_N", Comfirm);
        UIManager.Instance.GetGameObject(transform.name, "Mode_N").AddComponent<RotateMode>();
    }

    public void Right()
    {
        RoleCreateSceneCtrl.Instance.OnNextButtonClick();
        UpdateInfo();
    }
    public void Left()
    {
        RoleCreateSceneCtrl.Instance.OnPrevButtonClick();
        UpdateInfo();
    }
    public void Comfirm()
    {
        RoleCreateSceneCtrl.Instance.OnComfirmRole();
    }

    private void UpdateInfo()
    {
        GameObject pausePanel = UIManager.Instance.GetGameObject(this.name, "InfoPanel_N");
        string info;
        if (RoleCreateSceneCtrl.Instance.selectedIndex == 1)
        {
            info = "特工Red\n第一人称作战角色\nWASD移动\n鼠标瞄准射击";
        }
        else
        {
            info = "特工Blue\n第三人称作战角色\nWASD移动\n鼠标攻击，多段攻击";
        }
        pausePanel.transform.GetChild(0).GetComponent<Text>().text = info;
    }
}