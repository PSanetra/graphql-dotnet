using GraphQL.Types;
using GraphQLParser.AST;

namespace GraphQL.Tests.Types;

public class CaseInsensitiveEnumGraphTypeTests
{
    private enum Colors
    {
        Red = 1,
        Blue,
        Green,
        CamelBrown
    }

    private class ColorsEnum : CaseInsensitiveEnumerationGraphType
    {
        public ColorsEnum()
        {
            Name = "Colors";

            foreach (var colorValue in Enum.GetValues(typeof(Colors)).OfType<Colors>())
            {
                Add(new EnumValueDefinition(colorValue.ToString("G"), colorValue));
            }
        }
    }

    private readonly ColorsEnum type = new();

    [Fact]
    public void parsing_custom_casing_should_not_throw()
    {
        type.ParseValue("rED").ShouldBe(Colors.Red);
        type.ParseValue("CamelBrown").ShouldBe(Colors.CamelBrown);
        type.ParseValue("CAMELBROWN").ShouldBe(Colors.CamelBrown);
        type.ParseValue("CAMEL_BROWN").ShouldBe(Colors.CamelBrown);
    }

    [Fact]
    public void parsing_unknown_value_should_throw()
    {
        Should.Throw<InvalidOperationException>(() => type.ParseValue("UNKNOWN")).Message.ShouldBe("Unable to convert 'UNKNOWN' value of type 'String' to the scalar type 'Colors'");
    }

    [Fact]
    public void canParse_custom_casing_should_return_true()
    {
        type.CanParseValue("rED").ShouldBeTrue();
        type.CanParseValue("CamelBrown").ShouldBeTrue();
        type.CanParseValue("CAMELBROWN").ShouldBeTrue();
        type.CanParseValue("CAMEL_BROWN").ShouldBeTrue();
    }

    [Fact]
    public void canParse_unknown_should_return_false()
    {
        type.CanParseValue("UNKNOWN").ShouldBeFalse();
    }

    [Fact]
    public void parse_value_is_null_safe()
    {
        type.ParseValue(null).ShouldBe(null);
    }

    [Fact]
    public void does_not_allow_nulls_to_be_added()
    {
        Assert.Throws<ArgumentNullException>(() => new CaseInsensitiveEnumerationGraphType().Add(null!));
    }

    [Fact]
    public void parse_literal_from_name()
    {
        type.ParseLiteral(new GraphQLEnumValue { Name = new GraphQLName("RED") }).ShouldBe(Colors.Red);
    }

    [Fact]
    public void serialize_by_value()
    {
        type.Serialize(Colors.Red).ShouldBe("Red");
    }

    [Fact]
    public void serialize_by_underlying_value()
    {
        type.Serialize((int)Colors.Red).ShouldBe("Red");
    }

    [Fact]
    public void serialize_by_name_throws()
    {
        Should.Throw<InvalidOperationException>(() => type.Serialize("RED"));
    }

    [Fact]
    public void serialize_should_work_with_null_values()
    {
        var en = new CaseInsensitiveEnumerationGraphType();
        en.Add("one", 100500);
        en.Add("two", null);

        en.Serialize(100500).ShouldBe("one");
        en.Serialize(null).ShouldBe("two");
    }

    [Fact]
    public void toast_should_work_with_null_values()
    {
        var en = new CaseInsensitiveEnumerationGraphType();
        en.Add("one", 100500);
        en.Add("two", null);

        en.ToAST(100500).ShouldBeOfType<GraphQLEnumValue>().Name.Value.ShouldBe("one");
        en.ToAST(null).ShouldBeOfType<GraphQLEnumValue>().Name.Value.ShouldBe("two");
    }

    [Fact]
    public void toAST_returns_enum_value()
    {
        type.ToAST(Colors.Red)
            .ShouldNotBeNull()
            .ShouldBeOfType<GraphQLEnumValue>()
            .Name.Value.ShouldBe("Red");
    }
}
