using UnityEngine;

public abstract class SingletonInSceneNoInstance<T> : MonoBehaviour where T : MonoBehaviour
{
  public static T Instance { get; private set; }

  //===========================================

  protected void Awake()
  {
    if (Instance != null)
    {
      Destroy(this);
    }

    Instance = GetComponent<T>();
  }

  //===========================================
}