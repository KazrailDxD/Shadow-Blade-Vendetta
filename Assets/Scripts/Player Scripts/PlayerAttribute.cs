using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerAttribute : MonoBehaviour
{
    private const int m_maxHealth = 20;

    //Private Variablen
    
    private int m_health = CONSTANTS.PLAYER_HEALTH;
    private int m_damage = CONSTANTS.PLAYER_DAMAGE;
    private int m_defensive = CONSTANTS.PLAYER_DEFENCE;
    private int m_haste = CONSTANTS.PLAYER_HASTE;
    private bool m_alive = true;
    Animator animitor = null;

    //Player Sound
    [SerializeField] AudioSource playerSource;
    [SerializeField] AudioClip hitClip;
    [SerializeField] Slider slider;

    PlayerController m_playerController = null;

    public GameObject attackBuffUI;
    public GameObject defenseBuffUI;
    public GameObject hasteBuffUI;

    public TextMeshProUGUI attackBuffUIText;
    public TextMeshProUGUI defenseBuffUIText;
    public TextMeshProUGUI hasteBuffUIText;

    int attackBuffAmount = 0;
    int defenseBuffAmount = 0;
    int hasteBuffAmount = 0;

    public static Action OnPlayerDied = () => { };
    public static Action OnBuffChanged = () => { };
   
    //Spieler Eigenschaften
    public int PlayerHealth
    {
        
        get { return m_health; }
        
        set 
        {
            if (!IsAlive) return;
            if (value <= 0)
			{
                m_health = 0;
                m_alive = false;
                m_playerController.enabled = false;
                OnPlayerDied.Invoke();
                animitor.SetTrigger("IsDead");
                Destroy(this.gameObject,0.7f);
                // set death animation
			}
            else if (value >= m_maxHealth)
			{
                m_health = m_maxHealth;
			}
			else
			{
                if (value<m_health)
                {
                    animitor.SetTrigger("IsHurt");
                }
                m_health = value;
            }

            slider.value = (float)m_health / (float)m_maxHealth;
        }
    }

    public int AttackPoints
    {
        get { return m_damage; }
        set { m_damage = value; }
    }

    public int DefensivePoints
    {
        get { return m_defensive; }
        set { m_defensive = value; }
    }

    public int Haste 
    {
        get { return m_haste; }
        set { m_haste = value; } 
    }

    public bool IsAlive 
    {
        get { return m_alive; }
        set { m_alive = value; }
    }

	private void Start()
	{
        m_playerController = GetComponent<PlayerController>();
        animitor = GetComponent<Animator>();
	}

	private void OnEnable()
	{
        OnBuffChanged += UpdateBuffUI;
	}

	private void OnDisable()
	{
        OnBuffChanged -= UpdateBuffUI;
    }

    private void UpdateBuffUI()
    {
        attackBuffUI.SetActive(attackBuffAmount > 0);
        defenseBuffUI.SetActive(defenseBuffAmount > 0);
        hasteBuffUI.SetActive(hasteBuffAmount > 0);

        attackBuffUIText.text = attackBuffAmount.ToString();
        defenseBuffUIText.text = defenseBuffAmount.ToString();
        hasteBuffUIText.text = hasteBuffAmount.ToString();
    }

    public void Hit(int _damage)
	{
        int damage = Mathf.RoundToInt(_damage * (1f - (m_defensive / 100f)));
        PlayerHealth -= damage;
        playerSource.PlayOneShot(hitClip);
	}

    public void RegenerateHP(int _hp)
	{
        PlayerHealth += _hp;
	}

    public void AddAttackPowerTemp(int _amount)
	{
        StartCoroutine(AddAttackPowerTempCo(_amount));
	}

    public void AddDefensePowerTemp(int _amount)
    {
        StartCoroutine(AddDefensePowerTempCo(_amount));
    }

    public void AddHasteTemp(int _amount)
    {
        StartCoroutine(AddHasteTempCo(_amount));
    }

    private IEnumerator AddAttackPowerTempCo(int _amount)
	{
        attackBuffAmount++;
        m_damage += _amount;
        OnBuffChanged.Invoke();

        yield return new WaitForSeconds(CONSTANTS.COLLECTABLE_BUFF_DURATION);

        attackBuffAmount--;
        m_damage -= _amount;
        OnBuffChanged.Invoke();
    }

    private IEnumerator AddDefensePowerTempCo(int _amount)
    {
        defenseBuffAmount++;
        m_defensive += _amount;
        OnBuffChanged.Invoke();

        yield return new WaitForSeconds(CONSTANTS.COLLECTABLE_BUFF_DURATION);

        defenseBuffAmount--;
        m_defensive -= _amount;
        OnBuffChanged.Invoke();
    }

    private IEnumerator AddHasteTempCo(int _amount)
    {
        hasteBuffAmount++;
        m_haste += _amount;
        OnBuffChanged.Invoke();

        yield return new WaitForSeconds(CONSTANTS.COLLECTABLE_BUFF_DURATION);

        hasteBuffAmount--;
        m_haste -= _amount;
        OnBuffChanged.Invoke();
    }
}
