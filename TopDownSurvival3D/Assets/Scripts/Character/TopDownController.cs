using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GridBrushBase;

namespace TopDownSurvival3D.Character
{
  public class TopDownController : MonoBehaviour
  {
    [Header("УСКОРЕНИЕ")]
    [SerializeField, Tooltip("Как быстро увеличивается скорость персонажа")]
    private float _acceleration = 9.0f;
    [SerializeField, Tooltip("Как быстро уменьшается скорость персонажа")]
    private float _deceleration = 11.0f;

    [Header("СКОРОСТИ")]
    [SerializeField] private float _speedWalking = 4.0f;
    [SerializeField] private float _speedRunnning = 7.0f;
    [SerializeField] private float _speedRotation = 7.0f;

    [Header("АНИМАЦИИ ПЕРСОНАЖА")]
    [SerializeField] private Animator _characterAnimator;

    //-------------------------------------------

    private CharacterController characterController;

    private CharacterBehaviour characterBehaviour;

    private Vector3 velocity;
    private Vector3 lastMovement;
    private Vector3 rotationDirection;

    private Quaternion tempRotation;
    private Quaternion newMovementQuaternion;

    private float animationBlend;

    //===========================================

    private void Awake()
    {
      characterController = GetComponent<CharacterController>();

      characterBehaviour = GetComponent<CharacterBehaviour>();

      _characterAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
      Move();

      RotateFromMouse();
    }

    //===========================================

    private void Move()
    {
      // Получить ввод движения
      Vector2 frameInput = Vector3.ClampMagnitude(characterBehaviour.GetInputMovement(), 1.0f);
      // Рассчитать направление в локальном пространстве, используя входные данные игрока
      var desiredDirection = new Vector3(frameInput.x, 0.0f, frameInput.y);
      // Расчет скорости
      desiredDirection *= characterBehaviour.IsRunning() ? _speedRunnning : _speedWalking;

      if (desiredDirection != Vector3.zero)
        animationBlend = Mathf.Lerp(animationBlend, characterBehaviour.IsRunning() ? _speedRunnning : _speedWalking, Time.deltaTime * _acceleration);
      else
        animationBlend = Mathf.Lerp(animationBlend, 0f, Time.deltaTime * _deceleration);

      _characterAnimator.SetFloat(Animator.StringToHash("Speed"), animationBlend);

      if (!characterController.isGrounded)
      {
        velocity.y -= 35 * Time.deltaTime;
      }

      /*if (desiredDirection.normalized.magnitude >= 0.01f)
      {
        lastMovement = desiredDirection.normalized;
      }
      if (lastMovement != Vector3.zero)
      {
        tempRotation = Quaternion.LookRotation(lastMovement);

        newMovementQuaternion = Quaternion.Slerp(transform.rotation, tempRotation, Time.deltaTime * _speedRotation);
      }*/

      velocity = Vector3.Lerp(velocity, new Vector3(desiredDirection.x, velocity.y, desiredDirection.z), Time.deltaTime * (desiredDirection.sqrMagnitude > 0.0f ? _acceleration : _deceleration));

      Vector3 applied = velocity * Time.deltaTime;
      characterController.Move(applied);
    }

    private void RotateFromMouseVector()
    {
      var mainCamera = characterBehaviour.GetCameraWorld();
      var mousePosition = characterBehaviour.GetMousePosition();

      Vector3 worldMousePosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.transform.position.y - transform.position.y));

      Vector3 direction = worldMousePosition - transform.position;
      direction.y = 0f;

      if (direction != Vector3.zero)
      {
        transform.rotation = Quaternion.LookRotation(direction);
      }
    }

    //===========================================

    private void RotateFromMouse()
    {
      Ray ray = characterBehaviour.GetCameraWorld().ScreenPointToRay(characterBehaviour.GetMousePosition());

      if (Physics.Raycast(ray, out RaycastHit hit))
      {
        hit.point -= ray.direction * (hit.point.y - transform.position.y) / ray.direction.y;

        Vector3 forward = hit.point - transform.position;
        forward.y = 0f;

        transform.rotation = Quaternion.LookRotation(forward);
      }
    }

    //===========================================
  }
}