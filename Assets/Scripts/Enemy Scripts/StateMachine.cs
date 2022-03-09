using UnityEngine;

public class StateMachine
{
	public State CurrentState { get; set; }

	// ========================================================================
	public void Initialize(State _initialState)
	{
		ChangeState(_initialState);
	}

	// ========================================================================
	public void Update()
	{
		CurrentState.Update();
	}

	// ========================================================================
	public void ChangeState(State _newState)
	{
		CurrentState = _newState;
		CurrentState.Start();
	}

	// ========================================================================
	public void ChangeStateRandomly(State _state1, State _state2)
	{
		int randomPercent = Random.Range(1, 100);
		if (randomPercent <= 50)
		{
			ChangeState(_state1);
		}
		else
		{
			ChangeState(_state2);
		}
	}
}
