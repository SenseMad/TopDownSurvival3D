﻿using UnityEngine;

public abstract class SingletonInGame<T> : MonoBehaviour where T : MonoBehaviour
{
  private static T _instance;

  //=========================================

  public static T Instance
  {
    get
    {
      if (_instance == null)
      {
        var singletonObject = new GameObject($"{typeof(T)}");
        _instance = singletonObject.AddComponent<T>();
        DontDestroyOnLoad(singletonObject);
      }

      return _instance;
    }
  }

  //=========================================

  protected void Awake()
  {
    if (_instance == null)
    {
      _instance = GetComponent<T>();
      DontDestroyOnLoad(this);
      return;
    }

    Destroy(this);
  }

  //=========================================
}