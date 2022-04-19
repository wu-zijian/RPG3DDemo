using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 暂停窗口控制器
/// </summary>
public class UIPauseCtrl : UIBase
{
    void Start()
    {
        AddButtonListen("Resume_N", ResumeGame);
        AddButtonListen("Restart_N", Restart);
        AddButtonListen("Back_N", Back);
        AddSliderListen("Music_N", SetMainVolume);
        AddSliderListen("Effect_N", SetVolume);
        AddToggleListen("FullScreen_N", SetFullScreen);
    }

    /// <summary>
    /// 继续游戏
    /// </summary>
    public void ResumeGame()
    {
        GameObject pausePanel = UIManager.Instance.GetPanel("PausePanel");
        if (pausePanel != null)
            pausePanel.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
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

    /// <summary>
    /// 设置主音量
    /// </summary>
    public void SetMainVolume(float value)
    {
        SoundsManager.Instance.mainAudioMixer.SetFloat("MainVolume", value);
    }

    /// <summary>
    /// 设置音效音量
    /// </summary>
    public void SetVolume(float value)
    {
        SoundsManager.Instance.audioMixer.SetFloat("MainVolume", value);
    }

    /// <summary>
    /// 设置是否全屏
    /// </summary>
    public void SetFullScreen(bool value)
    {
        if (value)
        {
            //获取设置当前屏幕分辩率
            Resolution[] resolutions = Screen.resolutions;
            //设置当前分辨率
            Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height, true);

            Screen.fullScreen = true;  //设置成全屏
        }
        else
        {
            //获取设置当前屏幕分辩率
            Resolution[] resolutions = Screen.resolutions;
            //设置当前分辨率
            Screen.SetResolution(resolutions[resolutions.Length - 1].width - 100, resolutions[resolutions.Length - 1].height - 100, true);

            Screen.fullScreen = false;  //设置成不全屏
        }
    }
}
