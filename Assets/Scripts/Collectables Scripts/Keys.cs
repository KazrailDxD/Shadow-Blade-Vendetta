using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keys : MonoBehaviour
{
    //Sound
    [SerializeField] AudioSource audioSource;

    public static Action OnCoinCollected = () => { };

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            //Sound Und Grafik vom Boost
            this.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;

            audioSource.Play();

            OnCoinCollected.Invoke();
          
            Destroy(gameObject,audioSource.clip.length);
        }
    }
}
