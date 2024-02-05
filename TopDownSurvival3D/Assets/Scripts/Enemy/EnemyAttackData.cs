using UnityEngine;
using UnityEngine.AI;

using TopDownSurvival3D.Character;

namespace TopDownSurvival3D.Enemy
{
  [System.Serializable]
  public sealed class EnemyAttackData
  {
    [Header("¿“¿ ¿")]
    [SerializeField] private Transform _pointAttackRadius;
    [SerializeField, Min(0)] private float _attackRadius = 1.0f;
    [SerializeField, Min(0)] private float _attackDelay = 1.0f;

    //-------------------------------------------

    private float currentDelayAttack = 0f;

    //===========================================

    public EnemyAIState GetEnemyAIState(NavMeshAgent parNavMeshAgent, CharacterBehaviour parCharacterBehaviour)
    {
      NavMeshPath path = new NavMeshPath();
      parNavMeshAgent.CalculatePath(parCharacterBehaviour.transform.position, path);

      if (path.status != NavMeshPathStatus.PathPartial)
      {
        float dist = Vector3.Distance(_pointAttackRadius.position, parCharacterBehaviour.transform.position);
        if (dist <= _attackRadius + 0.2f)
          return EnemyAIState.Attack;

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
          float distance = Vector3.Distance(path.corners[i], path.corners[i + 1]);
          if (distance > _attackRadius)
          {
#if UNITY_EDITOR
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
#endif
            return EnemyAIState.Follow;
          }
        }
      }

      return EnemyAIState.Idle;
    }

    //===========================================



    //===========================================

    public void OnDrawGizmos()
    {
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireSphere(_pointAttackRadius.position, _attackRadius);
    }

    //===========================================
  }
}