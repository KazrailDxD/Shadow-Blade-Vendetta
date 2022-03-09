using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CoinCounterSript : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI coinText;

    [SerializeField] private GameObject bossplattform;

    [SerializeField] private GameObject boss;

    [SerializeField] private GameObject bossText;

    public int m_coinAmount = 0;
    public int CoinAmount 
    { 
        get
		{
            return m_coinAmount;
		}
        set
        {
            m_coinAmount = value;
            CoinAmountChanged.Invoke(value);
        }
    }

    public static Action<int> CoinAmountChanged = (a) => { };

	private void Start()
	{
        HandleCoinChange(CoinAmount);
	}

	private void OnEnable()
	{
        CoinAmountChanged += HandleCoinChange;
        Keys.OnCoinCollected += AddCoin;
	}

    private void OnDisable()
    {
        CoinAmountChanged -= HandleCoinChange;
        Keys.OnCoinCollected -= AddCoin;
    }

	private void AddCoin()
	{
        CoinAmount += 1;
	}

	private void HandleCoinChange(int _amount)
	{
        coinText.text = ": " + CoinAmount;

        if (_amount >= 10)
        {
            bossplattform.SetActive(true);
            boss.SetActive(true);
            bossText.SetActive(true);
            Destroy(bossText, 5f);
        }
    }
}
