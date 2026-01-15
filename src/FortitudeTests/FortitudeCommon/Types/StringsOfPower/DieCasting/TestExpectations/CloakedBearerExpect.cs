// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Runtime.CompilerServices;
using System.Text;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;


public delegate IStringBuilder BuildExpectedOutput(IRecycler sbFactory, ITheOneString tos, string className, string propertyName
  , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation);

public class CloakedBearerExpect<TChildScaffoldType, TChildScaffold> : FieldExpect<TChildScaffoldType>, IComplexFieldFormatExpectation
    where TChildScaffold : ISinglePropertyTestStringBearer, IUnknownPalantirRevealerFactory
{
    private ScaffoldingPartEntry? calledScaffoldingPart;

    // ReSharper disable twice ExplicitCallerInfoArgument
    public CloakedBearerExpect(TChildScaffoldType? input, string? valueFormatString = null
      , bool hasDefault = false, TChildScaffoldType? defaultValue = default
      , FormatFlags formatFlags = FormatFlags.DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) : 
        base(input, valueFormatString, hasDefault, defaultValue, formatFlags, name, srcFile, srcLine)
    {
        FieldValueExpectation = 
            new FieldExpect<TChildScaffoldType>
                (Input, ValueFormatString, HasDefault, DefaultValue, formatFlags, name, srcFile, srcLine);
    }

    public ITypedFormatExpectation<TChildScaffoldType?> FieldValueExpectation { get; }

    public override bool IsNullable => InputType.IsNullable();

    public TChildScaffold RevealerScaffold { get; set; } = default!;

    public BuildExpectedOutput WhenValueExpectedOutput { get; set; } = null!;

    public override IStringBuilder GetExpectedOutputFor(IRecycler sbFactory, ScaffoldingStringBuilderInvokeFlags condition, ITheOneString tos
      , string? formatString = null)
    {
        FieldValueExpectation.ClearExpectations();
        foreach (var expectedResult in ExpectedResults) { FieldValueExpectation.Add(expectedResult); }
        if (Input is string || Input is char[] || Input is ICharSequence || Input is StringBuilder)
        {
            condition |= AcceptsChars | AcceptsString | AcceptsCharArray | AcceptsCharSequence | AcceptsStringBuilder;
        }
        var expectValue = FieldValueExpectation.GetExpectedOutputFor(sbFactory, condition, tos, formatString);
        if (!expectValue.SequenceMatches(IFormatExpectation.NoResultExpectedValue) && Input != null)
        {
            expectValue.DecrementRefCount();
            expectValue = WhenValueExpectedOutput
                (sbFactory, tos, (Input?.GetType() ?? typeof(TChildScaffoldType)).CachedCSharpNameNoConstraints()
               , $"CloakedRevealer{RevealerScaffold.PropertyName}", condition, FieldValueExpectation);
        }
        return expectValue;
    }

    public override ISinglePropertyTestStringBearer CreateNewStringBearer(ScaffoldingPartEntry scaffoldEntry)
    {
        return scaffoldEntry.ScaffoldingFlags.IsNullableSpanFormattableOnly()
            ? scaffoldEntry.CreateStringBearerFunc(CoreType)()
            : scaffoldEntry.CreateStringBearerFunc(InputType, CoreType)();
    }

    public override IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry, StyleOptions stringStyle)
    {
        calledScaffoldingPart = new ScaffoldingPartEntry(typeof(TChildScaffold), scaffoldEntry.ScaffoldingFlags);
        RevealerScaffold      = calledScaffoldingPart.CreateTypedStringBearerFunc<TChildScaffold>()();
        var createdStringBearer = CreateNewStringBearer(scaffoldEntry);
        if (HasDefault && createdStringBearer is IMoldSupportedDefaultValue<object?> supportsObjectDefaultValue)
            supportsObjectDefaultValue.DefaultValue = DefaultValue;
        if (HasDefault && createdStringBearer is IMoldSupportedDefaultValue<TChildScaffoldType> supportsDefaultValue)
            supportsDefaultValue.DefaultValue = DefaultValue ?? default(TChildScaffoldType)!;
        if (createdStringBearer is IMoldSupportedDefaultValue<string?> supportsStringDefaultValue)
        {
            var expectedDefaultString = DefaultAsString(stringStyle.StyledTypeFormatter);
            FormattedDefault = new MutableString().Append(expectedDefaultString).ToString();
            supportsStringDefaultValue.DefaultValue =
                scaffoldEntry.ScaffoldingFlags.HasAnyOf(DefaultTreatedAsValueOut 
                                                      | DefaultTreatedAsStringOut)
             && !InputType.IsSpanFormattableOrNullable()
                    ? expectedDefaultString
                    : new MutableString().Append(DefaultValue).ToString();

            createdStringBearer = (ISinglePropertyTestStringBearer)supportsStringDefaultValue;
        }
        if (ValueFormatString != null && RevealerScaffold is ISupportsValueFormatString supportsValueFormatString)
        {
            supportsValueFormatString.ValueFormatString = ValueFormatString;

            RevealerScaffold = (TChildScaffold)supportsValueFormatString;
        }
        if (createdStringBearer is IMoldSupportedValue<object?> isObjectMold)
            isObjectMold.Value = Input;
        else { ((IMoldSupportedValue<TChildScaffoldType?>)createdStringBearer).Value = Input; }
        if (createdStringBearer is ISupportsUnknownValueRevealer supportsValueRevealer)
        {
            supportsValueRevealer.ValueRevealerDelegate = RevealerScaffold.CreateRevealerDelegate;
        }
        return createdStringBearer;
    }
}
