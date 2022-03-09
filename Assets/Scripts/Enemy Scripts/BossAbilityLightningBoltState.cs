using UnityEngine;

public class BossAbilityLightningBoltState : State
{
	private Boss m_boss;

	// ========================================================================
	public BossAbilityLightningBoltState(Boss _enemy) : base (_enemy)
	{
		m_boss = _enemy;
	}

	private float m_lightningBoltTimer = 0f;
	private int m_lightningBoltCount = 0;

	// ========================================================================
	public override void Start()
	{
		//Debug.Log("BossAbilityLightningBoltState");

		PlayAnimation();

		m_enemy.ResetStateTime();

		m_lightningBoltTimer = 0f;
		m_lightningBoltCount = 0;
	}

	// ========================================================================
	public override void Update()
	{
		m_lightningBoltTimer -= Time.deltaTime;

		if (m_lightningBoltTimer <= 0f)
		{
			m_lightningBoltTimer = 3f - 0.5f * m_lightningBoltCount;
			m_lightningBoltCount++;
			m_boss.SpawnLightningBolts();

			if (m_lightningBoltCount == 5)
			{
				m_boss.MoveBossProjectileSpawns(true);
				m_enemy.StateMachine.ChangeState(m_enemy.HuntState);
				return;
			}
			m_boss.MoveBossProjectileSpawns();
		}
	}

	// ========================================================================
	public override void PlayAnimation()
	{
		m_enemy.animator.SetTrigger(m_boss.LightningBoltStateAnimation);
	}
}
