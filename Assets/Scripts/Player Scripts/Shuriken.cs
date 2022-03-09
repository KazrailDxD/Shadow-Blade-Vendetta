using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
	public int Damage { get; set; }

	public LevelSystem LevelSystem { get; set; }

	Rigidbody2D rb = null;
    Collider2D col = null;

    private Vector3 m_rotation = new Vector3(0, 0, -3);

    private bool m_stuck = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        Destroy(this.gameObject, CONSTANTS.PROJECTILE_LIFETIME);
    }

	private void FixedUpdate()
	{
        if (!m_stuck) transform.Rotate(m_rotation);
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
        col.enabled = false;

        if (collision.collider.CompareTag("Enemy"))
        {
            Enemy e = collision.collider.GetComponent<Enemy>();
            e.Hit(Damage);
            if (!e.IsAlive)
            {
                LevelSystem.PlayerLV(e.Xp);
            }
            Destroy(this.gameObject);
        }

        m_stuck = true;
        rb.angularVelocity = 0f;
        Destroy(this.gameObject, 5f);
	}
}
