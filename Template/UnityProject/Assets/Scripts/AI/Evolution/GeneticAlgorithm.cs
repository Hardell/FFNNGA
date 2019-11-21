using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Class implementing a modified genetic algorithm
/// </summary>
public class GeneticAlgorithm
{
    /// <summary>
    /// Default min value of inital population parameters.
    /// </summary>
    public const float DefInitParamMin = -1.0f;
    /// <summary>
    /// Default max value of initial population parameters.
    /// </summary>
    public const float DefInitParamMax = 1.0f;

    /// <summary>
    /// Default probability of a parameter being swapped during crossover.
    /// </summary>
    public const float DefCrossSwapProb = 0.6f;

    /// <summary>
    /// Default probability of a parameter being mutated.
    /// </summary>
    public const float DefMutationProb = 0.3f;
    /// <summary>
    /// Default amount by which parameters may be mutated.
    /// </summary>
    public const float DefMutationAmount = 2.0f;

    /// <summary>
    /// Method template for methods used to evaluate (or start the evluation process of) the current population.
    /// </summary>
    /// <param name="currentPopulation">The current population.</param>
    public delegate void EvaluationOperator(IEnumerable<Genotype> currentPopulation);

    /// <summary>
    /// Method used to evaluate (or start the evaluation process of) the current population.
    /// </summary>
    public EvaluationOperator Evaluation;

    public GeneticAlgorithm()
    {
    }
}
