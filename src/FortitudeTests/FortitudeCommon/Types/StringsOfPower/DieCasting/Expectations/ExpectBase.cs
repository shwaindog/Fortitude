// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Runtime.CompilerServices;
using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.LoggerViews;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations;

public interface IFormatExpectation : ICodeLocationAwareListItem
{
    public const string NoResultExpectedValue = "No ResultExpected Value";

    Type InputType { get; }
    Type CoreType { get; }

    bool InputIsNull { get; }
    bool InputIsEmpty { get; }
    string? ValueFormatString { get; }
    bool IsStringLike { get; }

    FormatFlags FormatFlags { get; }

    string ShortTestName { get; }

    string GetExpectedOutputFor(ScaffoldingStringBuilderInvokeFlags condition, StyleOptions stringStyle, string? formatString = null);

    IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry, StyleOptions stringStyle);
}

public interface ITypedFormatExpectation<out T> : IFormatExpectation
{
    T? Input { get; }

    void ClearExpectations();

    void Add(EK key, string value);
    void Add(KeyValuePair<EK, string> newExpectedResult);
}

public abstract class ExpectBase<TInput> : ITypedFormatExpectation<TInput>, IEnumerable<KeyValuePair<EK, string>>
{
    private readonly string  srcFile;
    private readonly int     srcLine;
    private readonly string? name;

    private static readonly IVersatileFLogger Logger = FLog.FLoggerForType.As<IVersatileFLogger>();

    protected readonly List<KeyValuePair<EK, string>> ExpectedResults = new();

    // ReSharper disable once ConvertToPrimaryConstructor
    protected ExpectBase(TInput? input, string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = "", [CallerLineNumber] int srcLine = 0)
    {
        this.srcFile         = srcFile;
        this.srcLine         = srcLine;
        this.name = name;
        FormatFlags      = formatFlags;
        Input                = input;
        ValueFormatString         = valueFormatString;
    }

    public virtual Type InputType { get; } = typeof(TInput);

    public bool IsStringLike => InputType.IsAnyTypeHoldingChars();

    public virtual Type CoreType => InputType.IfNullableGetUnderlyingTypeOrThis();

    public TInput? Input { get; set; }
    
    public string? Name => name;

    public string? ValueFormatString { get; init; }

    public FormatFlags FormatFlags { get; init; }

    public bool InputIsNull => Input == null;
    public abstract bool InputIsEmpty { get; }

    public virtual bool HasIndexRangeLimiting => false;

    public virtual string ShortTestName
    {
        get
        {
            {
                var result = new MutableString();
                result.Append(InputType.CachedCSharpNameNoConstraints());
                if (Input == null) { result.Append("=null"); }
                else
                {
                    result.Append(AsStringDelimiterOpen);
                    if (name != null)
                    {
                        result.Append(name);
                    }
                    else
                    {
                        result.Append(Input);
                    }
                    result.Append(AsStringDelimiterClose).Append("_").Append(ValueFormatString);
                    if (FormatFlags != DefaultCallerTypeFlags)
                    {
                        result.Append(AsStringDelimiterClose).Append("_").Append(FormatFlags);
                    }
                }

                return result.ToString();
            }
        }
    }

    protected string AsStringDelimiterOpen =>
        InputType.Name switch
        {
            "String"        => "\""
          , "MutableString" => "\""
          , "StringBuilder" => "\""
          , "Char"          => "'"
          , "Char[]"        => "["
          , _               => "("
        };

    protected string AsStringDelimiterClose =>
        InputType.Name switch
        {
            "String"        => "\""
          , "MutableString" => "\""
          , "StringBuilder" => "\""
          , "Char"          => "'"
          , "Char[]"        => "]"
          , _               => ")"
        };

    public int AtIndex { get; set; }

    public Type? ListOwningType { get; set; }

    public string? ListMemberName { get; set; }

    public string ItemCodePath => ListOwningType != null
        ? $"{ListOwningType.Name}.{ListMemberName}[{AtIndex}]"
        : $"UnsetListOwnerType.UnknownListMemberName[{AtIndex}]";

    public virtual string GetExpectedOutputFor(ScaffoldingStringBuilderInvokeFlags condition, StyleOptions stringStyle, string? formatString = null)
    {
        for (var i = 0; i < ExpectedResults.Count; i++)
        {
            var existing = ExpectedResults[i];
            if (!existing.Key.IsMatchingScenario(condition, stringStyle.Style))
            {
                Logger.DebugAppend("Rejected -")?.FinalAppend(i);
                continue;
            }
            Logger.WarnAppend("Selected -")?.FinalAppend(i);
            var rawInternal = existing.Value;
            return rawInternal;
        }
        Logger.Error("No Match Found !");
        return IFormatExpectation.NoResultExpectedValue;
    }

    public void Add(EK key, string value)
    {
        ExpectedResults.Add(new KeyValuePair<EK, string>(key, value));
    }

    public void Add(KeyValuePair<EK, string> newExpectedResult)
    {
        Add(newExpectedResult.Key, newExpectedResult.Value);
    }

    public void ClearExpectations()
    {
        ExpectedResults.Clear();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<EK, string>> GetEnumerator() => ExpectedResults.GetEnumerator();

    public abstract ISinglePropertyTestStringBearer CreateNewStringBearer(ScaffoldingPartEntry scaffoldEntry);
    public abstract IStringBearer                   CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry, StyleOptions stringStyle);

    protected virtual void AdditionalToStringExpectFields(IStringBuilder sb, ScaffoldingStringBuilderInvokeFlags forThisScaffold)
    {
        AddExpectedResultsList(sb);
    }

    protected void AddExpectedResultsList(IStringBuilder sb)
    {
        sb.AppendLine();
        sb.AppendLine("ExpectedResults");
        var count = 0;
        foreach (var keyValuePair in ExpectedResults)
        {
            sb.Append(count++).Append(" - ").Append("{ ").Append(keyValuePair.Key).Append(", >").Append(keyValuePair.Value).AppendLine("< }");
        }
    }

    public string ToStringForScaffold(ScaffoldingStringBuilderInvokeFlags forThisScaffold = 0)
    {
        var sb = new MutableString();
        sb.AppendLine(GetType().CachedCSharpNameWithConstraints());
        if (srcFile.IsNotEmpty())
        {
            sb.AppendLine();
            sb.Append(new Uri("file://" + new FileInfo(srcFile).FullName + ":" + srcLine)).AppendLine();
        }
        if (name != null)
        {
            sb.Append("Name").Append(": ").Append(name).Append(", ");
        }
        sb.Append(nameof(InputType)).Append(": ").Append(InputType.CachedCSharpNameWithConstraints()).Append(", ");
        sb.Append(nameof(Input)).Append(": ");
        if (InputIsNull) { sb.Append("null"); }
        else { sb.Append(AsStringDelimiterOpen).Append(new MutableString().Append(Input).ToString()).Append(AsStringDelimiterClose); }
        sb.Append(", ").Append(nameof(ValueFormatString)).Append(": ").Append(ValueFormatString != null ? $"\"{ValueFormatString}\"" : "null");
        sb.Append(", ").Append(nameof(FormatFlags)).Append(": ").Append(FormatFlags);
        AdditionalToStringExpectFields(sb, forThisScaffold);
        return sb.ToString();
    }

    public override string ToString()
    {
        return ToStringForScaffold(0);
    }
}
