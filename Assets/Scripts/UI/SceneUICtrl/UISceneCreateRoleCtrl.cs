using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///场景UI管理
///</summary>
public class UISceneCreateRoleCtrl : MonoBehaviour
{
    GameObject CreateRolePanel;
    void Start()
    {
        Object tmpObj = Resources.Load("CreateRole/UI/RoleInfoPanel");
        CreateRolePanel = GameObject.Instantiate(tmpObj) as GameObject;
        CreateRolePanel.name = CreateRolePanel.name.Replace("(Clone)", "");
        CreateRolePanel.transform.SetParent(UIManager.Instance.GetMainCanvas(), false);
        CreateRolePanel.AddComponent<UIRoleCreateCtrl>();
    }

}
