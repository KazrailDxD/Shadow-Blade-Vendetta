using UnityEngine;

public class IdleState : State
{
	// ========================================================================
	public IdleState(Enemy _enemy) : base (_enemy)
	{

	}

	

	// ========================================================================
	public override void Start()
	{
		//Debug.Log("IdleState");
		PlayAnimation();


		m_enemy.ResetStateTime();
	}

	// ========================================================================
	public override void Update()
	{
		if (m_enemy.PlayerFound() && m_enemy.GetPlayerDistanceY() < 1f)
		{
			m_enemy.StateMachine.ChangeState(m_enemy.HuntState);
			return;
		}

		if (m_enemy.TimeSinceStateChange >= m_enemy.StateChangeDelay)
		{
			m_enemy.StateMachine.ChangeState(m_enemy.WalkState);
			return;
		}
	}

	// ========================================================================
	public override void PlayAnimation()
	{
		m_enemy.animator.SetTrigger(m_enemy.IdleStateAnimation);
	}
}
