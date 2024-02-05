using UnityEngine;
using UnityEngine.AI;

namespace TopDownSurvival3D.Enemy
{
  public class EnemyController : MonoBehaviour
  {


    //===========================================



    //===========================================

    public void TurningTowardsTraffic(NavMeshAgent parNavMeshAgent)
    {
      Vector3 velocity = parNavMeshAgent.velocity.normalized;

      if (velocity != Vector3.zero)
      {
        Quaternion targetRotation = Quaternion.LookRotation(velocity);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * parNavMeshAgent.angularSpeed);
      }
    }

    //===========================================



    //===========================================
  }
}