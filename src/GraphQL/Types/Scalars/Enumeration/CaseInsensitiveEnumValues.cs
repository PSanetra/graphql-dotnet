using GraphQLParser;

namespace GraphQL.Types;

/// <summary>
/// A class that represents a case insensitive set of enumeration definitions.
/// </summary>
public class CaseInsensitiveEnumValues : EnumValues
{
    private readonly IDictionary<string, EnumValueDefinition> _upperValues = new Dictionary<string, EnumValueDefinition>();
    private readonly IDictionary<string, EnumValueDefinition> _constantCaseValues = new Dictionary<string, EnumValueDefinition>();

    /// <inheritdoc />
    public override void Add(EnumValueDefinition value)
    {
        base.Add(value);
        _upperValues[value.Name.ToUpperInvariant()] = value;
        _constantCaseValues[value.Name.ToConstantCase()] = value;
    }

    /// <inheritdoc />
    public override EnumValueDefinition? FindByName(ROM name)
    {
        var value = base.FindByName(name);

        if (value != null)
        {
            return value;
        }

        var strName = name.ToString();

        _upperValues.TryGetValue(strName.ToUpperInvariant(), out value);

        if (value != null)
        {
            return value;
        }

        _constantCaseValues.TryGetValue(strName.ToConstantCase(), out value);

        return value;
    }
}
