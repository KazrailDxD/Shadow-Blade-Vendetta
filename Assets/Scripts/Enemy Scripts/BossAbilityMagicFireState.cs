using UnityEngine;

public class BossAbilityMagicFireState : State
{
	Boss m_boss;

	// ========================================================================
	public BossAbilityMagicFireState(Boss _enemy) : base (_enemy)
	{
		m_boss = _enemy;
	}

	// ========================================================================
	public override void Start()
	{
		//Debug.Log("BossAbilityMagicFireState");

		m_boss.ResetStateTime();

		if (m_boss.PlayerInLine())
		{
			m_boss.FacePlayer();

			m_boss.CastMagicFire();
		}
		else
		{
			m_boss.StateMachine.ChangeStateRandomly(m_boss.IdleState, m_boss.WalkState);
		}
	}

	// ========================================================================
	public override void Update()
	{

	}

	// ========================================================================
	public override void PlayAnimation()
	{
		// empty
	}
}
