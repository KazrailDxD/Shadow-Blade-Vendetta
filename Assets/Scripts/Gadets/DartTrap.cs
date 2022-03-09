using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartTrap : MonoBehaviour
{

   
    [SerializeField] private GameObject dartprefab = null;
    [SerializeField] private LayerMask playerRayLayerMask = new LayerMask();
    [SerializeField] private Transform projectileSpawnPoint = null;

    private PlayerController player = null;

    private float shotTimer = CONSTANTS.DARTTRAP_SHOT_TIMER;

	private float m_timer;
	public float Timer
	{
		get { return m_timer; }
		set 
        {
            if (value < 0f)
			{
                m_timer = 0f;
                return;
			}
            m_timer = value; 
        }
	}

    private float detectionRange = 3.0f;
    //Audio
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip dartClip;


    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        Timer -= Time.deltaTime;

        if (player == null) return;

        RaycastHit2D hitinfo = Physics2D.Raycast(projectileSpawnPoint.position, transform.TransformDirection(Vector3.left), detectionRange, playerRayLayerMask);
        Debug.DrawRay(projectileSpawnPoint.position, transform.TransformDirection(Vector3.left) * detectionRange, Color.red);
        if (hitinfo && hitinfo.collider.CompareTag("Player")) 
        {

            if (Timer <= 0f) 
            {
                Timer = shotTimer;
                SpawnDart();
            }

        }
    }
     private void SpawnDart() 
    {
        audioSource.PlayOneShot(dartClip);
        Instantiate(dartprefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
    }
}
