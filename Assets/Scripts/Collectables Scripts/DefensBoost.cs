using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensBoost : MonoBehaviour
{
    private int adddefence = CONSTANTS.COLLECTABLE_ADD_DEFENCE;

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

            collision.gameObject.GetComponent<PlayerAttribute>().AddDefensePowerTemp(adddefence);
          
            Destroy(gameObject,audioSource.clip.length);
        }
    }
}
