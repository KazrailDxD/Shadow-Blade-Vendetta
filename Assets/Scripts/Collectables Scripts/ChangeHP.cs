using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHP : MonoBehaviour
{
  
  
    private int giveHp =CONSTANTS.COLLECTABLE_ADD_HEALTH;

    //Sound
    [SerializeField] AudioSource audioSource;
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {

            //Sound Und Grafik vom Boost
            this.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;

            audioSource.Play();

            collision.gameObject.GetComponent<PlayerAttribute>().RegenerateHP(giveHp);
            
            Destroy(gameObject,audioSource.clip.length);
        }
    }

}
