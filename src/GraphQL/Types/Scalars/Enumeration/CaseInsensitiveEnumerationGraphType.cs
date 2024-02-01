namespace GraphQL.Types;

/// <inheritdoc />
public class CaseInsensitiveEnumerationGraphType : EnumerationGraphType
{
    /// <inheritdoc />
    protected override EnumValuesBase CreateValues() => new CaseInsensitiveEnumValues();
}
