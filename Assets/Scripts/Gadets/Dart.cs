using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
  

    private int dartdamage = CONSTANTS.DART_DAMAGE;
    private float dartspeed = CONSTANTS.DART_SPEED;
    Vector2 direction = Vector3.zero;

    Rigidbody2D rb = null;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        direction = transform.TransformDirection(Vector3.left);

        Destroy(this.gameObject, CONSTANTS.DART_LIFETIME);

        rb.AddForce(direction.normalized * dartspeed, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player")) 
        {
            PlayerAttribute player = collision.collider.GetComponent<PlayerAttribute>();
            player.Hit(dartdamage);
        }

        Destroy(this.gameObject);
    }
}
