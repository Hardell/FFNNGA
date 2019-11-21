
using System;

/// <summary>
/// Class representing a single layer of a fully connected feedforward neural network.
/// </summary>
public class NeuralLayer
{
    /// <summary>
    /// The amount of neurons in this layer.
    /// </summary>
    public uint NeuronCount
    {
        get;
    }

    /// <summary>
    /// The amount of neurons this layer is connected to, i.e., the amount of neurons of the next layer.
    /// </summary>
    public uint OutputCount
    {
        get;
    }

    public double[,] Weights
    {
        get;
        set;
    }

    public NeuralLayer(uint nodeCount, uint outputCount)
    {
        this.NeuronCount = nodeCount;
        this.OutputCount = outputCount;

        Weights = new double[nodeCount + 1, outputCount]; // + 1 for bias node
    }

    /// <summary>
    /// Processes the given inputs using the current weights to the next layer.
    /// </summary>
    /// <param name="inputs">The inputs to be processed.</param>
    /// <returns>The calculated outputs.</returns>
    public double[] ProcessInputs(double[] inputs)
    {
        if (inputs.Length != NeuronCount)
            throw new Exception("Wrong inputs in NL");

        var output = new double[OutputCount];
        var biasedInput = new double[NeuronCount + 1];
        inputs.CopyTo(biasedInput, 0);
        biasedInput[biasedInput.Length - 1] = 1;

        for (var i = 0; i < OutputCount; i++)
        {
            for (var j = 0; j < NeuronCount; j++)
            {
                output[i] += inputs[j] * Weights[j, i];
            }
        }

        //Apply activation function to sum, if set
        for (var i = 0; i < output.Length; i++)
            output[i] = MathHelper.SoftSignFunction(output[i]);

        return output;
    }
}
