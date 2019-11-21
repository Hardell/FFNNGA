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

    // Population size, to be set in Unity Editor
    [SerializeField]
    private int PopulationSize = 30;

    // Topology of the agent's FNN, to be set in Unity Editor
    [SerializeField]
    private uint[] FNNTopology;

    // The current population of agents.
    private readonly List<Agent> agents = new List<Agent>();

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

    // Starts the evaluation by first creating new agents from the current population and then restarting the track manager.
    private void StartEvaluation(IEnumerable<Genotype> currentPopulation)
    {
        //Create new agents from currentPopulation
        agents.Clear();
        AgentsAliveCount = 0;

        foreach (var genotype in currentPopulation)
            agents.Add(new Agent(genotype, FNNTopology));

        TrackManager.Instance.SetCarAmount(agents.Count);
        var carsEnum = TrackManager.Instance.GetCarEnumerator();
        foreach (var agent in agents)
        {
            if (!carsEnum.MoveNext())
            {
                Debug.LogError("Cars enum ended before agents.");
                break;
            }

            carsEnum.Current.Agent = agent;
            AgentsAliveCount++;
            agent.AgentDied += OnAgentDied;
        }

        TrackManager.Instance.Restart();
    }

    // Callback for when an agent died.
    private void OnAgentDied(Agent agent)
    {
        AgentsAliveCount--;

        if (AgentsAliveCount == 0)
            AllAgentsDied?.Invoke();
    }
}
