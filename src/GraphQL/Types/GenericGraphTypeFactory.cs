namespace GraphQL.Types;

/// <summary>
/// A generic factory to create instances of specific TGraphType implementations from parameterless constructors.
/// </summary>
public class GenericGraphTypeFactory<TGraphType> : IGraphTypeFactory<TGraphType> where TGraphType : IGraphType
{
    /// <summary>
    /// Create a new instance of TGraphType. Requires a parameterless constructor.
    /// </summary>
    /// <returns></returns>
    public TGraphType Create() => Activator.CreateInstance<TGraphType>();
}
