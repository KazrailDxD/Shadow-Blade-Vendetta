using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
	private string path = "";

    [SerializeField] private GameObject player = null;
	[SerializeField] private CoinCounterSript coinCounter = null;

	[SerializeField] private GameObject[] collectablePrefabs = null;

	private PlayerAttribute playerAttributes = null;
    private LevelSystem levelSystem = null;

	private bool allowSaving = true;

	public static Action SaveGameLoaded = () => { };

	private void Awake()
	{
		path = Application.persistentDataPath + "/SaveGame.sbv";

		playerAttributes = player.GetComponent<PlayerAttribute>();
		levelSystem = player.GetComponent<LevelSystem>();
	}

	private void Start()
	{
		bool fileAvailable = File.Exists(path);

		if (fileAvailable)
		{
			DestroyAllCollectables();
		}

		StartCoroutine(Load());
	}

	private void OnEnable()
	{
		PlayerAttribute.OnPlayerDied += DeleteSaveGame;
		Boss.OnBossDied += DeleteSaveGame;

		PauseMenu.OnGamePaused += Save;
	}

	private void OnDisable()
	{
		PlayerAttribute.OnPlayerDied -= DeleteSaveGame;
		Boss.OnBossDied -= DeleteSaveGame;

		PauseMenu.OnGamePaused -= Save;
	}

	private void Save()
	{
		if (!allowSaving) return;
		try
		{
			using (FileStream fs = new FileStream(path, FileMode.Create))
			{
				BinaryWriter writer = new BinaryWriter(fs);
				writer.Write(player.transform.position.x);
				writer.Write(player.transform.position.y);
				writer.Write(player.transform.position.z);

				writer.Write(levelSystem.PlayerLevel);
				writer.Write(levelSystem.CurrentExp);
				writer.Write(levelSystem.MaxExp);

				writer.Write(playerAttributes.PlayerHealth);
				writer.Write(playerAttributes.AttackPoints);
				writer.Write(playerAttributes.DefensivePoints);
				writer.Write(playerAttributes.Haste);
				writer.Write(playerAttributes.IsAlive);

				writer.Write(coinCounter.CoinAmount);

				foreach(var collectable in GameObject.FindGameObjectsWithTag("Attack"))
				{
					writer.Write(0);
					writer.Write(collectable.transform.position.x);
					writer.Write(collectable.transform.position.y);
					writer.Write(collectable.transform.position.z);
				}

				foreach (var collectable in GameObject.FindGameObjectsWithTag("Defense"))
				{
					writer.Write(1);
					writer.Write(collectable.transform.position.x);
					writer.Write(collectable.transform.position.y);
					writer.Write(collectable.transform.position.z);
				}

				foreach (var collectable in GameObject.FindGameObjectsWithTag("Food"))
				{
					writer.Write(2);
					writer.Write(collectable.transform.position.x);
					writer.Write(collectable.transform.position.y);
					writer.Write(collectable.transform.position.z);
				}

				foreach (var collectable in GameObject.FindGameObjectsWithTag("Haste"))
				{
					writer.Write(3);
					writer.Write(collectable.transform.position.x);
					writer.Write(collectable.transform.position.y);
					writer.Write(collectable.transform.position.z);
				}

				foreach (var collectable in GameObject.FindGameObjectsWithTag("Key"))
				{
					writer.Write(4);
					writer.Write(collectable.transform.position.x);
					writer.Write(collectable.transform.position.y);
					writer.Write(collectable.transform.position.z);
				}
			}
		}
		catch(Exception e)
		{
			Debug.LogError("Saving game failed!" + e.Message);
		}
		
	}

	private IEnumerator Load()
	{
		if (File.Exists(path))
		{
			yield return new WaitForEndOfFrame();
			try
			{
				using (FileStream fs = new FileStream(path, FileMode.Open))
				{
					BinaryReader reader = new BinaryReader(fs);
					Vector3 playerPos = Vector3.zero;
					playerPos.x = reader.ReadSingle();
					playerPos.y = reader.ReadSingle();
					playerPos.z = reader.ReadSingle();
					player.transform.position = playerPos;

					levelSystem.PlayerLevel = reader.ReadInt32();
					levelSystem.CurrentExp = reader.ReadInt32();
					levelSystem.MaxExp = reader.ReadInt32();

					playerAttributes.PlayerHealth = reader.ReadInt32();
					playerAttributes.AttackPoints = reader.ReadInt32();
					playerAttributes.DefensivePoints = reader.ReadInt32();
					playerAttributes.Haste = reader.ReadInt32();
					playerAttributes.IsAlive = reader.ReadBoolean();

					coinCounter.CoinAmount = reader.ReadInt32();

					while (reader.BaseStream.Position <= reader.BaseStream.Length - 16)
					{
						float x, y, z = 0;
						int collectableType = reader.ReadInt32();
						x = reader.ReadSingle();
						y = reader.ReadSingle();
						z = reader.ReadSingle();

						Vector3 collectablePos = new Vector3(x, y, z);
						GameObject collectable = Instantiate(collectablePrefabs[collectableType]);
						collectable.transform.position = collectablePos;
					}
				}
				SaveGameLoaded.Invoke();
			}
			catch (Exception e)
			{
				Debug.LogError("Loading save game failed! " + e.Message);
			}
		}
		else
		{
			yield return null;
		}
	}

	private void DeleteSaveGame()
	{
		allowSaving = false;
		if (File.Exists(path))
		{
			File.Delete(path);
		}
	}

	private static void DestroyAllCollectables()
	{
		foreach (var collectable in GameObject.FindGameObjectsWithTag("Attack"))
		{
			Destroy(collectable.gameObject);
		}

		foreach (var collectable in GameObject.FindGameObjectsWithTag("Defense"))
		{
			Destroy(collectable.gameObject);
		}

		foreach (var collectable in GameObject.FindGameObjectsWithTag("Food"))
		{
			Destroy(collectable.gameObject);
		}

		foreach (var collectable in GameObject.FindGameObjectsWithTag("Haste"))
		{
			Destroy(collectable.gameObject);
		}

		foreach (var collectable in GameObject.FindGameObjectsWithTag("Key"))
		{
			Destroy(collectable.gameObject);
		}
	}
}
