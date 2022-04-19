using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 在创建角色的界面鼠标拖动查看角色不同角度
/// </summary>
public class RotateMode : MonoBehaviour
{
    public Transform modelTransform;
    private bool isRotate = false;
    private Vector3 startPoint;
    private Vector3 startAngel;
    [Range(0, 1)]
    public float rotateScale = 0.5f;

    void Start()
    {
        modelTransform = transform.Find("ModeContainer").GetComponent<Transform>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isRotate)
        {
            isRotate = true;
            startPoint = Input.mousePosition;
            startAngel = modelTransform.eulerAngles;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isRotate = false;
        }
        if (isRotate)
        {
            var currentPoint = Input.mousePosition;
            var x = startPoint.x - currentPoint.x;
            modelTransform.eulerAngles = startAngel + new Vector3(0, x * rotateScale, 0);
        }
    }
}
