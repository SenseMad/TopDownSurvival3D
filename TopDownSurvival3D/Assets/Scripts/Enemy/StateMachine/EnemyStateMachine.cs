using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
  private EnemyBaseState currentState;

  //private Dictionary<, EnemyBaseState>

  public EnemyIdleState idleState = new EnemyIdleState();
  public EnemyFollowState followState = new EnemyFollowState();
  public EnemyAttackState attackState = new EnemyAttackState();
  public EnemyDiedState diedState = new EnemyDiedState();

  //===========================================

  private void Start()
  {
    currentState = idleState;

    currentState.EnterState(this);
  }

  private void Update()
  {
    currentState.UpdateState(this);
  }

  //===========================================

  public void SwitchState(EnemyBaseState parState)
  {
    currentState = parState;
    parState.EnterState(this);
  }

  //===========================================



  //===========================================
}