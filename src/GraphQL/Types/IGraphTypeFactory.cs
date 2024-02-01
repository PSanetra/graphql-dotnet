namespace GraphQL.Types;

/// <summary>
/// A factory to create instances of specific TGraphType implementations.
/// </summary>
public interface IGraphTypeFactory<out TGraphType> where TGraphType : IGraphType
{
    /// <summary>
    /// Create a new instance of TGraphType
    /// </summary>
    /// <returns></returns>
    public TGraphType Create();
}
