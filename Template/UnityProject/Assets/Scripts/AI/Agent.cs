using System;
using System.Collections.Generic;

/// <summary>
/// Class that combines a genotype and a feedforward neural network (FNN).
/// </summary>
public class Agent : IComparable<Agent>
{
    private bool isAlive;

    public bool IsAlive
    {
        get => isAlive;
        private set
        {
            if (isAlive == value) return;
            isAlive = value;

            if (!isAlive)
                AgentDied?.Invoke(this);
        }
    }

    public event Action<Agent> AgentDied;

    public Agent()
    {
    }

    public void Reset()
    {
        IsAlive = true;
    }

    public void Kill()
    {
        IsAlive = false;
    }

    public int CompareTo(Agent other)
    {
        return 1;
    }
}

