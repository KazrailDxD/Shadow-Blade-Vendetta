using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{

    private Rigidbody2D rbPlattform;

    private float delay = CONSTANTS.PLATFORM_DELAY;

    // Start is called before the first frame update
    void Start()
    {
        rbPlattform = GetComponent<Rigidbody2D>();
    }

   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player")) 
        {
            StartCoroutine(Falling());
        }
    }
    IEnumerator Falling() 
    {
        yield return new WaitForSeconds(delay);      
        rbPlattform.constraints = RigidbodyConstraints2D.None;  //ersetzt isKinematic
        rbPlattform.freezeRotation = true;
        Destroy(this.gameObject, 0.8f);
    }

}
