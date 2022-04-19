﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

///<summary>
///泛型单例
///</summary>
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    // 子类可以继承或重写Awake()方法
    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = (T)this;
        }
    }

    public static bool IsInstialized
    {
        get
        {
            return instance != null;
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
