using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class representing one member of a population
/// </summary>
public class Genotype : IComparable<Genotype>, IEnumerable<float>
{
    private float[] genes;

    // Overridden indexer for convenient parameter access.
    public float this[int index]
    {
        get => genes[index];
        set => genes[index] = value;
    }

    public Genotype()
    {
    }

    public int CompareTo(Genotype other)
    {
        return 1;
    }

    public IEnumerator<float> GetEnumerator()
    {
        foreach (var t in genes)
            yield return t;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        foreach (var t in genes)
            yield return t;
    }
}
