namespace OpenGameKit.Generators.Abstractions;

/// <summary>
/// Used by generators to provide a parameter to the generated data.
/// </summary>
public interface IParameterDescription
{
    /// <summary>
    /// The name of the parameter. This is used to retrieve the value after
    /// it has been generated.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// A parameter is made up of one or more waves that are combined to
    /// get the final value.
    /// </summary>
    public IReadOnlyList<IParameterWave> Waves { get; }
}
