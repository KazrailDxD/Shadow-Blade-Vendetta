using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
	[SerializeField] private GameObject UI = null;
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Backspace))
		{
			UI.SetActive(!UI.activeSelf);
		}
	}
}
