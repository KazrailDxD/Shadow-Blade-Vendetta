using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
	[SerializeField] protected GameObject m_projectilePrefab = null;
	[SerializeField] protected Transform m_projectileSpawnPoint = null;
	[SerializeField] protected Transform m_bossProjectileSpawnPivot = null;
	[SerializeField] protected GameObject m_bossProjectilePrefab = null;
	[SerializeField] protected Transform[] m_bossProjectileSpawns = null;

	[SerializeField] AudioClip bossClipfirebolt;
	[SerializeField] AudioClip bossClipLightning;

	// States
	[System.NonSerialized] public State BossAbilityLightningBoltState;
	[System.NonSerialized] public State BossAbilityMagicFireState;

	[System.NonSerialized] public string MagicFireStateAnimation = "MagicFire";
	[System.NonSerialized] public string LightningBoltStateAnimation = "LightningBolt";
	public ParticleSystem bossDead;

	public static Action OnBossDied = () => { };

	Coroutine m_cycleBossAbilities = null;

	private bool m_engaged;

	public bool Engaged
	{
		get { return m_engaged; }
		set 
		{
			if (value == m_engaged) return;
			if(m_isAlive)
			{
				m_engaged = value;
			}
			else
			{
				m_engaged = false;
			}

			BossEngaged.Invoke(m_engaged);
		}
	}

	public static Action<bool> BossEngaged = (a) => { };

	// ========================================================================
	private void Start()
	{
		base.Initialize();

		m_runSpeed = 4.5f;

		BossAbilityLightningBoltState = new BossAbilityLightningBoltState(this);
		BossAbilityMagicFireState = new BossAbilityMagicFireState(this);

		m_health = CONSTANTS.BOSS_HEALTH;
		m_damage = CONSTANTS.BOSS_DAMAGE;
	}

	// ========================================================================
	private void OnEnable()
	{
		BossEngaged += Engagement;
		PlayerAttribute.OnPlayerDied += EncounterEnd;
	}

	// ========================================================================
	private void OnDisable()
	{
		BossEngaged -= Engagement;
		PlayerAttribute.OnPlayerDied -= EncounterEnd;
	}

	// ========================================================================
	private void EncounterEnd()
	{
		Engaged = false;
	}

	// ========================================================================
	private void Engagement(bool _engaged)
	{
		if (_engaged)
		{
			m_sightRange = Mathf.Infinity;
			m_detectionRange = Mathf.Infinity;
			StateMachine.ChangeState(HuntState);
			m_cycleBossAbilities = StartCoroutine(CycleBossAbilities());
		}
		else
		{
			StopCoroutine(m_cycleBossAbilities);
		}
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

			// random chance to knock player away on hit
			int randomPercent = UnityEngine.Random.Range(0, 99);
			if (randomPercent < 15)
			{
				float deltaX = m_player.transform.position.x - transform.position.x;
				Vector2 throwDirection = deltaX < 0f ? new Vector2(-2f, 1f) : new Vector2(2f, 1f);
				float throwStrength = 8f;
				m_playerRigidBody.AddForce(throwDirection.normalized * throwStrength, ForceMode2D.Impulse);
			}

			audioSource.PlayOneShot(clipAttack);
		}
	}

	// ========================================================================
	protected override void Die()
	{
		base.Die();
		CreateDust();

		OnBossDied.Invoke();
		Engaged = false;
	}

	// ========================================================================
	public override void Hit(int _hit)
	{
		base.Hit(_hit);
		if (!Engaged) Engaged = true;
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

	// ========================================================================
	public void CastMagicFire()
	{
		animator.SetTrigger(MagicFireStateAnimation);

		StartCoroutine(CastMagicFireCo());
	}

	// ========================================================================
	private IEnumerator CastMagicFireCo()
	{
		FireRangeProjectile();
		audioSource.PlayOneShot(bossClipfirebolt);
		yield return new WaitForSeconds(0.25f);

		StateMachine.ChangeState(HuntState);
	}

	// ========================================================================
	public void MoveBossProjectileSpawns(bool _reset = false)
	{
		if (_reset)
		{
			m_bossProjectileSpawnPivot.transform.localPosition = Vector2.zero;
		}
		else
		{
			m_bossProjectileSpawnPivot.transform.Translate(Vector2.right * 0.25f);
		}

	}

	// ========================================================================
	private IEnumerator CycleBossAbilities()
	{
		while (true)
		{
			yield return new WaitForSeconds(5f);

			StateMachine.ChangeState(BossAbilityLightningBoltState);
			
			yield return new WaitForSeconds(12f);

			StateMachine.ChangeState(BossAbilityMagicFireState);

			yield return new WaitForSeconds(5f);
		}
	}

	// ========================================================================
	public void SpawnLightningBolts()
	{
		audioSource.PlayOneShot(bossClipLightning);
		foreach (var spawn in m_bossProjectileSpawns)
		{
			var projectile = Instantiate(m_bossProjectilePrefab, spawn.position, Quaternion.identity);
			projectile.GetComponent<EnemyProjectile>().Damage = CONSTANTS.BASE_ENEMY_DAMAGE;
		}
	}

	// ========================================================================
	public override void Run()
	{
		base.Run();
		Engaged = true;
	}

	private void CreateDust() 
	{
		bossDead.Play();
	}

}
