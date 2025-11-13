// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Runtime.CompilerServices;
using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

public interface IStringLikeExpectation : ISingleFieldExpectation
{

    bool HasIndexRangeLimiting { get; }
}

public class StringLikeExpect<TInput> : StringLikeExpect<TInput, string>
{
    // ReSharper disable twice ExplicitCallerInfoArgument
    public StringLikeExpect
    (
        TInput? input
      , string? formatString = null
      , bool hasDefault = false
      , string? defaultValue = null
      , int fromIndex = 0
      , int length = Int32.MaxValue
      , FieldContentHandling contentHandling = FieldContentHandling.DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0)
        : base(input, formatString, hasDefault, defaultValue, fromIndex, length, contentHandling, name, srcFile, srcLine) { }
}

public class StringLikeExpect<TInput, TDefault>: FieldExpect<TInput, TDefault>, IStringLikeExpectation
{
    // ReSharper disable twice ExplicitCallerInfoArgument
    public StringLikeExpect
    (
        TInput? input
      , string? formatString = null
      , bool hasDefault = false
      , TDefault? defaultValue = default
      , int fromIndex = 0
      , int length = Int32.MaxValue
      , FieldContentHandling contentHandling = FieldContentHandling.DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0)
        : base(input, formatString, hasDefault, defaultValue, contentHandling, name, srcFile, srcLine)
    {
        FromIndex = fromIndex;
        Length    = length;
    }

    public override bool HasIndexRangeLimiting => FromIndex != 0 || Length != int.MaxValue;

    public int FromIndex { get; init; }
    public int Length { get; init; }

    public override bool InputIsEmpty
    {
        get
        {
            switch (Input)
            {
                case string stringValue:
                    return stringValue.Length == 0
                        || FromIndex >= stringValue.Length || Length <= 0;
                case char[] charArrayValue:
                    return charArrayValue.Length == 0
                        || FromIndex >= charArrayValue.Length || Length <= 0;
                case ICharSequence charSeqValue:
                    return charSeqValue.Length == 0
                        || FromIndex >= charSeqValue.Length || Length <= 0;
                case StringBuilder sbValue:
                    return sbValue.Length == 0
                        || FromIndex >= sbValue.Length || Length <= 0;
                default: return Input != null && Equals(Input, default(TInput));
            }
        }
    }
    
    public override IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry, StyleOptions stringStyle)
    {
        var createdStringBearer = base.CreateStringBearerWithValueFor(scaffoldEntry, stringStyle);
        if (FormatString != null && createdStringBearer is ISupportsIndexRangeLimiting indexRangeLimiting)
        {
            indexRangeLimiting.FromIndex = FromIndex;
            indexRangeLimiting.Length    = Length;
        }
        return createdStringBearer;
    }

    protected override void AdditionalExpectFields(IStringBuilder sb)
    {
        if (FromIndex != 0 || Length != int.MaxValue)
        {
            sb.Append(", ").Append(nameof(FromIndex)).Append(": ").Append(FromIndex).Append(", ");
            sb.Append(nameof(Length)).Append(": ").Append(Length);
        }
    }
}
