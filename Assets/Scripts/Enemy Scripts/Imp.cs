using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imp : Enemy
{
	[SerializeField] protected GameObject m_projectilePrefab = null;
	[SerializeField] protected Transform m_projectileSpawnPoint = null;

	private void Start()
	{
		m_attackRange = 6f;

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
			FireRangeProjectile();
			audioSource.PlayOneShot(clipAttack);
		}
	}

	// ========================================================================
	public void FireRangeProjectile()
	{
		StartCoroutine(FireRangeProjectileCo());
	}

	// ========================================================================
	protected IEnumerator FireRangeProjectileCo()
	{
		yield return new WaitForSeconds(0.25f);
		if (m_player == null) yield break;

		Vector2 direction = m_playerAbilitySystem.Pivot.transform.position - transform.position;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		m_projectileSpawnPoint.rotation = Quaternion.Euler(0, 0, angle);

		GameObject projectile = Instantiate(m_projectilePrefab);
		projectile.transform.position = m_projectileSpawnPoint.position;
		projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
		projectile.GetComponent<Rigidbody2D>().velocity = m_projectileSpawnPoint.right * 20f;

		projectile.GetComponent<EnemyProjectile>().Damage = m_damage;
	}

	protected override void Die()
	{
		base.Die();

		Destroy(this.gameObject, 1.8f);
	}
}
