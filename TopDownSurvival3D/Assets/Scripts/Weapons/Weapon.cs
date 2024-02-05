using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TopDownSurvival3D.Character;
using UnityEngine.InputSystem.HID;

namespace TopDownSurvival3D.Weapon
{
  public sealed class Weapon : WeaponBehaviour
  {
    public Transform pointer;

    [Header("УРОН")]
    [SerializeField, Min(0)] private int _damage = 10;

    [Header("СТРЕЛЬБА")]
    [SerializeField] private ShootingModes _shootingMode;
    [SerializeField, Min(0)] private int _shotCount = 1;
    [SerializeField, Min(0)] private float _shotsPerMinutes = 200;
    [SerializeField] private Transform[] _startPoints;

    [Header("ЛУЧ")]
    [SerializeField] private LayerMask _layerMask;
    [SerializeField, Min(0)] private float _distance = Mathf.Infinity;

    [Header("РАЗБРОС")]
    [SerializeField] private bool _useSpread = true;
    [SerializeField, Min(0)] private float _spreadFactor = 1.0f;

    [Header("ПАТРОНЫ")]
    [SerializeField, Min(0)] private int _maxAmountAmmo = 100;
    [SerializeField, Min(0)] private int _maxAmountAmmoInMagazine = 20;

    [Header("ПЕРЕЗАРЯДКА")]
    [SerializeField] private bool _autoRecharge = false;
    [SerializeField, Min(0)] private float _rechargeTime = 1.0f;

    [Header("ЭФФЕКТЫ")]
    [SerializeField] private Transform _muzzleEffectPrefab;
    [SerializeField] private Transform _hitEffectPrefab;
    [SerializeField, Min(0)] private float _hitEffectDestroyDelay = 2.0f;

    [Header("ЗВУКИ")]
    [SerializeField] private AudioClip _audioClipFire;
    [SerializeField] private AudioClip _audioClipRecharge;

    //-------------------------------------------

    private CharacterBehaviour characterBehaviour;

    private AudioSource audioSource;

    private LineRenderer lineRenderer;

    private Coroutine rechargeStarted;

    private int currentAmountAmmo;
    private int currentAmountAmmoInMagazine;

    private float lastShotTime;

    //===========================================

    public override int GetWeaponDamage() => _damage;

    public override float GetRateOfFire() => _shotsPerMinutes;

    public override ShootingModes GetShootingMode() => _shootingMode;

    //===========================================

    private void Awake()
    {
      audioSource = GetComponent<AudioSource>();

      lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    private void Start()
    {
      characterBehaviour = CharacterBehaviour.Instance;

      currentAmountAmmo = _maxAmountAmmo;
      currentAmountAmmoInMagazine = _maxAmountAmmoInMagazine;
    }
    
    private void Update()
    {
      Camera camera = characterBehaviour.GetCameraWorld();
      Vector3 mouse = characterBehaviour.GetMousePosition();

      Ray rayCamera = camera.ScreenPointToRay(mouse);

      if (Physics.Raycast(rayCamera, out RaycastHit hit, _distance, _layerMask))
      {
        Vector3 direction = hit.point - _startPoints[0].position;

        Ray rayCharacter = new Ray(_startPoints[0].position, direction);

        if (Physics.Raycast(rayCharacter, out RaycastHit hit1, _distance, _layerMask))
        {
          Debug.DrawRay(rayCharacter.origin, rayCharacter.direction * hit1.distance, Color.red);

          pointer.position = hit1.point;
        }
      }
    }

    //===========================================

    public override void PerformShooting()
    {
      if (rechargeStarted != null)
        return;

      if (currentAmountAmmoInMagazine == 0)
        return;

      if (!(Time.time - lastShotTime > 60.0f / _shotsPerMinutes))
        return;

      for (int i = 0; i < _shotCount; i++)
      {
        PerformRaycast();
      }

      lastShotTime = Time.time;

      currentAmountAmmoInMagazine--;

      if (_autoRecharge && currentAmountAmmoInMagazine == 0)
      {
        PerformRecharge();
      }
    }

    #region Перезарядка

    public override void PerformRecharge()
    {
      Recharge(_maxAmountAmmoInMagazine);
    }

    private void Recharge(int parValue)
    {
      if (rechargeStarted != null)
      {
        Debug.LogWarning($"Перезарадка оружия {name} уже запущена");
        return;
      }

      if (parValue < 0)
      {
        Debug.LogError("Значение перезарядки не может быть < 0");
        return;
      }

      if (currentAmountAmmo == 0)
      {
        Debug.LogWarning("Текущее количество патронов = 0, перезарядка невозможна");
        return;
      }

      if (currentAmountAmmoInMagazine >= _maxAmountAmmoInMagazine)
      {
        Debug.Log("Текущее количество патронов в магазине >= максимальному значению");
        return;
      }

      rechargeStarted = StartCoroutine(RechargeStarted(parValue));
    }

    private IEnumerator RechargeStarted(int parValue)
    {
      int amountAmmoBefore = currentAmountAmmo;
      int amountAmmoInMagazineBefore = currentAmountAmmoInMagazine;

      if (parValue + amountAmmoInMagazineBefore > _maxAmountAmmoInMagazine)
        parValue = _maxAmountAmmoInMagazine - amountAmmoInMagazineBefore;

      if (amountAmmoBefore - parValue <= 0)
        parValue = amountAmmoBefore;

      if (audioSource != null && _audioClipRecharge != null)
      {
        audioSource.Stop();
        audioSource.clip = _audioClipRecharge;
        audioSource.Play();
      }

      yield return new WaitForSeconds(_rechargeTime);

      currentAmountAmmo -= parValue;
      currentAmountAmmoInMagazine += parValue;

      int amountAmmoAfter = currentAmountAmmo - amountAmmoBefore;
      int amountAmmoInMagazineAfter = currentAmountAmmoInMagazine - amountAmmoInMagazineBefore;

      rechargeStarted = null;
    }

    #endregion

    //===========================================

    private void PerformRaycast()
    {
      //lineRenderer.SetPosition(0, transform.position);

      Camera camera = characterBehaviour.GetCameraWorld();
      Vector3 mouse = characterBehaviour.GetMousePosition();

      Ray rayCamera = camera.ScreenPointToRay(mouse);

      if (Physics.Raycast(rayCamera, out RaycastHit hit, _distance, _layerMask))
      {
        Vector3 direction = hit.point - _startPoints[0].position;

        Ray rayCharacter = new Ray(_startPoints[0].position, direction);

        if (Physics.Raycast(rayCharacter, out RaycastHit hit1, _distance, _layerMask))
        {
          Vector3 directionCharacter = hit1.point - _startPoints[0].position;

          Collider hitCollider = hit1.collider;

          if (hitCollider)
          {
            Debug.Log($"{hitCollider.name}");
          }

          //lineRenderer.SetPosition(1, hit1.point);
        }
      }
    }

    private Vector3 CalculateSpread()
    {
      return new Vector3
      {
        x = Random.Range(-_spreadFactor, _spreadFactor),
        y = Random.Range(-_spreadFactor, _spreadFactor),
        z = Random.Range(-_spreadFactor, _spreadFactor)
      };
    }

    //===========================================
  }
}