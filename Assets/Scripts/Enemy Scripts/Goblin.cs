using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemy
{

	// ========================================================================
	void Start()
	{
		base.Initialize();
	}

	// ========================================================================
	public override void AttackPlayer()
	{
		if (m_player == null) return;
		if (m_attackTimer <= 0f)
		{
			m_attackTimer = m_attackTime;
			animator.SetTrigger(AttackStateAnimation);
			m_playerAttribute.Hit(m_damage);
			audioSource.PlayOneShot(clipAttack);
		}
	}

	// ========================================================================
	protected override void Die()
	{
		base.Die();

		Destroy(this.gameObject, 2f);
	}

	// ========================================================================
	public override bool PlayerFound()
	{
		if (GetPlayerDistanceY() > 0.8f) return false;
		return base.PlayerFound();
	}

	// ========================================================================
	public override void IgnorePlayer()
	{
		StartCoroutine(IgnorePlayerCo());
	}

	// ========================================================================
	private IEnumerator IgnorePlayerCo()
	{
		m_ignorePlayer = true;
		yield return new WaitForSeconds(2f);
		m_ignorePlayer = false;
	}

	// ========================================================================
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.collider.CompareTag("Player"))
		{
			m_ignorePlayer = false;
		}
	}
}
