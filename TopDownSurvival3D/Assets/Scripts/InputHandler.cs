using UnityEngine;

namespace TopDownSurvival3D.Character
{
  public class InputHandler : SingletonInGame<InputHandler>
  {
    public IA_Player IA_Player { get; private set; }

    //===========================================

    private new void Awake()
    {
      IA_Player = new IA_Player();
    }

    private void OnEnable()
    {
      IA_Player.Enable();
    }

    private void OnDisable()
    {
      IA_Player.Disable();
    }

    //===========================================

    public bool CanProcessInput()
    {
      return Cursor.lockState == CursorLockMode.Locked;
    }

    public Vector2 GetInputMovement()
    {
      return !CanProcessInput() ? IA_Player.Player.Move.ReadValue<Vector2>() : default;
    }

    //===========================================
  }
}