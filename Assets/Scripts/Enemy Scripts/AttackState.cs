using UnityEngine;

public class AttackState : State
{
	// ========================================================================
	public AttackState(Enemy _enemy) : base (_enemy)
	{

	}

	// ========================================================================
	public override void Start()
	{
        //Debug.Log("AttackState");

        m_enemy.ResetStateTime();
	}

	// ========================================================================
	public override void Update()
	{

		if (!m_enemy.PlayerInAttackRange())
		{
			m_enemy.StateMachine.ChangeState(m_enemy.HuntState);
			return;
		}

		if (!m_enemy.PlayerInLine())
		{
			m_enemy.StateMachine.ChangeState(m_enemy.IdleState);
		}

		m_enemy.FacePlayer();
		m_enemy.AttackPlayer();
	}

	// ========================================================================
	public override void PlayAnimation()
	{
		// empty
	}
}
