using TopDownSurvival3D.Weapon;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TopDownSurvival3D.Character
{
  public sealed class Character : CharacterBehaviour
  {
    [SerializeField] private Camera _cameraWorld;

    //-------------------------------------------

    public WeaponBehaviour equippedWeapon;

    private bool isRunning;

    private bool cursorLocked;

    private bool holdButtonFire;

    //===========================================

    public InputHandler InputHandler { get; private set; }

    //-------------------------------------------

    public override Camera GetCameraWorld() => _cameraWorld;

    public override bool IsRunning() => isRunning;

    public override bool IsCursorLocked() => cursorLocked;

    public override Vector2 GetInputMovement() => InputHandler.GetInputMovement();
    public override Vector3 GetMousePosition() => Mouse.current.position.ReadValue();

    //===========================================

    protected override void Awake()
    {
      base.Awake();

      UpdateCursorState();

      InputHandler = InputHandler.Instance;
    }

    protected override void Update()
    {
      if (holdButtonFire)
        equippedWeapon.PerformShooting();
    }

    private void OnEnable()
    {
      InputHandler.IA_Player.Player.Run.started += OnTryRun;
      InputHandler.IA_Player.Player.Run.canceled += OnTryRun;

      InputHandler.IA_Player.Player.Fire.started += OnTryFire;
      InputHandler.IA_Player.Player.Fire.performed += OnTryFire;
      InputHandler.IA_Player.Player.Fire.canceled += OnTryFire;
    }

    private void OnDisable()
    {
      InputHandler.IA_Player.Player.Run.started -= OnTryRun;
      InputHandler.IA_Player.Player.Run.canceled -= OnTryRun;

      InputHandler.IA_Player.Player.Fire.started -= OnTryFire;
      InputHandler.IA_Player.Player.Fire.performed -= OnTryFire;
      InputHandler.IA_Player.Player.Fire.canceled -= OnTryFire;
    }

    //===========================================

    private void UpdateCursorState()
    {
      cursorLocked = false;

      Cursor.visible = !cursorLocked;
      Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
    }

    //===========================================

    private void OnTryRun(InputAction.CallbackContext context)
    {
      /*if (cursorLocked)
        return;*/

      switch (context.phase)
      {
        case InputActionPhase.Started:
          isRunning = true;
          break;
        case InputActionPhase.Canceled:
          isRunning = false;
          break;
      }
    }

    private void OnTryFire(InputAction.CallbackContext context)
    {
      /*if (!cursorLocked)
        return;*/

      if (equippedWeapon == null)
        return;

      switch (context.phase)
      {
        case InputActionPhase.Started:
          holdButtonFire = true;
          break;
        case InputActionPhase.Performed:
          if (equippedWeapon.GetShootingMode() != ShootingModes.Single)
            break;

          holdButtonFire = false;
          equippedWeapon.PerformShooting();
          break;
        case InputActionPhase.Canceled:
          holdButtonFire = false;
          break;
      }
    }

    //===========================================
  }
}