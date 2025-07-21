// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;
using static FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.ComparisonOperatorType;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching;

public enum ComparisonOperatorType
{
    GreaterThanOrEqualTo
  , LessThanOrEqualsTo
  , GreaterThan
  , LessThan
  , Equals
  , NotEquals
  , IsNull
  , IsNotNull
  , IsEmpty
  , IsNotEmpty
  , IsNullOrEmpty
  , IsNotNullOrEmpty
}

public static class ComparisonOperatorTypedExtensions
{
    public static Action<ComparisonOperatorType, IStyledTypeStringAppender> ComparisonOperatorTypeFormatter
        = FormatComparisonOperatorTypeAppender;

    public static void FormatComparisonOperatorTypeAppender(this ComparisonOperatorType matchOnField, IStyledTypeStringAppender sbc)
    {
        var sb = sbc.BackingStringBuilder;

        switch (matchOnField)
        {
            case GreaterThanOrEqualTo: sb.Append($"{nameof(GreaterThanOrEqualTo)}"); break;
            case LessThanOrEqualsTo:   sb.Append($"{nameof(LessThanOrEqualsTo)}"); break;
            case GreaterThan:          sb.Append($"{nameof(GreaterThan)}"); break;
            case LessThan:             sb.Append($"{nameof(LessThan)}"); break;
            case NotEquals:            sb.Append($"{nameof(NotEquals)}"); break;
            case IsNull:               sb.Append($"{nameof(IsNull)}"); break;
            case IsNotNull:            sb.Append($"{nameof(IsNotNull)}"); break;
            case IsEmpty:              sb.Append($"{nameof(IsEmpty)}"); break;
            case IsNotEmpty:           sb.Append($"{nameof(IsNotEmpty)}"); break;
            case IsNullOrEmpty:        sb.Append($"{nameof(IsNullOrEmpty)}"); break;
            case IsNotNullOrEmpty:     sb.Append($"{nameof(IsNotNullOrEmpty)}"); break;

            
            case ComparisonOperatorType.Equals: sb.Append($"{nameof(ComparisonOperatorType.Equals)}"); break;

            default: sb.Append($"{nameof(GreaterThanOrEqualTo)}"); break;
        }
    }
}