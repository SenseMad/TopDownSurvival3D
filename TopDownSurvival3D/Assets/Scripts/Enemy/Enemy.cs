using System.Collections;
using System.Collections.Generic;
using TopDownSurvival3D.Character;
using UnityEngine;
using UnityEngine.AI;

namespace TopDownSurvival3D.Enemy
{
  public class Enemy : EnemyController
  {
    [SerializeField] private EnemyAttackData _enemyAttackData;

    //-------------------------------------------

    private EnemyAIState enemyAIState;

    private CharacterBehaviour characterBehaviour;
    private EnemyController enemyController;

    private NavMeshAgent navMeshAgent;

    //===========================================

    private void Awake()
    {
      enemyController = GetComponent<EnemyController>();

      navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
      characterBehaviour = CharacterBehaviour.Instance;
    }

    private void Update()
    {
      //UpdateEnemyAIState();
    }

    //===========================================

    private void UpdateEnemyAIState()
    {
      if (enemyAIState == EnemyAIState.Died)
        return;

      enemyAIState = _enemyAttackData.GetEnemyAIState(navMeshAgent, characterBehaviour);

      switch (enemyAIState)
      {
        case EnemyAIState.Idle:
          break;
        case EnemyAIState.Follow:
          navMeshAgent.SetDestination(characterBehaviour.transform.position);
          enemyController.TurningTowardsTraffic(navMeshAgent);
          break;
        case EnemyAIState.Attack:
          navMeshAgent.SetDestination(transform.position);
          break;
        case EnemyAIState.Died:
          break;
      }
    }

    //===========================================



    //===========================================
  }
}