using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LevelSystem))]
[RequireComponent(typeof(PlayerAttribute))]

public class AbilitySystem : MonoBehaviour
{

    // Abilities
    private Ability Katana = new Ability();
	private Ability Shuriken = new Ability() { RequiredLevel = 5 };

	private Ability DancingBlades = new Ability() { RequiredLevel = 10, HasCooldown = true, Cooldown = 60};

	private Ability[] Abilities;

	private PlayerController playerController = null;

	private PlayerAttribute attributes = null;
	private LevelSystem levelSystem = null;

	[SerializeField] private Transform shurikenSpawn = null;
	[SerializeField] private GameObject ShurikenPrefab = null;

	public Transform Pivot = null;
	[SerializeField] private Transform attackPoint = null;
	[SerializeField] private float attackRange = 0.5f;
	[SerializeField] private LayerMask enemyLayer;

	//Animation Player
	private Animator animator = null;

	//Sound Katana,DaningBlade und Shurikan
	[SerializeField] AudioSource PlayerSource;
	[SerializeField] AudioClip katanaClip;
	[SerializeField] AudioClip ShurikanClip;
	[SerializeField] AudioClip DancingBladeClip;
	[SerializeField] Image[] cdSlider;
	[SerializeField] GameObject[] cdButtons;
	public ParticleSystem dancingBladesDust;
	


	private void Start()
	{
		playerController = GetComponent<PlayerController>();
		attributes = GetComponent<PlayerAttribute>();
		levelSystem = GetComponent<LevelSystem>();
		animator = GetComponent < Animator>();

		Abilities = new Ability[]
		{
			Katana,
			Shuriken,
			DancingBlades,
		};

	}

	void Update()
    {
		for (int i = 0; i < Abilities.Length; i++)
		{
            
			Abilities[i].Update();
			cdSlider[i].fillAmount = Abilities[i].CurrentCooldown / Abilities[i].CooldownStartedAt;
		}

		if (Input.GetMouseButtonDown(0))
		{
			if (levelSystem.PlayerLevel < Katana.RequiredLevel) return;
			if (Katana.IsOnCooldown) return;

			Katana.StartCooldown(attributes.Haste);
			StartGlobalCooldown();

			CastKatana();
			PlayerSource.PlayOneShot(katanaClip);
		}

		if (Input.GetMouseButtonDown(1))
		{
			if (levelSystem.PlayerLevel < Shuriken.RequiredLevel) return;
			if (Shuriken.IsOnCooldown) return;

            Shuriken.StartCooldown(attributes.Haste);
			StartGlobalCooldown();

			CastShuriken();
			PlayerSource.PlayOneShot(ShurikanClip);
		}

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			if (levelSystem.PlayerLevel < DancingBlades.RequiredLevel) return;
			if (DancingBlades.IsOnCooldown) return;

			DancingBlades.StartCooldown(attributes.Haste);
			StartGlobalCooldown();

			StartCoroutine(CastDancingBlades());
			PlayerSource.PlayOneShot(DancingBladeClip);
		}
    }

	private void OnEnable()
	{
		LevelSystem.OnLevelUp += UpdateAbilityUI;
	}

	private void OnDisable()
	{
		LevelSystem.OnLevelUp -= UpdateAbilityUI;
	}

	private void CastKatana()
	{
		animator.SetTrigger("IsAttacking");

		KatanaHit();
	}

	private IEnumerator CastDancingBlades()
	{
		animator.speed = 1.5f;
		float timeBetweenHits = Ability.GetHastedCooldown(CONSTANTS.BASE_GLOBAL_COOLDOWN, attributes.Haste) / 2;
		for (float i = 0f; i < 2f; i += timeBetweenHits)
		{
			if ((i / timeBetweenHits) % 2 == 0)
			{
				animator.SetTrigger("IsAttacking1");
				CreateDancingDust();
			}
			else
			{
				animator.SetTrigger("IsAttacking");
			}

			KatanaHit();
			yield return new WaitForSeconds(timeBetweenHits);
		}
		animator.speed = 1f;
	}

	private void KatanaHit()
	{
		if (playerController.flip.flipX)
		{
			Pivot.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
		}
		else
		{
			Pivot.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
		}

		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

		foreach (Collider2D enemy in hitEnemies)
		{
			int damage = KatanaDamage();
			Enemy e = enemy.GetComponent<Enemy>();
			e.Hit(damage);
			if (!e.IsAlive)
			{
				levelSystem.PlayerLV(e.Xp);
			}
		}
	}

	private void CastShuriken()
	{
		Vector2 aimDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(shurikenSpawn.position.x, shurikenSpawn.position.y);
		GameObject shuriken = Instantiate(ShurikenPrefab);
		shuriken.transform.position = shurikenSpawn.position;
		shuriken.GetComponent<Rigidbody2D>().AddForce(aimDirection.normalized * 20, ForceMode2D.Impulse);

		Shuriken s = shuriken.GetComponent<Shuriken>();
		s.Damage = ShurikenDamage();
		s.LevelSystem = levelSystem;
	}

	private void StartGlobalCooldown()
	{
		foreach (var ability in Abilities)
		{

			ability.StartGlobalCooldown(attributes.Haste);
		}
	}

	private void UpdateAbilityUI(int _newLevel)
	{
		for (int i = 0; i < Abilities.Length; i++)
		{
			cdButtons[i].SetActive(_newLevel >= Abilities[i].RequiredLevel);
		}
	}

	private int KatanaDamage()
	{
		return (int)(CONSTANTS.BASE_KATANA_DAMAGE * (1 + attributes.AttackPoints / 100f));
	}

	private int ShurikenDamage()
	{
		return (int)(CONSTANTS.BASE_SHURIKEN_DAMAGE * (1 + attributes.AttackPoints / 100f));
	}

	private void OnDrawGizmosSelected()
	{
		if (attackPoint == null) return;

		Gizmos.DrawWireSphere(attackPoint.position, attackRange);
	}

	void CreateDancingDust() 
	{
		dancingBladesDust.Play();
	}

}
