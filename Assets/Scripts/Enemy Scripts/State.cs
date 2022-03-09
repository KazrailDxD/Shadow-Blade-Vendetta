using UnityEngine;
public abstract class State
{
    protected Enemy m_enemy;

    // ========================================================================
    public State(Enemy _enemy)
	{
        m_enemy = _enemy;
	}

    // ========================================================================
    public abstract void Start();

    // ========================================================================
    public abstract void Update();

    // ========================================================================
    public abstract void PlayAnimation();
}
