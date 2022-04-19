using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

///<summary>
///角色选择场景控制
///</summary>
public class RoleCreateSceneCtrl : Singleton<RoleCreateSceneCtrl>
{
    public GameObject[] rolePrefabs;
    private GameObject[] roleGameObject;
    public Transform mode;
    public int selectedIndex = 0;
    private int length;
    void Start()
    {
        length = rolePrefabs.Length;
        roleGameObject = new GameObject[length];
        for (int i = 0; i < length; i++)
        {
            roleGameObject[i] = GameObject.Instantiate(rolePrefabs[i], mode) as GameObject;
            roleGameObject[i].layer = LayerMask.NameToLayer("Ignore");
        }
        UpdateRoleShow();
    }

    /// <summary>
    /// 展示角色
    /// </summary>
    private void UpdateRoleShow()
    {
        for (int i = 0; i < length; i++)
        {
            roleGameObject[i].SetActive(false);
        }
        roleGameObject[selectedIndex].SetActive(true);
    }

    /// <summary>
    /// 右按钮
    /// </summary>
    public void OnNextButtonClick()
    {
        selectedIndex++;
        selectedIndex %= length;
        UpdateRoleShow();
    }

    /// <summary>
    /// 左按钮
    /// </summary>
    public void OnPrevButtonClick()
    {
        selectedIndex--;
        if (selectedIndex == -1)
        {
            selectedIndex = length - 1;
        }
        UpdateRoleShow();
    }

    /// <summary>
    /// 加载下一个场景
    /// </summary>
    public void OnComfirmRole()
    {
        PlayerPrefs.SetInt("CurrentRoleIndex", selectedIndex);
        SceneManager.LoadScene("1_1");
    }
}
