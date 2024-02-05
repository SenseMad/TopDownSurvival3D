using UnityEngine;

namespace TopDownSurvival3D.Weapon
{
  public abstract class WeaponBehaviour : MonoBehaviour
  {
    //public abstract int GetAmmunitionTotal();
    //public abstract int GetAmmunitionCurrent();

    public abstract int GetWeaponDamage();

    public abstract float GetRateOfFire();

    public abstract ShootingModes GetShootingMode();

    //===========================================

    public abstract void PerformShooting();

    public abstract void PerformRecharge();

    //===========================================
  }
}