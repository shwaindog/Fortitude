// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.LoggerViews;
using FortitudeCommon.Types.Code;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;


public interface IInputBearerFormatExpectation : ICodeLocationAware
{
    public const string NoResultExpectedValue = "No ResultExpected Value";

    Type InputType { get; }

    string? ValueFormatString { get; }

    FormatFlags FormatFlags { get; }

    string ShortTestName { get; }

    IStringBuilder GetExpectedOutputFor(IRecycler sbFactory,  StringStyle stringStyle);

    IStringBearer TestStringBearer { get; }
}

public class InputBearerExpect<TInput> : IInputBearerFormatExpectation, IEnumerable<KeyValuePair<EK, string>>
    where TInput : IStringBearer
{
    private readonly string  srcFile;
    private readonly int     srcLine;
    private readonly string? name;

    private static readonly IVersatileFLogger Logger = FLog.FLoggerForType.As<IVersatileFLogger>();

    protected readonly List<KeyValuePair<EK, string>> ExpectedResults = new();
    
    // ReSharper disable twice ExplicitCallerInfoArgument
    public InputBearerExpect(TInput input, string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerMemberName] string callingMemberName = ""
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) 
    {
        MemberName = callingMemberName;
        this.srcFile    = srcFile;
        this.srcLine    = srcLine;
        this.name       = name;
        
        FormatFlags            = formatFlags;
        Input                  = input;
        ValueFormatString      = valueFormatString;
        
        
        var stackTrace = new StackTrace(1, false);

        var method = stackTrace.GetFrame(0)!.GetMethod();
        var type   = method?.DeclaringType ?? typeof(string);
        // Console.Out.WriteLine($"Frame[{0}] = type-{type.FullName}:method-{method?.Name}");
        // if (type.FullName?.StartsWith("System.") ?? true)
        // {
        //     var fullStackTrace = new StackTrace(0, false);
        //
        //     for (var i = 0; i < fullStackTrace.FrameCount; i++)
        //     {
        //         var frame = fullStackTrace.GetFrame(1);
        //         Console.Out.WriteLine($"Frame[{i}] = {frame}");
        //     }
        // }
        this.ContainingType = type;
    }

    public TInput Input { get; set; }

    public IStringBearer TestStringBearer
    {
        get
        {
            var toReturn = Input;
            if (toReturn is ISupportFormattingFlags setsFormatFlags)
            {
                setsFormatFlags.FormattingFlags = FormatFlags;
            }
            if (toReturn is ISupportSecondFormattingFlags setSecondsFormatFlags)
            {
                setSecondsFormatFlags.SecondFormattingFlags = FormatFlags;
            }
            if (toReturn is ISupportsValueFormatString setsFormatString)
            {
                setsFormatString.ValueFormatString = ValueFormatString;
            }
            return Input;
        }
    }

    public Type InputType { get; } = typeof(TInput);

    public string? ValueFormatString { get; init; }

    public FormatFlags FormatFlags { get; init; }
    
    public Type ContainingType { get; }
    
    public string ContainingTypeNameFullName => ContainingType.FullName!;

    public string MemberName { get; }

    public Uri CodePath => new ("file://" + new FileInfo(srcFile).FullName + ":" + srcLine);

    public string ShortTestName
    {
        get
        {
            {
                var result = new MutableString();
                result.Append(InputType.CachedCSharpNameNoConstraints());
                result.Append("(");
                if (name != null)
                {
                    result.Append(name);
                }
                else
                {
                    result.Append(Input);
                }
                result.Append(")").Append("_").Append(ValueFormatString);
                if (FormatFlags != DefaultCallerTypeFlags)
                {
                    result.Append("(").Append("_").Append(FormatFlags);
                }

                return result.ToString();
            }
        }
    }
    
    public BuildExpectedOutput WhenValueExpectedOutput { get; set; } = null!;
    

    public void Add(EK key, string value)
    {
        ExpectedResults.Add(new KeyValuePair<EK, string>(key, value));
    }

    public void Add(KeyValuePair<EK, string> newExpectedResult)
    {
        Add(newExpectedResult.Key, newExpectedResult.Value);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<EK, string>> GetEnumerator() => ExpectedResults.GetEnumerator();

    public virtual IStringBuilder GetExpectedOutputFor(IRecycler sbFactory, StringStyle stringStyle)
    {
        var sb = sbFactory.Borrow<CharArrayStringBuilder>();
        for (var i = 0; i < ExpectedResults.Count; i++)
        {
            var existing = ExpectedResults[i];
            if (!existing.Key.IsMatchingScenario(existing.Key.MatchScaff, stringStyle))
            {
                Logger.DebugAppend("Rejected -")?.FinalAppend(i);
                continue;
            }
            Logger.WarnAppend("Selected -")?.FinalAppend(i);
            var rawInternal = existing.Value;
            sb.Append(rawInternal);
            return sb;
        }
        Logger.Error("No Match Found !");
        sb.Append(IFormatExpectation.NoResultExpectedValue);
        return sb;
    }

    public string ToStringForStringBearer()
    {
        var sb = new MutableString();
        sb.AppendLine(GetType().CachedCSharpNameWithConstraints());
        if (srcFile.IsNotEmpty())
        {
            sb.AppendLine();
            sb.Append(CodePath).AppendLine();
        }
        if (name != null)
        {
            sb.Append("Name").Append(": ").Append(name).Append(", ");
        }
        sb.Append(nameof(InputType)).Append(": ").Append(InputType.CachedCSharpNameWithConstraints()).Append(", ");
        sb.Append(nameof(Input)).Append(": ");
         sb.Append("(").Append(new MutableString().Append(Input).ToString()).Append(")"); 
        sb.Append(", ").Append(nameof(ValueFormatString)).Append(": ").Append(ValueFormatString != null ? $"\"{ValueFormatString}\"" : "null");
        sb.Append(", ").Append(nameof(FormatFlags)).Append(": ").Append(FormatFlags);
        return sb.ToString();
    }

    public override string ToString()
    {
        return ToStringForStringBearer();
    }
}
