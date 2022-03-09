using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	protected int m_health = CONSTANTS.BASE_ENEMY_HEALTH;

	public delegate void EnemyKilled();
	public static event EnemyKilled OnEnemyKilled = () => { };

	[NonSerialized] public Animator animator = null;

	public ParticleSystem hitDust;
	
	public int Health
	{
		get { return m_health; }
		set 
        {
			if (!m_isAlive) return;
            if (value <= 0)
			{
				value = 0;
				Die();
			}
			else
			{
				StartCoroutine(PlayHurtAnimation());
				audioSource.PlayOneShot(clipHurt);
			}
			m_health = value;
        }
	}

	public int Xp { get; private set; } = CONSTANTS.ENEMY_XP;

	protected bool m_isAlive = true;
	public bool IsAlive
	{
		get { return m_isAlive; }
		private set { m_isAlive = value; }
	}

	protected float m_timeSinceLastTurn = 0f;
	public float TimeSinceLastTurn
	{
		get { return m_timeSinceLastTurn; }
		private set { m_timeSinceLastTurn = value; }
	}

	protected float m_detectionRange;
	protected float DetectionRange
	{
		get { return m_detectionRange; }
		set 
		{
			if (value < m_baseDetectionRange)
			{
				m_detectionRange = m_baseDetectionRange;
			}
			else
			{
				m_detectionRange = value;
			}
		}
	}


	protected int m_damage = CONSTANTS.BASE_ENEMY_DAMAGE;

	protected bool m_facingRight = true;

	protected float m_walkSpeed = 1f;
	protected float m_runSpeed = 2.5f;
	protected float m_attackRange = 0.8f;
	protected float m_sightRange = 10;
	protected float m_baseDetectionRange = 2.5f;
	protected float m_sightAngle = 90f;

	protected float m_groundCheckDistance = 0.75f;

	[SerializeField] protected GameObject[] m_checkers = null;
	[SerializeField] protected LayerMask m_groundCheckLayerMask = new LayerMask();
	[SerializeField] protected LayerMask m_playerRayLayerMask = new LayerMask();

	protected PlayerController m_player = null;
	protected PlayerAttribute m_playerAttribute = null;
	protected AbilitySystem m_playerAbilitySystem = null;
	protected Rigidbody2D m_playerRigidBody = null;

	// States
	[NonSerialized] public StateMachine StateMachine;
	[NonSerialized] public State IdleState;
	[NonSerialized] public State WalkState;
	[NonSerialized] public State HuntState;
	[NonSerialized] public State AttackState;
	[NonSerialized] public State DeathState;

	[NonSerialized] public string IdleStateAnimation = "Idle";
	[NonSerialized] public string WalkStateAnimation = "Walk";
	[NonSerialized] public string HuntStateAnimation = "Run";
	[NonSerialized] public string AttackStateAnimation = "Attack";
	[NonSerialized] public string HurtAnimation = "Hurt";
	[NonSerialized] public string DeathStateAnimation = "Death";

	public float StateChangeDelay { get; set; } = 4f;

	protected float m_timeSinceStateChange = 0f;
	public float TimeSinceStateChange
	{
		get { return m_timeSinceStateChange; }
		private set { m_timeSinceStateChange = value; }
	}

	protected const float m_attackTime = 1.2f;
	protected float m_attackTimer = 0f;

	protected bool m_ignorePlayer = false; // ignore player after losing sight for a short moment

	//Enemy sounds 
	[SerializeField] protected AudioSource audioSource;

	[SerializeField] protected AudioClip clipHurt;
	[SerializeField] protected AudioClip clipDeath;
	[SerializeField] protected AudioClip clipAttack;

	private BoxCollider2D m_collider = null;
	private Rigidbody2D m_rigidBody = null;
	

	// ========================================================================
	private void Awake()
	{
		m_player = FindObjectOfType<PlayerController>();
		if (m_player != null)
		{
			m_playerAttribute = m_player.GetComponent<PlayerAttribute>();
			m_playerAbilitySystem = m_player.GetComponent<AbilitySystem>();
			m_playerRigidBody = m_player.GetComponent<Rigidbody2D>();
		}
	}

	// ========================================================================
	protected virtual void Initialize()
	{
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();

		m_collider = GetComponent<BoxCollider2D>();
		m_rigidBody = GetComponent<Rigidbody2D>();

		StateMachine = new StateMachine();
		IdleState = new IdleState(this);
		WalkState = new WalkState(this);
		HuntState = new HuntState(this);
		AttackState = new AttackState(this);
		DeathState = new DeathState(this);
		
		StateMachine.Initialize(IdleState);

		DetectionRange = m_baseDetectionRange;
	}

	// ========================================================================
	private void Update()
	{
		m_timeSinceStateChange += Time.deltaTime;
		m_timeSinceLastTurn += Time.deltaTime;
		m_attackTimer -= Time.deltaTime;

		if (DetectionRange > m_baseDetectionRange) DetectionRange -= Time.deltaTime;

		StateMachine.Update();
	}

	// ========================================================================
	public virtual void Hit(int hit)
    {
		CreateHitDust();
		Health -= hit;
    }

	// ========================================================================
	protected virtual void Die()
	{
		StateMachine.ChangeState(DeathState);
		m_isAlive = false;

		m_collider.enabled = false;
		m_rigidBody.gravityScale = 0f;

		audioSource.PlayOneShot(clipDeath);

		OnEnemyKilled.Invoke();
	}

	// ========================================================================
	public virtual void Walk()
	{
		transform.Translate(Vector2.right * m_walkSpeed * Time.deltaTime);
	}

	// ========================================================================
	public virtual void Run()
	{
		transform.Translate(Vector2.right * m_runSpeed * Time.deltaTime);
	}

	// ========================================================================
	public bool PlayerInAttackRange()
	{
		return GetPlayerDistanceX() <= m_attackRange;
	}

	// ========================================================================
	public bool GroundAhead()
	{
		Vector2 direction = m_facingRight ? new Vector2(1f, -3f) : new Vector2(-1f, -3f);

		direction = direction.normalized;
		Debug.DrawRay(m_checkers[0].transform.position, direction * m_groundCheckDistance, Color.green);
		RaycastHit2D hitinfo = Physics2D.Raycast(m_checkers[0].transform.position, direction, m_groundCheckDistance, m_groundCheckLayerMask);
		if (hitinfo)
		{
			return true;
		}
		return false;
	}

	// ========================================================================
	public bool ObstacleAhead()
	{
		Vector2 direction = m_facingRight ? Vector2.right : Vector2.left;

		for (int i = 0; i < m_checkers.Length; i++)
		{
			Debug.DrawRay(m_checkers[i].transform.position, direction * m_groundCheckDistance, Color.green);
			RaycastHit2D hitinfo = Physics2D.Raycast(m_checkers[i].transform.position, direction, m_groundCheckDistance, m_groundCheckLayerMask);
			if (hitinfo)
			{
				return true;
			}
		}
		return false;
	}

	// ========================================================================
	public virtual bool PlayerFound()
	{
		if (m_ignorePlayer) return false;
		if (m_player == null) return false;
		Vector2 playerDirection = m_playerAbilitySystem.Pivot.transform.position - m_checkers[1].transform.position;
		RaycastHit2D hitinfo = Physics2D.Raycast(m_checkers[1].transform.position, playerDirection.normalized, m_sightRange, m_playerRayLayerMask);
		Debug.DrawRay(m_checkers[1].transform.position, playerDirection.normalized * m_sightRange, Color.yellow);
		if (hitinfo && hitinfo.collider.CompareTag("Player"))
		{
			float angle = Vector2.Angle(playerDirection, transform.TransformVector(1f, 0f, 0f));

			if (angle < m_sightAngle * 0.5 || hitinfo.distance < DetectionRange)
			{
				return true;
			}
		}
		return false;
	}

	// ========================================================================
	public bool PlayerInLine()
	{
		if (m_player == null) return false;
		Vector2 playerDirection = m_player.transform.position - transform.position;
		RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, playerDirection.normalized, Mathf.Infinity, m_playerRayLayerMask);
		return hitinfo && hitinfo.collider.CompareTag("Player");
	}

	// ========================================================================
	public void TurnLeft()
	{
		m_timeSinceLastTurn = 0f;
		transform.eulerAngles = new Vector3(0, -180, 0);
		m_facingRight = false;
	}

	// ========================================================================
	public void TurnRight()
	{
		m_timeSinceLastTurn = 0f;
		transform.eulerAngles = new Vector3(0, 0, 0);
		m_facingRight = true;
	}

	// ========================================================================
	public void Turn()
	{
		if (m_facingRight)
		{
			TurnLeft();
		}
		else
		{
			TurnRight();
		}
	}

	// ========================================================================
	public void TurnRandomly()
	{
		int randomPercent = UnityEngine.Random.Range(1, 100);
		if (randomPercent <= 50)
		{
			TurnRight();
		}
		else
		{
			TurnLeft();
		}
	}

	// ========================================================================
	public virtual void AttackPlayer() { }


	// ========================================================================
	public void IncreaseDetectionRange()
	{
		DetectionRange = m_sightRange;
	}

	// ========================================================================
	public void FacePlayer()
	{
		if (m_player == null) return;
		if (m_facingRight && m_player.transform.position.x < transform.position.x)
		{
			TurnLeft();
		}
		else if (!m_facingRight && m_player.transform.position.x > transform.position.x)
		{
			TurnRight();
		}
	}

	// ========================================================================
	public void ResetStateTime()
	{
		m_timeSinceStateChange = 0;
		StateChangeDelay = RandomStateChangeDelay();
	}

	// ========================================================================
	public float GetPlayerDistanceY()
	{
		if (m_player == null) return 0f;
		return Mathf.Abs(m_player.transform.position.y - transform.position.y);
	}

	// ========================================================================
	public float GetPlayerDistanceX()
	{
		if (m_player == null) return 0f;
		return Mathf.Abs(m_player.transform.position.x - transform.position.x);
	}

	// ========================================================================
	private float RandomStateChangeDelay()
	{
		return UnityEngine.Random.Range(3, 15);
	}

	// ========================================================================
	public virtual void IgnorePlayer()
	{

	}

	// ========================================================================
	private IEnumerator PlayHurtAnimation()
	{
		animator.SetTrigger(HurtAnimation);
		yield return new WaitForSeconds(0.5f);
		StateMachine.CurrentState.PlayAnimation();
	}

	public void CreateHitDust() 
	{
		hitDust.Play();
	}
}
