using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// 游戏结束窗口控制器
/// </summary>
public class UIDefeatPanel : UIBase
{
    void Start()
    {
        AddButtonListen("Restart_N", Restart);
        AddButtonListen("Back_N", Back);
        Time.timeScale = 0;
    }

    /// <summary>
    /// 重新开始
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    /// <summary>
    /// 返回界面
    /// </summary>
    public void Back()
    {
        SceneManager.LoadScene("Create_Role");
        Time.timeScale = 1;
    }
}

