using UnityEngine;

public class DeathState : State
{
	// ========================================================================
	public DeathState(Enemy _enemy) : base (_enemy)
	{

	}

	// ========================================================================
	public override void Start()
	{
		//Debug.Log("DeathState");

		PlayAnimation();

		m_enemy.ResetStateTime();
	}

	// ========================================================================
	public override void Update()
	{
		
	}

	// ========================================================================
	public override void PlayAnimation()
	{
		m_enemy.animator.SetTrigger(m_enemy.DeathStateAnimation);
	}
}
