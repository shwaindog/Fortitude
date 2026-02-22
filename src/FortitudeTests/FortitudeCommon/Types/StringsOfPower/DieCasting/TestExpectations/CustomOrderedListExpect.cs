// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;

// ReSharper disable twice ExplicitCallerInfoArgument
public class CustomOrderedListExpect<TInputElement>
(
    List<TInputElement>? inputList
  , string? valueFormatString = null
  , Expression<Func<OrderedCollectionPredicate<TInputElement>>>? elementFilterExpression = null
  , FormatFlags formatFlags = DefaultCallerTypeFlags
  , string? name = null
  , [CallerFilePath] string srcFile = ""
  , [CallerLineNumber] int srcLine = 0)
    : CustomOrderedListExpect<TInputElement, TInputElement>
        (inputList, valueFormatString, elementFilterExpression, formatFlags
       , name, srcFile, srcLine);

public class CustomOrderedListExpect<TInputElement, TFilterBase> : OrderedListExpect<TInputElement, TFilterBase>, IComplexOrderedListExpect
{
    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public CustomOrderedListExpect(List<TInputElement>? inputList, string? valueFormatString = null
      , Expression<Func<OrderedCollectionPredicate<TFilterBase>>>? elementFilter = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0)
        : base(inputList, valueFormatString, elementFilter, formatFlags, name, srcFile, srcLine)
    {
        FieldValueExpectation =
            new FieldExpect<TInputElement?>
                (default, ValueFormatString, false, default, formatFlags, name, srcFile, srcLine);
    }

    public ITypedFormatExpectation<TInputElement?> FieldValueExpectation { get; }

    public BuildExpectedOutput WhenValueExpectedOutput { get; set; } = null!;

    public override bool IsTwoInARowTypes =>
        FieldValueExpectation.InputType == ElementType;

    public override IStringBuilder GetExpectedOutputFor(IRecycler sbFactory, ScaffoldingStringBuilderInvokeFlags condition, ITheOneString tos
      , string? formatString = null)
    {
        FieldValueExpectation.ClearExpectations();
        foreach (var expectedResult in ExpectedResults) { FieldValueExpectation.Add(expectedResult); }
        var expectValue = FieldValueExpectation.GetExpectedOutputFor(sbFactory, condition, tos, formatString);
        if (!expectValue.SequenceMatches(IFormatExpectation.NoResultExpectedValue)
         && !expectValue.SequenceMatches("null") && !expectValue.SequenceMatches(""))
        {
            expectValue = WhenValueExpectedOutput
                (sbFactory
               , tos
               , InputIsNull
               & !(condition.HasCallsAsReadOnlySpan()
                || condition.HasCallsAsSpan())
                     ? null
                     : CollectionCallType
               , ""
               , condition
               , FieldValueExpectation);
        }
        return expectValue;
    }
};
