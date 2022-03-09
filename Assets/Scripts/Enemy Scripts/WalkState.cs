using UnityEngine;

public class WalkState : State
{
	// ========================================================================
	public WalkState(Enemy _enemy) : base (_enemy)
	{

	}

	// ========================================================================
	public override void Start()
	{
		//Debug.Log("WalkState");

		PlayAnimation();

        m_enemy.ResetStateTime();
	}

	// ========================================================================
	public override void Update()
	{
		if (!m_enemy.GroundAhead() || m_enemy.ObstacleAhead())
		{
			if (m_enemy.TimeSinceLastTurn > 2f)
			{
				m_enemy.Turn();
			}
			else
			{
				m_enemy.StateMachine.ChangeState(m_enemy.IdleState);
				return;
			}
		}

		if (m_enemy.PlayerFound() && m_enemy.GetPlayerDistanceY() < 1f)
		{
			m_enemy.StateMachine.ChangeState(m_enemy.HuntState);
			return;
		}

		m_enemy.Walk();

		if (m_enemy.TimeSinceStateChange >= m_enemy.StateChangeDelay)
		{
			m_enemy.StateMachine.ChangeState(m_enemy.IdleState);
			return;
		}
	}

	// ========================================================================
	public override void PlayAnimation()
	{
		m_enemy.animator.SetTrigger(m_enemy.WalkStateAnimation);
	}
}
