using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackBoost : MonoBehaviour
{
    private int adddamage =CONSTANTS.COLLECTABLE_ADD_DAMAGE;

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

            collision.gameObject.GetComponent<PlayerAttribute>().AddAttackPowerTemp(adddamage);
            
            Destroy(gameObject,audioSource.clip.length);

        }
    }
}
