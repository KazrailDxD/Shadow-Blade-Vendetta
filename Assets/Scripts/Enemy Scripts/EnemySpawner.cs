using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
      
    public GameObject[] enemie = null;

    public Transform[] spawnPoints = null;

    public float enemyDetectionRange = 10f;

    public LayerMask unitLayer;



    private int numEnemiesToSpawn =40;
   

  
    void Start()
    {

        if(spawnPoints.Length == 0) 
        {
            Debug.LogError("keine spawn punkte gestzt");
        }



        //bevor spiel startet wie viele genger schon vorhanden sind im Lv
        SpawnEnemies();
    }


   
    void OnEnable()
    {
        Enemy.OnEnemyKilled += ReSpawnEnemies;
    }

	private void OnDisable()
	{
        Enemy.OnEnemyKilled -= ReSpawnEnemies;
    }

	private void SpawnEnemies()
	{
        StartCoroutine(SpawnEnemiesCo(0f));
    }

    //die gegner werden zufällig im leve verteilt spawnen
    private void ReSpawnEnemies()
    {
        StartCoroutine(SpawnEnemiesCo(20f));
    }

    private IEnumerator SpawnEnemiesCo(float _delay)
    {
        yield return new WaitForSeconds(_delay);

        List<Transform> validSpawnPoints = new List<Transform>();
        foreach (Transform spawnPoint in spawnPoints)
        {
            Collider2D[] detectedUnits = Physics2D.OverlapCircleAll(spawnPoint.position, enemyDetectionRange, unitLayer);
            if (detectedUnits.Length == 0)
            {
                validSpawnPoints.Add(spawnPoint);
            }
        }

        int actualNumberToSpawn = Mathf.Min(numEnemiesToSpawn, validSpawnPoints.Count);

        for (int i = 0; i < actualNumberToSpawn; i++)
        {
            Transform spawn = validSpawnPoints[Random.Range(0, validSpawnPoints.Count)];

            validSpawnPoints.Remove(spawn);

            int enemieindex = Random.Range(0, enemie.Length);

            Instantiate(enemie[enemieindex], spawn.position, Quaternion.identity);
        }
    }

	private void OnDrawGizmosSelected()
	{
		foreach(Transform spawn in spawnPoints)
		{
            Gizmos.DrawWireSphere(spawn.position, enemyDetectionRange);
		}
	}

}
