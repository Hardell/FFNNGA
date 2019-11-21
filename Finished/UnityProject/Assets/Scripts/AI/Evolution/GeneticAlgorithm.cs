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
    /// Default percent of genotypes in a new population that are mutated.
    /// </summary>
    public const float DefMutationPerc = 1.0f;

    /// <summary>
    /// Method template for methods used to evaluate (or start the evluation process of) the current population.
    /// </summary>
    /// <param name="currentPopulation">The current population.</param>
    public delegate void EvaluationOperator(IEnumerable<Genotype> currentPopulation);

    /// <summary>
    /// Method used to evaluate (or start the evaluation process of) the current population.
    /// </summary>
    public EvaluationOperator Evaluation;

    private static Random randomizer = new Random();

    private List<Genotype> currentPopulation;

    /// <summary>
    /// The amount of genotypes in a population.
    /// </summary>
    public uint PopulationSize
    {
        get;
        private set;
    }

    /// <summary>
    /// The amount of generations that have already passed.
    /// </summary>
    public uint GenerationCount
    {
        get;
        private set;
    }

    /// <summary>
    /// Initialises a new genetic algorithm instance, creating a initial population of given size with genotype
    /// of given parameter count.
    /// </summary>
    /// <param name="genotypeParamCount">The amount of parameters per genotype.</param>
    /// <param name="populationSize">The size of the population.</param>
    /// <remarks>
    /// The parameters of the genotypes of the inital population are set to the default float value.
    /// and call <see cref="Start"/> to start the genetic algorithm.
    /// </remarks>
    public GeneticAlgorithm(uint genotypeParamCount, uint populationSize)
    {
        this.PopulationSize = populationSize;
        //Initialise empty population
        currentPopulation = new List<Genotype>((int) populationSize);
        for (var i = 0; i < populationSize; i++)
            currentPopulation.Add(new Genotype(new float[genotypeParamCount]));

        DefaultPopulationInitialisation(currentPopulation);
        GenerationCount = 1;
    }

    public void Start()
    {
        Evaluation(currentPopulation);
    }

    public void EvaluationFinished()
    {
        //Calculate fitness from evaluation
        DefaultFitnessCalculation(currentPopulation);

        //Sort the results
        currentPopulation.Sort();

        //Apply Selection
        var intermediatePopulation = DefaultSelectionOperator(currentPopulation);

        //Apply Recombination
        var newPopulation = RandomRecombination(intermediatePopulation, PopulationSize);

        //Apply Mutation
        MutateAllButBestTwo(newPopulation);

        //Set current population to newly generated one and start evaluation again
        currentPopulation = newPopulation;
        GenerationCount++;

        Evaluation(currentPopulation);
    }

    /// <summary>
    /// Initialises the population by setting each parameter to a random value in the default range.
    /// </summary>
    /// <param name="population">The population to be initialised.</param>
    public static void DefaultPopulationInitialisation(IEnumerable<Genotype> population)
    {
        //Set parameters to random values in set range
        foreach (var genotype in population)
            genotype.SetRandomParameters(DefInitParamMin, DefInitParamMax);
    }

    /// <summary>
    /// Calculates the fitness of each genotype by the formula: fitness = evaluation / averageEvaluation.
    /// </summary>
    /// <param name="currentPopulation">The current population.</param>
    public static void DefaultFitnessCalculation(IEnumerable<Genotype> currentPopulation)
    {
        //First calculate average evaluation of whole population
        var populationSize = currentPopulation.Count();
        float totalEvaluation = 0;
        foreach (var genotype in currentPopulation)
        {
            totalEvaluation += genotype.Evaluation;
        }

        var averageEvaluation = totalEvaluation / populationSize;

        //Now assign fitness with formula fitness = evaluation / averageEvaluation
        foreach (var genotype in currentPopulation)
            genotype.Fitness = genotype.Evaluation / averageEvaluation;
    }

    /// <summary>
    /// Only selects the best three genotypes of the current population and copies them to the intermediate population.
    /// </summary>
    /// <param name="currentPopulation">The current population.</param>
    /// <returns>The intermediate population.</returns>
    public static List<Genotype> DefaultSelectionOperator(List<Genotype> currentPopulation)
    {
        var intermediatePopulation = new List<Genotype>
        {
            currentPopulation[0], currentPopulation[1], currentPopulation[2]
        };

        return intermediatePopulation;
    }

    // Recombination operator for the genetic algorithm, recombining random genotypes of the intermediate population
    private static List<Genotype> RandomRecombination(List<Genotype> intermediatePopulation, uint newPopulationSize)
    {
        //Check arguments
        if (intermediatePopulation.Count < 2)
            throw new ArgumentException("The intermediate population has to be at least of size 2 for this operator.");

        //Always add best two (unmodified)
        var newPopulation = new List<Genotype>
        {
            intermediatePopulation[0],
            intermediatePopulation[1]
        };


        while (newPopulation.Count < newPopulationSize)
        {
            //Get two random indices that are not the same
            int randomIndex1 = randomizer.Next(0, intermediatePopulation.Count), randomIndex2;
            do
            {
                randomIndex2 = randomizer.Next(0, intermediatePopulation.Count);
            } while (randomIndex2 == randomIndex1);

            Genotype offspring1, offspring2;
            CompleteCrossover(intermediatePopulation[randomIndex1], intermediatePopulation[randomIndex2], DefCrossSwapProb, out offspring1, out offspring2);

            newPopulation.Add(offspring1);
            if (newPopulation.Count < newPopulationSize)
                newPopulation.Add(offspring2);
        }

        return newPopulation;
    }

    // Mutates all members of the new population with the default probability, while leaving the first 2 genotypes in the list untouched.
    private static void MutateAllButBestTwo(List<Genotype> newPopulation)
    {
        for (var i = 2; i < newPopulation.Count; i++)
        {
            if (randomizer.NextDouble() < DefMutationPerc)
                MutateGenotype(newPopulation[i], DefMutationProb, DefMutationAmount);
        }
    }

    public static void CompleteCrossover(Genotype parent1, Genotype parent2, float swapChance, out Genotype offspring1, out Genotype offspring2)
    {
        //Initialise new parameter vectors
        var parameterCount = parent1.ParameterCount;
        float[] off1Parameters = new float[parameterCount], off2Parameters = new float[parameterCount];

        //Iterate over all parameters randomly swapping
        for (var i = 0; i < parameterCount; i++)
        {
            if (randomizer.Next() < swapChance)
            {
                //Swap parameters
                off1Parameters[i] = parent2[i];
                off2Parameters[i] = parent1[i];
            }
            else
            {
                //Don't swap parameters
                off1Parameters[i] = parent1[i];
                off2Parameters[i] = parent2[i];
            }
        }

        offspring1 = new Genotype(off1Parameters);
        offspring2 = new Genotype(off2Parameters);
    }

    /// <summary>
    /// Mutates the given genotype by adding a random value in range [-mutationAmount, mutationAmount] to each parameter with a probability of mutationProb.
    /// </summary>
    /// <param name="genotype">The genotype to be mutated.</param>
    /// <param name="mutationProb">The probability of a parameter being mutated.</param>
    /// <param name="mutationAmount">A parameter may be mutated by an amount in range [-mutationAmount, mutationAmount].</param>
    public static void MutateGenotype(Genotype genotype, float mutationProb, float mutationAmount)
    {
        for (var i = 0; i < genotype.ParameterCount; i++)
        {
            if (randomizer.NextDouble() < mutationProb)
            {
                //Mutate by random amount in range [-mutationAmount, mutationAmount]
                genotype[i] += (float)(randomizer.NextDouble() * (mutationAmount * 2) - mutationAmount);
            }
        }
    }

}
