using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class representing one member of a population
/// </summary>
public class Genotype : IComparable<Genotype>, IEnumerable<float>
{
    private static Random randomizer = new Random();

    /// <summary>
    /// The current evaluation of this genotype.
    /// </summary>
    public float Evaluation
    {
        get;
        set;
    }
    /// <summary>
    /// The current fitness (e.g, the evaluation of this genotype relative
    /// to the average evaluation of the whole population) of this genotype.
    /// </summary>
    public float Fitness
    {
        get;
        set;
    }

    // The vector of parameters of this genotype.
    private float[] parameters;

    /// <summary>
    /// The amount of parameters stored in the parameter vector of this genotype.
    /// </summary>
    public int ParameterCount => parameters?.Length ?? 0;

    // Overridden indexer for convenient parameter access.
    public float this[int index]
    {
        get => parameters[index];
        set => parameters[index] = value;
    }

    /// <summary>
    /// Instance of a new genotype with given parameter vector and initial fitness of 0.
    /// </summary>
    /// <param name="parameters">The parameter vector to initialise this genotype with.</param>
    public Genotype(float[] parameters)
    {
        this.parameters = parameters;
        Fitness = 0;
    }

    /// <summary>
    /// Compares this genotype with another genotype depending on their fitness values.
    /// </summary>
    /// <param name="other">The genotype to compare this genotype with.</param>
    /// <returns>The result of comparing the two floating point values representing the genotypes fitness in reverse order.</returns>
    public int CompareTo(Genotype other)
    {
        return other.Fitness.CompareTo(this.Fitness); //in reverse order for larger fitness being first in list
    }

    public IEnumerator<float> GetEnumerator()
    {
        foreach (var t in parameters)
            yield return t;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        foreach (var t in parameters)
            yield return t;
    }

    /// <summary>
    /// Sets the parameters of this genotype to random values in given range.
    /// </summary>
    /// <param name="minValue">The minimum inclusive value a parameter may be initialised with.</param>
    /// <param name="maxValue">The maximum exclusive value a parameter may be initialised with.</param>
    public void SetRandomParameters(float minValue, float maxValue)
    {
        //Check arguments
        if (minValue > maxValue) throw new ArgumentException("Minimum value may not exceed maximum value.");

        //Generate random parameter vector
        float range = maxValue - minValue;
        for (int i = 0; i < parameters.Length; i++)
            parameters[i] = (float)(randomizer.NextDouble() * range + minValue); //Create a random float between minValue and maxValue
    }

}
