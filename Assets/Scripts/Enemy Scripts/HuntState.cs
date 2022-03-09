using UnityEngine;

public class HuntState : State
{
	// ========================================================================
	public HuntState(Enemy _enemy) : base (_enemy)
	{

	}

	// ========================================================================
	public override void Start()
	{
		//Debug.Log("HuntState");

		PlayAnimation();

		m_enemy.ResetStateTime();

		m_enemy.IncreaseDetectionRange();
	}

	// ========================================================================
	public override void Update()
	{
		if (m_enemy.PlayerInAttackRange())
		{
			m_enemy.StateMachine.ChangeState(m_enemy.AttackState);
			m_enemy.IgnorePlayer();
			return;
		}

		if (!m_enemy.PlayerFound() || !m_enemy.GroundAhead())
		{
			m_enemy.StateMachine.ChangeStateRandomly(m_enemy.WalkState, m_enemy.IdleState);
			m_enemy.IgnorePlayer();
			return;
		}

		if (m_enemy.GetPlayerDistanceY() < 1f)
		{
			m_enemy.FacePlayer();
		}
		
		m_enemy.Run();
	}

	// ========================================================================
	public override void PlayAnimation()
	{
		m_enemy.animator.SetTrigger(m_enemy.HuntStateAnimation);
	}
}
