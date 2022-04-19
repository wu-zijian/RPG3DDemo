using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 对战UI控制器
/// </summary>
public class UISceneFightingCtrl : UIBase
{
    void Start()
    {
        UIManager.Instance.GetGameObject(transform.name, "PlayerHealth_N").AddComponent<UIPlayerHealth>();
        AddButtonListen("Set_N", OpenSet);
    }

    /// <summary>
    /// 打开暂停设置界面
    /// </summary>
    private void OpenSet()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        GameObject PausePanel = UIManager.Instance.GetPanel("PausePanel");
        if (PausePanel != null)
        {
            PausePanel.SetActive(true);
        }
        else
        {
            Object tmpObj = Resources.Load("Fighting/UI/PausePanel");
            PausePanel = GameObject.Instantiate(tmpObj) as GameObject;
            PausePanel.name = PausePanel.name.Replace("(Clone)", "");
            PausePanel.transform.SetParent(UIManager.Instance.GetMainCanvas(), false);
            PausePanel.AddComponent<UIPauseCtrl>();

            //初始化
            float value;
            SoundsManager.Instance.mainAudioMixer.GetFloat("MainVolume", out value);
            PausePanel.transform.Find("Music_N").gameObject.GetComponent<Slider>().value = value;
            SoundsManager.Instance.audioMixer.GetFloat("MainVolume", out value);
            PausePanel.transform.Find("Effect_N").gameObject.GetComponent<Slider>().value = value;

            PausePanel.transform.Find("FullScreen_N").gameObject.GetComponent<Toggle>().isOn = Screen.fullScreen;
        }
    }
}