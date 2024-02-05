public abstract class EnemyBaseState
{
  public abstract void EnterState(EnemyStateMachine parState);

  public abstract void UpdateState(EnemyStateMachine parState);

  /*public abstract void ExitState(EnemyStateMachine parState);

  public abstract void CheckSwitchStates(EnemyStateMachine parState);

  public abstract void InitializeSubState(EnemyStateMachine parState);*/
}