using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSound : MonoBehaviour
{

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip BossfightClip;
    [SerializeField] AudioClip InGameLevelSound;
    [SerializeField] AudioClip BossIsComming;
    [SerializeField] AudioClip LevelComplettet;

    //Plattform deaktive setzen
    [SerializeField] private GameObject bossplattform;


    // Start is called before the first frame update
    void Start()
    {
       
        audioSource.clip = InGameLevelSound;
        audioSource.Play();
    }

    private void OnEnable()
    {
        Boss.OnBossDied += StageClear;
        CoinCounterSript.CoinAmountChanged += HandleCoinChange;
    }

	private void OnDisable()
    {
        Boss.OnBossDied -= StageClear;
        CoinCounterSript.CoinAmountChanged -= HandleCoinChange;
    }

    private void HandleCoinChange(int _amount)
    {
        if (_amount == 10)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(BossIsComming);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            audioSource.Stop();
            audioSource.PlayOneShot(BossfightClip);
            bossplattform.SetActive(false);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            audioSource.Stop();
            audioSource.PlayOneShot(InGameLevelSound);

        }
        
    }

    private void StageClear() 
    {
        audioSource.Stop();
        audioSource.PlayOneShot(LevelComplettet);
    }

}
