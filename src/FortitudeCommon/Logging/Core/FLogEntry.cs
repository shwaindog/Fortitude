// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Runtime.CompilerServices;
using System.Text;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core;



public interface IFLogEntryFactory
{
    IFLogEntry GetFLogEntry();
}

public record struct LoggingLocation(string MemberName, string SourceFilePath, int SourceLineNumber);

public interface IFLogEntry : IReusableObject<IFLogEntry>
{
    uint     IssueSequenceNumber { get; }
    uint     InstanceNumber { get; }

    DateTime LogDateTime         { get; }

    LoggingLocation LogLocation { get; }

    StringBuilder SB { get; }
}

public interface IMutableFLogEntry : IFLogEntry
{
    [MustUseReturnValue("Use WithOnlyParam or AndFinalParam to complete LogEntry")] 
    IFLogFirstFormatterParameterEntry? StringFormat
    (
        string formattedString
      , StringBuildingStyle style = StringBuildingStyle.Default
      , MalformedFormatHandling malformedFormatHandling = MalformedFormatHandling.DefaultMalformedHandling
      , [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = ""
      , [CallerLineNumber] int sourceLineNumber = 0);

    [MustUseReturnValue("Use FinalAppend to complete LogEntry")] 
    IFLogAppenderEntry StringAppender
    (StringBuildingStyle style = StringBuildingStyle.Default
      , [CallerMemberName] string memberName = ""
      , [CallerFilePath] string sourceFilePath = ""
      , [CallerLineNumber] int sourceLineNumber = 0);
}


public class FLogEntry : ReusableObject<IFLogEntry>, IFLogEntry
{
    internal static readonly LoggingLocation NotSetLocation = new ("NotSet", "NotSet", 0);

    private IFLogAppenderEntry? appenderBuilder;

    private IFLogFirstFormatterParameterEntry? formatBuilder;

    private static uint totalInstanceCount;

    public FLogEntry()
    {
        InstanceNumber = Interlocked.Increment(ref totalInstanceCount);
    }

    public FLogEntry(IFLogEntry toClone) : this()
    {
        LogDateTime = toClone.LogDateTime;
        LogLocation = NotSetLocation;
        SB.Append(toClone.SB);
    }

    public IFLogAppenderEntry? StringAppender
        (StringBuildingStyle style = StringBuildingStyle.Default, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
    {
        LogDateTime      = TimeContext.UtcNow;
        Style            = style;
        LogLocation      = new LoggingLocation(memberName, sourceFilePath, sourceLineNumber);

        return null;
    }

    public IFLogFirstFormatterParameterEntry? StringFormat
    (string formattedString, StringBuildingStyle style = StringBuildingStyle.Default
      , MalformedFormatHandling malformedFormatHandling = MalformedFormatHandling.DefaultMalformedHandling, string memberName = ""
      , string sourceFilePath = "", int sourceLineNumber = 0) 
    {
        LogDateTime = TimeContext.UtcNow;
        Style       = style;
        LogLocation = new LoggingLocation(memberName, sourceFilePath, sourceLineNumber);

        return null;
    }

    public uint IssueSequenceNumber { get; set; }

    public uint InstanceNumber { get; }

    public DateTime        LogDateTime { get; private set; }
    public LoggingLocation LogLocation { get; private set; }

    public StringBuilder SB { get; } = new();

    public StringBuildingStyle Style { get; private set; }

    public override void StateReset()
    {
        LogDateTime = DateTime.MinValue;
        Style       = StringBuildingStyle.Default;
        LogLocation = NotSetLocation;
        SB.Clear();
        base.StateReset();
    }

    public override IFLogEntry Clone() =>
        Recycler?.Borrow<FLogEntry>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new FLogEntry(this);

    public override FLogEntry CopyFrom(IFLogEntry source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        LogDateTime = source.LogDateTime;
        LogLocation = source.LogLocation;
        SB.Append(source.SB);

        return this;
    }
}
