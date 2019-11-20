using System;

/// <summary>
/// Class representing a fully connected feedforward neural network.
/// </summary>
public class NeuralNetwork
{
    /// <summary>
    /// The individual neural layers of this network.
    /// </summary>
    public NeuralLayer[] Layers
    {
        get;
    }

    /// <summary>
    /// An array of unsigned integers representing the node count
    /// of each layer of the network from input to output layer.
    /// </summary>
    public uint[] Topology
    {
        get;
    }

    /// <summary>
    /// The amount of overall weights of the connections of this network.
    /// </summary>
    public int WeightCount
    {
        get;
    }

    /// <summary>
    /// Initialises a new fully connected feedforward neural network with given topology.
    /// </summary>
    /// <param name="topology">An array of unsigned integers representing the node count of each layer from input to output layer.</param>
    public NeuralNetwork(params uint[] topology)
    {
        this.Topology = topology;

        //Calculate overall weight count
        WeightCount = 0;
        for (var i = 0; i < topology.Length - 1; i++)
            WeightCount += (int) ((topology[i] + 1) * topology[i + 1]); // + 1 for bias node

        //Initialise layers
        Layers = new NeuralLayer[topology.Length - 1];
        for (var i = 0; i<Layers.Length; i++)
            Layers[i] = new NeuralLayer(topology[i], topology[i + 1]);
    }

    /// <summary>
    /// Processes the given inputs using the current network's weights.
    /// </summary>
    /// <param name="inputs">The inputs to be processed.</param>
    /// <returns>The calculated outputs.</returns>
    public double[] ProcessInputs(double[] inputs)
    {
        //Process inputs by propagating values through all layers
        var outputs = inputs;
        foreach (var layer in Layers)
            outputs = layer.ProcessInputs(outputs);

        return outputs;
    }
}
