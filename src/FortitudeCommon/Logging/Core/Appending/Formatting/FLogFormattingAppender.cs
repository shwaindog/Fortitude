using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Logging.Config.Appending.Formatting;
using FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Formatting;


public interface IFLogFormattingAppender : IFLogAppender
{
    IBlockingFormatWriterResolverHandle FormatWriterResolver { get; }

    int FormatWriterRequestQueue { get; }

    long TotalBytesAppended { get; }

    long TotalCharsAppended { get; }

    FormattingAppenderSinkType FormatAppenderType { get; }

    IFLogEntryFormatter Formatter { get; }
}

public interface IMutableFLogFormattingAppender : IFLogFormattingAppender, IMutableFLogAppender
{
    new IFLogEntryFormatter Formatter { get; set; }
}

public delegate void FormatWriterReceivedHandler<in T>(T formatWriter) where T : IFormatWriter;

public abstract class FLogFormattingAppender : FLogAppender, IMutableFLogFormattingAppender
{
    protected FLogFormattingAppender(IFormattingAppenderConfig formattingAppenderConfig, IFLogContext context)
        : base(formattingAppenderConfig, context)
    {
        Formatter = new FLogEntryFormatter(formattingAppenderConfig.LogEntryFormatLayout);
    }

    public IFLogEntryFormatter Formatter { get; set; }

    public abstract IBlockingFormatWriterResolverHandle FormatWriterResolver { get; }
    
    public abstract int FormatWriterRequestQueue { get; }

    public abstract FormattingAppenderSinkType FormatAppenderType { get; protected set; }

    public long TotalBytesAppended { get; protected set; }

    public long TotalCharsAppended { get; protected set; }
}
