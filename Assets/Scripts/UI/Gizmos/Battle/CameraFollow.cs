using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///简单地相机跟随
///</summary>
public class CameraFollow : MonoBehaviour
{
    void Update()
    {
        if (FightSceneCtrl.Instance != null && FightSceneCtrl.Instance.currentPlayer != null)
        {
            Transform t = FightSceneCtrl.Instance.currentPlayer.transform;
            transform.position = new Vector3(t.position.x, t.position.y + 100, t.position.z);
        }
    }
}
