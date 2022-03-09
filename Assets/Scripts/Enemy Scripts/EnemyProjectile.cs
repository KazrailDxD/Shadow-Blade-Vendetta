
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
	public int Damage { private get; set; }

	void Start()
	{
		Destroy(this.gameObject, CONSTANTS.PROJECTILE_LIFETIME);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("Player"))
		{
			PlayerAttribute player = collision.collider.GetComponent<PlayerAttribute>();

			player.Hit(Damage);
		}

		Destroy(this.gameObject);
	}
}
