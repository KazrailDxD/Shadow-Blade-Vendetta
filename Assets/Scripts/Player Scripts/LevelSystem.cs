using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class LevelSystem :MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI levelText;
   [SerializeField] private TextMeshProUGUI expText;
   [SerializeField] private GameObject shurikenText;
   [SerializeField] private GameObject dancingBladesText;

    private PlayerAttribute playerAttribute;
	public ParticleSystem lvlUpDust;

	public static Action<int> OnLevelUp = (a) => { };

	public int PlayerLevel { get; set; } = 1;
	public int MaxExp { get; set; } = 100;

	private int m_currentExp = 0;
	public int CurrentExp
	{
		get { return m_currentExp; }
		set
		{

			m_currentExp = value;
		}
	}
    private int attackAdd = 15;
    private int hasteAdd = 8;
    private int defensiveAdd = 2;

    //für dass nächste level um die Maxerfahrung zu erhöhen
    private float ExpMutiplikator = 1.4f;

	//Sound
	[SerializeField] AudioSource audioSource;
	[SerializeField] AudioClip levelUpClip;

	//exp Bar
	[SerializeField] Image expFillBar;

	private string levelFormat = null;
	private string expFormat = null;

	private void Start()
	{
		playerAttribute = GetComponent<PlayerAttribute>();

		levelFormat = levelText.text;
		expFormat = expText.text;

		UpdateLevelUI();
	}

	private void OnEnable()
	{
		SaveSystem.SaveGameLoaded += UpdateLevelUI;
		OnLevelUp += NewAbilityAlertUI;
	}

	private void OnDisable()
	{
		SaveSystem.SaveGameLoaded -= UpdateLevelUI;
		OnLevelUp -= NewAbilityAlertUI;
	}

	//Das Lv des Spieler erhöht sich durch die Erfahrung des Getötetem Monster
	public void PlayerLV(int _monsterExpDropp) 
    {
		CurrentExp += _monsterExpDropp;
		if (CurrentExp >= MaxExp)
		{
			PlayerLevel += 1;
			LvlUpDust();
			audioSource.PlayOneShot(levelUpClip);
			CurrentExp -= MaxExp;
			MaxExperience();
			playerAttribute.AttackPoints += attackAdd;
			playerAttribute.DefensivePoints += defensiveAdd;
			playerAttribute.Haste += hasteAdd;

			OnLevelUp.Invoke(PlayerLevel);
		}
		UpdateLevelUI();
	}

	private void UpdateLevelUI()
	{
		expText.text = string.Format(expFormat, CurrentExp, MaxExp);
		levelText.text = string.Format(levelFormat, PlayerLevel);
		expFillBar.fillAmount = (float)CurrentExp / (float)MaxExp;
	}

	private void NewAbilityAlertUI(int _newLevel)
	{
		if (_newLevel == 5)
		{
			StartCoroutine(ShowShurikenText());
		}
		if (_newLevel == 10)
		{
			StartCoroutine(ShowDancingBladesText());
		}
	}

	private IEnumerator ShowShurikenText()
	{
		shurikenText.SetActive(true);
		yield return new WaitForSeconds(5f);
		shurikenText.SetActive(false);
	}

	private IEnumerator ShowDancingBladesText()
	{
		dancingBladesText.SetActive(true);
		yield return new WaitForSeconds(5f);
		dancingBladesText.SetActive(false);
	}


	//Die Benötigte Max erfahrung neu berechnet und ausgegeben sobald der spieler um ein lv steigt 
	private void MaxExperience() 
    {
        MaxExp = Mathf.RoundToInt(MaxExp * ExpMutiplikator);           
      
    }

	public void LvlUpDust() 
	{
		lvlUpDust.Play();
	}

	
  
    
} 

