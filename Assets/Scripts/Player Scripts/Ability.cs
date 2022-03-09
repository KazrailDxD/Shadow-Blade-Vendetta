using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability
{
	public bool HasCooldown { get; set; } = false;
	public float Cooldown { get; set; } = 0f;
	public bool IsOnCooldown { get; private set; } = false;

	private float m_currentCooldown;
	public float CurrentCooldown
	{
		get { return m_currentCooldown; }
		private set
		{ 
			if (value <= 0f)
			{
				value = 0f;
				IsOnCooldown = false;
			}
			else if (value > 0f)
			{
				IsOnCooldown = true;
			}

			m_currentCooldown = value;
		}
	}

	public int RequiredLevel { get; set; } = 1;

	public float CooldownStartedAt { get; private set; } = 1f;

	public void Update()
	{
		CurrentCooldown -= Time.deltaTime;
	}

	public void StartCooldown(float _haste)
	{
		if (!HasCooldown) return;
		if (!IsOnCooldown)
		{
			IsOnCooldown = true;
			m_currentCooldown = GetHastedCooldown(Cooldown, _haste);
			CooldownStartedAt = m_currentCooldown;
		}
	}

	public void StartGlobalCooldown(float _haste)
	{
		float gcd = GetHastedCooldown(CONSTANTS.BASE_GLOBAL_COOLDOWN, _haste);
		if (CurrentCooldown < gcd)
		{
			IsOnCooldown = true;
			m_currentCooldown = gcd;
			CooldownStartedAt = m_currentCooldown;
		}
	}

	// Cooldown calculation
	public static float GetHastedCooldown(float _cooldown, float _haste)
	{
		return _cooldown / ((_haste / 100f) + 1f);
	}
}
