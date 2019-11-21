using UnityEngine;
using System.Collections.Generic;
using System;

public class EvolutionManager : MonoBehaviour
{
    public static EvolutionManager Instance
    {
        get;
        private set;
    }

    public int AgentsAliveCount
    {
        get;
        private set;
    }

    /// <summary>
    /// Event for when all agents have died.
    /// </summary>
    public event Action AllAgentsDied;

    public uint GenerationCount => 1;


    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one EvolutionManager in the Scene.");
            return;
        }
        Instance = this;
    }

    public void StartEvolution()
    {
    }

    // Callback for when an agent died.
    private void OnAgentDied(Agent agent)
    {
        AgentsAliveCount--;

        if (AgentsAliveCount == 0)
            AllAgentsDied?.Invoke();
    }
}
