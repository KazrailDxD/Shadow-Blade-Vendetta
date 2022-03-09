using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Deathzone : MonoBehaviour
{
	private int deathzonedamage = 1000;
	private void Update()
	{
		//if (transform.position.y <= CONSTANTS.DEATH_ZONE_Y_POSITION )
		//{
		//	SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		//}
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
		if (collision.collider.CompareTag("Player"))
		{
			PlayerAttribute player = collision.collider.GetComponent<PlayerAttribute>();
			player.Hit(deathzonedamage);
		}
	}
}
