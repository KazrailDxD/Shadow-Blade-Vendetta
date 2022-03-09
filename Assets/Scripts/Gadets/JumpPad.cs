using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{

    float jumpHight = CONSTANTS.JUMPPAD_ADD_JUMPHIGH;

    //Sound 
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip jumpClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioSource.PlayOneShot(jumpClip);
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpHight), ForceMode2D.Impulse);

        }
    }


}
