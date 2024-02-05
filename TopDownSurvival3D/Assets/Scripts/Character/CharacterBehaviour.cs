using UnityEngine;

namespace TopDownSurvival3D.Character
{
  public abstract class CharacterBehaviour : SingletonInSceneNoInstance<CharacterBehaviour>
  {
    protected virtual new void Awake()
    {
      base.Awake();
    }

    protected virtual void Start() { }

    protected virtual void Update() { }

    protected virtual void LateUpdate() { }

    //===========================================

    public abstract Camera GetCameraWorld();

    public abstract bool IsRunning();

    public abstract bool IsCursorLocked();

    public abstract Vector2 GetInputMovement();
    public abstract Vector3 GetMousePosition();

    //===========================================



    //===========================================
  }
}